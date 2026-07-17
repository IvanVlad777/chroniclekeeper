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
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    CharacterDto,
    CorporationDetailsDto,
    FactionDto,
    ProfessionDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addCorporationFaction,
    addCorporationProfession,
    createCorporateLeadership,
    deleteCorporateLeadership,
    getCorporationById,
    removeCorporationFaction,
    removeCorporationProfession,
    updateCorporateLeadership,
} from "../../../../api/corporations";
import { getFactions } from "../../../../api/factions";
import { getProfessions } from "../../../../api/professions";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "🏢";

interface LeadershipFormState {
    name: string;
    description: string;
    position: string;
    salary: string;
    isMajorShareholder: boolean;
    professionId: string;
    characterId: string;
}
const emptyLeadershipForm: LeadershipFormState = {
    name: "",
    description: "",
    position: "",
    salary: "0",
    isMajorShareholder: false,
    professionId: "",
    characterId: "",
};

export default function CorporationDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("corporation");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [corporation, setCorporation] =
        useState<CorporationDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [factionCandidates, setFactionCandidates] = useState<
        FactionDto[] | null
    >(null);
    const [professionCandidates, setProfessionCandidates] = useState<
        ProfessionDto[] | null
    >(null);

    // Picker podaci za inline leadership formu
    const [professionsForForm, setProfessionsForForm] = useState<
        ProfessionDto[]
    >([]);
    const [charactersForForm, setCharactersForForm] = useState<CharacterDto[]>(
        []
    );

    // Vodstvo: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [leadershipFormFor, setLeadershipFormFor] = useState<number | null>(
        null
    );
    const [leadershipForm, setLeadershipForm] = useState<LeadershipFormState>(
        emptyLeadershipForm
    );
    const [leadershipError, setLeadershipError] = useState<string | null>(null);

    useEffect(() => {
        const corporationId = Number(id);
        if (!corporationId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getCorporationById(corporationId)
            .then((data) => {
                if (cancelled) return;
                setCorporation(data);
                return Promise.all([
                    getProfessions(data.worldId),
                    getCharacters(data.worldId),
                ]);
            })
            .then((pickerData) => {
                if (!cancelled && pickerData) {
                    setProfessionsForForm(pickerData[0]);
                    setCharactersForForm(pickerData[1]);
                }
            })
            .catch((err) => {
                console.error("Failed to load corporation:", err);
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

    // ----- Leadership -----

    const openNewLeadership = () => {
        setLeadershipForm(emptyLeadershipForm);
        setLeadershipError(null);
        setLeadershipFormFor(0);
    };
    const openEditLeadership = (
        l: CorporationDetailsDto["leadership"][number]
    ) => {
        setLeadershipForm({
            name: l.name ?? "",
            description: l.description ?? "",
            position: l.position ?? "",
            salary: String(l.salary ?? 0),
            isMajorShareholder: l.isMajorShareholder,
            professionId: l.professionId ? String(l.professionId) : "",
            characterId: l.characterId ? String(l.characterId) : "",
        });
        setLeadershipError(null);
        setLeadershipFormFor(l.id);
    };
    async function onSaveLeadership(e: FormEvent) {
        e.preventDefault();
        if (!corporation || leadershipFormFor === null) return;
        if (!leadershipForm.name.trim()) {
            setLeadershipError(t("leadership.requiredMissing"));
            return;
        }
        setLeadershipError(null);
        setBusy(true);
        try {
            const payload = {
                name: leadershipForm.name.trim(),
                description: leadershipForm.description,
                position: leadershipForm.position,
                salary: Number(leadershipForm.salary) || 0,
                isMajorShareholder: leadershipForm.isMajorShareholder,
                professionId: leadershipForm.professionId
                    ? Number(leadershipForm.professionId)
                    : null,
                characterId: leadershipForm.characterId
                    ? Number(leadershipForm.characterId)
                    : null,
            };
            if (leadershipFormFor === 0) {
                await createCorporateLeadership({
                    ...payload,
                    corporationId: corporation.id,
                });
            } else {
                await updateCorporateLeadership(leadershipFormFor, payload);
            }
            setLeadershipFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save corporate leadership entry:", err);
            setLeadershipError(apiErrorMessage(err, t("leadership.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteLeadership(lId: number, name: string) {
        if (!window.confirm(t("leadership.deleteConfirm", { name }))) return;
        setLeadershipError(null);
        setBusy(true);
        try {
            await deleteCorporateLeadership(lId);
            refetch();
        } catch (err) {
            console.error("Failed to delete corporate leadership entry:", err);
            setLeadershipError(
                apiErrorMessage(err, t("leadership.deleteFailed"))
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
                    <Link to="/storymap/corporations" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !corporation) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const refLink = (
        ref: { id: number; name: string } | null | undefined,
        path: string
    ) =>
        ref ? (
            <Link className={s.refLink} to={`/storymap/${path}/${ref.id}`}>
                {ref.name}
            </Link>
        ) : (
            dash
        );

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/corporations">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{corporation.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {corporation.industrySector || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{corporation.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/corporations/${corporation.id}/edit`
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
                        label={t("fields.revenue")}
                        value={corporation.revenue}
                    />
                    <OrnateDisplayBox
                        label={t("fields.numberOfEmployees")}
                        value={corporation.numberOfEmployees}
                    />
                    <OrnateDisplayBox
                        label={t("fields.flags")}
                        value={
                            [
                                corporation.isPubliclyTraded
                                    ? t("fields.isPubliclyTraded")
                                    : null,
                                corporation.isStateOwned
                                    ? t("fields.isStateOwned")
                                    : null,
                            ]
                                .filter(Boolean)
                                .join(" · ") || dash
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.parentCorporation")}
                        value={refLink(
                            corporation.parentCorporation,
                            "corporations"
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.industry")}
                        value={refLink(corporation.industry, "industries")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.taxationSystem")}
                        value={refLink(
                            corporation.taxationSystem,
                            "taxation-systems"
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.bankingSystem")}
                        value={refLink(
                            corporation.bankingSystem,
                            "banking-systems"
                        )}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {corporation.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {corporation.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Subsidiaries (read-only; set via subsidiary's own form) ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("subsidiaries.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {corporation.subsidiaries.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                corporation.subsidiaries.map((sub) => (
                    <div key={sub.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/corporations/${sub.id}`}
                        >
                            {sub.name}
                        </Link>
                    </div>
                ))
            )}

            {/* ----- Leadership ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("leadership.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && leadershipFormFor === null && (
                    <button
                        type="button"
                        className={s.addInline}
                        onClick={openNewLeadership}
                    >
                        + {t("leadership.add")}
                    </button>
                )}
            </div>
            {corporation.leadership.length === 0 &&
            leadershipFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                corporation.leadership.map((l) => (
                    <div key={l.id} className={s.childRow}>
                        <span className={s.childName}>{l.name}</span>
                        <div className={s.childBody}>
                            {l.description && (
                                <p className={s.childDesc}>{l.description}</p>
                            )}
                            <p className={s.childMeta}>
                                {[
                                    l.position,
                                    l.characterId && l.characterName
                                        ? l.characterName
                                        : null,
                                    l.professionName,
                                    l.isMajorShareholder
                                        ? t("leadership.majorShareholder")
                                        : null,
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                                {l.characterId && (
                                    <>
                                        {" "}
                                        <Link
                                            className={s.refLink}
                                            to={`/storymap/characters/${l.characterId}`}
                                        >
                                            →
                                        </Link>
                                    </>
                                )}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button
                                    type="button"
                                    className={s.childActionBtn}
                                    disabled={busy}
                                    onClick={() => openEditLeadership(l)}
                                >
                                    {t("form.edit")}
                                </button>
                                <button
                                    type="button"
                                    className={`${s.childActionBtn} ${s.childActionDanger}`}
                                    disabled={busy}
                                    onClick={() =>
                                        onDeleteLeadership(l.id, l.name)
                                    }
                                >
                                    {t("leadership.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {leadershipError && leadershipFormFor === null && (
                <p className={s.miniError} role="alert">
                    {leadershipError}
                </p>
            )}
            {leadershipFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveLeadership}>
                    <h3 className={s.childFormTitle}>
                        {leadershipFormFor === 0
                            ? t("leadership.addTitle")
                            : t("leadership.editTitle")}
                    </h3>
                    <OrnateField label={t("leadership.name")} required>
                        <OrnateTextInput
                            value={leadershipForm.name}
                            display
                            maxLength={100}
                            onChange={(e) =>
                                setLeadershipForm((f) => ({
                                    ...f,
                                    name: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("leadership.description")}>
                        <OrnateTextArea
                            value={leadershipForm.description}
                            rows={2}
                            maxLength={4000}
                            onChange={(e) =>
                                setLeadershipForm((f) => ({
                                    ...f,
                                    description: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("leadership.position")}>
                            <OrnateTextInput
                                value={leadershipForm.position}
                                maxLength={100}
                                placeholder={t("leadership.positionHint")}
                                onChange={(e) =>
                                    setLeadershipForm((f) => ({
                                        ...f,
                                        position: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("leadership.salary")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="any"
                                value={leadershipForm.salary}
                                onChange={(e) =>
                                    setLeadershipForm((f) => ({
                                        ...f,
                                        salary: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("leadership.character")}>
                            <OrnateSelect
                                value={leadershipForm.characterId}
                                onChange={(e) =>
                                    setLeadershipForm((f) => ({
                                        ...f,
                                        characterId: e.target.value,
                                    }))
                                }
                            >
                                <option value="">{t("none")}</option>
                                {charactersForForm.map((c) => (
                                    <option key={c.id} value={c.id}>
                                        {c.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("leadership.profession")}>
                            <OrnateSelect
                                value={leadershipForm.professionId}
                                onChange={(e) =>
                                    setLeadershipForm((f) => ({
                                        ...f,
                                        professionId: e.target.value,
                                    }))
                                }
                            >
                                <option value="">{t("none")}</option>
                                {professionsForForm.map((p) => (
                                    <option key={p.id} value={p.id}>
                                        {p.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("leadership.isMajorShareholder")}
                        checked={leadershipForm.isMajorShareholder}
                        onChange={(e) =>
                            setLeadershipForm((f) => ({
                                ...f,
                                isMajorShareholder: e.target.checked,
                            }))
                        }
                    />
                    {leadershipError && (
                        <p className={s.miniError} role="alert">
                            {leadershipError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setLeadershipFormFor(null)}
                        >
                            {t("leadership.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("leadership.save")}
                        </Button>
                    </div>
                </form>
            )}

            {/* ----- Cross-links ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.factions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={corporation.factions}
                candidates={factionCandidates}
                onLoadCandidates={() =>
                    getFactions(corporation.worldId).then(setFactionCandidates)
                }
                onAdd={(factionId) =>
                    addCorporationFaction(corporation.id, factionId)
                }
                onRemove={(factionId) =>
                    removeCorporationFaction(corporation.id, factionId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(factionId) => `/storymap/factions/${factionId}`}
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
                <span className={s.sectionTitle}>{t("links.professions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={corporation.memberProfessions}
                candidates={professionCandidates}
                onLoadCandidates={() =>
                    getProfessions(corporation.worldId).then(
                        setProfessionCandidates
                    )
                }
                onAdd={(professionId) =>
                    addCorporationProfession(corporation.id, professionId)
                }
                onRemove={(professionId) =>
                    removeCorporationProfession(corporation.id, professionId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(professionId) => `/storymap/professions/${professionId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <HistoryBlock
                targetType="Corporation"
                targetId={corporation.id}
                worldId={corporation.worldId}
                history={corporation.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
