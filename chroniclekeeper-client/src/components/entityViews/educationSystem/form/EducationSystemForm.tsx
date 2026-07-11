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
    createEducationSystem,
    deleteEducationSystem,
    getEducationSystemById,
    updateEducationSystem,
} from "../../../../api/educationSystems";
import { EducationSystemUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isStateControlled: boolean;
    allowsPrivateInstitutions: boolean;
    includesReligiousEducation: boolean;
    supportsGuildTraining: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isStateControlled: false,
    allowsPrivateInstitutions: false,
    includesReligiousEducation: false,
    supportsGuildTraining: false,
};

function toDto(f: FormState): EducationSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isStateControlled: f.isStateControlled,
        allowsPrivateInstitutions: f.allowsPrivateInstitutions,
        includesReligiousEducation: f.includesReligiousEducation,
        supportsGuildTraining: f.supportsGuildTraining,
    };
}

/** Zajednička forma za /education-systems/new i /education-systems/:id/edit. */
export default function EducationSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("educationSystem");
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

        getEducationSystemById(editId)
            .then((es) => {
                if (cancelled) return;
                setForm({
                    name: es.name ?? "",
                    description: es.description ?? "",
                    isStateControlled: es.isStateControlled,
                    allowsPrivateInstitutions: es.allowsPrivateInstitutions,
                    includesReligiousEducation:
                        es.includesReligiousEducation,
                    supportsGuildTraining: es.supportsGuildTraining,
                });
            })
            .catch((err) => {
                console.error("Failed to load education system:", err);
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
                await updateEducationSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createEducationSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/education-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save education system:", err);
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
            await deleteEducationSystem(editId);
            navigate("/storymap/education-systems");
        } catch (err) {
            console.error("Failed to delete education system:", err);
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
                glyph="▦"
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
                    <OrnateCheckbox
                        label={t("fields.isStateControlled")}
                        checked={form.isStateControlled}
                        onChange={(e) =>
                            set("isStateControlled", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.allowsPrivateInstitutions")}
                        checked={form.allowsPrivateInstitutions}
                        onChange={(e) =>
                            set("allowsPrivateInstitutions", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.includesReligiousEducation")}
                        checked={form.includesReligiousEducation}
                        onChange={(e) =>
                            set(
                                "includesReligiousEducation",
                                e.target.checked
                            )
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.supportsGuildTraining")}
                        checked={form.supportsGuildTraining}
                        onChange={(e) =>
                            set("supportsGuildTraining", e.target.checked)
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
                                ? `/storymap/education-systems/${editId}`
                                : "/storymap/education-systems"
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
