import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { SocialHierarchyDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getSocialHierarchyById } from "../../../../api/socialHierarchies";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚜";

export default function SocialHierarchyDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("socialHierarchy");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<SocialHierarchyDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const hid = Number(id);
        if (!hid) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getSocialHierarchyById(hid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                console.error("Failed to load social hierarchy:", err);
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
                    <Link
                        to="/storymap/social-hierarchies"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !item) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/social-hierarchies">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/social-hierarchies/${item.id}/edit`
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
                        label={t("fields.isCasteSystem")}
                        value={item.isCasteSystem ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.allowsUpwardMobility")}
                        value={item.allowsUpwardMobility ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.allowsIntermarriage")}
                        value={item.allowsIntermarriage ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.enforcesLegalSeparation")}
                        value={item.enforcesLegalSeparation ? "✓" : dash}
                    />
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

            {item.history && (
                <>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("fields.history")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <div className={s.members}>
                        <Link
                            to={`/storymap/histories/${item.history.id}`}
                            className={s.memberLink}
                        >
                            <Tag tone="neutral">{item.history.name}</Tag>
                        </Link>
                    </div>
                </>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("classes.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {item.classes.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.members}>
                    {item.classes.map((c) => (
                        <Link
                            key={c.id}
                            to={`/storymap/social-classes/${c.id}`}
                            className={s.memberLink}
                        >
                            <Tag tone="neutral">{c.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("nations.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {item.nations.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.members}>
                    {item.nations.map((n) => (
                        <Link
                            key={n.id}
                            to={`/storymap/nations/${n.id}`}
                            className={s.memberLink}
                        >
                            <Tag tone="neutral">{n.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
