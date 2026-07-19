import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReadRefList } from "../../../linking/ReadRefList";
import {
    RaceDto,
    SpeciesDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    createRace,
    deleteRace,
    getSpeciesById,
    updateRace,
} from "../../../../api/species";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface RaceFormState {
    name: string;
    description: string;
    appearanceTraits: string;
    geneticFeatures: string;
    adaptations: string;
}

const emptyRaceForm: RaceFormState = {
    name: "",
    description: "",
    appearanceTraits: "",
    geneticFeatures: "",
    adaptations: "",
};

export default function SpeciesDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("species");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [species, setSpecies] = useState<SpeciesDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Inline forma rase: null = zatvorena, 0 = nova, >0 = edit tog id-a
    const [raceFormFor, setRaceFormFor] = useState<number | null>(null);
    const [raceForm, setRaceForm] = useState<RaceFormState>(emptyRaceForm);
    const [raceError, setRaceError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    const setR = <K extends keyof RaceFormState>(
        key: K,
        value: RaceFormState[K]
    ) => setRaceForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        const speciesId = Number(id);
        if (!speciesId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getSpeciesById(speciesId)
            .then((data) => {
                if (!cancelled) setSpecies(data);
            })
            .catch((err) => {
                console.error("Failed to load species:", err);
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

    const openNewRace = () => {
        setRaceForm(emptyRaceForm);
        setRaceError(null);
        setRaceFormFor(0);
    };

    const openEditRace = (race: RaceDto) => {
        setRaceForm({
            name: race.name ?? "",
            description: race.description ?? "",
            appearanceTraits: race.appearanceTraits ?? "",
            geneticFeatures: race.geneticFeatures ?? "",
            adaptations: race.adaptations ?? "",
        });
        setRaceError(null);
        setRaceFormFor(race.id);
    };

    async function onSaveRace(e: FormEvent) {
        e.preventDefault();
        if (!species || raceFormFor === null) return;
        if (!raceForm.name.trim()) {
            setRaceError(t("races.requiredMissing"));
            return;
        }
        setRaceError(null);
        setBusy(true);
        try {
            const payload = {
                name: raceForm.name.trim(),
                description: raceForm.description,
                appearanceTraits: raceForm.appearanceTraits,
                geneticFeatures: raceForm.geneticFeatures,
                adaptations: raceForm.adaptations,
            };
            if (raceFormFor === 0) {
                await createRace({
                    ...payload,
                    sapientSpeciesId: species.id,
                });
            } else {
                await updateRace(raceFormFor, payload);
            }
            setRaceFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save race:", err);
            setRaceError(apiErrorMessage(err, t("races.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDeleteRace(race: RaceDto) {
        if (!window.confirm(t("races.deleteConfirm", { name: race.name }))) {
            return;
        }
        setRaceError(null);
        setBusy(true);
        try {
            await deleteRace(race.id);
            refetch();
        } catch (err) {
            console.error("Failed to delete race:", err);
            setRaceError(apiErrorMessage(err, t("races.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="⚘"
                title={t("notfound")}
                action={
                    <Link to="/storymap/species" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !species) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = [
        t(`sapientTypes.${species.sapientType}`),
        species.isHumanoid ? t("humanoid") : t("notHumanoid"),
        species.lifespan || null,
    ]
        .filter(Boolean)
        .join(" · ");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/species">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{species.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kicker}</div>
                    <h1 className={s.name}>{species.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/species/${species.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.sapientType")}
                        value={t(`sapientTypes.${species.sapientType}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.lifespan")}
                        value={species.lifespan || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isHumanoid")}
                        value={
                            species.isHumanoid
                                ? t("humanoid")
                                : t("notHumanoid")
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.isSentient")}
                        value={species.isSentient ? t("yes") : t("no")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.commonName")}
                        value={species.commonName || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.scientificName")}
                        value={species.scientificName || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.averageLifespan")}
                        value={
                            species.averageLifespan
                                ? t("years", { value: species.averageLifespan })
                                : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.size")}
                        value={
                            species.height || species.weight
                                ? t("sizeValue", {
                                      height: species.height || 0,
                                      weight: species.weight || 0,
                                  })
                                : dash
                        }
                    />
                    {species.isArtificial && (
                        <OrnateDisplayBox
                            label={t("fields.artificialOrigin")}
                            value={
                                species.artificialOrigin
                                    ? t(
                                          `artificialOrigins.${species.artificialOrigin}`
                                      )
                                    : t("artificial")
                            }
                        />
                    )}
                    {species.parentCreature && (
                        <OrnateDisplayBox
                            label={t("fields.parentCreature")}
                            value={
                                <Link
                                    to={`/storymap/species/${species.parentCreature.id}`}
                                >
                                    {species.parentCreature.name}
                                </Link>
                            }
                        />
                    )}
                </DisplayGrid>
            </div>

            {species.subspecies.length > 0 && (
                <>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("subspecies.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <p className={s.prose}>
                        {species.subspecies.map((sub, i) => (
                            <span key={sub.id}>
                                {i > 0 && " · "}
                                <Link to={`/storymap/species/${sub.id}`}>
                                    {sub.name}
                                </Link>
                            </span>
                        ))}
                    </p>
                </>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("reverse.occupations")}</span>
                <span className={s.sectionLine} />
            </div>
            <ReadRefList
                items={species.frequentOccupations}
                linkTo={(id) => `/storymap/professions/${id}`}
                noneLabel={t("none")}
            />

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("reverse.cultures")}</span>
                <span className={s.sectionLine} />
            </div>
            <ReadRefList
                items={species.cultures}
                linkTo={(id) => `/storymap/cultures/${id}`}
                noneLabel={t("none")}
            />

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("reverse.folklore")}</span>
                <span className={s.sectionLine} />
            </div>
            <ReadRefList
                items={species.folklore}
                linkTo={(id) => `/storymap/folklore/${id}`}
                noneLabel={t("none")}
            />

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>
                    {t("fields.description")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {species.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {species.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("races.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && raceFormFor === null && (
                    <button
                        type="button"
                        className={s.addInline}
                        onClick={openNewRace}
                    >
                        + {t("races.add")}
                    </button>
                )}
            </div>
            {species.races.length === 0 && raceFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                species.races.map((race) => (
                    <div key={race.id} className={s.raceRow}>
                        <span className={s.raceName}>{race.name}</span>
                        <div className={s.raceBody}>
                            {race.description && (
                                <p className={s.raceDesc}>
                                    {race.description}
                                </p>
                            )}
                            {(race.appearanceTraits ||
                                race.geneticFeatures ||
                                race.adaptations) && (
                                <p className={s.raceMeta}>
                                    {[
                                        race.appearanceTraits,
                                        race.geneticFeatures,
                                        race.adaptations,
                                    ]
                                        .filter(Boolean)
                                        .join(" · ")}
                                </p>
                            )}
                        </div>
                        {canEdit && (
                            <span className={s.raceActions}>
                                <button
                                    type="button"
                                    className={s.raceActionBtn}
                                    disabled={busy}
                                    onClick={() => openEditRace(race)}
                                >
                                    {t("form.edit")}
                                </button>
                                <button
                                    type="button"
                                    className={`${s.raceActionBtn} ${s.raceActionDanger}`}
                                    disabled={busy}
                                    onClick={() => onDeleteRace(race)}
                                >
                                    {t("races.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {raceError && raceFormFor === null && (
                <p className={s.miniError} role="alert">
                    {raceError}
                </p>
            )}

            {raceFormFor !== null && (
                <form className={s.raceForm} onSubmit={onSaveRace}>
                    <h3 className={s.raceFormTitle}>
                        {raceFormFor === 0
                            ? t("races.addTitle")
                            : t("races.editTitle")}
                    </h3>
                    <OrnateField label={t("races.name")} required>
                        <OrnateTextInput
                            value={raceForm.name}
                            display
                            maxLength={100}
                            onChange={(e) => setR("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("races.description")}>
                        <OrnateTextArea
                            value={raceForm.description}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) =>
                                setR("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("races.appearanceTraits")}>
                        <OrnateTextInput
                            value={raceForm.appearanceTraits}
                            maxLength={500}
                            onChange={(e) =>
                                setR("appearanceTraits", e.target.value)
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("races.geneticFeatures")}>
                            <OrnateTextInput
                                value={raceForm.geneticFeatures}
                                maxLength={500}
                                onChange={(e) =>
                                    setR("geneticFeatures", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("races.adaptations")}>
                            <OrnateTextInput
                                value={raceForm.adaptations}
                                maxLength={500}
                                onChange={(e) =>
                                    setR("adaptations", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    {raceError && (
                        <p className={s.miniError} role="alert">
                            {raceError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setRaceFormFor(null)}
                        >
                            {t("races.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("races.save")}
                        </Button>
                    </div>
                </form>
            )}
        </div>
    );
}
