import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    CultureDetailsDto,
    NationDto,
    SocialClassDto,
    SpeciesDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addCultureNation,
    addCultureSapientSpecies,
    addCultureSocialClass,
    getCultureById,
    removeCultureNation,
    removeCultureSapientSpecies,
    removeCultureSocialClass,
} from "../../../../api/cultures";
import { getNations } from "../../../../api/nations";
import { getSpecies } from "../../../../api/species";
import { getSocialClasses } from "../../../../api/socialClasses";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "☉";

export default function CultureDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("culture");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [culture, setCulture] = useState<CultureDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [nationCandidates, setNationCandidates] = useState<
        NationDto[] | null
    >(null);
    const [speciesCandidates, setSpeciesCandidates] = useState<
        SpeciesDto[] | null
    >(null);
    const [socialClassCandidates, setSocialClassCandidates] = useState<
        SocialClassDto[] | null
    >(null);

    useEffect(() => {
        const cultureId = Number(id);
        if (!cultureId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getCultureById(cultureId)
            .then((data) => {
                if (!cancelled) setCulture(data);
            })
            .catch((err) => {
                console.error("Failed to load culture:", err);
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
                    <Link to="/storymap/cultures" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !culture) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = [
        t(`xenophobiaLevels.${culture.xenophobiaLevel}`),
        t(`technologicalLevels.${culture.technologicalLevel}`),
    ]
        .filter(Boolean)
        .join(" · ");

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/cultures">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{culture.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{kicker}</div>
                    <h1 className={s.name}>{culture.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/cultures/${culture.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.language")}
                        value={
                            culture.language ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/languages/${culture.language.id}`}
                                >
                                    {culture.language.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.religion")}
                        value={
                            culture.religion ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/religions/${culture.religion.id}`}
                                >
                                    {culture.religion.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.hasOralTradition")}
                        value={culture.hasOralTradition ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.xenophobiaLevel")}
                        value={t(
                            `xenophobiaLevels.${culture.xenophobiaLevel}`
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.technologicalLevel")}
                        value={t(
                            `technologicalLevels.${culture.technologicalLevel}`
                        )}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {culture.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {culture.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("fields.commonValues")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {culture.commonValues ? (
                <p className={s.prose}>{culture.commonValues}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("fields.socialStructure")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {culture.socialStructure ? (
                <p className={s.prose}>{culture.socialStructure}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("fields.conflictResolution")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {culture.conflictResolution ? (
                <p className={s.prose}>{culture.conflictResolution}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.nations")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={culture.nations}
                candidates={nationCandidates}
                onLoadCandidates={() =>
                    getNations(culture.worldId).then(setNationCandidates)
                }
                onAdd={(nationId) => addCultureNation(culture.id, nationId)}
                onRemove={(nationId) =>
                    removeCultureNation(culture.id, nationId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(nationId) => `/storymap/nations/${nationId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.species")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={culture.practicedBySpecies}
                candidates={speciesCandidates}
                onLoadCandidates={() =>
                    getSpecies(culture.worldId).then(setSpeciesCandidates)
                }
                onAdd={(speciesId) =>
                    addCultureSapientSpecies(culture.id, speciesId)
                }
                onRemove={(speciesId) =>
                    removeCultureSapientSpecies(culture.id, speciesId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(speciesId) => `/storymap/species/${speciesId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("links.socialClasses")}
                </span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={culture.influencedSocialClasses}
                candidates={socialClassCandidates}
                onLoadCandidates={() =>
                    getSocialClasses(culture.worldId).then(
                        setSocialClassCandidates
                    )
                }
                onAdd={(socialClassId) =>
                    addCultureSocialClass(culture.id, socialClassId)
                }
                onRemove={(socialClassId) =>
                    removeCultureSocialClass(culture.id, socialClassId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(socialClassId) =>
                    `/storymap/social-classes/${socialClassId}`
                }
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />
        </div>
    );
}
