import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { LinkEditor } from "../../../linking/LinkEditor";
import { ReferenceDto, ReligiousOrderDetailsDto } from "../../../../interfaces/loreInterfaces";
import {
    addReligiousOrderFaction,
    getReligiousOrderById,
    removeReligiousOrderFaction,
} from "../../../../api/religiousOrders";
import { getFactions } from "../../../../api/factions";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "../mythology.module.css";

const glyph = "🕍";

export default function ReligiousOrderDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("mythology");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<ReligiousOrderDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [factionCandidates, setFactionCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const n = Number(id);
        if (!n) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        getReligiousOrderById(n)
            .then((data) => !cancelled && setItem(data))
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("religiousOrder.loadError"));
            })
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound)
        return (
            <EmptyState
                glyph={glyph}
                title={t("form.notFound")}
                action={<Link to="/storymap/religious-orders" className={s.backLink}>← {t("form.backToList")}</Link>}
            />
        );
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/religious-orders">{t("religiousOrder.listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{item.orderType || t("religiousOrder.listTitle")}</div>
                    <h1 className={s.detailName}>{item.name}</h1>
                </div>
                {canEdit && (
                    <Button variant="ghost" onClick={() => navigate(`/storymap/religious-orders/${item.id}/edit`)}>{t("form.edit")}</Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.religion")}
                        value={item.religion ? <Link className={s.refLink} to={`/storymap/religions/${item.religion.id}`}>{item.religion.name}</Link> : dash}
                    />
                    <OrnateDisplayBox label={t("religiousOrder.fields.isMilitant")} value={item.isMilitant ? "✓" : dash} />
                    <OrnateDisplayBox label={t("religiousOrder.fields.isSecretive")} value={item.isSecretive ? "✓" : dash} />
                    <OrnateDisplayBox label={t("religiousOrder.fields.beliefs")} value={item.beliefs || dash} />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("religiousOrder.links.factions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={item.factions}
                candidates={factionCandidates}
                onLoadCandidates={() => getFactions(item.worldId).then((fs) => setFactionCandidates(fs.map((f) => ({ id: f.id, name: f.name }))))}
                onAdd={(factionId) => addReligiousOrderFaction(item.id, factionId)}
                onRemove={(factionId) => removeReligiousOrderFaction(item.id, factionId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(factionId) => `/storymap/factions/${factionId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("religiousOrder.links.deities")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.deities.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                item.deities.map((d) => (
                    <div key={d.id} className={s.childRow}>
                        <Link className={s.refLink} to={`/storymap/deities/${d.id}`}>{d.name}</Link>
                    </div>
                ))
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("religiousOrder.links.clergyTraining")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.clergyTraining.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                item.clergyTraining.map((c) => (
                    <div key={c.id} className={s.childRow}>
                        <span>{c.name}</span>
                    </div>
                ))
            )}

            <HistoryBlock targetType="ReligiousOrder" targetId={item.id} worldId={item.worldId} history={item.history} canEdit={canEdit} onChanged={refetch} />
        </div>
    );
}
