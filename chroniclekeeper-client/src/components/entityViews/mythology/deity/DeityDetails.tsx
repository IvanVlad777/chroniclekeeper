import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { LinkEditor } from "../../../linking/LinkEditor";
import { DeityDetailsDto, ReferenceDto } from "../../../../interfaces/loreInterfaces";
import {
    addDeityAlly,
    addDeityOrder,
    addDeityRival,
    getDeities,
    getDeityById,
    removeDeityAlly,
    removeDeityOrder,
    removeDeityRival,
} from "../../../../api/deities";
import { getReligiousOrders } from "../../../../api/religiousOrders";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "../mythology.module.css";

const glyph = "☉";

export default function DeityDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("mythology");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<DeityDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [orderCandidates, setOrderCandidates] = useState<ReferenceDto[] | null>(null);
    const [allyCandidates, setAllyCandidates] = useState<ReferenceDto[] | null>(null);
    const [rivalCandidates, setRivalCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const n = Number(id);
        if (!n) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        getDeityById(n)
            .then((data) => !cancelled && setItem(data))
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("deity.loadError"));
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
                action={<Link to="/storymap/deities" className={s.backLink}>← {t("form.backToList")}</Link>}
            />
        );
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    const deity = item;
    const otherDeities = (ds: { id: number; name: string }[]): ReferenceDto[] =>
        ds.filter((d) => d.id !== deity.id).map((d) => ({ id: d.id, name: d.name }));

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/deities">{t("deity.listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{deity.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{t(`deityTypes.${deity.deityType}`)}</div>
                    <h1 className={s.detailName}>{deity.name}</h1>
                </div>
                {canEdit && (
                    <Button variant="ghost" onClick={() => navigate(`/storymap/deities/${deity.id}/edit`)}>{t("form.edit")}</Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox label={t("deity.fields.domain")} value={deity.domain || dash} />
                    <OrnateDisplayBox
                        label={t("fields.religion")}
                        value={deity.religion ? <Link className={s.refLink} to={`/storymap/religions/${deity.religion.id}`}>{deity.religion.name}</Link> : dash}
                    />
                    <OrnateDisplayBox label={t("deity.fields.deityType")} value={t(`deityTypes.${deity.deityType}`)} />
                    <OrnateDisplayBox label={t("deity.fields.isMonotheistic")} value={deity.isMonotheistic ? "✓" : dash} />
                    <OrnateDisplayBox label={t("deity.fields.isImmortal")} value={deity.isImmortal ? "✓" : dash} />
                    <OrnateDisplayBox label={t("deity.fields.canManifestPhysically")} value={deity.canManifestPhysically ? "✓" : dash} />
                    <OrnateDisplayBox label={t("deity.fields.grantsPowers")} value={deity.grantsPowers ? "✓" : dash} />
                    <OrnateDisplayBox label={t("deity.fields.worshipMethods")} value={deity.worshipMethods || dash} />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {deity.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{deity.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("deity.links.orders")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={deity.ordersDedicatedToDeity}
                candidates={orderCandidates}
                onLoadCandidates={() => getReligiousOrders(deity.worldId).then((os) => setOrderCandidates(os.map((o) => ({ id: o.id, name: o.name }))))}
                onAdd={(orderId) => addDeityOrder(deity.id, orderId)}
                onRemove={(orderId) => removeDeityOrder(deity.id, orderId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(orderId) => `/storymap/religious-orders/${orderId}`}
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
                <span className={s.sectionTitle}>{t("deity.links.allies")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={deity.alliedDeities}
                candidates={allyCandidates}
                onLoadCandidates={() => getDeities(deity.worldId).then((ds) => setAllyCandidates(otherDeities(ds)))}
                onAdd={(otherId) => addDeityAlly(deity.id, otherId)}
                onRemove={(otherId) => removeDeityAlly(deity.id, otherId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(otherId) => `/storymap/deities/${otherId}`}
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
                <span className={s.sectionTitle}>{t("deity.links.rivals")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={deity.rivalDeities}
                candidates={rivalCandidates}
                onLoadCandidates={() => getDeities(deity.worldId).then((ds) => setRivalCandidates(otherDeities(ds)))}
                onAdd={(otherId) => addDeityRival(deity.id, otherId)}
                onRemove={(otherId) => removeDeityRival(deity.id, otherId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(otherId) => `/storymap/deities/${otherId}`}
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
                <span className={s.sectionTitle}>{t("deity.links.sacredTexts")}</span>
                <span className={s.sectionLine} />
            </div>
            {deity.sacredTexts.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                deity.sacredTexts.map((x) => (
                    <div key={x.id} className={s.childRow}>
                        <Link className={s.refLink} to={`/storymap/religious-texts/${x.id}`}>{x.name}</Link>
                    </div>
                ))
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("deity.links.sacredSites")}</span>
                <span className={s.sectionLine} />
            </div>
            {deity.sacredSitesOfDeity.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                deity.sacredSitesOfDeity.map((x) => (
                    <div key={x.id} className={s.childRow}>
                        <Link className={s.refLink} to={`/storymap/holy-sites/${x.id}`}>{x.name}</Link>
                    </div>
                ))
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("deity.links.majorMyths")}</span>
                <span className={s.sectionLine} />
            </div>
            {deity.majorMyths.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                deity.majorMyths.map((x) => (
                    <div key={x.id} className={s.childRow}>
                        <span>{x.name}</span>
                    </div>
                ))
            )}

            <HistoryBlock targetType="Deity" targetId={deity.id} worldId={deity.worldId} history={deity.history} canEdit={canEdit} onChanged={refetch} />
        </div>
    );
}
