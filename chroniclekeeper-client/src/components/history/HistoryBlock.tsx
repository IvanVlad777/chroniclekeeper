import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateField, OrnateSelect, OrnateTextInput } from "../ornate";
import {
    HistoryDetailsDto,
    HistoryDto,
    ReferenceDto,
} from "../../interfaces/loreInterfaces";
import {
    createHistory,
    getHistories,
    getHistoryById,
    linkHistory,
    unlinkHistory,
    type HistoryLinkTargetType,
} from "../../api/histories";
import { apiErrorMessage } from "../../utils/apiError";
import s from "./historyBlock.module.css";

interface HistoryBlockProps {
    targetType: HistoryLinkTargetType;
    targetId: number;
    worldId: number;
    /** Currently linked history (from the entity's DetailsDto), or null. */
    history: ReferenceDto | null | undefined;
    canEdit: boolean;
    /** Called after a link/unlink/create so the parent can refetch the entity. */
    onChanged: () => void;
}

type Mode = "idle" | "create" | "link";

/**
 * Generic, reusable "History" section for an entity detail page. Shows the
 * linked history (summary + its timelines) and lets an editor create a new
 * history or link an existing one right here — not from the edit form.
 */
export function HistoryBlock({
    targetType,
    targetId,
    worldId,
    history,
    canEdit,
    onChanged,
}: HistoryBlockProps) {
    const { t } = useTranslation();

    const [details, setDetails] = useState<HistoryDetailsDto | null>(null);
    const [mode, setMode] = useState<Mode>("idle");
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Create form
    const [newName, setNewName] = useState("");
    // Link form
    const [options, setOptions] = useState<HistoryDto[]>([]);
    const [picked, setPicked] = useState("");

    // Load the linked history's summary + timelines for the rich block.
    useEffect(() => {
        if (!history) {
            setDetails(null);
            return;
        }
        let cancelled = false;
        getHistoryById(history.id)
            .then((d) => !cancelled && setDetails(d))
            .catch(() => !cancelled && setDetails(null));
        return () => {
            cancelled = true;
        };
    }, [history]);

    const openLink = async () => {
        setMode("link");
        setError(null);
        try {
            const list = await getHistories(worldId);
            setOptions(list);
        } catch (err) {
            setError(apiErrorMessage(err, t("historyBlock.linkFailed")));
        }
    };

    const reset = () => {
        setMode("idle");
        setNewName("");
        setPicked("");
        setError(null);
    };

    async function onCreate() {
        if (!newName.trim()) {
            setError(t("historyBlock.nameRequired"));
            return;
        }
        setBusy(true);
        setError(null);
        try {
            const created = await createHistory({
                name: newName.trim(),
                description: "",
                worldId,
                summary: "",
                isOfficial: false,
            });
            await linkHistory(created.id, targetType, targetId);
            reset();
            onChanged();
        } catch (err) {
            setError(apiErrorMessage(err, t("historyBlock.createFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onLink() {
        if (!picked) {
            setError(t("historyBlock.pickRequired"));
            return;
        }
        setBusy(true);
        setError(null);
        try {
            await linkHistory(Number(picked), targetType, targetId);
            reset();
            onChanged();
        } catch (err) {
            setError(apiErrorMessage(err, t("historyBlock.linkFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onUnlink() {
        if (!history) return;
        if (!window.confirm(t("historyBlock.confirmUnlink"))) return;
        setBusy(true);
        setError(null);
        try {
            await unlinkHistory(history.id, targetType, targetId);
            onChanged();
        } catch (err) {
            setError(apiErrorMessage(err, t("historyBlock.unlinkFailed")));
        } finally {
            setBusy(false);
        }
    }

    return (
        <section className={s.block}>
            <div className={s.head}>
                <span className={s.title}>{t("historyBlock.label")}</span>
                <span className={s.line} />
                {history && canEdit && (
                    <button
                        type="button"
                        className={s.unlink}
                        disabled={busy}
                        onClick={onUnlink}
                    >
                        {t("historyBlock.unlink")}
                    </button>
                )}
            </div>

            {history ? (
                <div className={s.linked}>
                    <Link
                        to={`/storymap/histories/${history.id}`}
                        className={s.historyName}
                    >
                        {history.name}
                    </Link>
                    {details?.summary && (
                        <p className={s.summary}>{details.summary}</p>
                    )}
                    {details && details.timelines.length > 0 && (
                        <div className={s.timelines}>
                            {details.timelines.map((tl) => (
                                <Link
                                    key={tl.id}
                                    to={`/storymap/timelines/${tl.id}`}
                                    className={s.timelineChip}
                                >
                                    {tl.name}
                                    <span className={s.timelineCount}>
                                        {tl.eventCount}
                                    </span>
                                </Link>
                            ))}
                        </div>
                    )}
                </div>
            ) : (
                <>
                    {!canEdit && (
                        <p className={s.none}>{t("historyBlock.none")}</p>
                    )}

                    {canEdit && mode === "idle" && (
                        <div className={s.actions}>
                            <p className={s.noneEditor}>
                                {t("historyBlock.noneEditor")}
                            </p>
                            <div className={s.actionButtons}>
                                <Button
                                    size="sm"
                                    onClick={() => {
                                        setMode("create");
                                        setError(null);
                                    }}
                                >
                                    + {t("historyBlock.create")}
                                </Button>
                                <Button
                                    size="sm"
                                    variant="ghost"
                                    onClick={openLink}
                                >
                                    {t("historyBlock.linkExisting")}
                                </Button>
                            </div>
                        </div>
                    )}

                    {canEdit && mode === "create" && (
                        <div className={s.form}>
                            <OrnateField
                                label={t("historyBlock.createName")}
                                required
                            >
                                <OrnateTextInput
                                    value={newName}
                                    display
                                    maxLength={100}
                                    onChange={(e) => setNewName(e.target.value)}
                                />
                            </OrnateField>
                            {error && (
                                <p className={s.error} role="alert">
                                    {error}
                                </p>
                            )}
                            <div className={s.formActions}>
                                <Button
                                    size="sm"
                                    variant="ghost"
                                    disabled={busy}
                                    onClick={reset}
                                >
                                    {t("historyBlock.cancel")}
                                </Button>
                                <Button
                                    size="sm"
                                    disabled={busy}
                                    onClick={onCreate}
                                >
                                    {t("historyBlock.createAction")}
                                </Button>
                            </div>
                        </div>
                    )}

                    {canEdit && mode === "link" && (
                        <div className={s.form}>
                            <OrnateField label={t("historyBlock.linkPick")}>
                                <OrnateSelect
                                    value={picked}
                                    onChange={(e) => setPicked(e.target.value)}
                                >
                                    <option value="">
                                        {t("historyBlock.linkPickPlaceholder")}
                                    </option>
                                    {options.map((h) => (
                                        <option key={h.id} value={h.id}>
                                            {h.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            {error && (
                                <p className={s.error} role="alert">
                                    {error}
                                </p>
                            )}
                            <div className={s.formActions}>
                                <Button
                                    size="sm"
                                    variant="ghost"
                                    disabled={busy}
                                    onClick={reset}
                                >
                                    {t("historyBlock.cancel")}
                                </Button>
                                <Button
                                    size="sm"
                                    disabled={busy}
                                    onClick={onLink}
                                >
                                    {t("historyBlock.attach")}
                                </Button>
                            </div>
                        </div>
                    )}

                    {error && mode === "idle" && (
                        <p className={s.error} role="alert">
                            {error}
                        </p>
                    )}
                </>
            )}
        </section>
    );
}
