import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TagEditor } from "../../../tagging/TagEditor";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    ecosystemLocationTypes,
    LocationDetailsDto,
    LocationType,
    SpeciesDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addRegionNativeSpecies,
    getLocation,
    removeRegionNativeSpecies,
} from "../../../../api/locations";
import { getSpecies } from "../../../../api/species";
import { useAuth } from "../../../../hooks/useAuth";
import { locationGlyphs } from "../locationGlyphs";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const isEcosystemType = (t: LocationType) =>
    (ecosystemLocationTypes as readonly LocationType[]).includes(t);

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
    const [speciesCandidates, setSpeciesCandidates] = useState<SpeciesDto[] | null>(
        null
    );

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
                    {location.type === "Continent" && (
                        <OrnateDisplayBox
                            label={t("fields.continentSpecifics")}
                            value={location.continentSpecifics || dash}
                        />
                    )}
                    {location.type === "Region" && (
                        <OrnateDisplayBox
                            label={t("fields.regionSpecifics")}
                            value={location.regionSpecifics || dash}
                        />
                    )}
                    {location.type === "District" && (
                        <OrnateDisplayBox
                            label={t("fields.districtType")}
                            value={location.districtType || dash}
                        />
                    )}
                    {location.type === "City" && (
                        <OrnateDisplayBox
                            label={t("fields.isCapital")}
                            value={location.isCapital ? t("form.yes") : t("form.no")}
                        />
                    )}
                    {(location.type === "Country" || location.type === "City") && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.governmentSystem")}
                                value={
                                    location.governmentSystem ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/government-systems/${location.governmentSystem.id}`}
                                        >
                                            {location.governmentSystem.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                            <OrnateDisplayBox
                                label={t("fields.legalSystem")}
                                value={
                                    location.legalSystem ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/legal-systems/${location.legalSystem.id}`}
                                        >
                                            {location.legalSystem.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                            <OrnateDisplayBox
                                label={t("fields.educationSystem")}
                                value={
                                    location.educationSystem ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/education-systems/${location.educationSystem.id}`}
                                        >
                                            {location.educationSystem.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                            <OrnateDisplayBox
                                label={t("fields.economicSystem")}
                                value={
                                    location.economicSystem ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/economic-systems/${location.economicSystem.id}`}
                                        >
                                            {location.economicSystem.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                        </>
                    )}
                    {(location.type === "Lake" ||
                        location.type === "Sea" ||
                        location.type === "Ocean" ||
                        location.type === "River") && (
                        <OrnateDisplayBox
                            label={t("fields.waterDepth")}
                            value={location.waterDepth != null ? location.waterDepth.toLocaleString() : dash}
                        />
                    )}
                    {location.type === "Lake" && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.volume")}
                                value={location.volume != null ? location.volume.toLocaleString() : dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.maxDepth")}
                                value={location.maxDepth != null ? location.maxDepth.toLocaleString() : dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.isFreshwater")}
                                value={location.isFreshwater ? t("form.yes") : t("form.no")}
                            />
                        </>
                    )}
                    {location.type === "River" && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.riverLength")}
                                value={location.riverLength != null ? location.riverLength.toLocaleString() : dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.sourceLocation")}
                                value={
                                    location.sourceLocation ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/locations/${location.sourceLocation.id}`}
                                        >
                                            {location.sourceLocation.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                            <OrnateDisplayBox
                                label={t("fields.mouthLocation")}
                                value={
                                    location.mouthLocation ? (
                                        <Link
                                            className={s.parentLink}
                                            to={`/storymap/locations/${location.mouthLocation.id}`}
                                        >
                                            {location.mouthLocation.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                        </>
                    )}
                    {location.type === "Mountain" && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.maxElevation")}
                                value={location.maxElevation != null ? location.maxElevation.toLocaleString() : dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.prominence")}
                                value={location.prominence != null ? location.prominence.toLocaleString() : dash}
                            />
                        </>
                    )}
                    {location.type === "MountainRange" && (
                        <OrnateDisplayBox
                            label={t("fields.mountainRangeLength")}
                            value={
                                location.mountainRangeLength != null
                                    ? location.mountainRangeLength.toLocaleString()
                                    : dash
                            }
                        />
                    )}
                    {location.type === "Swamp" && (
                        <OrnateDisplayBox
                            label={t("fields.isSaltwater")}
                            value={location.isSaltwater ? t("form.yes") : t("form.no")}
                        />
                    )}
                    {location.type === "Desert" && (
                        <OrnateDisplayBox
                            label={t("fields.desertKind")}
                            value={location.desertKind ? t(`desertKinds.${location.desertKind}`) : dash}
                        />
                    )}
                    {location.type === "Forest" && (
                        <OrnateDisplayBox
                            label={t("fields.forestKind")}
                            value={location.forestKind ? t(`forestKinds.${location.forestKind}`) : dash}
                        />
                    )}
                    {location.type === "Cave" && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.caveDepth")}
                                value={location.caveDepth != null ? location.caveDepth.toLocaleString() : dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.caveKind")}
                                value={location.caveKind ? t(`caveKinds.${location.caveKind}`) : dash}
                            />
                        </>
                    )}
                    {location.type === "Grassland" && (
                        <OrnateDisplayBox
                            label={t("fields.grasslandKind")}
                            value={location.grasslandKind ? t(`grasslandKinds.${location.grasslandKind}`) : dash}
                        />
                    )}
                </DisplayGrid>
                {isEcosystemType(location.type) && location.uniqueFeatures && (
                    <p className={`${s.prose} ${s.muted}`}>
                        {t("fields.uniqueFeatures")}: {location.uniqueFeatures}
                    </p>
                )}
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
                    <div>
                        <div className={s.listLabel}>{t("schoolsHere")}</div>
                        {location.schools.length === 0 ? (
                            <p className={s.none}>{t("none")}</p>
                        ) : (
                            location.schools.map((school) => (
                                <Link
                                    key={school.id}
                                    to={`/storymap/schools/${school.id}`}
                                    className={s.listRow}
                                >
                                    <span className={s.listGlyph}>✎</span>
                                    <span className={s.listName}>
                                        {school.name}
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
                    <HistoryBlock
                        targetType="Location"
                        targetId={location.id}
                        worldId={location.worldId}
                        history={location.history}
                        canEdit={canEdit}
                        onChanged={refetch}
                    />
                </div>
            </div>

            {location.type === "Region" && (
                <>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>{t("links.nativeSpecies")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    <LinkEditor
                        items={location.nativeSpecies}
                        candidates={speciesCandidates}
                        onLoadCandidates={() =>
                            getSpecies(location.worldId).then(setSpeciesCandidates)
                        }
                        onAdd={(speciesId) =>
                            addRegionNativeSpecies(location.id, speciesId)
                        }
                        onRemove={(speciesId) =>
                            removeRegionNativeSpecies(location.id, speciesId)
                        }
                        onChanged={refetch}
                        canEdit={canEdit}
                        linkTo={(speciesId) => `/storymap/species/${speciesId}`}
                        addLabel={t("links.add")}
                        noneLabel={t("none")}
                        pickLabel={t("links.pick")}
                        cancelLabel={t("form.cancel")}
                        confirmLabel={t("links.confirm")}
                        removeLabel={(name) => t("links.remove", { name })}
                        addFailedLabel={t("links.addFailed")}
                        removeFailedLabel={t("links.removeFailed")}
                    />
                </>
            )}
        </div>
    );
}
