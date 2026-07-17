import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    LocationDto,
    NaturalResourceDto,
    TradeRouteDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addTradeRouteLocation,
    addTradeRouteResource,
    getTradeRouteById,
    removeTradeRouteLocation,
    removeTradeRouteResource,
} from "../../../../api/tradeRoutes";
import { getLocations } from "../../../../api/locations";
import { getNaturalResources } from "../../../../api/naturalResources";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⛵";

export default function TradeRouteDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("tradeRoute");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [tradeRoute, setTradeRoute] = useState<TradeRouteDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [locationCandidates, setLocationCandidates] = useState<
        LocationDto[] | null
    >(null);
    const [resourceCandidates, setResourceCandidates] = useState<
        NaturalResourceDto[] | null
    >(null);

    useEffect(() => {
        const tradeRouteId = Number(id);
        if (!tradeRouteId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getTradeRouteById(tradeRouteId)
            .then((data) => {
                if (!cancelled) setTradeRoute(data);
            })
            .catch((err) => {
                console.error("Failed to load trade route:", err);
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

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link to="/storymap/trade-routes" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !tradeRoute) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/trade-routes">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{tradeRoute.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {tradeRoute.routeType || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{tradeRoute.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/trade-routes/${tradeRoute.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.routeType")}
                        value={tradeRoute.routeType || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.mainGoods")}
                        value={tradeRoute.mainGoods || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {tradeRoute.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {tradeRoute.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Waypoint locations ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.locations")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={tradeRoute.locations}
                candidates={locationCandidates}
                onLoadCandidates={() =>
                    getLocations(tradeRoute.worldId).then(setLocationCandidates)
                }
                onAdd={(locationId) =>
                    addTradeRouteLocation(tradeRoute.id, locationId)
                }
                onRemove={(locationId) =>
                    removeTradeRouteLocation(tradeRoute.id, locationId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(locationId) => `/storymap/locations/${locationId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            {/* ----- Resources traded ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.resources")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={tradeRoute.resourcesTraded}
                candidates={resourceCandidates}
                onLoadCandidates={() =>
                    getNaturalResources(tradeRoute.worldId).then(
                        setResourceCandidates
                    )
                }
                onAdd={(resourceId) =>
                    addTradeRouteResource(tradeRoute.id, resourceId)
                }
                onRemove={(resourceId) =>
                    removeTradeRouteResource(tradeRoute.id, resourceId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(resourceId) =>
                    `/storymap/natural-resources/${resourceId}`
                }
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <HistoryBlock
                targetType="TradeRoute"
                targetId={tradeRoute.id}
                worldId={tradeRoute.worldId}
                history={tradeRoute.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
