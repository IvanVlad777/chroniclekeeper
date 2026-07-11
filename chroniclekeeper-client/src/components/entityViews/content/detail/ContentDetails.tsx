import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReferencesSection } from "../ReferencesSection";
import { ContentDetailsDto } from "../../../../interfaces/loreInterfaces";
import {
    createChapter,
    createEpisode,
    deleteChapter,
    deleteEpisode,
    getChapterById,
    getContentById,
    getEpisodeById,
    updateChapter,
    updateEpisode,
} from "../../../../api/contents";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "📖";

interface ChildFormState {
    name: string;
    description: string;
    order: string;
    season: string;
}
const emptyChildForm: ChildFormState = {
    name: "",
    description: "",
    order: "",
    season: "",
};

export default function ContentDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("content");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [content, setContent] = useState<ContentDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    // dijete (Chapter/Episode): null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [childFormFor, setChildFormFor] = useState<number | null>(null);
    const [childForm, setChildForm] = useState<ChildFormState>(emptyChildForm);
    const [childError, setChildError] = useState<string | null>(null);

    useEffect(() => {
        const contentId = Number(id);
        if (!contentId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getContentById(contentId)
            .then((data) => {
                if (!cancelled) setContent(data);
            })
            .catch((err) => {
                console.error("Failed to load content:", err);
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

    const openNewChild = () => {
        setChildForm(emptyChildForm);
        setChildError(null);
        setChildFormFor(0);
    };
    async function openEditChapter(chapterId: number) {
        setChildError(null);
        try {
            const ch = await getChapterById(chapterId);
            setChildForm({ name: ch.name, description: ch.description, order: String(ch.order), season: "" });
            setChildFormFor(chapterId);
        } catch (err) {
            console.error("Failed to load chapter:", err);
            setChildError(apiErrorMessage(err, t("chapters.saveFailed")));
        }
    }

    async function openEditEpisode(episodeId: number) {
        setChildError(null);
        try {
            const ep = await getEpisodeById(episodeId);
            setChildForm({ name: ep.name, description: ep.description, order: String(ep.order), season: String(ep.season) });
            setChildFormFor(episodeId);
        } catch (err) {
            console.error("Failed to load episode:", err);
            setChildError(apiErrorMessage(err, t("episodes.saveFailed")));
        }
    }

    async function onSaveChapter(e: FormEvent) {
        e.preventDefault();
        if (!content || childFormFor === null) return;
        if (!childForm.name.trim()) {
            setChildError(t("chapters.requiredMissing"));
            return;
        }
        setChildError(null);
        setBusy(true);
        try {
            const payload = {
                name: childForm.name.trim(),
                description: childForm.description,
                order: childForm.order.trim() ? Number(childForm.order) : 0,
            };
            if (childFormFor === 0) {
                await createChapter({ ...payload, bookId: content.id });
            } else {
                await updateChapter(childFormFor, payload);
            }
            setChildFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save chapter:", err);
            setChildError(apiErrorMessage(err, t("chapters.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteChapter(chId: number, name: string) {
        if (!window.confirm(t("chapters.deleteConfirm", { name }))) return;
        setChildError(null);
        setBusy(true);
        try {
            await deleteChapter(chId);
            refetch();
        } catch (err) {
            console.error("Failed to delete chapter:", err);
            setChildError(apiErrorMessage(err, t("chapters.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onSaveEpisode(e: FormEvent) {
        e.preventDefault();
        if (!content || childFormFor === null) return;
        if (!childForm.name.trim()) {
            setChildError(t("episodes.requiredMissing"));
            return;
        }
        setChildError(null);
        setBusy(true);
        try {
            const payload = {
                name: childForm.name.trim(),
                description: childForm.description,
                order: childForm.order.trim() ? Number(childForm.order) : 0,
                season: childForm.season.trim() ? Number(childForm.season) : 0,
            };
            if (childFormFor === 0) {
                await createEpisode({ ...payload, seriesId: content.id });
            } else {
                await updateEpisode(childFormFor, payload);
            }
            setChildFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save episode:", err);
            setChildError(apiErrorMessage(err, t("episodes.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteEpisode(epId: number, name: string) {
        if (!window.confirm(t("episodes.deleteConfirm", { name }))) return;
        setChildError(null);
        setBusy(true);
        try {
            await deleteEpisode(epId);
            refetch();
        } catch (err) {
            console.error("Failed to delete episode:", err);
            setChildError(apiErrorMessage(err, t("episodes.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link to="/storymap/contents" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !content) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const fmtDate = (d?: string | null) =>
        d ? new Date(d).toLocaleDateString() : dash;

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/contents">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{content.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{content.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/contents/${content.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("columns.type")}
                        value={content.type ? t(`types.${content.type}`) : dash}
                    />
                    {content.type === "Article" && (
                        <>
                            <OrnateDisplayBox label={t("fields.source")} value={content.source || dash} />
                            <OrnateDisplayBox label={t("fields.publishDate")} value={fmtDate(content.publishDate)} />
                        </>
                    )}
                    {(content.type === "Book" || content.type === "Comic") && (
                        <OrnateDisplayBox label={t("fields.author")} value={content.author || dash} />
                    )}
                    {content.type === "Book" && (
                        <OrnateDisplayBox label={t("fields.releaseDate")} value={fmtDate(content.releaseDate)} />
                    )}
                    {content.type === "Comic" && (
                        <OrnateDisplayBox label={t("fields.issueNumber")} value={content.issueNumber ?? dash} />
                    )}
                    {content.type === "Movie" && (
                        <>
                            <OrnateDisplayBox label={t("fields.director")} value={content.director || dash} />
                            <OrnateDisplayBox label={t("fields.releaseDate")} value={fmtDate(content.releaseDate)} />
                            <OrnateDisplayBox label={t("fields.durationMinutes")} value={content.durationMinutes ?? dash} />
                            <OrnateDisplayBox
                                label={t("fields.prequel")}
                                value={
                                    content.prequel ? (
                                        <Link to={`/storymap/contents/${content.prequel.id}`}>
                                            {content.prequel.name}
                                        </Link>
                                    ) : (
                                        dash
                                    )
                                }
                            />
                        </>
                    )}
                    {content.type === "Series" && (
                        <>
                            <OrnateDisplayBox label={t("fields.creator")} value={content.creator || dash} />
                            <OrnateDisplayBox label={t("fields.seasons")} value={content.seasons ?? dash} />
                        </>
                    )}
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {content.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{content.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {content.type === "Movie" && content.sequels.length > 0 && (
                <>
                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("sequels.label")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    <div className={s.chipRow}>
                        {content.sequels.map((seq) => (
                            <Link key={seq.id} to={`/storymap/contents/${seq.id}`} className={s.chipLink}>
                                {seq.name}
                            </Link>
                        ))}
                    </div>
                </>
            )}

            {content.type === "Book" && (
                <>
                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("chapters.label")}</span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                        {canEdit && childFormFor === null && (
                            <button type="button" className={s.addInline} onClick={openNewChild}>
                                + {t("chapters.add")}
                            </button>
                        )}
                    </div>
                    {content.chapters.length === 0 && childFormFor === null ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        content.chapters.map((ch) => (
                            <div key={ch.id} className={s.childRow}>
                                <Link to={`/storymap/chapters/${ch.id}`} className={s.childName}>
                                    {ch.name}
                                </Link>
                                {canEdit && (
                                    <span className={s.childActions}>
                                        <button
                                            type="button"
                                            className={s.childActionBtn}
                                            disabled={busy}
                                            onClick={() => openEditChapter(ch.id)}
                                        >
                                            {t("form.edit")}
                                        </button>
                                        <button
                                            type="button"
                                            className={`${s.childActionBtn} ${s.childActionDanger}`}
                                            disabled={busy}
                                            onClick={() => onDeleteChapter(ch.id, ch.name)}
                                        >
                                            {t("chapters.delete")}
                                        </button>
                                    </span>
                                )}
                            </div>
                        ))
                    )}
                    {childError && childFormFor === null && (
                        <p className={s.miniError} role="alert">{childError}</p>
                    )}
                    {childFormFor !== null && (
                        <form className={s.childForm} onSubmit={onSaveChapter}>
                            <h3 className={s.childFormTitle}>
                                {childFormFor === 0 ? t("chapters.addTitle") : t("chapters.editTitle")}
                            </h3>
                            <OrnateField label={t("chapters.name")} required>
                                <OrnateTextInput value={childForm.name} display maxLength={100} onChange={(e) => setChildForm((f) => ({ ...f, name: e.target.value }))} />
                            </OrnateField>
                            <OrnateField label={t("chapters.order")}>
                                <OrnateTextInput type="number" value={childForm.order} onChange={(e) => setChildForm((f) => ({ ...f, order: e.target.value }))} />
                            </OrnateField>
                            {childError && <p className={s.miniError} role="alert">{childError}</p>}
                            <div className={s.miniActions}>
                                <Button variant="ghost" size="sm" disabled={busy} onClick={() => setChildFormFor(null)}>{t("chapters.cancel")}</Button>
                                <Button type="submit" size="sm" disabled={busy}>{t("chapters.save")}</Button>
                            </div>
                        </form>
                    )}
                </>
            )}

            {content.type === "Series" && (
                <>
                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("episodes.label")}</span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                        {canEdit && childFormFor === null && (
                            <button type="button" className={s.addInline} onClick={openNewChild}>
                                + {t("episodes.add")}
                            </button>
                        )}
                    </div>
                    {content.episodes.length === 0 && childFormFor === null ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        content.episodes.map((ep) => (
                            <div key={ep.id} className={s.childRow}>
                                <Link to={`/storymap/episodes/${ep.id}`} className={s.childName}>
                                    {ep.name}
                                </Link>
                                {canEdit && (
                                    <span className={s.childActions}>
                                        <button
                                            type="button"
                                            className={s.childActionBtn}
                                            disabled={busy}
                                            onClick={() => openEditEpisode(ep.id)}
                                        >
                                            {t("form.edit")}
                                        </button>
                                        <button
                                            type="button"
                                            className={`${s.childActionBtn} ${s.childActionDanger}`}
                                            disabled={busy}
                                            onClick={() => onDeleteEpisode(ep.id, ep.name)}
                                        >
                                            {t("episodes.delete")}
                                        </button>
                                    </span>
                                )}
                            </div>
                        ))
                    )}
                    {childError && childFormFor === null && (
                        <p className={s.miniError} role="alert">{childError}</p>
                    )}
                    {childFormFor !== null && (
                        <form className={s.childForm} onSubmit={onSaveEpisode}>
                            <h3 className={s.childFormTitle}>
                                {childFormFor === 0 ? t("episodes.addTitle") : t("episodes.editTitle")}
                            </h3>
                            <OrnateField label={t("episodes.name")} required>
                                <OrnateTextInput value={childForm.name} display maxLength={100} onChange={(e) => setChildForm((f) => ({ ...f, name: e.target.value }))} />
                            </OrnateField>
                            <div className={s.row2}>
                                <OrnateField label={t("episodes.season")}>
                                    <OrnateTextInput type="number" value={childForm.season} onChange={(e) => setChildForm((f) => ({ ...f, season: e.target.value }))} />
                                </OrnateField>
                                <OrnateField label={t("episodes.order")}>
                                    <OrnateTextInput type="number" value={childForm.order} onChange={(e) => setChildForm((f) => ({ ...f, order: e.target.value }))} />
                                </OrnateField>
                            </div>
                            {childError && <p className={s.miniError} role="alert">{childError}</p>}
                            <div className={s.miniActions}>
                                <Button variant="ghost" size="sm" disabled={busy} onClick={() => setChildFormFor(null)}>{t("episodes.cancel")}</Button>
                                <Button type="submit" size="sm" disabled={busy}>{t("episodes.save")}</Button>
                            </div>
                        </form>
                    )}
                </>
            )}

            <ReferencesSection worldId={content.worldId} contentId={content.id} canEdit={canEdit} />
        </div>
    );
}
