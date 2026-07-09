import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { SocialClassDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getSocialClassById } from "../../../../api/socialClasses";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⚖";

export default function SocialClassDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("socialClass");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [socialClass, setSocialClass] = useState<SocialClassDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const scId = Number(id);
        if (!scId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getSocialClassById(scId)
            .then((data) => {
                if (!cancelled) setSocialClass(data);
            })
            .catch((err) => {
                console.error("Failed to load social class:", err);
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
                    <Link to="/storymap/social-classes" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !socialClass) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const traits = [
        socialClass.isNoble && t("fields.isNoble"),
        socialClass.isMerchantClass && t("fields.isMerchantClass"),
        socialClass.isOutcast && t("fields.isOutcast"),
    ].filter((v): v is string => Boolean(v));
    const kicker = traits.join(" · ");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/social-classes">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{socialClass.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    {kicker && <div className={s.kicker}>{kicker}</div>}
                    <h1 className={s.name}>{socialClass.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/social-classes/${socialClass.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={6}>
                    <OrnateDisplayBox
                        label={t("fields.isNoble")}
                        value={socialClass.isNoble ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isMerchantClass")}
                        value={socialClass.isMerchantClass ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isOutcast")}
                        value={socialClass.isOutcast ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.canOwnLand")}
                        value={socialClass.canOwnLand ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.canHoldOffice")}
                        value={socialClass.canHoldOffice ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.hasTaxExemptions")}
                        value={socialClass.hasTaxExemptions ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {socialClass.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {socialClass.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("members.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {socialClass.members.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.members}>
                    {socialClass.members.map((m) => (
                        <Link
                            key={m.id}
                            to={`/storymap/characters/${m.id}`}
                            className={s.memberLink}
                        >
                            <Tag tone="neutral">{m.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
