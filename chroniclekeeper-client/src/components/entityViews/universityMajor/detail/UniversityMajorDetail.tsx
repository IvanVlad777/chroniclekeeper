import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    ReferenceDto,
    UniversityMajorDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addUniversityMajorProfessor,
    addUniversityMajorStudent,
    getUniversityMajorById,
    removeUniversityMajorProfessor,
    removeUniversityMajorStudent,
} from "../../../../api/universities";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import s from "../../detailShared.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function UniversityMajorDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("university");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<UniversityMajorDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [candidates, setCandidates] = useState<ReferenceDto[] | null>(null);

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
        getUniversityMajorById(mid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("major.loadError"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={5} />;
    if (notFound) return <EmptyState glyph="⚑" title={t("major.notFound")} />;
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                {item.university && (
                    <>
                        <Link to={`/storymap/universities/${item.university.id}`}>
                            {item.university.name}
                        </Link>
                        <span className={s.breadcrumbSep}>/</span>
                    </>
                )}
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>🎓 {t("major.single")}</div>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox label={t("major.fields.majorName")} value={item.majorName || dash} />
                    <OrnateDisplayBox label={t("major.fields.degreeLevel")} value={item.degreeLevel || dash} />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("major.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={s.prose}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("major.none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("major.professors")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={item.professors}
                candidates={candidates}
                onLoadCandidates={() => getCharacters(item.worldId).then(setCandidates)}
                onAdd={(cid) => addUniversityMajorProfessor(item.id, cid)}
                onRemove={(cid) => removeUniversityMajorProfessor(item.id, cid)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("major.link")}
                noneLabel={t("major.none")}
                pickLabel={t("major.pick")}
                cancelLabel={t("major.cancel")}
                confirmLabel={t("major.confirm")}
                removeLabel={(name) => t("major.remove", { name })}
                addFailedLabel={t("major.addFailed")}
                removeFailedLabel={t("major.removeFailed")}
            />

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("major.students")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={item.students}
                candidates={candidates}
                onLoadCandidates={() => getCharacters(item.worldId).then(setCandidates)}
                onAdd={(cid) => addUniversityMajorStudent(item.id, cid)}
                onRemove={(cid) => removeUniversityMajorStudent(item.id, cid)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("major.link")}
                noneLabel={t("major.none")}
                pickLabel={t("major.pick")}
                cancelLabel={t("major.cancel")}
                confirmLabel={t("major.confirm")}
                removeLabel={(name) => t("major.remove", { name })}
                addFailedLabel={t("major.addFailed")}
                removeFailedLabel={t("major.removeFailed")}
            />
        </div>
    );
}
