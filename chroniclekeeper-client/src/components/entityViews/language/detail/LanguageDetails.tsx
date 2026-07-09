import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LanguageDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getLanguageById } from "../../../../api/languages";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "✒";

export default function LanguageDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("language");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [language, setLanguage] = useState<LanguageDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const languageId = Number(id);
        if (!languageId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getLanguageById(languageId)
            .then((data) => {
                if (!cancelled) setLanguage(data);
            })
            .catch((err) => {
                console.error("Failed to load language:", err);
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
                    <Link to="/storymap/languages" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !language) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = language.isExtinct ? t("extinct") : "";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/languages">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{language.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    {kicker && <div className={s.kicker}>{kicker}</div>}
                    <h1 className={s.name}>{language.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/languages/${language.id}/edit`
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
                        label={t("fields.writingSystem")}
                        value={language.writingSystem || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.dialects")}
                        value={language.dialects || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isExtinct")}
                        value={language.isExtinct ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {language.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {language.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("cultures.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {language.cultures.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.cultures}>
                    {language.cultures.map((c) => (
                        <Link
                            key={c.id}
                            to={`/storymap/cultures/${c.id}`}
                            className={s.cultureLink}
                        >
                            <Tag tone="neutral">{c.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
