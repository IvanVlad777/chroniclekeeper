import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReligionDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getReligionById } from "../../../../api/religions";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "✤";

export default function ReligionDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("religion");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [religion, setReligion] = useState<ReligionDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const religionId = Number(id);
        if (!religionId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getReligionById(religionId)
            .then((data) => {
                if (!cancelled) setReligion(data);
            })
            .catch((err) => {
                console.error("Failed to load religion:", err);
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
                    <Link to="/storymap/religions" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !religion) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = religion.isStateReligion ? t("fields.isStateReligion") : "";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/religions">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{religion.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    {kicker && <div className={s.kicker}>{kicker}</div>}
                    <h1 className={s.name}>{religion.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/religions/${religion.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("fields.hasDeities")}
                        value={religion.hasDeities ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isStateReligion")}
                        value={religion.isStateReligion ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {religion.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {religion.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.coreBeliefs")}</span>
                <span className={s.sectionLine} />
            </div>
            {religion.coreBeliefs ? (
                <p className={s.prose}>{religion.coreBeliefs}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("fields.practices")}</span>
                <span className={s.sectionLine} />
            </div>
            {religion.practices ? (
                <p className={s.prose}>{religion.practices}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("followers.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {religion.followers.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.followers}>
                    {religion.followers.map((f) => (
                        <Link
                            key={f.id}
                            to={`/storymap/characters/${f.id}`}
                            className={s.followerLink}
                        >
                            <Tag tone="neutral">{f.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
