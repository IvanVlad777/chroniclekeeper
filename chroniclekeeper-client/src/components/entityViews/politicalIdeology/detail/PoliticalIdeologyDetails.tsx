import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { PoliticalIdeologyDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getPoliticalIdeologyById } from "../../../../api/politicalIdeologies";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "◐";

export default function PoliticalIdeologyDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("politicalIdeology");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [ideology, setIdeology] =
        useState<PoliticalIdeologyDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const ideologyId = Number(id);
        if (!ideologyId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getPoliticalIdeologyById(ideologyId)
            .then((data) => {
                if (!cancelled) setIdeology(data);
            })
            .catch((err) => {
                console.error("Failed to load political ideology:", err);
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
                    <Link
                        to="/storymap/political-ideologies"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !ideology) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const traits = [
        ideology.isAuthoritarian && t("fields.isAuthoritarian"),
        ideology.isSocialist && t("fields.isSocialist"),
        ideology.isLiberal && t("fields.isLiberal"),
        ideology.isRadical && t("fields.isRadical"),
        ideology.isMilitaristic && t("fields.isMilitaristic"),
    ].filter((v): v is string => Boolean(v));
    const kicker = traits.join(" · ");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/political-ideologies">
                    {t("listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{ideology.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    {kicker && <div className={s.kicker}>{kicker}</div>}
                    <h1 className={s.name}>{ideology.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/political-ideologies/${ideology.id}/edit`
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
                        label={t("fields.isAuthoritarian")}
                        value={ideology.isAuthoritarian ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isSocialist")}
                        value={ideology.isSocialist ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isLiberal")}
                        value={ideology.isLiberal ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isRadical")}
                        value={ideology.isRadical ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isMilitaristic")}
                        value={ideology.isMilitaristic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.supportsFreeMarket")}
                        value={ideology.supportsFreeMarket ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.supportsPlannedEconomy")}
                        value={ideology.supportsPlannedEconomy ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {ideology.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {ideology.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("parties.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {ideology.affiliatedPoliticalParties.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.refs}>
                    {ideology.affiliatedPoliticalParties.map((p) => (
                        <Link
                            key={p.id}
                            to={`/storymap/political-parties/${p.id}`}
                            className={s.refLink}
                        >
                            <Tag tone="neutral">{p.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("governmentSystems.label")}
                </span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {ideology.affiliatedGovernmentSystems.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.refs}>
                    {ideology.affiliatedGovernmentSystems.map((g) => (
                        <Link
                            key={g.id}
                            to={`/storymap/government-systems/${g.id}`}
                            className={s.refLink}
                        >
                            <Tag tone="neutral">{g.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
