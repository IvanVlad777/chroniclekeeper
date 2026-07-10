import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { PoliticalPartyDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getPoliticalPartyById } from "../../../../api/politicalParties";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "✪";

export default function PoliticalPartyDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("politicalParty");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [party, setParty] = useState<PoliticalPartyDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const partyId = Number(id);
        if (!partyId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getPoliticalPartyById(partyId)
            .then((data) => {
                if (!cancelled) setParty(data);
            })
            .catch((err) => {
                console.error("Failed to load political party:", err);
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
                        to="/storymap/political-parties"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !party) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = t(`scaleLevels.${party.influenceLevel}`);

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/political-parties">
                    {t("listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{party.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {kicker}
                        {party.isBanned && (
                            <Tag tone="accent">{t("fields.isBanned")}</Tag>
                        )}
                    </div>
                    <h1 className={s.name}>{party.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/political-parties/${party.id}/edit`
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
                        label={t("fields.politicalIdeology")}
                        value={
                            party.politicalIdeology ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/political-ideologies/${party.politicalIdeology.id}`}
                                >
                                    {party.politicalIdeology.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.governmentSystem")}
                        value={
                            party.governmentSystem ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/government-systems/${party.governmentSystem.id}`}
                                >
                                    {party.governmentSystem.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.influenceLevel")}
                        value={t(`scaleLevels.${party.influenceLevel}`)}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {party.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {party.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("fields.ideologyDescription")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {party.ideologyDescription ? (
                <p className={s.prose}>{party.ideologyDescription}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
