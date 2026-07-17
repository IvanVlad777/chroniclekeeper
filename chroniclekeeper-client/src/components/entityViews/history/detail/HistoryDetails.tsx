import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    HistoryDetailsDto,
    HistoryLinkDto,
} from "../../../../interfaces/loreInterfaces";
import { getHistoryById } from "../../../../api/histories";
import { createTimeline } from "../../../../api/timelines";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⌛";

/** Detail route per linked-entity type discriminator. */
const linkRoutes: Record<string, string> = {
    Character: "/storymap/characters",
    Location: "/storymap/locations",
    Faction: "/storymap/factions",
    Nation: "/storymap/nations",
    ClimateZone: "/storymap/climate-zones",
    ClimateDetail: "/storymap/climate-details",
    Season: "/storymap/seasons",
    Creature: "/storymap/creatures",
    EconomicSystem: "/storymap/economic-systems",
    Currency: "/storymap/currencies",
    BankingSystem: "/storymap/banking-systems",
    TaxationSystem: "/storymap/taxation-systems",
    TradeRoute: "/storymap/trade-routes",
    NaturalResource: "/storymap/natural-resources",
    ExtractionMethod: "/storymap/extraction-methods",
    Industry: "/storymap/industries",
    Guild: "/storymap/guilds",
    Corporation: "/storymap/corporations",
};

function linkTarget(link: HistoryLinkDto): string | null {
    const base = linkRoutes[link.type];
    return base ? `${base}/${link.id}` : null;
}

export default function HistoryDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("history");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [history, setHistory] = useState<HistoryDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Inline "Add timeline"
    const [addingTimeline, setAddingTimeline] = useState(false);
    const [newTimelineName, setNewTimelineName] = useState("");
    const [addError, setAddError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    useEffect(() => {
        const historyId = Number(id);
        if (!historyId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getHistoryById(historyId)
            .then((data) => {
                if (!cancelled) setHistory(data);
            })
            .catch((err) => {
                console.error("Failed to load history:", err);
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

    async function onAddTimeline(e: FormEvent) {
        e.preventDefault();
        if (!history || !newTimelineName.trim()) {
            setAddError(t("timelines.nameRequired"));
            return;
        }
        setAddError(null);
        setBusy(true);
        try {
            const created = await createTimeline({
                name: newTimelineName.trim(),
                description: "",
                worldId: history.worldId,
                historyId: history.id,
            });
            setAddingTimeline(false);
            setNewTimelineName("");
            navigate(`/storymap/timelines/${created.id}`);
        } catch (err) {
            console.error("Failed to create timeline:", err);
            setAddError(apiErrorMessage(err, t("timelines.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link to="/storymap/histories" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !history) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/histories">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{history.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{history.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/histories/${history.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("columns.status")}
                        value={
                            history.isOfficial
                                ? t("official")
                                : t("unofficial")
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.summary")}</span>
                <span className={s.sectionLine} />
            </div>
            {history.summary ? (
                <p className={`${s.prose} ${s.dropCap}`}>{history.summary}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>
                    {t("fields.description")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {history.description ? (
                <p className={s.prose}>{history.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* Linked entities — who this history belongs to (reverse links). */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("linked.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {history.linkedEntities.length === 0 ? (
                <p className={s.none}>{t("linked.none")}</p>
            ) : (
                <div className={s.linkRow}>
                    {history.linkedEntities.map((link) => {
                        const to = linkTarget(link);
                        const body = (
                            <>
                                <span className={s.linkType}>
                                    {t(`linked.types.${link.type}`)}
                                </span>
                                <span className={s.linkName}>{link.name}</span>
                            </>
                        );
                        return to ? (
                            <Link
                                key={`${link.type}-${link.id}`}
                                to={to}
                                className={s.linkChip}
                            >
                                {body}
                            </Link>
                        ) : (
                            <span
                                key={`${link.type}-${link.id}`}
                                className={s.linkChip}
                            >
                                {body}
                            </span>
                        );
                    })}
                </div>
            )}

            {/* Timelines — the history's children, as hub cards. */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("timelines.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && !addingTimeline && (
                    <Button size="sm" onClick={() => setAddingTimeline(true)}>
                        + {t("timelines.add")}
                    </Button>
                )}
            </div>

            {addingTimeline && (
                <form className={s.addTimeline} onSubmit={onAddTimeline}>
                    <OrnateField label={t("timelines.newName")} required>
                        <OrnateTextInput
                            value={newTimelineName}
                            display
                            maxLength={100}
                            onChange={(e) => setNewTimelineName(e.target.value)}
                        />
                    </OrnateField>
                    {addError && (
                        <p className={s.miniError} role="alert">
                            {addError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => {
                                setAddingTimeline(false);
                                setNewTimelineName("");
                                setAddError(null);
                            }}
                        >
                            {t("timelines.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("timelines.create")}
                        </Button>
                    </div>
                </form>
            )}

            {history.timelines.length === 0 && !addingTimeline ? (
                <p className={s.none}>{t("timelines.empty")}</p>
            ) : (
                <div className={s.timelineGrid}>
                    {history.timelines.map((tl) => {
                        const span = [tl.firstDate, tl.lastDate]
                            .filter(Boolean)
                            .join(" – ");
                        return (
                            <Link
                                key={tl.id}
                                to={`/storymap/timelines/${tl.id}`}
                                className={s.timelineCard}
                            >
                                <span className={s.timelineCardName}>
                                    {tl.name}
                                </span>
                                {span && (
                                    <span className={s.timelineCardSpan}>
                                        {span}
                                    </span>
                                )}
                                <span className={s.timelineCardMeta}>
                                    <span>
                                        {t("timelines.eventCount", {
                                            count: tl.eventCount,
                                        })}
                                    </span>
                                    {tl.majorEventCount > 0 && (
                                        <span className={s.timelineCardMajor}>
                                            ✦{" "}
                                            {t("timelines.majorCount", {
                                                count: tl.majorEventCount,
                                            })}
                                        </span>
                                    )}
                                </span>
                            </Link>
                        );
                    })}
                </div>
            )}
        </div>
    );
}
