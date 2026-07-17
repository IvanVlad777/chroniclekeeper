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
    createMilitaryDoctrine,
    deleteMilitaryDoctrine,
    getMilitaryDoctrineById,
    updateMilitaryDoctrine,
} from "../../../../api/militaryDoctrines";
import { getHistories } from "../../../../api/histories";
import {
    HistoryDto,
    MilitaryDoctrineUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    strategy: string;
    philosophy: string;
    prioritizesInfantry: boolean;
    prioritizesCavalry: boolean;
    prioritizesArtillery: boolean;
    prioritizesNavalForces: boolean;
    prioritizesAirForces: boolean;
    requiresHeavyIndustry: boolean;
    usesMercenaries: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    strategy: "",
    philosophy: "",
    prioritizesInfantry: false,
    prioritizesCavalry: false,
    prioritizesArtillery: false,
    prioritizesNavalForces: false,
    prioritizesAirForces: false,
    requiresHeavyIndustry: false,
    usesMercenaries: false,
    historyId: "",
};

function toDto(f: FormState): MilitaryDoctrineUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        strategy: f.strategy,
        philosophy: f.philosophy,
        prioritizesInfantry: f.prioritizesInfantry,
        prioritizesCavalry: f.prioritizesCavalry,
        prioritizesArtillery: f.prioritizesArtillery,
        prioritizesNavalForces: f.prioritizesNavalForces,
        prioritizesAirForces: f.prioritizesAirForces,
        requiresHeavyIndustry: f.requiresHeavyIndustry,
        usesMercenaries: f.usesMercenaries,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function MilitaryDoctrineForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("militaryDoctrine");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
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
        getHistories(selectedWorld.id)
            .then(async (historiesData) => {
                if (cancelled) return;
                setHistories(historiesData);
                if (isEdit) {
                    const d = await getMilitaryDoctrineById(editId);
                    if (cancelled) return;
                    setForm({
                        name: d.name ?? "",
                        description: d.description ?? "",
                        strategy: d.strategy ?? "",
                        philosophy: d.philosophy ?? "",
                        prioritizesInfantry: d.prioritizesInfantry,
                        prioritizesCavalry: d.prioritizesCavalry,
                        prioritizesArtillery: d.prioritizesArtillery,
                        prioritizesNavalForces: d.prioritizesNavalForces,
                        prioritizesAirForces: d.prioritizesAirForces,
                        requiresHeavyIndustry: d.requiresHeavyIndustry,
                        usesMercenaries: d.usesMercenaries,
                        historyId: d.historyId ? String(d.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load doctrine form data:", err);
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
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateMilitaryDoctrine(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createMilitaryDoctrine({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/military-doctrines/${targetId}`);
        } catch (err) {
            console.error("Failed to save doctrine:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) return;
        setSaveError(null);
        setBusy(true);
        try {
            await deleteMilitaryDoctrine(editId);
            navigate("/storymap/military-doctrines");
        } catch (err) {
            console.error("Failed to delete doctrine:", err);
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
                glyph="📜"
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
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.strategy")}>
                        <OrnateTextInput
                            value={form.strategy}
                            maxLength={200}
                            placeholder={t("fields.strategyHint")}
                            onChange={(e) => set("strategy", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.philosophy")}>
                        <OrnateTextInput
                            value={form.philosophy}
                            maxLength={200}
                            placeholder={t("fields.philosophyHint")}
                            onChange={(e) => set("philosophy", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("none")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.priorities")}>
                        <div className={s.col}>
                            <OrnateCheckbox
                                label={t("priorities.infantry")}
                                checked={form.prioritizesInfantry}
                                onChange={(e) =>
                                    set("prioritizesInfantry", e.target.checked)
                                }
                            />
                            <OrnateCheckbox
                                label={t("priorities.cavalry")}
                                checked={form.prioritizesCavalry}
                                onChange={(e) =>
                                    set("prioritizesCavalry", e.target.checked)
                                }
                            />
                            <OrnateCheckbox
                                label={t("priorities.artillery")}
                                checked={form.prioritizesArtillery}
                                onChange={(e) =>
                                    set("prioritizesArtillery", e.target.checked)
                                }
                            />
                            <OrnateCheckbox
                                label={t("priorities.navalForces")}
                                checked={form.prioritizesNavalForces}
                                onChange={(e) =>
                                    set(
                                        "prioritizesNavalForces",
                                        e.target.checked
                                    )
                                }
                            />
                            <OrnateCheckbox
                                label={t("priorities.airForces")}
                                checked={form.prioritizesAirForces}
                                onChange={(e) =>
                                    set("prioritizesAirForces", e.target.checked)
                                }
                            />
                        </div>
                    </OrnateField>
                    <OrnateField label={t("fields.traits")}>
                        <div className={s.col}>
                            <OrnateCheckbox
                                label={t("traits.requiresHeavyIndustry")}
                                checked={form.requiresHeavyIndustry}
                                onChange={(e) =>
                                    set(
                                        "requiresHeavyIndustry",
                                        e.target.checked
                                    )
                                }
                            />
                            <OrnateCheckbox
                                label={t("traits.usesMercenaries")}
                                checked={form.usesMercenaries}
                                onChange={(e) =>
                                    set("usesMercenaries", e.target.checked)
                                }
                            />
                        </div>
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
                    <Button variant="danger" disabled={busy} onClick={onDelete}>
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
                                ? `/storymap/military-doctrines/${editId}`
                                : "/storymap/military-doctrines"
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
