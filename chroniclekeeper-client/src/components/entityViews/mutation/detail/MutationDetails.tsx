import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { MutationDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getMutationById } from "../../../../api/mutations";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "🧬";

export default function MutationDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("mutation");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<MutationDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const mid = Number(id);
        if (!mid) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getMutationById(mid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                console.error("Failed to load mutation:", err);
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
                    <Link to="/storymap/mutations" className={s.backLink}>
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
                <Link to="/storymap/mutations">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {t(`origins.${item.origin}`)} ·{" "}
                        {t(`effects.${item.effect}`)}
                    </div>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/mutations/${item.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.origin")}
                        value={t(`origins.${item.origin}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.effect")}
                        value={t(`effects.${item.effect}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.mutantCreature")}
                        value={
                            item.mutantCreature ? (
                                <Link
                                    to={`/storymap/creatures/${item.mutantCreature.id}`}
                                    className={s.memberLink}
                                >
                                    {item.mutantCreature.name}
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
        </div>
    );
}
