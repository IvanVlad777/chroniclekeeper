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
    createCulture,
    deleteCulture,
    getCultureById,
    updateCulture,
} from "../../../../api/cultures";
import { getLanguages } from "../../../../api/languages";
import { getReligions } from "../../../../api/religions";
import {
    CultureUpdateDto,
    LanguageDto,
    ReligionDto,
    TechnologicalLevel,
    XenophobiaLevel,
    technologicalLevels,
    xenophobiaLevels,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    languageId: string;
    religionId: string;
    commonValues: string;
    hasOralTradition: boolean;
    socialStructure: string;
    xenophobiaLevel: XenophobiaLevel;
    technologicalLevel: TechnologicalLevel;
    conflictResolution: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    languageId: "",
    religionId: "",
    commonValues: "",
    hasOralTradition: false,
    socialStructure: "",
    xenophobiaLevel: "Neutral",
    technologicalLevel: "Medieval",
    conflictResolution: "",
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): CultureUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        languageId: Number(f.languageId),
        religionId: toId(f.religionId),
        commonValues: f.commonValues,
        hasOralTradition: f.hasOralTradition,
        socialStructure: f.socialStructure,
        xenophobiaLevel: f.xenophobiaLevel,
        technologicalLevel: f.technologicalLevel,
        conflictResolution: f.conflictResolution,
    };
}

/** Zajednička forma za /cultures/new i /cultures/:id/edit. */
export default function CultureForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("culture");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [languages, setLanguages] = useState<LanguageDto[]>([]);
    const [religions, setReligions] = useState<ReligionDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    // Učitavanje šifrarnika (jezici, religije) + kulture u edit modu
    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        Promise.all([
            getLanguages(selectedWorld.id),
            getReligions(selectedWorld.id),
        ])
            .then(async ([languagesData, religionsData]) => {
                if (cancelled) return;
                setLanguages(languagesData);
                setReligions(religionsData);

                if (isEdit) {
                    const c = await getCultureById(editId);
                    if (cancelled) return;
                    setForm({
                        name: c.name ?? "",
                        description: c.description ?? "",
                        languageId: c.languageId ? String(c.languageId) : "",
                        religionId: c.religionId ? String(c.religionId) : "",
                        commonValues: c.commonValues ?? "",
                        hasOralTradition: c.hasOralTradition,
                        socialStructure: c.socialStructure ?? "",
                        xenophobiaLevel: c.xenophobiaLevel,
                        technologicalLevel: c.technologicalLevel,
                        conflictResolution: c.conflictResolution ?? "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load culture form data:", err);
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
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        if (!form.languageId) {
            setSaveError(t("form.languageMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateCulture(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createCulture({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/cultures/${targetId}`);
        } catch (err) {
            console.error("Failed to save culture:", err);
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
            await deleteCulture(editId);
            navigate("/storymap/cultures");
        } catch (err) {
            console.error("Failed to delete culture:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="☉"
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
                    <OrnateField label={t("fields.commonValues")}>
                        <OrnateTextArea
                            value={form.commonValues}
                            rows={5}
                            maxLength={2000}
                            onChange={(e) =>
                                set("commonValues", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.socialStructure")}>
                        <OrnateTextArea
                            value={form.socialStructure}
                            rows={5}
                            maxLength={2000}
                            onChange={(e) =>
                                set("socialStructure", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.conflictResolution")}>
                        <OrnateTextArea
                            value={form.conflictResolution}
                            rows={5}
                            maxLength={2000}
                            onChange={(e) =>
                                set("conflictResolution", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("form.language")} required>
                        <OrnateSelect
                            value={form.languageId}
                            onChange={(e) =>
                                set("languageId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {languages.map((l) => (
                                <option key={l.id} value={l.id}>
                                    {l.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("form.religion")}>
                        <OrnateSelect
                            value={form.religionId}
                            onChange={(e) =>
                                set("religionId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {religions.map((r) => (
                                <option key={r.id} value={r.id}>
                                    {r.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.xenophobiaLevel")}>
                            <OrnateSelect
                                value={form.xenophobiaLevel}
                                onChange={(e) =>
                                    set(
                                        "xenophobiaLevel",
                                        e.target.value as XenophobiaLevel
                                    )
                                }
                            >
                                {xenophobiaLevels.map((level) => (
                                    <option key={level} value={level}>
                                        {t(`xenophobiaLevels.${level}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("fields.technologicalLevel")}>
                            <OrnateSelect
                                value={form.technologicalLevel}
                                onChange={(e) =>
                                    set(
                                        "technologicalLevel",
                                        e.target.value as TechnologicalLevel
                                    )
                                }
                            >
                                {technologicalLevels.map((level) => (
                                    <option key={level} value={level}>
                                        {t(`technologicalLevels.${level}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("fields.hasOralTradition")}
                        checked={form.hasOralTradition}
                        onChange={(e) =>
                            set("hasOralTradition", e.target.checked)
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
                                ? `/storymap/cultures/${editId}`
                                : "/storymap/cultures"
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
