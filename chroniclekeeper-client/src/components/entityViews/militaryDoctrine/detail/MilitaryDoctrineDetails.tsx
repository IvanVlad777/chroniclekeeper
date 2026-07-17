import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { MilitaryDoctrineDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getMilitaryDoctrineById } from "../../../../api/militaryDoctrines";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "./styles.module.css";

const glyph = "📜";

export default function MilitaryDoctrineDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("militaryDoctrine");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [doctrine, setDoctrine] = useState<MilitaryDoctrineDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const doctrineId = Number(id);
        if (!doctrineId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getMilitaryDoctrineById(doctrineId)
            .then((data) => {
                if (!cancelled) setDoctrine(data);
            })
            .catch((err) => {
                console.error("Failed to load doctrine:", err);
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
                    <Link to="/storymap/military-doctrines" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !doctrine) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const priorities = [
        doctrine.prioritizesInfantry ? t("priorities.infantry") : null,
        doctrine.prioritizesCavalry ? t("priorities.cavalry") : null,
        doctrine.prioritizesArtillery ? t("priorities.artillery") : null,
        doctrine.prioritizesNavalForces ? t("priorities.navalForces") : null,
        doctrine.prioritizesAirForces ? t("priorities.airForces") : null,
    ]
        .filter(Boolean)
        .join(" · ");
    const traits = [
        doctrine.requiresHeavyIndustry ? t("traits.requiresHeavyIndustry") : null,
        doctrine.usesMercenaries ? t("traits.usesMercenaries") : null,
    ]
        .filter(Boolean)
        .join(" · ");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/military-doctrines">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{doctrine.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{t("listTitle")}</div>
                    <h1 className={s.name}>{doctrine.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/military-doctrines/${doctrine.id}/edit`
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
                        label={t("fields.strategy")}
                        value={doctrine.strategy || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.philosophy")}
                        value={doctrine.philosophy || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.priorities")}
                        value={priorities || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.traits")}
                        value={traits || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {doctrine.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{doctrine.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.organizations")}</span>
                <span className={s.sectionLine} />
            </div>
            {doctrine.organizations.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                doctrine.organizations.map((o) => (
                    <div key={o.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/military-organizations/${o.id}`}
                        >
                            {o.name}
                        </Link>
                    </div>
                ))
            )}

            <HistoryBlock
                targetType="MilitaryDoctrine"
                targetId={doctrine.id}
                worldId={doctrine.worldId}
                history={doctrine.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
