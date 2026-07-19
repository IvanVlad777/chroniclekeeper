import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextInput,
    Tag,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    CharacterDto,
    SchoolDetailsDto,
    TradeSchoolDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addSchoolStudent,
    addSchoolTeacher,
    createSchoolSubject,
    deleteSchoolSubject,
    getSchoolById,
    removeSchoolStudent,
    removeSchoolTeacher,
    updateSchoolSubject,
} from "../../../../api/schools";
import { getCharacters } from "../../../../api/characters";
import { getTradeSchoolById } from "../../../../api/tradeSchools";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "☰";

interface SubjectFormState {
    name: string;
    description: string;
    subjectName: string;
    isMandatory: boolean;
}
const emptySubjectForm: SubjectFormState = {
    name: "",
    description: "",
    subjectName: "",
    isMandatory: false,
};

export default function SchoolDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("school");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [school, setSchool] = useState<SchoolDetailsDto | null>(null);
    const [tradeSchool, setTradeSchool] = useState<TradeSchoolDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [subjectFormFor, setSubjectFormFor] = useState<number | null>(null);
    const [subjectForm, setSubjectForm] =
        useState<SubjectFormState>(emptySubjectForm);
    const [subjectError, setSubjectError] = useState<string | null>(null);
    const [charCandidates, setCharCandidates] = useState<CharacterDto[] | null>(
        null
    );

    useEffect(() => {
        const schoolId = Number(id);
        if (!schoolId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getSchoolById(schoolId)
            .then((data) => {
                if (cancelled) return;
                setSchool(data);
                if (data.schoolType === "TradeSchool") {
                    return getTradeSchoolById(schoolId).then((ts) => {
                        if (!cancelled) setTradeSchool(ts);
                    });
                }
                setTradeSchool(null);
            })
            .catch((err) => {
                console.error("Failed to load school:", err);
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

    const openNewSubject = () => {
        setSubjectForm(emptySubjectForm);
        setSubjectError(null);
        setSubjectFormFor(0);
    };
    const openEditSubject = (
        subj: SchoolDetailsDto["subjects"][number]
    ) => {
        setSubjectForm({
            name: subj.name ?? "",
            description: subj.description ?? "",
            subjectName: subj.subjectName ?? "",
            isMandatory: subj.isMandatory,
        });
        setSubjectError(null);
        setSubjectFormFor(subj.id);
    };
    async function onSaveSubject(e: FormEvent) {
        e.preventDefault();
        if (!school || subjectFormFor === null) return;
        if (!subjectForm.name.trim()) {
            setSubjectError(t("subjects.requiredMissing"));
            return;
        }
        setSubjectError(null);
        setBusy(true);
        try {
            const payload = {
                name: subjectForm.name.trim(),
                description: subjectForm.description,
                subjectName: subjectForm.subjectName,
                isMandatory: subjectForm.isMandatory,
            };
            if (subjectFormFor === 0) {
                await createSchoolSubject({ ...payload, schoolId: school.id });
            } else {
                await updateSchoolSubject(subjectFormFor, payload);
            }
            setSubjectFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save subject:", err);
            setSubjectError(apiErrorMessage(err, t("subjects.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteSubject(subjId: number, name: string) {
        if (!window.confirm(t("subjects.deleteConfirm", { name }))) return;
        setSubjectError(null);
        setBusy(true);
        try {
            await deleteSchoolSubject(subjId);
            refetch();
        } catch (err) {
            console.error("Failed to delete subject:", err);
            setSubjectError(apiErrorMessage(err, t("subjects.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link to="/storymap/schools" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !school) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const isTrade = school.schoolType === "TradeSchool";
    const editHref = isTrade
        ? `/storymap/schools/${school.id}/edit-trade`
        : `/storymap/schools/${school.id}/edit`;

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/schools">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{school.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {t(`schoolTypeLabel.${school.schoolType}`)}
                    </div>
                    <h1 className={s.name}>{school.name}</h1>
                </div>
                {canEdit && (
                    <Button variant="ghost" onClick={() => navigate(editHref)}>
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={isTrade ? 5 : 3}>
                    <OrnateDisplayBox
                        label={t("fields.isPublic")}
                        value={school.isPublic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isReligious")}
                        value={school.isReligious ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.location")}
                        value={
                            school.location ? (
                                <Link
                                    className={s.backLink}
                                    to={`/storymap/locations/${school.location.id}`}
                                >
                                    {school.location.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    {isTrade && tradeSchool && (
                        <>
                            <OrnateDisplayBox
                                label={t("fields.specialization")}
                                value={tradeSchool.specialization || dash}
                            />
                            <OrnateDisplayBox
                                label={t("fields.durationYears")}
                                value={String(tradeSchool.durationYears)}
                            />
                            <OrnateDisplayBox
                                label={t("fields.isGovernmentRecognized")}
                                value={
                                    tradeSchool.isGovernmentRecognized
                                        ? "✓"
                                        : dash
                                }
                            />
                        </>
                    )}
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {school.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{school.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("subjects.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && subjectFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewSubject}>
                        + {t("subjects.add")}
                    </button>
                )}
            </div>
            {school.subjects.length === 0 && subjectFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                school.subjects.map((subj) => (
                    <div key={subj.id} className={s.childRow}>
                        <Link to={`/storymap/school-subjects/${subj.id}`} className={s.childName}>
                            {subj.subjectName || subj.name}
                        </Link>
                        <div className={s.childBody}>
                            {subj.description && <p className={s.childDesc}>{subj.description}</p>}
                            <p className={s.childMeta}>
                                {subj.isMandatory ? t("subjects.mandatory") : t("subjects.optional")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditSubject(subj)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteSubject(subj.id, subj.name)}>
                                    {t("subjects.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {subjectError && subjectFormFor === null && (
                <p className={s.miniError} role="alert">{subjectError}</p>
            )}
            {subjectFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveSubject}>
                    <h3 className={s.childFormTitle}>
                        {subjectFormFor === 0 ? t("subjects.addTitle") : t("subjects.editTitle")}
                    </h3>
                    <OrnateField label={t("subjects.name")} required>
                        <OrnateTextInput value={subjectForm.name} display maxLength={100} onChange={(e) => setSubjectForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("subjects.subjectName")}>
                        <OrnateTextInput value={subjectForm.subjectName} maxLength={200} onChange={(e) => setSubjectForm((f) => ({ ...f, subjectName: e.target.value }))} />
                    </OrnateField>
                    <OrnateCheckbox label={t("subjects.isMandatory")} checked={subjectForm.isMandatory} onChange={(e) => setSubjectForm((f) => ({ ...f, isMandatory: e.target.checked }))} />
                    {subjectError && <p className={s.miniError} role="alert">{subjectError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setSubjectFormFor(null)}>{t("subjects.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("subjects.save")}</Button>
                    </div>
                </form>
            )}

            {isTrade && tradeSchool && (
                <>
                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("trainedProfessions.label")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    {tradeSchool.trainedProfessions.length === 0 ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        <div className={s.chips}>
                            {tradeSchool.trainedProfessions.map((p) => (
                                <Link key={p.id} to={`/storymap/professions/${p.id}`}>
                                    <Tag tone="neutral">{p.name}</Tag>
                                </Link>
                            ))}
                        </div>
                    )}

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("apprenticeships.label")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    {tradeSchool.apprenticeships.length === 0 ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        <div className={s.chips}>
                            {tradeSchool.apprenticeships.map((a) => (
                                <Tag key={a.id} tone="neutral">{a.name}</Tag>
                            ))}
                        </div>
                    )}
                </>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("alumni.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {school.alumni.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chips}>
                    {school.alumni.map((a) => (
                        <Tag key={a.id} tone="neutral">{a.name}</Tag>
                    ))}
                </div>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("students.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={school.students}
                candidates={charCandidates}
                onLoadCandidates={() =>
                    getCharacters(school.worldId).then(setCharCandidates)
                }
                onAdd={(cid) => addSchoolStudent(school.id, cid)}
                onRemove={(cid) => removeSchoolStudent(school.id, cid)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("students.add")}
                noneLabel={t("none")}
                pickLabel={t("students.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("students.confirm")}
                removeLabel={(name) => t("students.remove", { name })}
                addFailedLabel={t("students.addFailed")}
                removeFailedLabel={t("students.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("teachers.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={school.teachers}
                candidates={charCandidates}
                onLoadCandidates={() =>
                    getCharacters(school.worldId).then(setCharCandidates)
                }
                onAdd={(cid) => addSchoolTeacher(school.id, cid)}
                onRemove={(cid) => removeSchoolTeacher(school.id, cid)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("teachers.add")}
                noneLabel={t("none")}
                pickLabel={t("teachers.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("teachers.confirm")}
                removeLabel={(name) => t("teachers.remove", { name })}
                addFailedLabel={t("teachers.addFailed")}
                removeFailedLabel={t("teachers.removeFailed")}
            />
        </div>
    );
}
