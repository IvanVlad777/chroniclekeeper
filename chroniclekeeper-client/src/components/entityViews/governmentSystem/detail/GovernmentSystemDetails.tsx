import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { GovernmentSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getGovernmentSystemById } from "../../../../api/governmentSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⌂";

export default function GovernmentSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("governmentSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [system, setSystem] = useState<GovernmentSystemDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const systemId = Number(id);
        if (!systemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getGovernmentSystemById(systemId)
            .then((data) => {
                if (!cancelled) setSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load government system:", err);
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
                        to="/storymap/government-systems"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !system) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = t(`electionSystems.${system.electionSystem}`);

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/government-systems">
                    {t("listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{system.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kicker}</div>
                    <h1 className={s.name}>{system.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/government-systems/${system.id}/edit`
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
                        label={t("fields.politicalIdeology")}
                        value={
                            system.politicalIdeology ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/political-ideologies/${system.politicalIdeology.id}`}
                                >
                                    {system.politicalIdeology.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.stabilityLevel")}
                        value={t(`scaleLevels.${system.stabilityLevel}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.hasTermLimits")}
                        value={
                            system.hasTermLimits
                                ? system.maxTermLength
                                    ? String(system.maxTermLength)
                                    : "✓"
                                : dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.isDemocratic")}
                        value={system.isDemocratic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isMonarchic")}
                        value={system.isMonarchic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isReligious")}
                        value={system.isReligious ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isFederal")}
                        value={system.isFederal ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isCentralized")}
                        value={system.isCentralized ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {system.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {system.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("parties.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {system.politicalParties.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.refs}>
                    {system.politicalParties.map((p) => (
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
        </div>
    );
}
