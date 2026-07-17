import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateMultiSelect,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    CharacterDto,
    LocationDto,
    TimelineDetailsDto,
    TimelineEventDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addTimelineEvent,
    deleteTimelineEvent,
    getTimeline,
    updateTimelineEvent,
} from "../../../../api/timelines";
import { getCharacters } from "../../../../api/characters";
import { getLocations } from "../../../../api/locations";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface EventFormState {
    name: string;
    date: string;
    sortOrder: string;
    era: string;
    description: string;
    consequences: string;
    isMajorEvent: boolean;
    locationId: string;
    involvedCharacterIds: string[];
}

const emptyEventForm: EventFormState = {
    name: "",
    date: "",
    sortOrder: "",
    era: "",
    description: "",
    consequences: "",
    isMajorEvent: false,
    locationId: "",
    involvedCharacterIds: [],
};

/** Runs of this many+ consecutive minor events collapse into one cluster row. */
const CLUSTER_THRESHOLD = 3;

type TimelineRow =
    | { kind: "event"; ev: TimelineEventDto }
    | { kind: "cluster"; key: string; events: TimelineEventDto[] };

/** Split an era's events into event rows + collapsed clusters of minor runs. */
function buildRows(events: TimelineEventDto[], eraKey: string): TimelineRow[] {
    const rows: TimelineRow[] = [];
    let run: TimelineEventDto[] = [];
    const flush = () => {
        if (run.length >= CLUSTER_THRESHOLD) {
            rows.push({
                kind: "cluster",
                key: `${eraKey}-${run[0].id}`,
                events: run,
            });
        } else {
            run.forEach((ev) => rows.push({ kind: "event", ev }));
        }
        run = [];
    };
    events.forEach((ev) => {
        if (ev.isMajorEvent) {
            flush();
            rows.push({ kind: "event", ev });
        } else {
            run.push(ev);
        }
    });
    flush();
    return rows;
}

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
    // Which minor-event clusters are expanded (key = era index + run start).
    const [expandedClusters, setExpandedClusters] = useState<Set<string>>(
        new Set()
    );
    const toggleCluster = (key: string) =>
        setExpandedClusters((prev) => {
            const next = new Set(prev);
            if (next.has(key)) next.delete(key);
            else next.add(key);
            return next;
        });

    // World lookups for the event form's location + involved-character pickers.
    const [locations, setLocations] = useState<LocationDto[]>([]);
    const [characters, setCharacters] = useState<CharacterDto[]>([]);

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

    // Load world locations + characters for the event pickers (editors only).
    const worldId = timeline?.worldId;
    useEffect(() => {
        if (!canEdit || !worldId) return;
        let cancelled = false;
        Promise.all([getLocations(worldId), getCharacters(worldId)])
            .then(([locs, chars]) => {
                if (cancelled) return;
                setLocations(locs);
                setCharacters(chars);
            })
            .catch((err) =>
                console.error("Failed to load event pickers:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [canEdit, worldId]);

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
            era: ev.era ?? "",
            description: ev.description ?? "",
            consequences: ev.consequences ?? "",
            isMajorEvent: ev.isMajorEvent,
            locationId: ev.location ? String(ev.location.id) : "",
            involvedCharacterIds: ev.involvedCharacters.map((c) => String(c.id)),
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
                era: eventForm.era.trim(),
                description: eventForm.description,
                consequences: eventForm.consequences,
                isMajorEvent: eventForm.isMajorEvent,
                locationId: eventForm.locationId
                    ? Number(eventForm.locationId)
                    : null,
                involvedCharacterIds: eventForm.involvedCharacterIds.map(Number),
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

    // Group consecutive events sharing an era label (empty era = no header).
    const eraGroups: { era: string; events: TimelineEventDto[] }[] = [];
    events.forEach((ev) => {
        const era = ev.era?.trim() ?? "";
        const last = eraGroups[eraGroups.length - 1];
        if (last && last.era === era) last.events.push(ev);
        else eraGroups.push({ era, events: [ev] });
    });

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
            <OrnateField label={t("events.era")} hint={t("events.eraHint")}>
                <OrnateTextInput
                    value={eventForm.era}
                    maxLength={100}
                    onChange={(e) => setE("era", e.target.value)}
                />
            </OrnateField>
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
            <OrnateField label={t("events.location")}>
                <OrnateSelect
                    value={eventForm.locationId}
                    onChange={(e) => setE("locationId", e.target.value)}
                >
                    <option value="">{t("events.noLocation")}</option>
                    {locations.map((l) => (
                        <option key={l.id} value={l.id}>
                            {l.name}
                        </option>
                    ))}
                </OrnateSelect>
            </OrnateField>
            <OrnateField label={t("events.involved")}>
                <OrnateMultiSelect
                    value={eventForm.involvedCharacterIds}
                    onChange={(next) => setE("involvedCharacterIds", next)}
                    placeholder={t("events.involvedPlaceholder")}
                    options={characters.map((c) => ({
                        value: String(c.id),
                        label: c.name,
                    }))}
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

    const renderEvent = (ev: TimelineEventDto) => (
        <div
            key={ev.id}
            className={`${s.trow} ${ev.isMajorEvent ? s.trowMajor : ""}`}
        >
            <div className={s.tdate}>{ev.date}</div>
            <div className={s.tnode}>
                <span
                    className={`${s.node} ${ev.isMajorEvent ? s.nodeMajor : ""}`}
                >
                    {ev.isMajorEvent ? "✦" : ""}
                </span>
            </div>
            <div className={s.tcell}>
                {eventFormFor === ev.id ? (
                    eventFormNode
                ) : (
                    <div
                        className={`${s.card} ${
                            ev.isMajorEvent ? s.cardMajor : ""
                        }`}
                    >
                        <div className={s.cardHead}>
                            <span className={s.eventTitle}>{ev.name}</span>
                            {canEdit && (
                                <span className={s.eventActions}>
                                    <button
                                        type="button"
                                        className={s.eventActionBtn}
                                        disabled={busy}
                                        onClick={() => openEditEvent(ev)}
                                    >
                                        {t("form.edit")}
                                    </button>
                                    <button
                                        type="button"
                                        className={`${s.eventActionBtn} ${s.eventActionDanger}`}
                                        disabled={busy}
                                        onClick={() => onDeleteEvent(ev)}
                                    >
                                        {t("events.delete")}
                                    </button>
                                </span>
                            )}
                        </div>
                        {ev.description && (
                            <p className={s.eventDesc}>{ev.description}</p>
                        )}
                        {ev.consequences && (
                            <p className={s.consequences}>{ev.consequences}</p>
                        )}
                        {(ev.location ||
                            ev.involvedCharacters.length > 0) && (
                            <div className={s.eventChips}>
                                {ev.location && (
                                    <Link
                                        to={`/storymap/locations/${ev.location.id}`}
                                        className={`${s.eventChip} ${s.eventChipPlace}`}
                                    >
                                        <span className={s.eventChipGlyph}>
                                            ⚑
                                        </span>
                                        {ev.location.name}
                                    </Link>
                                )}
                                {ev.involvedCharacters.map((c) => (
                                    <Link
                                        key={c.id}
                                        to={`/storymap/characters/${c.id}`}
                                        className={s.eventChip}
                                    >
                                        <span className={s.eventChipGlyph}>
                                            ♟
                                        </span>
                                        {c.name}
                                    </Link>
                                ))}
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );

    // Era headers with a stable anchor index, for the era-rail navigator.
    const eraAnchors = eraGroups
        .map((g, gi) => ({ ...g, gi }))
        .filter((g) => g.era);

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
            {timeline.history && (
                <p className={s.description}>
                    {t("fields.history")}:{" "}
                    <Link to={`/storymap/histories/${timeline.history.id}`}>
                        {timeline.history.name}
                    </Link>
                </p>
            )}

            {eventFormFor === 0 && eventFormNode}

            {events.length === 0 && eventFormFor === null ? (
                <p className={s.none}>{t("events.empty")}</p>
            ) : (
                <div className={s.timelineWrap}>
                    <div className={s.timeline}>
                        {eraGroups.map((group, gi) => (
                            <div key={gi} className={s.eraGroup}>
                                {group.era && (
                                    <div
                                        id={`era-${gi}`}
                                        className={s.eraHeader}
                                    >
                                        <span className={s.eraName}>
                                            {group.era}
                                        </span>
                                        <span className={s.eraLine} />
                                        <span className={s.eraCount}>
                                            {t("events.eraEventCount", {
                                                count: group.events.length,
                                            })}
                                        </span>
                                    </div>
                                )}
                                {buildRows(group.events, String(gi)).map((row) =>
                                    row.kind === "event" ? (
                                        renderEvent(row.ev)
                                    ) : expandedClusters.has(row.key) ? (
                                        <div
                                            key={row.key}
                                            className={s.clusterOpen}
                                        >
                                            <button
                                                type="button"
                                                className={s.clusterCollapse}
                                                onClick={() =>
                                                    toggleCluster(row.key)
                                                }
                                            >
                                                {t("events.collapse")}
                                            </button>
                                            {row.events.map((ev) =>
                                                renderEvent(ev)
                                            )}
                                        </div>
                                    ) : (
                                        <div
                                            key={row.key}
                                            className={s.trow}
                                        >
                                            <div className={s.tdate} />
                                            <div className={s.tnode}>
                                                <span
                                                    className={s.clusterNode}
                                                />
                                            </div>
                                            <div className={s.tcell}>
                                                <button
                                                    type="button"
                                                    className={s.clusterRow}
                                                    onClick={() =>
                                                        toggleCluster(row.key)
                                                    }
                                                >
                                                    {t("events.cluster", {
                                                        count: row.events.length,
                                                    })}
                                                    <span
                                                        className={
                                                            s.clusterChevron
                                                        }
                                                    >
                                                        ▾
                                                    </span>
                                                </button>
                                            </div>
                                        </div>
                                    )
                                )}
                            </div>
                        ))}
                    </div>

                    {eraAnchors.length > 1 && (
                        <nav className={s.eraRail} aria-label={t("events.eras")}>
                            <div className={s.eraRailLabel}>
                                {t("events.eras")}
                            </div>
                            {eraAnchors.map((g) => (
                                <button
                                    key={g.gi}
                                    type="button"
                                    className={s.eraRailItem}
                                    onClick={() =>
                                        document
                                            .getElementById(`era-${g.gi}`)
                                            ?.scrollIntoView({
                                                behavior: "smooth",
                                                block: "start",
                                            })
                                    }
                                >
                                    <span className={s.eraRailName}>
                                        {g.era}
                                    </span>
                                    <span className={s.eraRailCount}>
                                        {g.events.length}
                                    </span>
                                </button>
                            ))}
                        </nav>
                    )}
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
