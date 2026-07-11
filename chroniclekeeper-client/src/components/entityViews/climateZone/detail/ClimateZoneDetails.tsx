import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    ClimateDetailDto,
    ClimateZoneDetailsDto,
    HistoryDto,
    LocationDto,
    SeasonDto,
    WeatherEffect,
    WeatherFrequency,
    WeatherPatternType,
    weatherFrequencies,
    weatherEffects,
    weatherPatternTypes,
} from "../../../../interfaces/loreInterfaces";
import {
    addClimateZoneDetail,
    addClimateZoneLocation,
    addClimateZoneSeason,
    createWeatherPattern,
    deleteWeatherPattern,
    getClimateZoneById,
    removeClimateZoneDetail,
    removeClimateZoneLocation,
    removeClimateZoneSeason,
    updateWeatherPattern,
} from "../../../../api/climateZones";
import { getClimateDetails } from "../../../../api/climateDetails";
import { getSeasons } from "../../../../api/seasons";
import { getLocations } from "../../../../api/locations";
import { getHistories } from "../../../../api/histories";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "☁";

interface WeatherPatternFormState {
    name: string;
    description: string;
    patternType: WeatherPatternType;
    frequency: WeatherFrequency;
    effects: WeatherEffect;
    historyId: string;
}
const emptyWeatherPatternForm: WeatherPatternFormState = {
    name: "",
    description: "",
    patternType: "Normal",
    frequency: "Seasonal",
    effects: "None",
    historyId: "",
};

export default function ClimateZoneDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("climateZone");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [climateZone, setClimateZone] = useState<ClimateZoneDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [historiesForForm, setHistoriesForForm] = useState<HistoryDto[]>([]);

    const [detailCandidates, setDetailCandidates] = useState<
        ClimateDetailDto[] | null
    >(null);
    const [seasonCandidates, setSeasonCandidates] = useState<SeasonDto[] | null>(
        null
    );
    const [locationCandidates, setLocationCandidates] = useState<
        LocationDto[] | null
    >(null);

    // Vremenski obrazac: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [weatherFormFor, setWeatherFormFor] = useState<number | null>(null);
    const [weatherForm, setWeatherForm] = useState<WeatherPatternFormState>(
        emptyWeatherPatternForm
    );
    const [weatherError, setWeatherError] = useState<string | null>(null);

    useEffect(() => {
        const climateZoneId = Number(id);
        if (!climateZoneId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getClimateZoneById(climateZoneId)
            .then((data) => {
                if (cancelled) return;
                setClimateZone(data);
                return getHistories(data.worldId);
            })
            .then((historiesData) => {
                if (!cancelled && historiesData) setHistoriesForForm(historiesData);
            })
            .catch((err) => {
                console.error("Failed to load climate zone:", err);
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

    // ----- Weather patterns -----

    const openNewWeatherPattern = () => {
        setWeatherForm(emptyWeatherPatternForm);
        setWeatherError(null);
        setWeatherFormFor(0);
    };
    const openEditWeatherPattern = (
        w: ClimateZoneDetailsDto["weatherPatterns"][number]
    ) => {
        setWeatherForm({
            name: w.name ?? "",
            description: w.description ?? "",
            patternType: w.patternType,
            frequency: w.frequency,
            effects: w.effects,
            historyId: w.historyId ? String(w.historyId) : "",
        });
        setWeatherError(null);
        setWeatherFormFor(w.id);
    };
    async function onSaveWeatherPattern(e: FormEvent) {
        e.preventDefault();
        if (!climateZone || weatherFormFor === null) return;
        if (!weatherForm.name.trim()) {
            setWeatherError(t("weatherPatterns.requiredMissing"));
            return;
        }
        setWeatherError(null);
        setBusy(true);
        try {
            const payload = {
                name: weatherForm.name.trim(),
                description: weatherForm.description,
                patternType: weatherForm.patternType,
                frequency: weatherForm.frequency,
                effects: weatherForm.effects,
                historyId: weatherForm.historyId
                    ? Number(weatherForm.historyId)
                    : null,
            };
            if (weatherFormFor === 0) {
                await createWeatherPattern({
                    ...payload,
                    climateZoneId: climateZone.id,
                });
            } else {
                await updateWeatherPattern(weatherFormFor, payload);
            }
            setWeatherFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save weather pattern:", err);
            setWeatherError(
                apiErrorMessage(err, t("weatherPatterns.saveFailed"))
            );
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteWeatherPattern(wId: number, name: string) {
        if (!window.confirm(t("weatherPatterns.deleteConfirm", { name })))
            return;
        setWeatherError(null);
        setBusy(true);
        try {
            await deleteWeatherPattern(wId);
            refetch();
        } catch (err) {
            console.error("Failed to delete weather pattern:", err);
            setWeatherError(
                apiErrorMessage(err, t("weatherPatterns.deleteFailed"))
            );
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
                    <Link to="/storymap/climate-zones" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !climateZone) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/climate-zones">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{climateZone.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{t(`zoneTypes.${climateZone.zoneType}`)}</div>
                    <h1 className={s.name}>{climateZone.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/climate-zones/${climateZone.id}/edit`
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
                        label={t("fields.averageTemperature")}
                        value={`${climateZone.averageTemperature}°C`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.averageHumidity")}
                        value={`${climateZone.averageHumidity}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.averagePrecipitation")}
                        value={`${climateZone.averagePrecipitation} mm`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.hasDistinctSeasons")}
                        value={climateZone.hasDistinctSeasons ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            climateZone.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${climateZone.history.id}`}
                                >
                                    {climateZone.history.name}
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
            {climateZone.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {climateZone.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Weather patterns ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("weatherPatterns.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && weatherFormFor === null && (
                    <button
                        type="button"
                        className={s.addInline}
                        onClick={openNewWeatherPattern}
                    >
                        + {t("weatherPatterns.add")}
                    </button>
                )}
            </div>
            {climateZone.weatherPatterns.length === 0 && weatherFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                climateZone.weatherPatterns.map((w) => (
                    <div key={w.id} className={s.childRow}>
                        <span className={s.childName}>{w.name}</span>
                        <div className={s.childBody}>
                            {w.description && (
                                <p className={s.childDesc}>{w.description}</p>
                            )}
                            <p className={s.childMeta}>
                                {[
                                    t(`weatherPatternTypes.${w.patternType}`),
                                    t(`weatherFrequencies.${w.frequency}`),
                                    t(`weatherEffects.${w.effects}`),
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button
                                    type="button"
                                    className={s.childActionBtn}
                                    disabled={busy}
                                    onClick={() => openEditWeatherPattern(w)}
                                >
                                    {t("form.edit")}
                                </button>
                                <button
                                    type="button"
                                    className={`${s.childActionBtn} ${s.childActionDanger}`}
                                    disabled={busy}
                                    onClick={() =>
                                        onDeleteWeatherPattern(w.id, w.name)
                                    }
                                >
                                    {t("weatherPatterns.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {weatherError && weatherFormFor === null && (
                <p className={s.miniError} role="alert">
                    {weatherError}
                </p>
            )}
            {weatherFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveWeatherPattern}>
                    <h3 className={s.childFormTitle}>
                        {weatherFormFor === 0
                            ? t("weatherPatterns.addTitle")
                            : t("weatherPatterns.editTitle")}
                    </h3>
                    <OrnateField label={t("weatherPatterns.name")} required>
                        <OrnateTextInput
                            value={weatherForm.name}
                            display
                            maxLength={100}
                            onChange={(e) =>
                                setWeatherForm((f) => ({
                                    ...f,
                                    name: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("weatherPatterns.description")}>
                        <OrnateTextArea
                            value={weatherForm.description}
                            rows={2}
                            maxLength={4000}
                            onChange={(e) =>
                                setWeatherForm((f) => ({
                                    ...f,
                                    description: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("weatherPatterns.patternType")}>
                            <OrnateSelect
                                value={weatherForm.patternType}
                                onChange={(e) =>
                                    setWeatherForm((f) => ({
                                        ...f,
                                        patternType: e.target
                                            .value as WeatherPatternType,
                                    }))
                                }
                            >
                                {weatherPatternTypes.map((type) => (
                                    <option key={type} value={type}>
                                        {t(`weatherPatternTypes.${type}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("weatherPatterns.frequency")}>
                            <OrnateSelect
                                value={weatherForm.frequency}
                                onChange={(e) =>
                                    setWeatherForm((f) => ({
                                        ...f,
                                        frequency: e.target.value as WeatherFrequency,
                                    }))
                                }
                            >
                                {weatherFrequencies.map((freq) => (
                                    <option key={freq} value={freq}>
                                        {t(`weatherFrequencies.${freq}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateField label={t("weatherPatterns.effects")}>
                        <OrnateSelect
                            value={weatherForm.effects}
                            onChange={(e) =>
                                setWeatherForm((f) => ({
                                    ...f,
                                    effects: e.target.value as WeatherEffect,
                                }))
                            }
                        >
                            {weatherEffects.map((effect) => (
                                <option key={effect} value={effect}>
                                    {t(`weatherEffects.${effect}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect
                            value={weatherForm.historyId}
                            onChange={(e) =>
                                setWeatherForm((f) => ({
                                    ...f,
                                    historyId: e.target.value,
                                }))
                            }
                        >
                            <option value="">{t("none")}</option>
                            {historiesForForm.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    {weatherError && (
                        <p className={s.miniError} role="alert">
                            {weatherError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setWeatherFormFor(null)}
                        >
                            {t("weatherPatterns.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("weatherPatterns.save")}
                        </Button>
                    </div>
                </form>
            )}

            {/* ----- Cross-links ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.climates")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={climateZone.climates}
                candidates={detailCandidates}
                onLoadCandidates={() =>
                    getClimateDetails(climateZone.worldId).then(setDetailCandidates)
                }
                onAdd={(detailId) =>
                    addClimateZoneDetail(climateZone.id, detailId)
                }
                onRemove={(detailId) =>
                    removeClimateZoneDetail(climateZone.id, detailId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(detailId) => `/storymap/climate-details/${detailId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.seasons")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={climateZone.seasons}
                candidates={seasonCandidates}
                onLoadCandidates={() =>
                    getSeasons(climateZone.worldId).then(setSeasonCandidates)
                }
                onAdd={(seasonId) => addClimateZoneSeason(climateZone.id, seasonId)}
                onRemove={(seasonId) =>
                    removeClimateZoneSeason(climateZone.id, seasonId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(seasonId) => `/storymap/seasons/${seasonId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.locations")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={climateZone.locations}
                candidates={locationCandidates}
                onLoadCandidates={() =>
                    getLocations(climateZone.worldId).then(setLocationCandidates)
                }
                onAdd={(locationId) =>
                    addClimateZoneLocation(climateZone.id, locationId)
                }
                onRemove={(locationId) =>
                    removeClimateZoneLocation(climateZone.id, locationId)
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
        </div>
    );
}
