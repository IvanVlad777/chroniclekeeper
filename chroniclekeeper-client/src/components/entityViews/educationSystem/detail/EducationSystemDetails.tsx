import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { EducationSystemDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getEducationSystemById } from "../../../../api/educationSystems";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "▦";

export default function EducationSystemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("educationSystem");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [system, setSystem] = useState<EducationSystemDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const systemId = Number(id);
        if (!systemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getEducationSystemById(systemId)
            .then((data) => {
                if (!cancelled) setSystem(data);
            })
            .catch((err) => {
                console.error("Failed to load education system:", err);
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
                        to="/storymap/education-systems"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !system) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/education-systems">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{system.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{system.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/education-systems/${system.id}/edit`
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
                        label={t("fields.isStateControlled")}
                        value={system.isStateControlled ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.allowsPrivateInstitutions")}
                        value={system.allowsPrivateInstitutions ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.includesReligiousEducation")}
                        value={system.includesReligiousEducation ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.supportsGuildTraining")}
                        value={system.supportsGuildTraining ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {system.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{system.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("schools.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {system.schools.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chips}>
                    {system.schools.map((sc) => (
                        <Link key={sc.id} to={`/storymap/schools/${sc.id}`} className={s.chipLink}>
                            <Tag tone="neutral">{sc.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("universities.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {system.universities.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chips}>
                    {system.universities.map((u) => (
                        <Link key={u.id} to={`/storymap/universities/${u.id}`} className={s.chipLink}>
                            <Tag tone="neutral">{u.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
