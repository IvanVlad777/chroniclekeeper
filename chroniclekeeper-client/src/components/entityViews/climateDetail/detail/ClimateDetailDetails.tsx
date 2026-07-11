import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ClimateDetailDto } from "../../../../interfaces/loreInterfaces";
import { getClimateDetailById } from "../../../../api/climateDetails";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "☂";

export default function ClimateDetailDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("climateDetail");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [climateDetail, setClimateDetail] = useState<ClimateDetailDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const climateDetailId = Number(id);
        if (!climateDetailId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getClimateDetailById(climateDetailId)
            .then((data) => {
                if (!cancelled) setClimateDetail(data);
            })
            .catch((err) => {
                console.error("Failed to load climate detail:", err);
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
                    <Link to="/storymap/climate-details" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !climateDetail) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/climate-details">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{climateDetail.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{climateDetail.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/climate-details/${climateDetail.id}/edit`
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
                        value={`${climateDetail.averageTemperature}°C`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.humidity")}
                        value={`${climateDetail.humidity}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.precipitation")}
                        value={`${climateDetail.precipitation} mm`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.windSpeed")}
                        value={`${climateDetail.windSpeed} km/h`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.windDirection")}
                        value={t(`windDirections.${climateDetail.windDirection}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.notableWeatherPhenomena")}
                        value={t(
                            `notableWeatherPhenomena.${climateDetail.notableWeatherPhenomena}`
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isExtremeClimate")}
                        value={climateDetail.isExtremeClimate ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {climateDetail.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {climateDetail.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
