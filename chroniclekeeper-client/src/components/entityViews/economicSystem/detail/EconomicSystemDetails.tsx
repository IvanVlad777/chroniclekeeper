import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { EconomicSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getEconomicSystemById } from "../../../../api/economicSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚜";

export default function EconomicSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("economicSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [economicSystem, setEconomicSystem] =
        useState<EconomicSystemDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const economicSystemId = Number(id);
        if (!economicSystemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getEconomicSystemById(economicSystemId)
            .then((data) => {
                if (!cancelled) setEconomicSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load economic system:", err);
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
                    <Link to="/storymap/economic-systems" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !economicSystem) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kind = economicSystem.isFeudal
        ? t("kinds.feudal")
        : economicSystem.isMarketDriven
          ? t("kinds.market")
          : t("kinds.command");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/economic-systems">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{economicSystem.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kind}</div>
                    <h1 className={s.name}>{economicSystem.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/economic-systems/${economicSystem.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.flags")}
                        value={
                            [
                                economicSystem.hasStateControl
                                    ? t("fields.hasStateControl")
                                    : null,
                                economicSystem.allowsCorporations
                                    ? t("fields.allowsCorporations")
                                    : null,
                                economicSystem.allowsGuilds
                                    ? t("fields.allowsGuilds")
                                    : null,
                            ]
                                .filter(Boolean)
                                .join(" · ") || dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.taxationSystem")}
                        value={
                            economicSystem.taxationSystem ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/taxation-systems/${economicSystem.taxationSystem.id}`}
                                >
                                    {economicSystem.taxationSystem.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.bankingSystem")}
                        value={
                            economicSystem.bankingSystem ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/banking-systems/${economicSystem.bankingSystem.id}`}
                                >
                                    {economicSystem.bankingSystem.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            economicSystem.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${economicSystem.history.id}`}
                                >
                                    {economicSystem.history.name}
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
            {economicSystem.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {economicSystem.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
