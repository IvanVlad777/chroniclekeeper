import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { HolySiteDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getHolySiteById } from "../../../../api/holySites";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "../mythology.module.css";

const glyph = "⛩";

export default function HolySiteDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("mythology");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<HolySiteDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const n = Number(id);
        if (!n) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        getHolySiteById(n)
            .then((data) => !cancelled && setItem(data))
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("holySite.loadError"));
            })
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound)
        return (
            <EmptyState
                glyph={glyph}
                title={t("form.notFound")}
                action={
                    <Link to="/storymap/holy-sites" className={s.backLink}>
                        ← {t("form.backToList")}
                    </Link>
                }
            />
        );
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/holy-sites">{t("holySite.listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{t("holySite.listTitle")}</div>
                    <h1 className={s.detailName}>{item.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() => navigate(`/storymap/holy-sites/${item.id}/edit`)}
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.religion")}
                        value={
                            item.religion ? (
                                <Link className={s.refLink} to={`/storymap/religions/${item.religion.id}`}>
                                    {item.religion.name}
                                </Link>
                            ) : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.location")}
                        value={
                            item.location ? (
                                <Link className={s.refLink} to={`/storymap/locations/${item.location.id}`}>
                                    {item.location.name}
                                </Link>
                            ) : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.deity")}
                        value={
                            item.deity ? (
                                <Link className={s.refLink} to={`/storymap/deities/${item.deity.id}`}>
                                    {item.deity.name}
                                </Link>
                            ) : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("holySite.fields.isPilgrimageDestination")}
                        value={item.isPilgrimageDestination ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("holySite.fields.significance")}
                        value={item.significance || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <HistoryBlock
                targetType="HolySite"
                targetId={item.id}
                worldId={item.worldId}
                history={item.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
