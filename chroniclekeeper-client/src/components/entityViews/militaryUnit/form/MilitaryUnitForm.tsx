import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
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
    createMilitaryUnit,
    deleteMilitaryUnit,
    getMilitaryUnitById,
    updateMilitaryUnit,
} from "../../../../api/militaryUnits";
import { getHistories } from "../../../../api/histories";
import {
    MilitaryUnitUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    unitType: string;
    size: string;
    isElite: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    unitType: "",
    size: "0",
    isElite: false,
    historyId: "",
};

function toDto(f: FormState): MilitaryUnitUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        unitType: f.unitType,
        size: Number(f.size) || 0,
        isElite: f.isElite,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function MilitaryUnitForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;
    const [searchParams] = useSearchParams();

    const navigate = useNavigate();
    const { t } = useTranslation("militaryUnit");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [historyOptions, setHistoryOptions] = useState<EntityOption[]>([]);
    // Vojska kojoj postrojba pripada: iz ?armyId na /new, ili iz postrojbe na /edit.
    const [armyId, setArmyId] = useState<number | null>(
        searchParams.get("armyId") ? Number(searchParams.get("armyId")) : null
    );
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
                setHistoryOptions(
                    historiesData.map((h) => ({ value: h.id, label: h.name }))
                );
                if (isEdit) {
                    const u = await getMilitaryUnitById(editId);
                    if (cancelled) return;
                    setArmyId(u.belongsToArmyId);
                    setForm({
                        name: u.name ?? "",
                        description: u.description ?? "",
                        unitType: u.unitType ?? "",
                        size: String(u.size ?? 0),
                        isElite: u.isElite,
                        historyId: u.historyId ? String(u.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load unit form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    const backTarget = armyId
        ? `/storymap/armies/${armyId}`
        : "/storymap/armies";

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        if (!isEdit && !armyId) {
            setSaveError(t("form.missingArmy"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateMilitaryUnit(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createMilitaryUnit({
                    ...toDto(form),
                    belongsToArmyId: armyId!,
                });
                targetId = created.id;
            }
            navigate(`/storymap/military-units/${targetId}`);
        } catch (err) {
            console.error("Failed to save unit:", err);
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
            await deleteMilitaryUnit(editId);
            navigate(backTarget);
        } catch (err) {
            console.error("Failed to delete unit:", err);
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
                glyph="🎖"
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
                    <OrnateField label={t("ranks.name")} required>
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
                            rows={7}
                            maxLength={4000}
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.unitType")}>
                        <OrnateTextInput
                            value={form.unitType}
                            maxLength={100}
                            placeholder={t("fields.unitTypeHint")}
                            onChange={(e) => set("unitType", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.size")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.size}
                                onChange={(e) => set("size", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.isElite")}>
                            <OrnateCheckbox
                                label={t("fields.elite")}
                                checked={form.isElite}
                                onChange={(e) =>
                                    set("isElite", e.target.checked)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("form.history")}>
                        <EntityPicker
                            kind="history"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.historyId}
                            options={historyOptions}
                            onChange={(v) => set("historyId", v)}
                            onCreated={(h) =>
                                setHistoryOptions((prev) => [
                                    ...prev,
                                    { value: h.id, label: h.name },
                                ])
                            }
                        />
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
                                ? `/storymap/military-units/${editId}`
                                : backTarget
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
