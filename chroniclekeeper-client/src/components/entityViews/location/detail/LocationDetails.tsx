import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TagEditor } from "../../../tagging/TagEditor";
import { LocationDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getLocation } from "../../../../api/locations";
import { useAuth } from "../../../../hooks/useAuth";
import { locationGlyphs } from "../locationGlyphs";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function LocationDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("location");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [location, setLocation] = useState<LocationDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const locationId = Number(id);
        if (!locationId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getLocation(locationId)
            .then((data) => {
                if (!cancelled) setLocation(data);
            })
            .catch((err) => {
                console.error("Failed to load location:", err);
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
                glyph="⚑"
                title={t("notfound")}
                action={
                    <Link to="/storymap/locations" className={s.parentLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !location) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const coordinates =
        location.latitude != null && location.longitude != null
            ? `${location.latitude}, ${location.longitude}`
            : dash;

    return (
        <div className={s.page}>
            <div className={s.banner}>
                <div className={s.bannerOverlay} />
                <div className={s.breadcrumb}>
                    <Link to="/storymap/locations">{t("listTitle")}</Link>
                    <span className={s.breadcrumbSep}>/</span>
                    <span className={s.breadcrumbCurrent}>
                        {location.name}
                    </span>
                </div>
                {canEdit && (
                    <div className={s.bannerActions}>
                        <Button
                            variant="ghost"
                            onClick={() =>
                                navigate(
                                    `/storymap/locations/${location.id}/edit`
                                )
                            }
                        >
                            {t("form.edit")}
                        </Button>
                    </div>
                )}
                <div className={s.bannerContent}>
                    <div className={s.kicker}>
                        {locationGlyphs[location.type]}{" "}
                        {t(`types.${location.type}`)}
                    </div>
                    <h1 className={s.name}>{location.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={5}>
                    <OrnateDisplayBox
                        label={t("fields.type")}
                        value={t(`types.${location.type}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.area")}
                        value={
                            location.area != null
                                ? location.area.toLocaleString()
                                : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.population")}
                        value={
                            location.population != null
                                ? location.population.toLocaleString()
                                : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.coordinates")}
                        value={coordinates}
                    />
                    <OrnateDisplayBox
                        label={t("fields.parent")}
                        value={
                            location.parentLocation ? (
                                <Link
                                    className={s.parentLink}
                                    to={`/storymap/locations/${location.parentLocation.id}`}
                                >
                                    {location.parentLocation.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.history")}
                        value={
                            location.history ? (
                                <Link
                                    className={s.parentLink}
                                    to={`/storymap/histories/${location.history.id}`}
                                >
                                    {location.history.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.body}>
                <div>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("fields.description")}
                        </span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                    </div>
                    {location.description ? (
                        <p className={`${s.prose} ${s.dropCap}`}>
                            {location.description}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}
                </div>

                <div className={s.side}>
                    <div>
                        <div className={s.listLabel}>{t("subLocations")}</div>
                        {location.subLocations.length === 0 ? (
                            <p className={s.none}>{t("none")}</p>
                        ) : (
                            location.subLocations.map((sub) => (
                                <Link
                                    key={sub.id}
                                    to={`/storymap/locations/${sub.id}`}
                                    className={s.listRow}
                                >
                                    <span className={s.listGlyph}>⌂</span>
                                    <span className={s.listName}>
                                        {sub.name}
                                    </span>
                                </Link>
                            ))
                        )}
                    </div>
                    <TagEditor
                        worldId={location.worldId}
                        targetType="Location"
                        targetId={location.id}
                        tags={location.tags}
                        canEdit={canEdit}
                        onChanged={refetch}
                        showLabel
                    />
                </div>
            </div>
        </div>
    );
}
