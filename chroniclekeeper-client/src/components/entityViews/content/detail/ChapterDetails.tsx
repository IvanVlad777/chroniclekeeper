import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReferencesSection } from "../ReferencesSection";
import { ChapterDto } from "../../../../interfaces/loreInterfaces";
import { getChapterById, getContentById } from "../../../../api/contents";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "📖";

export default function ChapterDetails() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("content");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [chapter, setChapter] = useState<ChapterDto | null>(null);
    const [bookName, setBookName] = useState<string>("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const chapterId = Number(id);
        if (!chapterId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getChapterById(chapterId)
            .then((data) => {
                if (cancelled) return;
                setChapter(data);
                return getContentById(data.bookId);
            })
            .then((book) => {
                if (!cancelled && book) setBookName(book.name);
            })
            .catch((err) => {
                console.error("Failed to load chapter:", err);
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
                    <Link to="/storymap/contents" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !chapter) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/contents">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <Link to={`/storymap/contents/${chapter.bookId}`}>{bookName}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{chapter.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{chapter.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox label={t("chapters.order")} value={chapter.order} />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {chapter.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{chapter.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <ReferencesSection worldId={chapter.worldId} chapterId={chapter.id} canEdit={canEdit} />
        </div>
    );
}
