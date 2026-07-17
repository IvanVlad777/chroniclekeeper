import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { MilitaryEquipmentDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getMilitaryEquipmentById } from "../../../../api/militaryEquipment";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "./styles.module.css";

const glyph = "🏹";

export default function MilitaryEquipmentDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("militaryEquipment");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [equipment, setEquipment] =
        useState<MilitaryEquipmentDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const equipmentId = Number(id);
        if (!equipmentId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getMilitaryEquipmentById(equipmentId)
            .then((data) => {
                if (!cancelled) setEquipment(data);
            })
            .catch((err) => {
                console.error("Failed to load equipment:", err);
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
                    <Link to="/storymap/military-equipment" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !equipment) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/military-equipment">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{equipment.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {equipment.equipmentType || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{equipment.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/military-equipment/${equipment.id}/edit`
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
                        label={t("fields.equipmentType")}
                        value={equipment.equipmentType || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {equipment.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {equipment.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.units")}</span>
                <span className={s.sectionLine} />
            </div>
            {equipment.units.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                equipment.units.map((u) => (
                    <div key={u.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/military-units/${u.id}`}
                        >
                            {u.name}
                        </Link>
                    </div>
                ))
            )}

            <HistoryBlock
                targetType="MilitaryEquipment"
                targetId={equipment.id}
                worldId={equipment.worldId}
                history={equipment.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
