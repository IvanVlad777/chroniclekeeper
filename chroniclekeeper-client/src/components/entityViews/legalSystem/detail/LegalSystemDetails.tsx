import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LegalSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getLegalSystemById } from "../../../../api/legalSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⛨";

export default function LegalSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("legalSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [legalSystem, setLegalSystem] =
        useState<LegalSystemDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const legalSystemId = Number(id);
        if (!legalSystemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getLegalSystemById(legalSystemId)
            .then((data) => {
                if (!cancelled) setLegalSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load legal system:", err);
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
                    <Link to="/storymap/legal-systems" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !legalSystem) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const kicker = t(`punishmentMethods.${legalSystem.punishmentMethods}`);

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/legal-systems">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>
                    {legalSystem.name}
                </span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kicker}</div>
                    <h1 className={s.name}>{legalSystem.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/legal-systems/${legalSystem.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("fields.judicialIndependence")}
                        value={t(
                            `scaleLevels.${legalSystem.judicialIndependence}`
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.punishmentMethods")}
                        value={t(
                            `punishmentMethods.${legalSystem.punishmentMethods}`
                        )}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {legalSystem.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {legalSystem.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.laws")}</span>
                <span className={s.sectionLine} />
            </div>
            {legalSystem.laws ? (
                <p className={s.prose}>{legalSystem.laws}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
