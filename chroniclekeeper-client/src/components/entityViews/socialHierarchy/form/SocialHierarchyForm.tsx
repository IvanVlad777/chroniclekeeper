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
    createSocialHierarchy,
    deleteSocialHierarchy,
    getSocialHierarchyById,
    updateSocialHierarchy,
} from "../../../../api/socialHierarchies";
import { getHistories } from "../../../../api/histories";
import {
    HistoryDto,
    SocialHierarchyUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isCasteSystem: boolean;
    allowsUpwardMobility: boolean;
    allowsIntermarriage: boolean;
    enforcesLegalSeparation: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isCasteSystem: false,
    allowsUpwardMobility: false,
    allowsIntermarriage: false,
    enforcesLegalSeparation: false,
    historyId: "",
};

function toDto(f: FormState): SocialHierarchyUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isCasteSystem: f.isCasteSystem,
        allowsUpwardMobility: f.allowsUpwardMobility,
        allowsIntermarriage: f.allowsIntermarriage,
        enforcesLegalSeparation: f.enforcesLegalSeparation,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /social-hierarchies/new i /:id/edit. */
export default function SocialHierarchyForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("socialHierarchy");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(isEdit);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (!selectedWorld) return;
        getHistories(selectedWorld.id)
            .then(setHistories)
            .catch((err) => console.error("Failed to load histories:", err));
    }, [selectedWorld]);

    useEffect(() => {
        if (!isEdit) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getSocialHierarchyById(editId)
            .then((h) => {
                if (cancelled) return;
                setForm({
                    name: h.name ?? "",
                    description: h.description ?? "",
                    isCasteSystem: h.isCasteSystem,
                    allowsUpwardMobility: h.allowsUpwardMobility,
                    allowsIntermarriage: h.allowsIntermarriage,
                    enforcesLegalSeparation: h.enforcesLegalSeparation,
                    historyId: h.historyId ? String(h.historyId) : "",
                });
            })
            .catch((err) => {
                console.error("Failed to load social hierarchy:", err);
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
                await updateSocialHierarchy(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createSocialHierarchy({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/social-hierarchies/${targetId}`);
        } catch (err) {
            console.error("Failed to save social hierarchy:", err);
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
            await deleteSocialHierarchy(editId);
            navigate("/storymap/social-hierarchies");
        } catch (err) {
            console.error("Failed to delete social hierarchy:", err);
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
                glyph="⚜"
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
                            rows={8}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("form.noHistory")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <span className={s.legend}>{t("form.rightsLegend")}</span>
                    <OrnateCheckbox
                        label={t("fields.isCasteSystem")}
                        checked={form.isCasteSystem}
                        onChange={(e) => set("isCasteSystem", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.allowsUpwardMobility")}
                        checked={form.allowsUpwardMobility}
                        onChange={(e) =>
                            set("allowsUpwardMobility", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.allowsIntermarriage")}
                        checked={form.allowsIntermarriage}
                        onChange={(e) =>
                            set("allowsIntermarriage", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.enforcesLegalSeparation")}
                        checked={form.enforcesLegalSeparation}
                        onChange={(e) =>
                            set("enforcesLegalSeparation", e.target.checked)
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
                                ? `/storymap/social-hierarchies/${editId}`
                                : "/storymap/social-hierarchies"
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
