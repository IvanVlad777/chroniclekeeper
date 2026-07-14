import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    LocationDto,
    NaturalResourceDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addNaturalResourceLocation,
    getNaturalResourceById,
    removeNaturalResourceLocation,
} from "../../../../api/naturalResources";
import { getLocations } from "../../../../api/locations";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⛰";

export default function NaturalResourceDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("naturalResource");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [naturalResource, setNaturalResource] =
        useState<NaturalResourceDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [locationCandidates, setLocationCandidates] = useState<
        LocationDto[] | null
    >(null);

    useEffect(() => {
        const naturalResourceId = Number(id);
        if (!naturalResourceId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getNaturalResourceById(naturalResourceId)
            .then((data) => {
                if (!cancelled) setNaturalResource(data);
            })
            .catch((err) => {
                console.error("Failed to load natural resource:", err);
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
                    <Link to="/storymap/natural-resources" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !naturalResource) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/natural-resources">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>
                    {naturalResource.name}
                </span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {naturalResource.resourceType || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{naturalResource.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/natural-resources/${naturalResource.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={5}>
                    <OrnateDisplayBox
                        label={t("fields.quantity")}
                        value={naturalResource.quantity}
                    />
                    <OrnateDisplayBox
                        label={t("fields.marketValue")}
                        value={naturalResource.marketValue}
                    />
                    <OrnateDisplayBox
                        label={t("fields.flags")}
                        value={
                            [
                                naturalResource.isRenewable
                                    ? t("fields.isRenewable")
                                    : null,
                                naturalResource.isStrategicResource
                                    ? t("fields.isStrategicResource")
                                    : null,
                            ]
                                .filter(Boolean)
                                .join(" · ") || dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.extractionMethod")}
                        value={
                            naturalResource.extractionMethod ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/extraction-methods/${naturalResource.extractionMethod.id}`}
                                >
                                    {naturalResource.extractionMethod.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            naturalResource.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${naturalResource.history.id}`}
                                >
                                    {naturalResource.history.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {naturalResource.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {naturalResource.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Locations (owner side) ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.locations")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={naturalResource.locations}
                candidates={locationCandidates}
                onLoadCandidates={() =>
                    getLocations(naturalResource.worldId).then(
                        setLocationCandidates
                    )
                }
                onAdd={(locationId) =>
                    addNaturalResourceLocation(naturalResource.id, locationId)
                }
                onRemove={(locationId) =>
                    removeNaturalResourceLocation(naturalResource.id, locationId)
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

            {/* ----- Export routes (read-only; write side lives on TradeRoute) ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("exportRoutes.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {naturalResource.exportRoutes.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                naturalResource.exportRoutes.map((r) => (
                    <div key={r.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/trade-routes/${r.id}`}
                        >
                            {r.name}
                        </Link>
                    </div>
                ))
            )}
        </div>
    );
}
