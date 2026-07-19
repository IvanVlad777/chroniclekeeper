import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
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
    createBattle,
    deleteBattle,
    getBattleById,
    updateBattle,
} from "../../../../api/battles";
import { getHistories } from "../../../../api/histories";
import { BattleUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    battleDate: string;
    location: string;
    outcome: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    battleDate: "",
    location: "",
    outcome: "",
    historyId: "",
};

function toDto(f: FormState): BattleUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        battleDate: f.battleDate || null,
        location: f.location,
        outcome: f.outcome,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function BattleForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("battle");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [historyOptions, setHistoryOptions] = useState<EntityOption[]>([]);
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
                    const b = await getBattleById(editId);
                    if (cancelled) return;
                    setForm({
                        name: b.name ?? "",
                        description: b.description ?? "",
                        battleDate: b.battleDate ?? "",
                        location: b.location ?? "",
                        outcome: b.outcome ?? "",
                        historyId: b.historyId ? String(b.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load battle form data:", err);
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
                await updateBattle(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createBattle({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/battles/${targetId}`);
        } catch (err) {
            console.error("Failed to save battle:", err);
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
            await deleteBattle(editId);
            navigate("/storymap/battles");
        } catch (err) {
            console.error("Failed to delete battle:", err);
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
                glyph="🗡"
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
                            rows={7}
                            maxLength={4000}
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.battleDate")}>
                        <OrnateTextInput
                            value={form.battleDate}
                            maxLength={100}
                            placeholder={t("fields.battleDateHint")}
                            onChange={(e) => set("battleDate", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.location")}>
                        <OrnateTextInput
                            value={form.location}
                            maxLength={200}
                            placeholder={t("fields.locationHint")}
                            onChange={(e) => set("location", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.outcome")}>
                        <OrnateTextInput
                            value={form.outcome}
                            maxLength={200}
                            placeholder={t("fields.outcomeHint")}
                            onChange={(e) => set("outcome", e.target.value)}
                        />
                    </OrnateField>
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
                                ? `/storymap/battles/${editId}`
                                : "/storymap/battles"
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
