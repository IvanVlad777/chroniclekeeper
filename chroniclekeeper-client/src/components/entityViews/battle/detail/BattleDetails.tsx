import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { BattleDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getBattleById } from "../../../../api/battles";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "./styles.module.css";

const glyph = "🗡";

export default function BattleDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("battle");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [battle, setBattle] = useState<BattleDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const battleId = Number(id);
        if (!battleId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getBattleById(battleId)
            .then((data) => {
                if (!cancelled) setBattle(data);
            })
            .catch((err) => {
                console.error("Failed to load battle:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("loaderror"));
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
                    <Link to="/storymap/battles" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !battle) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/battles">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{battle.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {battle.battleDate || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{battle.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/battles/${battle.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.battleDate")}
                        value={battle.battleDate || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.location")}
                        value={battle.location || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.outcome")}
                        value={battle.outcome || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {battle.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{battle.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.armies")}</span>
                <span className={s.sectionLine} />
            </div>
            {battle.participatingArmies.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                battle.participatingArmies.map((a) => (
                    <div key={a.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/armies/${a.id}`}
                        >
                            {a.name}
                        </Link>
                    </div>
                ))
            )}

            <HistoryBlock
                targetType="Battle"
                targetId={battle.id}
                worldId={battle.worldId}
                history={battle.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
