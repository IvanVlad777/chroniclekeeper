import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    CreatureDetailsDto,
    LocationDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addCreatureCity,
    getCreatureById,
    removeCreatureCity,
} from "../../../../api/creatures";
import { getLocations } from "../../../../api/locations";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "🐾";

export default function CreatureDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("creature");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [creature, setCreature] = useState<CreatureDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [cityCandidates, setCityCandidates] = useState<LocationDto[] | null>(null);

    useEffect(() => {
        const creatureId = Number(id);
        if (!creatureId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getCreatureById(creatureId)
            .then((data) => {
                if (!cancelled) setCreature(data);
            })
            .catch((err) => {
                console.error("Failed to load creature:", err);
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
                    <Link to="/storymap/creatures" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !creature) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const isPlantFamily =
        creature.subtype === "Plant" ||
        creature.subtype === "Tree" ||
        creature.subtype === "Crop";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/creatures">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{creature.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {t(`subtypes.${creature.subtype}`)}
                    </div>
                    <h1 className={s.name}>{creature.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/creatures/${creature.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("columns.type")}
                        value={t(`types.${creature.type}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.averageLifespan")}
                        value={creature.averageLifespan || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.height")}
                        value={creature.height || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.weight")}
                        value={creature.weight || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isSentient")}
                        value={creature.isSentient ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isArtificial")}
                        value={
                            creature.isArtificial
                                ? creature.artificialOrigin
                                    ? t(`artificialOrigins.${creature.artificialOrigin}`)
                                    : "✓"
                                : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.parentCreature")}
                        value={
                            creature.parentCreature ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/creatures/${creature.parentCreature.id}`}
                                >
                                    {creature.parentCreature.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.history")}
                        value={
                            creature.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${creature.history.id}`}
                                >
                                    {creature.history.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />

                    {creature.subtype === "Animal" && (
                        <>
                            <OrnateDisplayBox label={t("fields.diet")} value={creature.diet ? t(`dietTypes.${creature.diet}`) : dash} />
                            <OrnateDisplayBox label={t("fields.numberOfLegs")} value={creature.numberOfLegs ?? dash} />
                            <OrnateDisplayBox label={t("fields.isDomesticated")} value={creature.isDomesticated ? "✓" : dash} />
                            <OrnateDisplayBox label={t("fields.hasWings")} value={creature.hasWings ? "✓" : dash} />
                            <OrnateDisplayBox label={t("fields.isPackAnimal")} value={creature.isPackAnimal ? "✓" : dash} />
                            <OrnateDisplayBox label={t("fields.isAggressive")} value={creature.isAggressive ? "✓" : dash} />
                        </>
                    )}

                    {isPlantFamily && (
                        <>
                            <OrnateDisplayBox label={t("fields.plantType")} value={creature.plantType ? t(`plantTypes.${creature.plantType}`) : dash} />
                            <OrnateDisplayBox label={t("fields.scientificName")} value={creature.scientificName || dash} />
                            <OrnateDisplayBox label={t("fields.sunlight")} value={creature.sunlight ? t(`sunlightRequirements.${creature.sunlight}`) : dash} />
                            <OrnateDisplayBox label={t("fields.preferredSoil")} value={creature.preferredSoil ? t(`soilTypes.${creature.preferredSoil}`) : dash} />
                            <OrnateDisplayBox label={t("fields.rarity")} value={creature.rarity ? t(`rarities.${creature.rarity}`) : dash} />
                            {creature.subtype === "Tree" && (
                                <>
                                    <OrnateDisplayBox label={t("fields.maxHeight")} value={creature.maxHeight ?? dash} />
                                    <OrnateDisplayBox label={t("fields.lifespan")} value={creature.lifespan ?? dash} />
                                    <OrnateDisplayBox label={t("fields.leafType")} value={creature.leafType ? t(`leafTypes.${creature.leafType}`) : dash} />
                                </>
                            )}
                            {creature.subtype === "Crop" && (
                                <>
                                    <OrnateDisplayBox label={t("fields.yieldPerHectare")} value={creature.yieldPerHectare ?? dash} />
                                    <OrnateDisplayBox label={t("fields.cropType")} value={creature.cropType ? t(`cropTypes.${creature.cropType}`) : dash} />
                                    <OrnateDisplayBox label={t("fields.isDomesticated")} value={creature.isDomesticated ? "✓" : dash} />
                                </>
                            )}
                        </>
                    )}

                    {creature.subtype === "Fungus" && (
                        <>
                            <OrnateDisplayBox label={t("fields.scientificName")} value={creature.scientificName || dash} />
                            <OrnateDisplayBox label={t("fields.isEdible")} value={creature.isEdible ? "✓" : dash} />
                            <OrnateDisplayBox label={t("fields.isHallucinogenic")} value={creature.isHallucinogenic ? "✓" : dash} />
                            <OrnateDisplayBox label={t("fields.canCommunicate")} value={creature.canCommunicate ? "✓" : dash} />
                        </>
                    )}
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {creature.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{creature.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {creature.subspecies.length > 0 && (
                <>
                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("links.subspecies")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    <div className={s.chipRow}>
                        {creature.subspecies.map((sub) => (
                            <Link
                                key={sub.id}
                                to={`/storymap/creatures/${sub.id}`}
                                className={s.chipLink}
                            >
                                {sub.name}
                            </Link>
                        ))}
                    </div>
                </>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.cities")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={creature.cities}
                candidates={
                    cityCandidates
                        ? cityCandidates
                              .filter((l) => l.type === "City")
                              .map((l) => ({ id: l.id, name: l.name }))
                        : null
                }
                onLoadCandidates={() =>
                    getLocations(creature.worldId).then(setCityCandidates)
                }
                onAdd={(cityId) => addCreatureCity(creature.id, cityId)}
                onRemove={(cityId) => removeCreatureCity(creature.id, cityId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cityId) => `/storymap/locations/${cityId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />
        </div>
    );
}
