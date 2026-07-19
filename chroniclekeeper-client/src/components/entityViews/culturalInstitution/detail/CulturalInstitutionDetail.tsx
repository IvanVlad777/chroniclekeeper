import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    CulturalInstitutionDetailsDto,
    ReferenceDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addCulturalInstitutionArtist,
    getCulturalInstitutionById,
    removeCulturalInstitutionArtist,
} from "../../../../api/culturalInstitutions";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import s from "../../detailShared.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function CulturalInstitutionDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("cultureDetails");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<CulturalInstitutionDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [candidates, setCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const cid = Number(id);
        if (!cid) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getCulturalInstitutionById(cid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("detail.loadError"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={5} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="⚑"
                title={t("detail.notFound")}
                action={
                    <Link to="/storymap/cultural-institutions" className={s.backLink}>
                        ← {t("culturalInstitution.backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/cultural-institutions">
                    {t("culturalInstitution.listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>🏛 {t("culturalInstitution.single")}</div>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("culturalInstitution.fields.institutionType")}
                        value={item.institutionType || dash}
                    />
                    <OrnateDisplayBox
                        label={t("culturalInstitution.fields.culture")}
                        value={
                            item.culture ? (
                                <Link className={s.memberLink} to={`/storymap/cultures/${item.culture.id}`}>
                                    {item.culture.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("culturalInstitution.fields.city")}
                        value={item.city ? item.city.name : dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("detail.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={s.prose}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("detail.none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("culturalInstitution.notableArtists")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={item.notableArtists}
                candidates={candidates}
                onLoadCandidates={() => getCharacters(item.worldId).then(setCandidates)}
                onAdd={(characterId) => addCulturalInstitutionArtist(item.id, characterId)}
                onRemove={(characterId) => removeCulturalInstitutionArtist(item.id, characterId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("culturalInstitution.link")}
                noneLabel={t("detail.none")}
                pickLabel={t("culturalInstitution.pick")}
                cancelLabel={t("culturalInstitution.cancel")}
                confirmLabel={t("culturalInstitution.confirm")}
                removeLabel={(name) => t("culturalInstitution.remove", { name })}
                addFailedLabel={t("culturalInstitution.addFailed")}
                removeFailedLabel={t("culturalInstitution.removeFailed")}
            />
        </div>
    );
}
