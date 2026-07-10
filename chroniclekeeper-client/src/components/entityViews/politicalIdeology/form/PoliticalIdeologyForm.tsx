import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createPoliticalIdeology,
    deletePoliticalIdeology,
    getPoliticalIdeologyById,
    updatePoliticalIdeology,
} from "../../../../api/politicalIdeologies";
import { PoliticalIdeologyUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isAuthoritarian: boolean;
    isSocialist: boolean;
    isLiberal: boolean;
    isRadical: boolean;
    isMilitaristic: boolean;
    supportsFreeMarket: boolean;
    supportsPlannedEconomy: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isAuthoritarian: false,
    isSocialist: false,
    isLiberal: false,
    isRadical: false,
    isMilitaristic: false,
    supportsFreeMarket: false,
    supportsPlannedEconomy: false,
};

function toDto(f: FormState): PoliticalIdeologyUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isAuthoritarian: f.isAuthoritarian,
        isSocialist: f.isSocialist,
        isLiberal: f.isLiberal,
        isRadical: f.isRadical,
        isMilitaristic: f.isMilitaristic,
        supportsFreeMarket: f.supportsFreeMarket,
        supportsPlannedEconomy: f.supportsPlannedEconomy,
    };
}

/** Zajednička forma za /political-ideologies/new i /political-ideologies/:id/edit. */
export default function PoliticalIdeologyForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("politicalIdeology");
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

        getPoliticalIdeologyById(editId)
            .then((i) => {
                if (cancelled) return;
                setForm({
                    name: i.name ?? "",
                    description: i.description ?? "",
                    isAuthoritarian: i.isAuthoritarian,
                    isSocialist: i.isSocialist,
                    isLiberal: i.isLiberal,
                    isRadical: i.isRadical,
                    isMilitaristic: i.isMilitaristic,
                    supportsFreeMarket: i.supportsFreeMarket,
                    supportsPlannedEconomy: i.supportsPlannedEconomy,
                });
            })
            .catch((err) => {
                console.error("Failed to load political ideology:", err);
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
                await updatePoliticalIdeology(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createPoliticalIdeology({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/political-ideologies/${targetId}`);
        } catch (err) {
            console.error("Failed to save political ideology:", err);
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
            await deletePoliticalIdeology(editId);
            navigate("/storymap/political-ideologies");
        } catch (err) {
            console.error("Failed to delete political ideology:", err);
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
                glyph="◐"
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
                            rows={10}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <span className={s.legend}>{t("form.traitsLegend")}</span>
                    <OrnateCheckbox
                        label={t("fields.isAuthoritarian")}
                        checked={form.isAuthoritarian}
                        onChange={(e) =>
                            set("isAuthoritarian", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isSocialist")}
                        checked={form.isSocialist}
                        onChange={(e) =>
                            set("isSocialist", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isLiberal")}
                        checked={form.isLiberal}
                        onChange={(e) => set("isLiberal", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isRadical")}
                        checked={form.isRadical}
                        onChange={(e) => set("isRadical", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isMilitaristic")}
                        checked={form.isMilitaristic}
                        onChange={(e) =>
                            set("isMilitaristic", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.supportsFreeMarket")}
                        checked={form.supportsFreeMarket}
                        onChange={(e) =>
                            set("supportsFreeMarket", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.supportsPlannedEconomy")}
                        checked={form.supportsPlannedEconomy}
                        onChange={(e) =>
                            set("supportsPlannedEconomy", e.target.checked)
                        }
                    />
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
                                ? `/storymap/political-ideologies/${editId}`
                                : "/storymap/political-ideologies"
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
