import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
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
    TimelineDetailsDto,
    TimelineEventDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addTimelineEvent,
    deleteTimelineEvent,
    getTimeline,
    updateTimelineEvent,
} from "../../../../api/timelines";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface EventFormState {
    name: string;
    date: string;
    sortOrder: string;
    description: string;
    consequences: string;
    isMajorEvent: boolean;
}

const emptyEventForm: EventFormState = {
    name: "",
    date: "",
    sortOrder: "",
    description: "",
    consequences: "",
    isMajorEvent: false,
};

export default function TimelineDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("timeline");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [timeline, setTimeline] = useState<TimelineDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Inline forma eventa: null = zatvorena, 0 = nova, >0 = edit tog id-a
    const [eventFormFor, setEventFormFor] = useState<number | null>(null);
    const [eventForm, setEventForm] = useState<EventFormState>(emptyEventForm);
    const [eventError, setEventError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    const setE = <K extends keyof EventFormState>(
        key: K,
        value: EventFormState[K]
    ) => setEventForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        const timelineId = Number(id);
        if (!timelineId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getTimeline(timelineId)
            .then((data) => {
                if (!cancelled) setTimeline(data);
            })
            .catch((err) => {
                console.error("Failed to load timeline:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) {
                    setNotFound(true);
                } else {
                    setError(t("loaderror"));
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    const openNewEvent = () => {
        // Predloži sljedeći sortOrder na kraju kronologije
        const maxOrder = timeline?.events.length
            ? Math.max(...timeline.events.map((e) => e.sortOrder))
            : 0;
        setEventForm({ ...emptyEventForm, sortOrder: String(maxOrder + 10) });
        setEventError(null);
        setEventFormFor(0);
    };

    const openEditEvent = (ev: TimelineEventDto) => {
        setEventForm({
            name: ev.name ?? "",
            date: ev.date ?? "",
            sortOrder: String(ev.sortOrder ?? 0),
            description: ev.description ?? "",
            consequences: ev.consequences ?? "",
            isMajorEvent: ev.isMajorEvent,
        });
        setEventError(null);
        setEventFormFor(ev.id);
    };

    async function onSaveEvent(e: FormEvent) {
        e.preventDefault();
        if (!timeline || eventFormFor === null) return;
        if (!eventForm.name.trim()) {
            setEventError(t("events.requiredMissing"));
            return;
        }
        setEventError(null);
        setBusy(true);
        try {
            const payload = {
                name: eventForm.name.trim(),
                date: eventForm.date.trim(),
                sortOrder: eventForm.sortOrder.trim()
                    ? Number(eventForm.sortOrder)
                    : 0,
                description: eventForm.description,
                consequences: eventForm.consequences,
                isMajorEvent: eventForm.isMajorEvent,
            };
            if (eventFormFor === 0) {
                await addTimelineEvent(timeline.id, payload);
            } else {
                await updateTimelineEvent(eventFormFor, payload);
            }
            setEventFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save event:", err);
            setEventError(apiErrorMessage(err, t("events.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDeleteEvent(ev: TimelineEventDto) {
        if (!window.confirm(t("events.deleteConfirm", { name: ev.name }))) {
            return;
        }
        setEventError(null);
        setBusy(true);
        try {
            await deleteTimelineEvent(ev.id);
            refetch();
        } catch (err) {
            console.error("Failed to delete event:", err);
            setEventError(apiErrorMessage(err, t("events.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="⌛"
                title={t("notfound")}
                action={
                    <Link to="/storymap/timelines" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !timeline) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const events = [...timeline.events].sort(
        (a, b) => a.sortOrder - b.sortOrder
    );

    const eventFormNode = (
        <form className={s.eventForm} onSubmit={onSaveEvent}>
            <h3 className={s.eventFormTitle}>
                {eventFormFor === 0
                    ? t("events.addTitle")
                    : t("events.editTitle")}
            </h3>
            <OrnateField label={t("events.name")} required>
                <OrnateTextInput
                    value={eventForm.name}
                    display
                    maxLength={100}
                    onChange={(e) => setE("name", e.target.value)}
                />
            </OrnateField>
            <div className={s.row2}>
                <OrnateField
                    label={t("events.date")}
                    hint={t("events.dateHint")}
                >
                    <OrnateTextInput
                        value={eventForm.date}
                        maxLength={100}
                        onChange={(e) => setE("date", e.target.value)}
                    />
                </OrnateField>
                <OrnateField
                    label={t("events.sortOrder")}
                    hint={t("events.sortOrderHint")}
                >
                    <OrnateTextInput
                        type="number"
                        step="1"
                        value={eventForm.sortOrder}
                        onChange={(e) => setE("sortOrder", e.target.value)}
                    />
                </OrnateField>
            </div>
            <OrnateField label={t("events.description")}>
                <OrnateTextArea
                    value={eventForm.description}
                    rows={3}
                    maxLength={4000}
                    onChange={(e) => setE("description", e.target.value)}
                />
            </OrnateField>
            <OrnateField label={t("events.consequences")}>
                <OrnateTextArea
                    value={eventForm.consequences}
                    rows={2}
                    maxLength={2000}
                    onChange={(e) => setE("consequences", e.target.value)}
                />
            </OrnateField>
            <OrnateCheckbox
                label={t("events.major")}
                checked={eventForm.isMajorEvent}
                onChange={(e) => setE("isMajorEvent", e.target.checked)}
            />
            {eventError && (
                <p className={s.miniError} role="alert">
                    {eventError}
                </p>
            )}
            <div className={s.miniActions}>
                <Button
                    variant="ghost"
                    size="sm"
                    disabled={busy}
                    onClick={() => setEventFormFor(null)}
                >
                    {t("events.cancel")}
                </Button>
                <Button type="submit" size="sm" disabled={busy}>
                    {t("events.save")}
                </Button>
            </div>
        </form>
    );

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/timelines">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{timeline.name}</span>
            </div>
            <div className={s.headerRow}>
                <h1 className={s.name}>{timeline.name}</h1>
                <span className={s.spacer} />
                {canEdit && (
                    <>
                        <Button
                            variant="ghost"
                            onClick={() =>
                                navigate(
                                    `/storymap/timelines/${timeline.id}/edit`
                                )
                            }
                        >
                            {t("form.edit")}
                        </Button>
                        {eventFormFor === null && (
                            <Button onClick={openNewEvent}>
                                + {t("events.add")}
                            </Button>
                        )}
                    </>
                )}
            </div>
            {timeline.description && (
                <p className={s.description}>{timeline.description}</p>
            )}

            {eventFormFor === 0 && eventFormNode}

            {events.length === 0 && eventFormFor === null ? (
                <p className={s.none}>{t("events.empty")}</p>
            ) : (
                <div className={s.chronology}>
                    <div className={s.line} />
                    {events.map((ev) => (
                        <div key={ev.id} className={s.event}>
                            <span
                                className={`${s.dot} ${
                                    ev.isMajorEvent ? s.dotMajor : ""
                                }`}
                            />
                            {eventFormFor === ev.id ? (
                                eventFormNode
                            ) : (
                                <div
                                    className={`${s.card} ${
                                        ev.isMajorEvent ? s.cardMajor : ""
                                    }`}
                                >
                                    <div className={s.cardHead}>
                                        {ev.date && (
                                            <span className={s.date}>
                                                {ev.date}
                                            </span>
                                        )}
                                        <span className={s.eventTitle}>
                                            {ev.name}
                                        </span>
                                        {canEdit && (
                                            <span className={s.eventActions}>
                                                <button
                                                    type="button"
                                                    className={
                                                        s.eventActionBtn
                                                    }
                                                    disabled={busy}
                                                    onClick={() =>
                                                        openEditEvent(ev)
                                                    }
                                                >
                                                    {t("form.edit")}
                                                </button>
                                                <button
                                                    type="button"
                                                    className={`${s.eventActionBtn} ${s.eventActionDanger}`}
                                                    disabled={busy}
                                                    onClick={() =>
                                                        onDeleteEvent(ev)
                                                    }
                                                >
                                                    {t("events.delete")}
                                                </button>
                                            </span>
                                        )}
                                    </div>
                                    {ev.description && (
                                        <p className={s.eventDesc}>
                                            {ev.description}
                                        </p>
                                    )}
                                    {ev.consequences && (
                                        <p className={s.consequences}>
                                            {ev.consequences}
                                        </p>
                                    )}
                                </div>
                            )}
                        </div>
                    ))}
                </div>
            )}
            {eventError && eventFormFor === null && (
                <p className={s.miniError} role="alert">
                    {eventError}
                </p>
            )}
        </div>
    );
}
