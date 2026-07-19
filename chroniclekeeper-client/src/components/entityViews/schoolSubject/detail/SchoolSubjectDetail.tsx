import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    ReferenceDto,
    SchoolSubjectDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addSchoolSubjectTeacher,
    getSchoolSubjectById,
    removeSchoolSubjectTeacher,
} from "../../../../api/schools";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import s from "../../detailShared.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function SchoolSubjectDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("school");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<SchoolSubjectDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [candidates, setCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const sid = Number(id);
        if (!sid) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getSchoolSubjectById(sid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("subject.loadError"));
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
                title={t("subject.notFound")}
                action={
                    item?.school ? (
                        <Link to={`/storymap/schools/${item.school.id}`} className={s.backLink}>
                            ← {t("subject.backToSchool")}
                        </Link>
                    ) : undefined
                }
            />
        );
    }
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                {item.school && (
                    <>
                        <Link to={`/storymap/schools/${item.school.id}`}>{item.school.name}</Link>
                        <span className={s.breadcrumbSep}>/</span>
                    </>
                )}
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>📘 {t("subject.single")}</div>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox label={t("subject.fields.subjectName")} value={item.subjectName || dash} />
                    <OrnateDisplayBox
                        label={t("subject.fields.isMandatory")}
                        value={item.isMandatory ? t("subject.yes") : t("subject.no")}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("subject.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={s.prose}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("subject.none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("subject.teachers")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={item.teachers}
                candidates={candidates}
                onLoadCandidates={() => getCharacters(item.worldId).then(setCandidates)}
                onAdd={(characterId) => addSchoolSubjectTeacher(item.id, characterId)}
                onRemove={(characterId) => removeSchoolSubjectTeacher(item.id, characterId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("subject.link")}
                noneLabel={t("subject.none")}
                pickLabel={t("subject.pick")}
                cancelLabel={t("subject.cancel")}
                confirmLabel={t("subject.confirm")}
                removeLabel={(name) => t("subject.remove", { name })}
                addFailedLabel={t("subject.addFailed")}
                removeFailedLabel={t("subject.removeFailed")}
            />
        </div>
    );
}
