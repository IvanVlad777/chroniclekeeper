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
    createTimeline,
    deleteTimeline,
    getTimeline,
    updateTimeline,
} from "../../../../api/timelines";
import { getHistories } from "../../../../api/histories";
import { HistoryDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

/** Zajednička forma za /timelines/new i /timelines/:id/edit. */
export default function TimelineForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("timeline");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [historyId, setHistoryId] = useState("");
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    useEffect(() => {
        if (!selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getHistories(selectedWorld.id)
            .then(async (historiesData) => {
                if (cancelled) return;
                setHistories(historiesData);
                if (isEdit) {
                    const tl = await getTimeline(editId);
                    if (cancelled) return;
                    setName(tl.name ?? "");
                    setDescription(tl.description ?? "");
                    setHistoryId(tl.historyId ? String(tl.historyId) : "");
                }
            })
            .catch((err) => {
                console.error("Failed to load timeline form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            const historyIdValue = historyId ? Number(historyId) : null;
            if (isEdit) {
                await updateTimeline(editId, {
                    name: name.trim(),
                    description,
                    historyId: historyIdValue,
                });
                targetId = editId;
            } else {
                const created = await createTimeline({
                    name: name.trim(),
                    description,
                    worldId: selectedWorld.id,
                    historyId: historyIdValue,
                });
                targetId = created.id;
            }
            navigate(`/storymap/timelines/${targetId}`);
        } catch (err) {
            console.error("Failed to save timeline:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteTimeline(editId);
            navigate("/storymap/timelines");
        } catch (err) {
            console.error("Failed to delete timeline:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={6} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⌛"
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

            <div className={s.col}>
                <OrnateField label={t("columns.name")} required>
                    <OrnateTextInput
                        value={name}
                        display
                        maxLength={100}
                        onChange={(e) => setName(e.target.value)}
                    />
                </OrnateField>
                <OrnateField label={t("fields.description")}>
                    <OrnateTextArea
                        value={description}
                        rows={6}
                        maxLength={4000}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </OrnateField>
                <OrnateField label={t("fields.history")}>
                    <OrnateSelect
                        value={historyId}
                        onChange={(e) => setHistoryId(e.target.value)}
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

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && (
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
                                ? `/storymap/timelines/${editId}`
                                : "/storymap/timelines"
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
