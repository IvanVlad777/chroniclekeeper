import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TaxationSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getTaxationSystemById } from "../../../../api/taxationSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚖";

export default function TaxationSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("taxationSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [taxationSystem, setTaxationSystem] =
        useState<TaxationSystemDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const taxationSystemId = Number(id);
        if (!taxationSystemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getTaxationSystemById(taxationSystemId)
            .then((data) => {
                if (!cancelled) setTaxationSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load taxation system:", err);
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
                    <Link to="/storymap/taxation-systems" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !taxationSystem) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/taxation-systems">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{taxationSystem.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {taxationSystem.hasFlatTax
                            ? t("flatTax.flat")
                            : t("flatTax.progressive")}
                    </div>
                    <h1 className={s.name}>{taxationSystem.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/taxation-systems/${taxationSystem.id}/edit`
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
                        label={t("fields.incomeTaxRate")}
                        value={`${taxationSystem.incomeTaxRate}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.corporateTaxRate")}
                        value={`${taxationSystem.corporateTaxRate}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.tradeTariffRate")}
                        value={`${taxationSystem.tradeTariffRate}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.hasWealthTax")}
                        value={taxationSystem.hasWealthTax ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            taxationSystem.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${taxationSystem.history.id}`}
                                >
                                    {taxationSystem.history.name}
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
            {taxationSystem.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {taxationSystem.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
