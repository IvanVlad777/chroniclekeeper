import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReferencesSection } from "../ReferencesSection";
import { EpisodeDto } from "../../../../interfaces/loreInterfaces";
import { getContentById, getEpisodeById } from "../../../../api/contents";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "📖";

export default function EpisodeDetails() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("content");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [episode, setEpisode] = useState<EpisodeDto | null>(null);
    const [seriesName, setSeriesName] = useState<string>("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const episodeId = Number(id);
        if (!episodeId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getEpisodeById(episodeId)
            .then((data) => {
                if (cancelled) return;
                setEpisode(data);
                return getContentById(data.seriesId);
            })
            .then((series) => {
                if (!cancelled && series) setSeriesName(series.name);
            })
            .catch((err) => {
                console.error("Failed to load episode:", err);
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
                    <Link to="/storymap/contents" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !episode) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/contents">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <Link to={`/storymap/contents/${episode.seriesId}`}>{seriesName}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{episode.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{episode.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox label={t("episodes.season")} value={episode.season} />
                    <OrnateDisplayBox label={t("episodes.order")} value={episode.order} />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {episode.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{episode.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <ReferencesSection worldId={episode.worldId} episodeId={episode.id} canEdit={canEdit} />
        </div>
    );
}
