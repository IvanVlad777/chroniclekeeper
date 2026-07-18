import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextInput,
    Tag,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    CharacterDto,
    UniversityDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addUniversityProfessor,
    addUniversityStudent,
    createUniversityMajor,
    deleteUniversityMajor,
    getUniversityById,
    removeUniversityProfessor,
    removeUniversityStudent,
    updateUniversityMajor,
} from "../../../../api/universities";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚛";

interface MajorFormState {
    name: string;
    description: string;
    majorName: string;
    degreeLevel: string;
}
const emptyMajorForm: MajorFormState = {
    name: "",
    description: "",
    majorName: "",
    degreeLevel: "",
};

export default function UniversityDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("university");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [university, setUniversity] = useState<UniversityDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [majorFormFor, setMajorFormFor] = useState<number | null>(null);
    const [majorForm, setMajorForm] = useState<MajorFormState>(emptyMajorForm);
    const [majorError, setMajorError] = useState<string | null>(null);
    const [charCandidates, setCharCandidates] = useState<CharacterDto[] | null>(
        null
    );

    useEffect(() => {
        const universityId = Number(id);
        if (!universityId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getUniversityById(universityId)
            .then((data) => {
                if (!cancelled) setUniversity(data);
            })
            .catch((err) => {
                console.error("Failed to load university:", err);
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

    const openNewMajor = () => {
        setMajorForm(emptyMajorForm);
        setMajorError(null);
        setMajorFormFor(0);
    };
    const openEditMajor = (m: UniversityDetailsDto["majors"][number]) => {
        setMajorForm({
            name: m.name ?? "",
            description: m.description ?? "",
            majorName: m.majorName ?? "",
            degreeLevel: m.degreeLevel ?? "",
        });
        setMajorError(null);
        setMajorFormFor(m.id);
    };
    async function onSaveMajor(e: FormEvent) {
        e.preventDefault();
        if (!university || majorFormFor === null) return;
        if (!majorForm.name.trim()) {
            setMajorError(t("majors.requiredMissing"));
            return;
        }
        setMajorError(null);
        setBusy(true);
        try {
            const payload = {
                name: majorForm.name.trim(),
                description: majorForm.description,
                majorName: majorForm.majorName,
                degreeLevel: majorForm.degreeLevel,
            };
            if (majorFormFor === 0) {
                await createUniversityMajor({
                    ...payload,
                    universityId: university.id,
                });
            } else {
                await updateUniversityMajor(majorFormFor, payload);
            }
            setMajorFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save major:", err);
            setMajorError(apiErrorMessage(err, t("majors.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteMajor(majorId: number, name: string) {
        if (!window.confirm(t("majors.deleteConfirm", { name }))) return;
        setMajorError(null);
        setBusy(true);
        try {
            await deleteUniversityMajor(majorId);
            refetch();
        } catch (err) {
            console.error("Failed to delete major:", err);
            setMajorError(apiErrorMessage(err, t("majors.deleteFailed")));
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
                    <Link to="/storymap/universities" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !university) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/universities">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{university.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{university.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/universities/${university.id}/edit`
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
                        label={t("fields.focusesOnScience")}
                        value={university.focusesOnScience ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.focusesOnMagic")}
                        value={university.focusesOnMagic ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.focusesOnPhilosophy")}
                        value={university.focusesOnPhilosophy ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.focusesOnMilitaryStudies")}
                        value={
                            university.focusesOnMilitaryStudies ? "✓" : dash
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {university.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {university.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("majors.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && majorFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewMajor}>
                        + {t("majors.add")}
                    </button>
                )}
            </div>
            {university.majors.length === 0 && majorFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                university.majors.map((m) => (
                    <div key={m.id} className={s.childRow}>
                        <span className={s.childName}>{m.majorName || m.name}</span>
                        <div className={s.childBody}>
                            {m.description && <p className={s.childDesc}>{m.description}</p>}
                            {m.degreeLevel && <p className={s.childMeta}>{m.degreeLevel}</p>}
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditMajor(m)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteMajor(m.id, m.name)}>
                                    {t("majors.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {majorError && majorFormFor === null && (
                <p className={s.miniError} role="alert">{majorError}</p>
            )}
            {majorFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveMajor}>
                    <h3 className={s.childFormTitle}>
                        {majorFormFor === 0 ? t("majors.addTitle") : t("majors.editTitle")}
                    </h3>
                    <OrnateField label={t("majors.name")} required>
                        <OrnateTextInput value={majorForm.name} display maxLength={100} onChange={(e) => setMajorForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("majors.majorName")}>
                        <OrnateTextInput value={majorForm.majorName} maxLength={200} onChange={(e) => setMajorForm((f) => ({ ...f, majorName: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("majors.degreeLevel")}>
                        <OrnateTextInput value={majorForm.degreeLevel} maxLength={100} onChange={(e) => setMajorForm((f) => ({ ...f, degreeLevel: e.target.value }))} />
                    </OrnateField>
                    {majorError && <p className={s.miniError} role="alert">{majorError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setMajorFormFor(null)}>{t("majors.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("majors.save")}</Button>
                    </div>
                </form>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("alumni.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {university.alumni.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chips}>
                    {university.alumni.map((a) => (
                        <Tag key={a.id} tone="neutral">{a.name}</Tag>
                    ))}
                </div>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("students.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={university.students}
                candidates={charCandidates}
                onLoadCandidates={() =>
                    getCharacters(university.worldId).then(setCharCandidates)
                }
                onAdd={(cid) => addUniversityStudent(university.id, cid)}
                onRemove={(cid) => removeUniversityStudent(university.id, cid)}
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
                <span className={s.sectionTitle}>{t("professors.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={university.professors}
                candidates={charCandidates}
                onLoadCandidates={() =>
                    getCharacters(university.worldId).then(setCharCandidates)
                }
                onAdd={(cid) => addUniversityProfessor(university.id, cid)}
                onRemove={(cid) => removeUniversityProfessor(university.id, cid)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                addLabel={t("professors.add")}
                noneLabel={t("none")}
                pickLabel={t("professors.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("professors.confirm")}
                removeLabel={(name) => t("professors.remove", { name })}
                addFailedLabel={t("professors.addFailed")}
                removeFailedLabel={t("professors.removeFailed")}
            />
        </div>
    );
}
