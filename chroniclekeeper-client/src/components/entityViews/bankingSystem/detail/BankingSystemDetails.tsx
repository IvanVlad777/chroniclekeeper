import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { BankingSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getBankingSystemById } from "../../../../api/bankingSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "🏛";

export default function BankingSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("bankingSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [bankingSystem, setBankingSystem] =
        useState<BankingSystemDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const bankingSystemId = Number(id);
        if (!bankingSystemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getBankingSystemById(bankingSystemId)
            .then((data) => {
                if (!cancelled) setBankingSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load banking system:", err);
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
                    <Link to="/storymap/banking-systems" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !bankingSystem) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/banking-systems">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{bankingSystem.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {bankingSystem.systemType || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{bankingSystem.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/banking-systems/${bankingSystem.id}/edit`
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
                        label={t("fields.interestRate")}
                        value={`${bankingSystem.interestRate}%`}
                    />
                    <OrnateDisplayBox
                        label={t("fields.currency")}
                        value={
                            bankingSystem.currency ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/currencies/${bankingSystem.currency.id}`}
                                >
                                    {bankingSystem.currency.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.flags")}
                        value={
                            [
                                bankingSystem.allowsLoans
                                    ? t("fields.allowsLoans")
                                    : null,
                                bankingSystem.hasStateControl
                                    ? t("fields.hasStateControl")
                                    : null,
                                bankingSystem.supportsForeignInvestment
                                    ? t("fields.supportsForeignInvestment")
                                    : null,
                            ]
                                .filter(Boolean)
                                .join(" · ") || dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            bankingSystem.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${bankingSystem.history.id}`}
                                >
                                    {bankingSystem.history.name}
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
            {bankingSystem.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {bankingSystem.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
