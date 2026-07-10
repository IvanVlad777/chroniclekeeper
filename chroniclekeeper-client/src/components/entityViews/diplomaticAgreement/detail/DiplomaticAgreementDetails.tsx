import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { DiplomaticAgreementDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getDiplomaticAgreementById } from "../../../../api/diplomaticAgreements";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⚜";

export default function DiplomaticAgreementDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("diplomaticAgreement");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [agreement, setAgreement] =
        useState<DiplomaticAgreementDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const agreementId = Number(id);
        if (!agreementId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getDiplomaticAgreementById(agreementId)
            .then((data) => {
                if (!cancelled) setAgreement(data);
            })
            .catch((err) => {
                console.error("Failed to load diplomatic agreement:", err);
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
                        to="/storymap/diplomatic-agreements"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !agreement) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = agreement.agreementType || dash;

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/diplomatic-agreements">
                    {t("listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{agreement.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kicker}</div>
                    <h1 className={s.name}>{agreement.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/diplomatic-agreements/${agreement.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.firstNation")}
                        value={
                            <Link
                                className={s.refLink}
                                to={`/storymap/nations/${agreement.firstNation.id}`}
                            >
                                {agreement.firstNation.name}
                            </Link>
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.secondNation")}
                        value={
                            <Link
                                className={s.refLink}
                                to={`/storymap/nations/${agreement.secondNation.id}`}
                            >
                                {agreement.secondNation.name}
                            </Link>
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.isUnequal")}
                        value={agreement.isUnequal ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.signedDate")}
                        value={agreement.signedDate || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.terminationDate")}
                        value={agreement.terminationDate || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.durationYears")}
                        value={
                            agreement.durationYears != null
                                ? String(agreement.durationYears)
                                : dash
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {agreement.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {agreement.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.terms")}</span>
                <span className={s.sectionLine} />
            </div>
            {agreement.terms ? (
                <p className={s.prose}>{agreement.terms}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
