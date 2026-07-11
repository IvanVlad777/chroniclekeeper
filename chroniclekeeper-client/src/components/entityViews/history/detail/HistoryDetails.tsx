import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getHistoryById } from "../../../../api/histories";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⌛";

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
                        value={history.isOfficial ? t("official") : t("unofficial")}
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
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {history.description ? (
                <p className={s.prose}>{history.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("timelines.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {history.timelines.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chipRow}>
                    {history.timelines.map((tl) => (
                        <Link
                            key={tl.id}
                            to={`/storymap/timelines/${tl.id}`}
                            className={s.chipLink}
                        >
                            {tl.name}
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
