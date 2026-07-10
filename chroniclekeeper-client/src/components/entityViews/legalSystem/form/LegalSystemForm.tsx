import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createLegalSystem,
    deleteLegalSystem,
    getLegalSystemById,
    updateLegalSystem,
} from "../../../../api/legalSystems";
import {
    LegalSystemUpdateDto,
    PunishmentMethod,
    ScaleLevel,
    punishmentMethods,
    scaleLevels,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    laws: string;
    judicialIndependence: ScaleLevel;
    punishmentMethods: PunishmentMethod;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    laws: "",
    judicialIndependence: "Moderate",
    punishmentMethods: "Imprisonment",
};

function toDto(f: FormState): LegalSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        laws: f.laws,
        judicialIndependence: f.judicialIndependence,
        punishmentMethods: f.punishmentMethods,
    };
}

/** Zajednička forma za /legal-systems/new i /legal-systems/:id/edit. */
export default function LegalSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("legalSystem");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [loading, setLoading] = useState(isEdit);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (!isEdit) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getLegalSystemById(editId)
            .then((l) => {
                if (cancelled) return;
                setForm({
                    name: l.name ?? "",
                    description: l.description ?? "",
                    laws: l.laws ?? "",
                    judicialIndependence: l.judicialIndependence,
                    punishmentMethods: l.punishmentMethods,
                });
            })
            .catch((err) => {
                console.error("Failed to load legal system:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateLegalSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createLegalSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/legal-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save legal system:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteLegalSystem(editId);
            navigate("/storymap/legal-systems");
        } catch (err) {
            console.error("Failed to delete legal system:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⛨"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }
    if (loadError) {
        return (
            <ErrorState
                onRetry={() => setReloadKey((k) => k + 1)}
                detail={loadError}
            />
        );
    }

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={6}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.laws")}>
                        <OrnateTextArea
                            value={form.laws}
                            rows={8}
                            maxLength={4000}
                            onChange={(e) => set("laws", e.target.value)}
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.judicialIndependence")}>
                        <OrnateSelect
                            value={form.judicialIndependence}
                            onChange={(e) =>
                                set(
                                    "judicialIndependence",
                                    e.target.value as ScaleLevel
                                )
                            }
                        >
                            {scaleLevels.map((level) => (
                                <option key={level} value={level}>
                                    {t(`scaleLevels.${level}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.punishmentMethods")}>
                        <OrnateSelect
                            value={form.punishmentMethods}
                            onChange={(e) =>
                                set(
                                    "punishmentMethods",
                                    e.target.value as PunishmentMethod
                                )
                            }
                        >
                            {punishmentMethods.map((method) => (
                                <option key={method} value={method}>
                                    {t(`punishmentMethods.${method}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button
                        variant="danger"
                        disabled={busy}
                        onClick={onDelete}
                    >
                        {t("form.delete")}
                    </Button>
                )}
                <span className={s.footerSpacer} />
                <Button
                    variant="ghost"
                    disabled={busy}
                    onClick={() =>
                        navigate(
                            isEdit
                                ? `/storymap/legal-systems/${editId}`
                                : "/storymap/legal-systems"
                        )
                    }
                >
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>
                    {busy ? t("form.saving") : t("form.save")}
                </Button>
            </div>
        </form>
    );
}
