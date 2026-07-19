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
    EntityPicker,
    type EntityOption,
} from "../../../quickCreate/EntityPicker";
import {
    createUniversity,
    deleteUniversity,
    getUniversityById,
    updateUniversity,
} from "../../../../api/universities";
import { getEducationSystems } from "../../../../api/educationSystems";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    educationSystemId: string;
    focusesOnScience: boolean;
    focusesOnMagic: boolean;
    focusesOnPhilosophy: boolean;
    focusesOnMilitaryStudies: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    educationSystemId: "",
    focusesOnScience: false,
    focusesOnMagic: false,
    focusesOnPhilosophy: false,
    focusesOnMilitaryStudies: false,
};

/** Zajednička forma za /universities/new i /universities/:id/edit. */
export default function UniversityForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("university");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [educationSystemOptions, setEducationSystemOptions] = useState<
        EntityOption[]
    >([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        Promise.all([
            getEducationSystems(selectedWorld.id),
            isEdit ? getUniversityById(editId) : Promise.resolve(null),
        ])
            .then(([systems, uni]) => {
                if (cancelled) return;
                setEducationSystemOptions(
                    systems.map((es) => ({ value: es.id, label: es.name }))
                );
                if (uni) {
                    setForm({
                        name: uni.name ?? "",
                        description: uni.description ?? "",
                        educationSystemId: String(uni.educationSystemId),
                        focusesOnScience: uni.focusesOnScience,
                        focusesOnMagic: uni.focusesOnMagic,
                        focusesOnPhilosophy: uni.focusesOnPhilosophy,
                        focusesOnMilitaryStudies:
                            uni.focusesOnMilitaryStudies,
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load university form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        if (!isEdit && !form.educationSystemId) {
            setSaveError(t("form.educationSystemMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            const payload = {
                name: form.name.trim(),
                description: form.description,
                focusesOnScience: form.focusesOnScience,
                focusesOnMagic: form.focusesOnMagic,
                focusesOnPhilosophy: form.focusesOnPhilosophy,
                focusesOnMilitaryStudies: form.focusesOnMilitaryStudies,
            };
            if (isEdit) {
                await updateUniversity(editId, payload);
                targetId = editId;
            } else {
                const created = await createUniversity({
                    ...payload,
                    educationSystemId: Number(form.educationSystemId),
                });
                targetId = created.id;
            }
            navigate(`/storymap/universities/${targetId}`);
        } catch (err) {
            console.error("Failed to save university:", err);
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
            await deleteUniversity(editId);
            navigate("/storymap/universities");
        } catch (err) {
            console.error("Failed to delete university:", err);
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
                glyph="⚛"
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
                    <OrnateField
                        label={t("form.educationSystem")}
                        required={!isEdit}
                        hint={isEdit ? t("form.educationSystemLocked") : undefined}
                    >
                        <EntityPicker
                            kind="educationSystem"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            disabled={isEdit}
                            value={form.educationSystemId}
                            options={educationSystemOptions}
                            onChange={(v) => set("educationSystemId", v)}
                            onCreated={(es) =>
                                setEducationSystemOptions((prev) => [
                                    ...prev,
                                    { value: es.id, label: es.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.focusesOnScience")}
                        checked={form.focusesOnScience}
                        onChange={(e) =>
                            set("focusesOnScience", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.focusesOnMagic")}
                        checked={form.focusesOnMagic}
                        onChange={(e) =>
                            set("focusesOnMagic", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.focusesOnPhilosophy")}
                        checked={form.focusesOnPhilosophy}
                        onChange={(e) =>
                            set("focusesOnPhilosophy", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.focusesOnMilitaryStudies")}
                        checked={form.focusesOnMilitaryStudies}
                        onChange={(e) =>
                            set("focusesOnMilitaryStudies", e.target.checked)
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
                                ? `/storymap/universities/${editId}`
                                : "/storymap/universities"
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
