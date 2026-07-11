import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createSchool,
    deleteSchool,
    getSchoolById,
    updateSchool,
} from "../../../../api/schools";
import { getEducationSystems } from "../../../../api/educationSystems";
import { EducationSystemDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    educationSystemId: string;
    isPublic: boolean;
    isReligious: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    educationSystemId: "",
    isPublic: false,
    isReligious: false,
};

/** Zajednička forma za /schools/new i /schools/:id/edit (samo obične škole). */
export default function SchoolForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("school");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [educationSystems, setEducationSystems] = useState<
        EducationSystemDto[]
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
            isEdit ? getSchoolById(editId) : Promise.resolve(null),
        ])
            .then(([systems, school]) => {
                if (cancelled) return;
                setEducationSystems(systems);
                if (school) {
                    setForm({
                        name: school.name ?? "",
                        description: school.description ?? "",
                        educationSystemId: String(school.educationSystemId),
                        isPublic: school.isPublic,
                        isReligious: school.isReligious,
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load school form data:", err);
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
                isPublic: form.isPublic,
                isReligious: form.isReligious,
            };
            if (isEdit) {
                await updateSchool(editId, payload);
                targetId = editId;
            } else {
                const created = await createSchool({
                    ...payload,
                    educationSystemId: Number(form.educationSystemId),
                });
                targetId = created.id;
            }
            navigate(`/storymap/schools/${targetId}`);
        } catch (err) {
            console.error("Failed to save school:", err);
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
            await deleteSchool(editId);
            navigate("/storymap/schools");
        } catch (err) {
            console.error("Failed to delete school:", err);
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
                glyph="☰"
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
                        <OrnateSelect
                            value={form.educationSystemId}
                            disabled={isEdit}
                            onChange={(e) =>
                                set("educationSystemId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {educationSystems.map((es) => (
                                <option key={es.id} value={es.id}>
                                    {es.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isPublic")}
                        checked={form.isPublic}
                        onChange={(e) => set("isPublic", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isReligious")}
                        checked={form.isReligious}
                        onChange={(e) => set("isReligious", e.target.checked)}
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
                                ? `/storymap/schools/${editId}`
                                : "/storymap/schools"
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
