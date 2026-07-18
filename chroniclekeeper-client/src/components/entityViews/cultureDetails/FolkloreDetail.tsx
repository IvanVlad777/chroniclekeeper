import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { LinkEditor } from "../../linking/LinkEditor";
import { FolkloreDetailsDto, ReferenceDto } from "../../../interfaces/loreInterfaces";
import {
    addFolkloreEvent,
    addFolkloreSpecies,
    getFolkloreById,
    removeFolkloreEvent,
    removeFolkloreSpecies,
} from "../../../api/folklore";
import { getSpecies } from "../../../api/species";
import { getTimeline, getTimelines } from "../../../api/timelines";
import { useAuth } from "../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "📖";

export default function FolkloreDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("cultureDetails");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<FolkloreDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [eventCandidates, setEventCandidates] = useState<ReferenceDto[] | null>(null);
    const [speciesCandidates, setSpeciesCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const folkId = Number(id);
        if (!folkId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        getFolkloreById(folkId)
            .then((data) => !cancelled && setItem(data))
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("detail.loadError"));
            })
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    const loadEventCandidates = async (worldId: number) => {
        const timelines = await getTimelines(worldId);
        const details = await Promise.all(timelines.map((tl) => getTimeline(tl.id)));
        setEventCandidates(
            details
                .flatMap((d) => d.events)
                .map((e) => ({ id: e.id, name: e.name }))
        );
    };

    if (loading) return <LoadingSkeleton variant="block" rows={5} />;
    if (notFound)
        return (
            <EmptyState
                glyph={glyph}
                title={t("detail.notFound")}
                action={
                    <Link to="/storymap/cultures" className={s.backLink}>
                        ← {t("detail.backToCultures")}
                    </Link>
                }
            />
        );
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/cultures">{t("detail.cultures")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span>{item.name}</span>
            </div>
            <div className={s.kicker}>{t("sections.folklore")}</div>
            <h1 className={s.name}>{item.name}</h1>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("cultureLabel")}
                        value={
                            item.culture ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/cultures/${item.culture.id}`}
                                >
                                    {item.culture.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.moral")}
                        value={item.moral || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isHistorical")}
                        value={item.isHistorical ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            {item.story && (
                <>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>{t("fields.story")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    <p className={s.prose}>{item.story}</p>
                </>
            )}

            <div className={s.section}>
                <div className={s.sectionHead}>
                    <span className={s.sectionTitle}>{t("links.relatedEvents")}</span>
                    <span className={s.sectionLine} />
                </div>
                <LinkEditor
                    items={item.relatedEvents}
                    candidates={eventCandidates}
                    onLoadCandidates={() => loadEventCandidates(item.worldId)}
                    onAdd={(eventId) => addFolkloreEvent(item.id, eventId)}
                    onRemove={(eventId) => removeFolkloreEvent(item.id, eventId)}
                    onChanged={refetch}
                    canEdit={canEdit}
                    linkTo={() => "#"}
                    addLabel={t("links.add")}
                    noneLabel={t("none")}
                    pickLabel={t("links.pick")}
                    cancelLabel={t("actions.cancel")}
                    confirmLabel={t("links.confirm")}
                    removeLabel={(name) => t("links.remove", { name })}
                    addFailedLabel={t("links.addFailed")}
                    removeFailedLabel={t("links.removeFailed")}
                />
            </div>

            <div className={s.section}>
                <div className={s.sectionHead}>
                    <span className={s.sectionTitle}>{t("links.originatedFromSpecies")}</span>
                    <span className={s.sectionLine} />
                </div>
                <LinkEditor
                    items={item.originatedFromSpecies}
                    candidates={speciesCandidates}
                    onLoadCandidates={() =>
                        getSpecies(item.worldId).then((sp) =>
                            setSpeciesCandidates(
                                sp.map((x) => ({ id: x.id, name: x.name }))
                            )
                        )
                    }
                    onAdd={(speciesId) => addFolkloreSpecies(item.id, speciesId)}
                    onRemove={(speciesId) => removeFolkloreSpecies(item.id, speciesId)}
                    onChanged={refetch}
                    canEdit={canEdit}
                    linkTo={(speciesId) => `/storymap/species/${speciesId}`}
                    addLabel={t("links.add")}
                    noneLabel={t("none")}
                    pickLabel={t("links.pick")}
                    cancelLabel={t("actions.cancel")}
                    confirmLabel={t("links.confirm")}
                    removeLabel={(name) => t("links.remove", { name })}
                    addFailedLabel={t("links.addFailed")}
                    removeFailedLabel={t("links.removeFailed")}
                />
            </div>
        </div>
    );
}
