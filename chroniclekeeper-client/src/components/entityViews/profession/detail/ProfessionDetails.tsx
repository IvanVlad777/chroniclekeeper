import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
    Tag,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    ProfessionDetailsDto,
    SocialClassDto,
    SpeciesDto,
    TradeSchoolDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addProfessionSocialClass,
    addProfessionSpecies,
    addProfessionTradeSchool,
    createApprenticeship,
    createJobRank,
    createSpecialisation,
    deleteApprenticeship,
    deleteJobRank,
    deleteSpecialisation,
    getProfessionById,
    removeProfessionSocialClass,
    removeProfessionSpecies,
    removeProfessionTradeSchool,
    updateApprenticeship,
    updateJobRank,
    updateSpecialisation,
} from "../../../../api/professions";
import { getSpecies } from "../../../../api/species";
import { getSocialClasses } from "../../../../api/socialClasses";
import { getTradeSchools } from "../../../../api/tradeSchools";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚒";

interface JobRankFormState {
    name: string;
    description: string;
    rankTitle: string;
    rankLevel: string;
    responsibilities: string;
}
const emptyJobRankForm: JobRankFormState = {
    name: "",
    description: "",
    rankTitle: "",
    rankLevel: "",
    responsibilities: "",
};

interface ApprenticeshipFormState {
    name: string;
    description: string;
    tradeSchoolId: string;
    durationYears: string;
    isPaid: boolean;
    skillsTaught: string;
}
const emptyApprenticeshipForm: ApprenticeshipFormState = {
    name: "",
    description: "",
    tradeSchoolId: "",
    durationYears: "",
    isPaid: false,
    skillsTaught: "",
};

interface SpecialisationFormState {
    name: string;
    description: string;
    field: string;
}
const emptySpecialisationForm: SpecialisationFormState = {
    name: "",
    description: "",
    field: "",
};

export default function ProfessionDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("profession");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [profession, setProfession] = useState<ProfessionDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [tradeSchoolsForForm, setTradeSchoolsForForm] = useState<
        TradeSchoolDto[]
    >([]);

    const [speciesCandidates, setSpeciesCandidates] = useState<
        SpeciesDto[] | null
    >(null);
    const [socialClassCandidates, setSocialClassCandidates] = useState<
        SocialClassDto[] | null
    >(null);
    const [tradeSchoolCandidates, setTradeSchoolCandidates] = useState<
        TradeSchoolDto[] | null
    >(null);

    // Rang: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [jobRankFormFor, setJobRankFormFor] = useState<number | null>(null);
    const [jobRankForm, setJobRankForm] =
        useState<JobRankFormState>(emptyJobRankForm);
    const [jobRankError, setJobRankError] = useState<string | null>(null);

    const [apprenticeshipFormFor, setApprenticeshipFormFor] = useState<
        number | null
    >(null);
    const [apprenticeshipForm, setApprenticeshipForm] =
        useState<ApprenticeshipFormState>(emptyApprenticeshipForm);
    const [apprenticeshipError, setApprenticeshipError] = useState<
        string | null
    >(null);

    const [specialisationFormFor, setSpecialisationFormFor] = useState<
        number | null
    >(null);
    const [specialisationForm, setSpecialisationForm] =
        useState<SpecialisationFormState>(emptySpecialisationForm);
    const [specialisationError, setSpecialisationError] = useState<
        string | null
    >(null);

    useEffect(() => {
        const professionId = Number(id);
        if (!professionId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getProfessionById(professionId)
            .then((data) => {
                if (cancelled) return;
                setProfession(data);
                return getTradeSchools({ worldId: data.worldId });
            })
            .then((schools) => {
                if (!cancelled && schools) setTradeSchoolsForForm(schools);
            })
            .catch((err) => {
                console.error("Failed to load profession:", err);
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

    // ----- Job ranks -----

    const openNewJobRank = () => {
        setJobRankForm(emptyJobRankForm);
        setJobRankError(null);
        setJobRankFormFor(0);
    };
    const openEditJobRank = (jr: ProfessionDetailsDto["jobRanks"][number]) => {
        setJobRankForm({
            name: jr.name ?? "",
            description: jr.description ?? "",
            rankTitle: jr.rankTitle ?? "",
            rankLevel: jr.rankLevel != null ? String(jr.rankLevel) : "",
            responsibilities: jr.responsibilities ?? "",
        });
        setJobRankError(null);
        setJobRankFormFor(jr.id);
    };
    async function onSaveJobRank(e: FormEvent) {
        e.preventDefault();
        if (!profession || jobRankFormFor === null) return;
        if (!jobRankForm.name.trim()) {
            setJobRankError(t("jobRanks.requiredMissing"));
            return;
        }
        setJobRankError(null);
        setBusy(true);
        try {
            const payload = {
                name: jobRankForm.name.trim(),
                description: jobRankForm.description,
                rankTitle: jobRankForm.rankTitle,
                rankLevel: jobRankForm.rankLevel.trim()
                    ? Number(jobRankForm.rankLevel)
                    : 0,
                responsibilities: jobRankForm.responsibilities,
            };
            if (jobRankFormFor === 0) {
                await createJobRank({ ...payload, professionId: profession.id });
            } else {
                await updateJobRank(jobRankFormFor, payload);
            }
            setJobRankFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save job rank:", err);
            setJobRankError(apiErrorMessage(err, t("jobRanks.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteJobRank(jrId: number, name: string) {
        if (!window.confirm(t("jobRanks.deleteConfirm", { name }))) return;
        setJobRankError(null);
        setBusy(true);
        try {
            await deleteJobRank(jrId);
            refetch();
        } catch (err) {
            console.error("Failed to delete job rank:", err);
            setJobRankError(apiErrorMessage(err, t("jobRanks.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    // ----- Apprenticeships -----

    const openNewApprenticeship = () => {
        setApprenticeshipForm(emptyApprenticeshipForm);
        setApprenticeshipError(null);
        setApprenticeshipFormFor(0);
    };
    const openEditApprenticeship = (
        a: ProfessionDetailsDto["apprenticeships"][number]
    ) => {
        setApprenticeshipForm({
            name: a.name ?? "",
            description: a.description ?? "",
            tradeSchoolId: a.tradeSchoolId ? String(a.tradeSchoolId) : "",
            durationYears:
                a.durationYears != null ? String(a.durationYears) : "",
            isPaid: a.isPaid,
            skillsTaught: a.skillsTaught ?? "",
        });
        setApprenticeshipError(null);
        setApprenticeshipFormFor(a.id);
    };
    async function onSaveApprenticeship(e: FormEvent) {
        e.preventDefault();
        if (!profession || apprenticeshipFormFor === null) return;
        if (!apprenticeshipForm.name.trim()) {
            setApprenticeshipError(t("apprenticeships.requiredMissing"));
            return;
        }
        setApprenticeshipError(null);
        setBusy(true);
        try {
            const payload = {
                name: apprenticeshipForm.name.trim(),
                description: apprenticeshipForm.description,
                tradeSchoolId: apprenticeshipForm.tradeSchoolId
                    ? Number(apprenticeshipForm.tradeSchoolId)
                    : null,
                durationYears: apprenticeshipForm.durationYears.trim()
                    ? Number(apprenticeshipForm.durationYears)
                    : 0,
                isPaid: apprenticeshipForm.isPaid,
                skillsTaught: apprenticeshipForm.skillsTaught,
            };
            if (apprenticeshipFormFor === 0) {
                await createApprenticeship({
                    ...payload,
                    professionId: profession.id,
                });
            } else {
                await updateApprenticeship(apprenticeshipFormFor, payload);
            }
            setApprenticeshipFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save apprenticeship:", err);
            setApprenticeshipError(
                apiErrorMessage(err, t("apprenticeships.saveFailed"))
            );
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteApprenticeship(aId: number, name: string) {
        if (!window.confirm(t("apprenticeships.deleteConfirm", { name })))
            return;
        setApprenticeshipError(null);
        setBusy(true);
        try {
            await deleteApprenticeship(aId);
            refetch();
        } catch (err) {
            console.error("Failed to delete apprenticeship:", err);
            setApprenticeshipError(
                apiErrorMessage(err, t("apprenticeships.deleteFailed"))
            );
        } finally {
            setBusy(false);
        }
    }

    // ----- Specialisations -----

    const openNewSpecialisation = () => {
        setSpecialisationForm(emptySpecialisationForm);
        setSpecialisationError(null);
        setSpecialisationFormFor(0);
    };
    const openEditSpecialisation = (
        sp: ProfessionDetailsDto["specialisations"][number]
    ) => {
        setSpecialisationForm({
            name: sp.name ?? "",
            description: sp.description ?? "",
            field: sp.field ?? "",
        });
        setSpecialisationError(null);
        setSpecialisationFormFor(sp.id);
    };
    async function onSaveSpecialisation(e: FormEvent) {
        e.preventDefault();
        if (!profession || specialisationFormFor === null) return;
        if (!specialisationForm.name.trim()) {
            setSpecialisationError(t("specialisations.requiredMissing"));
            return;
        }
        setSpecialisationError(null);
        setBusy(true);
        try {
            const payload = {
                name: specialisationForm.name.trim(),
                description: specialisationForm.description,
                field: specialisationForm.field,
            };
            if (specialisationFormFor === 0) {
                await createSpecialisation({
                    ...payload,
                    professionId: profession.id,
                });
            } else {
                await updateSpecialisation(specialisationFormFor, payload);
            }
            setSpecialisationFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save specialisation:", err);
            setSpecialisationError(
                apiErrorMessage(err, t("specialisations.saveFailed"))
            );
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteSpecialisation(spId: number, name: string) {
        if (!window.confirm(t("specialisations.deleteConfirm", { name })))
            return;
        setSpecialisationError(null);
        setBusy(true);
        try {
            await deleteSpecialisation(spId);
            refetch();
        } catch (err) {
            console.error("Failed to delete specialisation:", err);
            setSpecialisationError(
                apiErrorMessage(err, t("specialisations.deleteFailed"))
            );
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
                    <Link to="/storymap/professions" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !profession) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/professions">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{profession.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{profession.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/professions/${profession.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("fields.requiredSkills")}
                        value={profession.requiredSkills || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.workEnvironment")}
                        value={profession.workEnvironment || dash}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {profession.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {profession.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Job ranks ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("jobRanks.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && jobRankFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewJobRank}>
                        + {t("jobRanks.add")}
                    </button>
                )}
            </div>
            {profession.jobRanks.length === 0 && jobRankFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                profession.jobRanks.map((jr) => (
                    <div key={jr.id} className={s.childRow}>
                        <span className={s.childName}>{jr.name}</span>
                        <div className={s.childBody}>
                            {jr.description && <p className={s.childDesc}>{jr.description}</p>}
                            <p className={s.childMeta}>
                                {[jr.rankTitle, `Lv. ${jr.rankLevel}`, jr.responsibilities]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditJobRank(jr)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteJobRank(jr.id, jr.name)}>
                                    {t("jobRanks.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {jobRankError && jobRankFormFor === null && (
                <p className={s.miniError} role="alert">{jobRankError}</p>
            )}
            {jobRankFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveJobRank}>
                    <h3 className={s.childFormTitle}>
                        {jobRankFormFor === 0 ? t("jobRanks.addTitle") : t("jobRanks.editTitle")}
                    </h3>
                    <OrnateField label={t("jobRanks.name")} required>
                        <OrnateTextInput value={jobRankForm.name} display maxLength={100} onChange={(e) => setJobRankForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("jobRanks.description")}>
                        <OrnateTextArea value={jobRankForm.description} rows={2} maxLength={4000} onChange={(e) => setJobRankForm((f) => ({ ...f, description: e.target.value }))} />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("jobRanks.rankTitle")}>
                            <OrnateTextInput value={jobRankForm.rankTitle} maxLength={100} onChange={(e) => setJobRankForm((f) => ({ ...f, rankTitle: e.target.value }))} />
                        </OrnateField>
                        <OrnateField label={t("jobRanks.rankLevel")}>
                            <OrnateTextInput type="number" value={jobRankForm.rankLevel} onChange={(e) => setJobRankForm((f) => ({ ...f, rankLevel: e.target.value }))} />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("jobRanks.responsibilities")}>
                        <OrnateTextArea value={jobRankForm.responsibilities} rows={2} maxLength={1000} onChange={(e) => setJobRankForm((f) => ({ ...f, responsibilities: e.target.value }))} />
                    </OrnateField>
                    {jobRankError && <p className={s.miniError} role="alert">{jobRankError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setJobRankFormFor(null)}>{t("jobRanks.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("jobRanks.save")}</Button>
                    </div>
                </form>
            )}

            {/* ----- Apprenticeships ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("apprenticeships.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && apprenticeshipFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewApprenticeship}>
                        + {t("apprenticeships.add")}
                    </button>
                )}
            </div>
            {profession.apprenticeships.length === 0 && apprenticeshipFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                profession.apprenticeships.map((a) => (
                    <div key={a.id} className={s.childRow}>
                        <span className={s.childName}>{a.name}</span>
                        <div className={s.childBody}>
                            {a.description && <p className={s.childDesc}>{a.description}</p>}
                            <p className={s.childMeta}>
                                {[
                                    `${a.durationYears} ${t("apprenticeships.years")}`,
                                    a.isPaid ? t("apprenticeships.paid") : t("apprenticeships.unpaid"),
                                    a.tradeSchoolName,
                                    a.skillsTaught,
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditApprenticeship(a)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteApprenticeship(a.id, a.name)}>
                                    {t("apprenticeships.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {apprenticeshipError && apprenticeshipFormFor === null && (
                <p className={s.miniError} role="alert">{apprenticeshipError}</p>
            )}
            {apprenticeshipFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveApprenticeship}>
                    <h3 className={s.childFormTitle}>
                        {apprenticeshipFormFor === 0 ? t("apprenticeships.addTitle") : t("apprenticeships.editTitle")}
                    </h3>
                    <OrnateField label={t("apprenticeships.name")} required>
                        <OrnateTextInput value={apprenticeshipForm.name} display maxLength={100} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("apprenticeships.description")}>
                        <OrnateTextArea value={apprenticeshipForm.description} rows={2} maxLength={4000} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, description: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("apprenticeships.tradeSchool")}>
                        <OrnateSelect value={apprenticeshipForm.tradeSchoolId} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, tradeSchoolId: e.target.value }))}>
                            <option value="">{t("none")}</option>
                            {tradeSchoolsForForm.map((ts) => (
                                <option key={ts.id} value={ts.id}>{ts.name}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("apprenticeships.durationYears")}>
                            <OrnateTextInput type="number" min={0} value={apprenticeshipForm.durationYears} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, durationYears: e.target.value }))} />
                        </OrnateField>
                        <OrnateCheckbox label={t("apprenticeships.isPaid")} checked={apprenticeshipForm.isPaid} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, isPaid: e.target.checked }))} />
                    </div>
                    <OrnateField label={t("apprenticeships.skillsTaught")}>
                        <OrnateTextArea value={apprenticeshipForm.skillsTaught} rows={2} maxLength={1000} onChange={(e) => setApprenticeshipForm((f) => ({ ...f, skillsTaught: e.target.value }))} />
                    </OrnateField>
                    {apprenticeshipError && <p className={s.miniError} role="alert">{apprenticeshipError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setApprenticeshipFormFor(null)}>{t("apprenticeships.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("apprenticeships.save")}</Button>
                    </div>
                </form>
            )}

            {/* ----- Specialisations ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("specialisations.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && specialisationFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewSpecialisation}>
                        + {t("specialisations.add")}
                    </button>
                )}
            </div>
            {profession.specialisations.length === 0 && specialisationFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                profession.specialisations.map((sp) => (
                    <div key={sp.id} className={s.childRow}>
                        <span className={s.childName}>{sp.name}</span>
                        <div className={s.childBody}>
                            {sp.description && <p className={s.childDesc}>{sp.description}</p>}
                            {sp.field && <p className={s.childMeta}>{sp.field}</p>}
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditSpecialisation(sp)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteSpecialisation(sp.id, sp.name)}>
                                    {t("specialisations.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {specialisationError && specialisationFormFor === null && (
                <p className={s.miniError} role="alert">{specialisationError}</p>
            )}
            {specialisationFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveSpecialisation}>
                    <h3 className={s.childFormTitle}>
                        {specialisationFormFor === 0 ? t("specialisations.addTitle") : t("specialisations.editTitle")}
                    </h3>
                    <OrnateField label={t("specialisations.name")} required>
                        <OrnateTextInput value={specialisationForm.name} display maxLength={100} onChange={(e) => setSpecialisationForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("specialisations.description")}>
                        <OrnateTextArea value={specialisationForm.description} rows={2} maxLength={4000} onChange={(e) => setSpecialisationForm((f) => ({ ...f, description: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("specialisations.field")}>
                        <OrnateTextInput value={specialisationForm.field} maxLength={200} onChange={(e) => setSpecialisationForm((f) => ({ ...f, field: e.target.value }))} />
                    </OrnateField>
                    {specialisationError && <p className={s.miniError} role="alert">{specialisationError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setSpecialisationFormFor(null)}>{t("specialisations.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("specialisations.save")}</Button>
                    </div>
                </form>
            )}

            {/* ----- Cross-links ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.species")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={profession.practicedBySpecies}
                candidates={speciesCandidates}
                onLoadCandidates={() => getSpecies(profession.worldId).then(setSpeciesCandidates)}
                onAdd={(speciesId) => addProfessionSpecies(profession.id, speciesId)}
                onRemove={(speciesId) => removeProfessionSpecies(profession.id, speciesId)}
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
                <span className={s.sectionTitle}>{t("links.socialClasses")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={profession.socialClasses}
                candidates={socialClassCandidates}
                onLoadCandidates={() => getSocialClasses(profession.worldId).then(setSocialClassCandidates)}
                onAdd={(socialClassId) => addProfessionSocialClass(profession.id, socialClassId)}
                onRemove={(socialClassId) => removeProfessionSocialClass(profession.id, socialClassId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(socialClassId) => `/storymap/social-classes/${socialClassId}`}
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
                <span className={s.sectionTitle}>{t("links.tradeSchools")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={profession.tradeSchools}
                candidates={tradeSchoolCandidates}
                onLoadCandidates={() => getTradeSchools({ worldId: profession.worldId }).then(setTradeSchoolCandidates)}
                onAdd={(tradeSchoolId) => addProfessionTradeSchool(profession.id, tradeSchoolId)}
                onRemove={(tradeSchoolId) => removeProfessionTradeSchool(profession.id, tradeSchoolId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(tradeSchoolId) => `/storymap/schools/${tradeSchoolId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            {/* ----- Characters (read-only) ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("characters.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>
            {profession.characters.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.citizens}>
                    {profession.characters.map((c) => (
                        <Link key={c.id} to={`/storymap/characters/${c.id}`} className={s.citizenLink}>
                            <Tag tone="neutral">{c.name}</Tag>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    );
}
