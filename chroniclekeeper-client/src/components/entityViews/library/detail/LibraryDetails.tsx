import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LibraryDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getLibraryById } from "../../../../api/libraries";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "❖";

export default function LibraryDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("library");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [library, setLibrary] = useState<LibraryDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const libraryId = Number(id);
        if (!libraryId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getLibraryById(libraryId)
            .then((data) => {
                if (!cancelled) setLibrary(data);
            })
            .catch((err) => {
                console.error("Failed to load library:", err);
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
                    <Link to="/storymap/libraries" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !library) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/libraries">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{library.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{library.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/libraries/${library.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.isPublic")}
                        value={library.isPublic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.focusesOnMagic")}
                        value={library.focusesOnMagic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.focusesOnHistory")}
                        value={library.focusesOnHistory ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.university")}
                        value={
                            library.university ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/universities/${library.university.id}`}
                                >
                                    {library.university.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.location")}
                        value={
                            library.location ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/locations/${library.location.id}`}
                                >
                                    {library.location.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {library.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{library.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}
        </div>
    );
}
