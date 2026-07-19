import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextInput,
    StatusPill,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TagEditor } from "../../../tagging/TagEditor";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import { AppearsInSection } from "../../content/AppearsInSection";
import {
    AbilityDto,
    CharacterDetailsDto,
    CharacterDto,
    ClothingDto,
    GuildDto,
    HobbyDto,
    RelationshipType,
    relationshipTypes,
    ReligionDto,
    SchoolDto,
    SpecialisationDto,
    UniversityDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addCharacterAbility,
    addCharacterClothing,
    addCharacterHobby,
    addCharacterSpecialisation,
    addRelationship,
    getCharacter,
    getCharacters,
    removeCharacterAbility,
    removeCharacterClothing,
    removeCharacterHobby,
    removeCharacterSpecialisation,
    removeRelationship,
} from "../../../../api/characters";
import { getAbilities } from "../../../../api/abilities";
import { getHobbies } from "../../../../api/hobbies";
import { getSpecialisations } from "../../../../api/professions";
import { getClothing } from "../../../../api/clothing";
import {
    createEducationRecord,
    deleteEducationRecord,
    updateEducationRecord,
} from "../../../../api/educationRecords";
import {
    createReligiousEducation,
    deleteReligiousEducation,
    updateReligiousEducation,
} from "../../../../api/religiousEducations";
import { getSchools } from "../../../../api/schools";
import { getUniversities } from "../../../../api/universities";
import { getGuilds } from "../../../../api/guilds";
import { getReligions } from "../../../../api/religions";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface EducationFormState {
    name: string;
    description: string;
    /** "" | `school:{id}` | `university:{id}` — jedan select za oba tipa ustanove. */
    institution: string;
    startDate: string;
    endDate: string;
    degree: string;
}

const emptyEducationForm: EducationFormState = {
    name: "",
    description: "",
    institution: "",
    startDate: "",
    endDate: "",
    degree: "",
};

interface ReligiousEducationFormState {
    name: string;
    description: string;
    religionId: string;
    startDate: string;
    completionDate: string;
    ordained: boolean;
}

const emptyReligiousEducationForm: ReligiousEducationFormState = {
    name: "",
    description: "",
    religionId: "",
    startDate: "",
    completionDate: "",
    ordained: false,
};

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function CharacterDetail() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("character");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [character, setCharacter] = useState<CharacterDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Likovi svijeta za select nove veze (učitava se samo za editore)
    const [worldCharacters, setWorldCharacters] = useState<CharacterDto[]>([]);
    const [worldSchools, setWorldSchools] = useState<SchoolDto[]>([]);
    const [worldUniversities, setWorldUniversities] = useState<
        UniversityDto[]
    >([]);
    const [worldReligions, setWorldReligions] = useState<ReligionDto[]>([]);
    const [worldGuilds, setWorldGuilds] = useState<GuildDto[]>([]);
    const [abilityCandidates, setAbilityCandidates] = useState<
        AbilityDto[] | null
    >(null);
    const [hobbyCandidates, setHobbyCandidates] = useState<HobbyDto[] | null>(
        null
    );
    const [specialisationCandidates, setSpecialisationCandidates] = useState<
        SpecialisationDto[] | null
    >(null);
    const [clothingCandidates, setClothingCandidates] = useState<
        ClothingDto[] | null
    >(null);

    // Inline forma za novu vezu
    const [relOpen, setRelOpen] = useState(false);
    const [relTarget, setRelTarget] = useState("");
    const [relType, setRelType] = useState<RelationshipType>("Friend");
    const [relDescription, setRelDescription] = useState("");
    const [relSecret, setRelSecret] = useState(false);
    const [relError, setRelError] = useState<string | null>(null);

    // Inline forma obrazovanja: null = zatvorena, 0 = novo, >0 = edit tog id-a
    const [eduFormFor, setEduFormFor] = useState<number | null>(null);
    const [eduForm, setEduForm] = useState<EducationFormState>(
        emptyEducationForm
    );
    const [eduError, setEduError] = useState<string | null>(null);

    // Inline forma vjerskog obrazovanja
    const [relEduFormFor, setRelEduFormFor] = useState<number | null>(null);
    const [relEduForm, setRelEduForm] = useState<ReligiousEducationFormState>(
        emptyReligiousEducationForm
    );
    const [relEduError, setRelEduError] = useState<string | null>(null);

    const [busy, setBusy] = useState(false);

    useEffect(() => {
        const characterId = Number(id);
        if (!characterId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getCharacter(characterId)
            .then((data) => {
                if (!cancelled) setCharacter(data);
            })
            .catch((err) => {
                console.error("Failed to load character:", err);
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

    // Editorima trebaju likovi svijeta za add-formu veze
    useEffect(() => {
        if (!canEdit || !character) return;
        let cancelled = false;
        getCharacters(character.worldId)
            .then((chars) => {
                if (!cancelled) setWorldCharacters(chars);
            })
            .catch((err) =>
                console.error("Failed to load world characters:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [canEdit, character?.id, character?.worldId, reloadKey]); // eslint-disable-line react-hooks/exhaustive-deps

    // Editorima trebaju škole/sveučilišta/religije za forme obrazovanja
    useEffect(() => {
        if (!canEdit || !character) return;
        let cancelled = false;
        Promise.all([
            getSchools({ worldId: character.worldId }),
            getUniversities({ worldId: character.worldId }),
            getReligions(character.worldId),
            getGuilds(character.worldId),
        ])
            .then(([schools, universities, religions, guilds]) => {
                if (cancelled) return;
                setWorldSchools(schools);
                setWorldUniversities(universities);
                setWorldReligions(religions);
                setWorldGuilds(guilds);
            })
            .catch((err) =>
                console.error("Failed to load education form data:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [canEdit, character?.id, character?.worldId, reloadKey]); // eslint-disable-line react-hooks/exhaustive-deps

    async function onAddRelationship(e: FormEvent) {
        e.preventDefault();
        if (!character || !relTarget) return;
        setRelError(null);
        setBusy(true);
        try {
            await addRelationship(character.id, {
                relatedCharacterId: Number(relTarget),
                type: relType,
                description: relDescription.trim(),
                isSecret: relSecret,
            });
            setRelOpen(false);
            setRelTarget("");
            setRelType("Friend");
            setRelDescription("");
            setRelSecret(false);
            refetch();
        } catch (err) {
            console.error("Failed to add relationship:", err);
            setRelError(apiErrorMessage(err, t("rel.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onRemoveRelationship(relationshipId: number) {
        if (!character) return;
        setBusy(true);
        try {
            await removeRelationship(character.id, relationshipId);
            refetch();
        } catch (err) {
            console.error("Failed to remove relationship:", err);
            setRelError(apiErrorMessage(err, t("rel.removeFailed")));
            setBusy(false);
        }
    }

    // ----- Obrazovanje (EducationRecord) -----

    const openNewEducation = () => {
        setEduForm(emptyEducationForm);
        setEduError(null);
        setEduFormFor(0);
    };

    async function onSaveEducation(e: FormEvent) {
        e.preventDefault();
        if (!character || eduFormFor === null) return;
        setEduError(null);
        setBusy(true);
        try {
            const [instType, instId] = eduForm.institution.split(":");
            const payload = {
                name: eduForm.name.trim() || eduForm.degree.trim() || t("education.label"),
                description: eduForm.description,
                characterId: character.id,
                schoolId: instType === "school" ? Number(instId) : null,
                universityId: instType === "university" ? Number(instId) : null,
                guildId: instType === "guild" ? Number(instId) : null,
                startDate: eduForm.startDate,
                endDate: eduForm.endDate || null,
                degree: eduForm.degree.trim(),
            };
            if (eduFormFor === 0) {
                await createEducationRecord({ ...payload, worldId: character.worldId });
            } else {
                await updateEducationRecord(eduFormFor, payload);
            }
            setEduFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save education record:", err);
            setEduError(apiErrorMessage(err, t("education.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDeleteEducation(id: number) {
        if (!window.confirm(t("education.deleteConfirm"))) return;
        setEduError(null);
        setBusy(true);
        try {
            await deleteEducationRecord(id);
            refetch();
        } catch (err) {
            console.error("Failed to delete education record:", err);
            setEduError(apiErrorMessage(err, t("education.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    // ----- Vjersko obrazovanje (ReligiousEducation) -----

    const openNewReligiousEducation = () => {
        setRelEduForm(emptyReligiousEducationForm);
        setRelEduError(null);
        setRelEduFormFor(0);
    };

    async function onSaveReligiousEducation(e: FormEvent) {
        e.preventDefault();
        if (!character || relEduFormFor === null) return;
        if (!relEduForm.religionId) {
            setRelEduError(t("religiousEducation.requiredMissing"));
            return;
        }
        setRelEduError(null);
        setBusy(true);
        try {
            if (relEduFormFor === 0) {
                await createReligiousEducation({
                    name: relEduForm.name.trim() || t("religiousEducation.label"),
                    description: relEduForm.description,
                    religionId: Number(relEduForm.religionId),
                    characterId: character.id,
                    startDate: relEduForm.startDate,
                    completionDate: relEduForm.completionDate || null,
                    ordained: relEduForm.ordained,
                });
            } else {
                await updateReligiousEducation(relEduFormFor, {
                    name: relEduForm.name.trim() || t("religiousEducation.label"),
                    description: relEduForm.description,
                    characterId: character.id,
                    startDate: relEduForm.startDate,
                    completionDate: relEduForm.completionDate || null,
                    ordained: relEduForm.ordained,
                });
            }
            setRelEduFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save religious education:", err);
            setRelEduError(apiErrorMessage(err, t("religiousEducation.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDeleteReligiousEducation(id: number) {
        if (!window.confirm(t("religiousEducation.deleteConfirm"))) return;
        setRelEduError(null);
        setBusy(true);
        try {
            await deleteReligiousEducation(id);
            refetch();
        } catch (err) {
            console.error("Failed to delete religious education:", err);
            setRelEduError(
                apiErrorMessage(err, t("religiousEducation.deleteFailed"))
            );
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="♟"
                title={t("notfound")}
                action={
                    <Link to="/storymap/characters" className={s.relLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !character) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    // Fictional in-world dates are free text now — display as-is.
    const fmtDate = (d?: string | null) => (d && d.trim() ? d.trim() : null);
    const lifespan =
        [fmtDate(character.birthDate), fmtDate(character.deathDate)]
            .filter(Boolean)
            .join(" — ") || dash;
    const kicker = [character.species?.name, character.race?.name]
        .filter(Boolean)
        .join(" · ");
    const status = character.deathDate ? "dead" : "living";

    const parentLink = (ref?: { id: number; name: string } | null) =>
        ref ? (
            <Link to={`/storymap/characters/${ref.id}`} className={s.relLink}>
                {ref.name}
            </Link>
        ) : (
            dash
        );

    const relCandidates = worldCharacters.filter(
        (c) => c.id !== character.id
    );

    return (
        <div className={s.page}>
            <div className={s.header}>
                <div className={s.breadcrumb}>
                    <Link to="/storymap/characters">{t("listTitle")}</Link>
                    <span className={s.breadcrumbSep}>/</span>
                    <span className={s.breadcrumbCurrent}>
                        {character.name}
                    </span>
                </div>
                <div className={s.headerRow}>
                    <div className={s.headerMain}>
                        {kicker && <div className={s.kicker}>{kicker}</div>}
                        <h1 className={s.name}>{character.name}</h1>
                        {character.title && (
                            <div className={s.epithet}>{character.title}</div>
                        )}
                    </div>
                    <div className={s.headerActions}>
                        <StatusPill status={status}>
                            {t(`status.${status}`)}
                        </StatusPill>
                        {canEdit && (
                            <Button
                                variant="ghost"
                                onClick={() =>
                                    navigate(
                                        `/storymap/characters/${character.id}/edit`
                                    )
                                }
                            >
                                {t("form.edit")}
                            </Button>
                        )}
                    </div>
                </div>
                <div className={s.flourish}>
                    <span className={s.flourishStar}>✦</span>
                    <span className={s.flourishLine} />
                </div>
            </div>

            <div className={s.body}>
                <div className={s.side}>
                    <div className={s.portrait} aria-hidden="true">
                        {character.name?.charAt(0)}
                    </div>
                    <div className={s.facts}>
                        <OrnateDisplayBox
                            label={t("lifespan")}
                            value={lifespan}
                        />
                        <OrnateDisplayBox
                            label={t("haircolor")}
                            value={character.hairColor || dash}
                        />
                        <OrnateDisplayBox
                            label={t("eyecolor")}
                            value={character.eyeColor || dash}
                        />
                        <OrnateDisplayBox
                            label={t("height")}
                            value={
                                character.height != null
                                    ? String(character.height)
                                    : dash
                            }
                        />
                        <OrnateDisplayBox
                            label={t("weight")}
                            value={
                                character.weight != null
                                    ? String(character.weight)
                                    : dash
                            }
                        />
                        <OrnateDisplayBox
                            label={t("father")}
                            value={parentLink(character.father)}
                        />
                        <OrnateDisplayBox
                            label={t("mother")}
                            value={parentLink(character.mother)}
                        />
                        <OrnateDisplayBox
                            label={t("socialClass")}
                            value={
                                character.socialClass ? (
                                    <Link
                                        to={`/storymap/social-classes/${character.socialClass.id}`}
                                        className={s.relLink}
                                    >
                                        {character.socialClass.name}
                                    </Link>
                                ) : (
                                    dash
                                )
                            }
                        />
                        <OrnateDisplayBox
                            label={t("nation")}
                            value={
                                character.nation ? (
                                    <Link
                                        to={`/storymap/nations/${character.nation.id}`}
                                        className={s.relLink}
                                    >
                                        {character.nation.name}
                                    </Link>
                                ) : (
                                    dash
                                )
                            }
                        />
                        <OrnateDisplayBox
                            label={t("religion")}
                            value={
                                character.religion ? (
                                    <Link
                                        to={`/storymap/religions/${character.religion.id}`}
                                        className={s.relLink}
                                    >
                                        {character.religion.name}
                                    </Link>
                                ) : (
                                    dash
                                )
                            }
                        />
                        <OrnateDisplayBox
                            label={t("profession.label")}
                            value={
                                character.profession ? (
                                    <Link
                                        to={`/storymap/professions/${character.profession.id}`}
                                        className={s.relLink}
                                    >
                                        {character.profession.name}
                                    </Link>
                                ) : (
                                    dash
                                )
                            }
                        />
                    </div>

                    <TagEditor
                        worldId={character.worldId}
                        targetType="Character"
                        targetId={character.id}
                        tags={character.tags}
                        canEdit={canEdit}
                        onChanged={refetch}
                    />

                    <HistoryBlock
                        targetType="Character"
                        targetId={character.id}
                        worldId={character.worldId}
                        history={character.history}
                        canEdit={canEdit}
                        onChanged={refetch}
                    />
                    <AppearsInSection
                        worldId={character.worldId}
                        entityType="Character"
                        entityId={character.id}
                    />
                </div>

                <div>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("description")}
                        </span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                    </div>
                    {character.description ? (
                        <p className={`${s.prose} ${s.dropCap}`}>
                            {character.description}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("features")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    {character.specialPhysicalFeatures ? (
                        <p className={s.prose}>
                            {character.specialPhysicalFeatures}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}

                    {(() => {
                        const bg = character.background;
                        const pr = character.personality;
                        const bgRows: [string, string][] = [
                            [t("background.familyStatus"), bg.familyStatus],
                            [t("background.childhood"), bg.childhood],
                            [t("background.upbringing"), bg.upbringing],
                            [
                                t("background.isImmigrant"),
                                bg.isImmigrant ? "✓" : "",
                            ],
                            [
                                t("background.migrationHistory"),
                                bg.migrationHistory,
                            ],
                        ].filter(([, v]) => v) as [string, string][];
                        const prRows: [string, string][] = [
                            [
                                t("personality.personalityTraits"),
                                pr.personalityTraits,
                            ],
                            [t("personality.motivations"), pr.motivations],
                            [t("personality.virtues"), pr.virtues],
                            [t("personality.flaws"), pr.flaws],
                            [t("personality.fears"), pr.fears],
                            [t("personality.ambitions"), pr.ambitions],
                            [
                                t("personality.psychologicalProfile"),
                                pr.psychologicalProfile,
                            ],
                        ].filter(([, v]) => v) as [string, string][];
                        if (bgRows.length === 0 && prRows.length === 0) {
                            return null;
                        }
                        return (
                            <>
                                {bgRows.length > 0 && (
                                    <>
                                        <div
                                            className={`${s.sectionHead} ${s.sectionSpacer}`}
                                        >
                                            <span className={s.sectionTitle}>
                                                {t("background.legend")}
                                            </span>
                                            <span className={s.sectionLine} />
                                        </div>
                                        {bgRows.map(([label, value]) => (
                                            <p key={label} className={s.prose}>
                                                <strong>{label}:</strong>{" "}
                                                {value}
                                            </p>
                                        ))}
                                    </>
                                )}
                                {prRows.length > 0 && (
                                    <>
                                        <div
                                            className={`${s.sectionHead} ${s.sectionSpacer}`}
                                        >
                                            <span className={s.sectionTitle}>
                                                {t("personality.legend")}
                                            </span>
                                            <span className={s.sectionLine} />
                                        </div>
                                        {prRows.map(([label, value]) => (
                                            <p key={label} className={s.prose}>
                                                <strong>{label}:</strong>{" "}
                                                {value}
                                            </p>
                                        ))}
                                    </>
                                )}
                            </>
                        );
                    })()}

                    <div className={s.linkGrid}>
                        <div>
                            <div className={s.listLabel}>{t("factions")}</div>
                            {character.factions.length === 0 ? (
                                <p className={s.none}>{t("none")}</p>
                            ) : (
                                character.factions.map((f) => (
                                    <div key={f.id} className={s.listRow}>
                                        <span className={s.listThumb} />
                                        <span className={s.listName}>
                                            {f.name}
                                        </span>
                                    </div>
                                ))
                            )}
                        </div>
                        <div>
                            <div className={s.listLabel}>
                                {t("relationships")}
                                {canEdit && !relOpen && (
                                    <button
                                        type="button"
                                        className={s.addInline}
                                        onClick={() => setRelOpen(true)}
                                    >
                                        + {t("rel.add")}
                                    </button>
                                )}
                            </div>
                            {character.relationships.length === 0 &&
                            !relOpen ? (
                                <p className={s.none}>{t("none")}</p>
                            ) : (
                                character.relationships.map((r) => (
                                    <div key={r.id} className={s.listRow}>
                                        <span className={s.relType}>
                                            {t(`relTypes.${r.type}`, {
                                                defaultValue: r.type,
                                            })}
                                        </span>
                                        <Link
                                            to={`/storymap/characters/${r.relatedCharacterId}`}
                                            className={s.relLink}
                                        >
                                            {r.relatedCharacterName}
                                        </Link>
                                        {canEdit && (
                                            <button
                                                type="button"
                                                className={s.chipRemove}
                                                aria-label={t("rel.remove")}
                                                disabled={busy}
                                                onClick={() =>
                                                    onRemoveRelationship(r.id)
                                                }
                                            >
                                                ×
                                            </button>
                                        )}
                                    </div>
                                ))
                            )}
                            {relOpen && (
                                <form
                                    className={s.miniForm}
                                    onSubmit={onAddRelationship}
                                >
                                    <OrnateField
                                        label={t("rel.target")}
                                        required
                                    >
                                        <OrnateSelect
                                            value={relTarget}
                                            onChange={(e) =>
                                                setRelTarget(e.target.value)
                                            }
                                        >
                                            <option value="">
                                                {t("none")}
                                            </option>
                                            {relCandidates.map((c) => (
                                                <option
                                                    key={c.id}
                                                    value={c.id}
                                                >
                                                    {c.name}
                                                </option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                    <OrnateField label={t("rel.type")}>
                                        <OrnateSelect
                                            value={relType}
                                            onChange={(e) =>
                                                setRelType(
                                                    e.target
                                                        .value as RelationshipType
                                                )
                                            }
                                        >
                                            {relationshipTypes.map((rt) => (
                                                <option key={rt} value={rt}>
                                                    {t(`relTypes.${rt}`)}
                                                </option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                    <OrnateField
                                        label={t("rel.description")}
                                    >
                                        <OrnateTextInput
                                            value={relDescription}
                                            maxLength={500}
                                            onChange={(e) =>
                                                setRelDescription(
                                                    e.target.value
                                                )
                                            }
                                        />
                                    </OrnateField>
                                    <OrnateCheckbox
                                        label={t("rel.secret")}
                                        checked={relSecret}
                                        onChange={(e) =>
                                            setRelSecret(e.target.checked)
                                        }
                                    />
                                    {relError && (
                                        <p
                                            className={s.miniError}
                                            role="alert"
                                        >
                                            {relError}
                                        </p>
                                    )}
                                    <div className={s.miniActions}>
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            disabled={busy}
                                            onClick={() => {
                                                setRelOpen(false);
                                                setRelError(null);
                                            }}
                                        >
                                            {t("form.cancel")}
                                        </Button>
                                        <Button
                                            type="submit"
                                            size="sm"
                                            disabled={busy || !relTarget}
                                        >
                                            {t("rel.addConfirm")}
                                        </Button>
                                    </div>
                                </form>
                            )}
                            {!relOpen && relError && (
                                <p className={s.miniError} role="alert">
                                    {relError}
                                </p>
                            )}
                        </div>
                    </div>

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("education.label")}
                        </span>
                        <span className={s.sectionLine} />
                        {canEdit && eduFormFor === null && (
                            <button
                                type="button"
                                className={s.addInline}
                                onClick={openNewEducation}
                            >
                                + {t("education.add")}
                            </button>
                        )}
                    </div>
                    {character.educations.length === 0 && eduFormFor === null ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        character.educations.map((edu) => (
                            <div key={edu.id} className={s.listRow}>
                                <span className={s.relType}>
                                    {edu.degree || edu.name}
                                </span>
                                <span className={s.listName}>
                                    {edu.schoolId
                                        ? worldSchools.find(
                                              (sc) => sc.id === edu.schoolId
                                          )?.name
                                        : edu.universityId
                                        ? worldUniversities.find(
                                              (u) => u.id === edu.universityId
                                          )?.name
                                        : edu.guildId
                                        ? worldGuilds.find(
                                              (g) => g.id === edu.guildId
                                          )?.name
                                        : dash}
                                </span>
                                {canEdit && (
                                    <button
                                        type="button"
                                        className={s.chipRemove}
                                        aria-label={t("education.delete")}
                                        disabled={busy}
                                        onClick={() => onDeleteEducation(edu.id)}
                                    >
                                        ×
                                    </button>
                                )}
                            </div>
                        ))
                    )}
                    {eduFormFor !== null && (
                        <form className={s.miniForm} onSubmit={onSaveEducation}>
                            <OrnateField label={t("education.institution")}>
                                <OrnateSelect
                                    value={eduForm.institution}
                                    onChange={(e) =>
                                        setEduForm((f) => ({
                                            ...f,
                                            institution: e.target.value,
                                        }))
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {worldSchools.map((sc) => (
                                        <option
                                            key={`school:${sc.id}`}
                                            value={`school:${sc.id}`}
                                        >
                                            {sc.name}
                                        </option>
                                    ))}
                                    {worldUniversities.map((u) => (
                                        <option
                                            key={`university:${u.id}`}
                                            value={`university:${u.id}`}
                                        >
                                            {u.name}
                                        </option>
                                    ))}
                                    {worldGuilds.map((g) => (
                                        <option
                                            key={`guild:${g.id}`}
                                            value={`guild:${g.id}`}
                                        >
                                            {g.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("education.degree")}>
                                <OrnateTextInput
                                    value={eduForm.degree}
                                    maxLength={100}
                                    onChange={(e) =>
                                        setEduForm((f) => ({
                                            ...f,
                                            degree: e.target.value,
                                        }))
                                    }
                                />
                            </OrnateField>
                            <div className={s.row2}>
                                <OrnateField label={t("education.startDate")}>
                                    <OrnateTextInput
                                        type="date"
                                        value={eduForm.startDate}
                                        onChange={(e) =>
                                            setEduForm((f) => ({
                                                ...f,
                                                startDate: e.target.value,
                                            }))
                                        }
                                    />
                                </OrnateField>
                                <OrnateField label={t("education.endDate")}>
                                    <OrnateTextInput
                                        type="date"
                                        value={eduForm.endDate}
                                        onChange={(e) =>
                                            setEduForm((f) => ({
                                                ...f,
                                                endDate: e.target.value,
                                            }))
                                        }
                                    />
                                </OrnateField>
                            </div>
                            {eduError && (
                                <p className={s.miniError} role="alert">
                                    {eduError}
                                </p>
                            )}
                            <div className={s.miniActions}>
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    disabled={busy}
                                    onClick={() => setEduFormFor(null)}
                                >
                                    {t("form.cancel")}
                                </Button>
                                <Button type="submit" size="sm" disabled={busy}>
                                    {t("education.save")}
                                </Button>
                            </div>
                        </form>
                    )}
                    {!eduFormFor && eduError && (
                        <p className={s.miniError} role="alert">
                            {eduError}
                        </p>
                    )}

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("religiousEducation.label")}
                        </span>
                        <span className={s.sectionLine} />
                        {canEdit && relEduFormFor === null && (
                            <button
                                type="button"
                                className={s.addInline}
                                onClick={openNewReligiousEducation}
                            >
                                + {t("religiousEducation.add")}
                            </button>
                        )}
                    </div>
                    {character.religiousEducations.length === 0 &&
                    relEduFormFor === null ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        character.religiousEducations.map((re) => (
                            <div key={re.id} className={s.listRow}>
                                <span className={s.relType}>
                                    {re.ordained
                                        ? t("religiousEducation.ordained")
                                        : t("religiousEducation.label")}
                                </span>
                                <span className={s.listName}>
                                    {worldReligions.find(
                                        (r) => r.id === re.religionId
                                    )?.name ?? dash}
                                </span>
                                {canEdit && (
                                    <button
                                        type="button"
                                        className={s.chipRemove}
                                        aria-label={t("religiousEducation.delete")}
                                        disabled={busy}
                                        onClick={() =>
                                            onDeleteReligiousEducation(re.id)
                                        }
                                    >
                                        ×
                                    </button>
                                )}
                            </div>
                        ))
                    )}
                    {relEduFormFor !== null && (
                        <form
                            className={s.miniForm}
                            onSubmit={onSaveReligiousEducation}
                        >
                            <OrnateField
                                label={t("religiousEducation.religion")}
                                required
                            >
                                <OrnateSelect
                                    value={relEduForm.religionId}
                                    onChange={(e) =>
                                        setRelEduForm((f) => ({
                                            ...f,
                                            religionId: e.target.value,
                                        }))
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {worldReligions.map((r) => (
                                        <option key={r.id} value={r.id}>
                                            {r.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <div className={s.row2}>
                                <OrnateField
                                    label={t("religiousEducation.startDate")}
                                >
                                    <OrnateTextInput
                                        type="date"
                                        value={relEduForm.startDate}
                                        onChange={(e) =>
                                            setRelEduForm((f) => ({
                                                ...f,
                                                startDate: e.target.value,
                                            }))
                                        }
                                    />
                                </OrnateField>
                                <OrnateField
                                    label={t(
                                        "religiousEducation.completionDate"
                                    )}
                                >
                                    <OrnateTextInput
                                        type="date"
                                        value={relEduForm.completionDate}
                                        onChange={(e) =>
                                            setRelEduForm((f) => ({
                                                ...f,
                                                completionDate:
                                                    e.target.value,
                                            }))
                                        }
                                    />
                                </OrnateField>
                            </div>
                            <OrnateCheckbox
                                label={t("religiousEducation.ordained")}
                                checked={relEduForm.ordained}
                                onChange={(e) =>
                                    setRelEduForm((f) => ({
                                        ...f,
                                        ordained: e.target.checked,
                                    }))
                                }
                            />
                            {relEduError && (
                                <p className={s.miniError} role="alert">
                                    {relEduError}
                                </p>
                            )}
                            <div className={s.miniActions}>
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    disabled={busy}
                                    onClick={() => setRelEduFormFor(null)}
                                >
                                    {t("form.cancel")}
                                </Button>
                                <Button type="submit" size="sm" disabled={busy}>
                                    {t("religiousEducation.save")}
                                </Button>
                            </div>
                        </form>
                    )}
                    {!relEduFormFor && relEduError && (
                        <p className={s.miniError} role="alert">
                            {relEduError}
                        </p>
                    )}

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("abilities.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <LinkEditor
                        items={character.abilities}
                        candidates={abilityCandidates}
                        onLoadCandidates={() =>
                            getAbilities(character.worldId).then(
                                setAbilityCandidates
                            )
                        }
                        onAdd={(abilityId) =>
                            addCharacterAbility(character.id, abilityId)
                        }
                        onRemove={(abilityId) =>
                            removeCharacterAbility(character.id, abilityId)
                        }
                        onChanged={refetch}
                        canEdit={canEdit}
                        linkTo={(abilityId) => `/storymap/abilities/${abilityId}`}
                        addLabel={t("abilities.add")}
                        noneLabel={t("none")}
                        pickLabel={t("abilities.pick")}
                        cancelLabel={t("form.cancel")}
                        confirmLabel={t("abilities.confirm")}
                        removeLabel={(name) => t("abilities.remove", { name })}
                        addFailedLabel={t("abilities.addFailed")}
                        removeFailedLabel={t("abilities.removeFailed")}
                    />

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("hobbies.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <LinkEditor
                        items={character.hobbies}
                        candidates={hobbyCandidates}
                        onLoadCandidates={() =>
                            getHobbies(character.worldId).then(
                                setHobbyCandidates
                            )
                        }
                        onAdd={(hobbyId) =>
                            addCharacterHobby(character.id, hobbyId)
                        }
                        onRemove={(hobbyId) =>
                            removeCharacterHobby(character.id, hobbyId)
                        }
                        onChanged={refetch}
                        canEdit={canEdit}
                        linkTo={(hobbyId) => `/storymap/hobbies/${hobbyId}`}
                        addLabel={t("hobbies.add")}
                        noneLabel={t("none")}
                        pickLabel={t("hobbies.pick")}
                        cancelLabel={t("form.cancel")}
                        confirmLabel={t("hobbies.confirm")}
                        removeLabel={(name) => t("hobbies.remove", { name })}
                        addFailedLabel={t("hobbies.addFailed")}
                        removeFailedLabel={t("hobbies.removeFailed")}
                    />

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("specialisations.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <LinkEditor
                        items={character.specialisations}
                        candidates={specialisationCandidates}
                        onLoadCandidates={() =>
                            getSpecialisations({
                                worldId: character.worldId,
                            }).then(setSpecialisationCandidates)
                        }
                        onAdd={(specialisationId) =>
                            addCharacterSpecialisation(
                                character.id,
                                specialisationId
                            )
                        }
                        onRemove={(specialisationId) =>
                            removeCharacterSpecialisation(
                                character.id,
                                specialisationId
                            )
                        }
                        onChanged={refetch}
                        canEdit={canEdit}
                        addLabel={t("specialisations.add")}
                        noneLabel={t("none")}
                        pickLabel={t("specialisations.pick")}
                        cancelLabel={t("form.cancel")}
                        confirmLabel={t("specialisations.confirm")}
                        removeLabel={(name) =>
                            t("specialisations.remove", { name })
                        }
                        addFailedLabel={t("specialisations.addFailed")}
                        removeFailedLabel={t("specialisations.removeFailed")}
                    />

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("clothing.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    <LinkEditor
                        items={character.clothing}
                        candidates={clothingCandidates}
                        onLoadCandidates={() =>
                            getClothing(character.worldId).then(
                                setClothingCandidates
                            )
                        }
                        onAdd={(clothingId) =>
                            addCharacterClothing(character.id, clothingId)
                        }
                        onRemove={(clothingId) =>
                            removeCharacterClothing(character.id, clothingId)
                        }
                        onChanged={refetch}
                        canEdit={canEdit}
                        addLabel={t("clothing.add")}
                        noneLabel={t("none")}
                        pickLabel={t("clothing.pick")}
                        cancelLabel={t("form.cancel")}
                        confirmLabel={t("clothing.confirm")}
                        removeLabel={(name) => t("clothing.remove", { name })}
                        addFailedLabel={t("clothing.addFailed")}
                        removeFailedLabel={t("clothing.removeFailed")}
                    />

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>
                            {t("equipment.label")}
                        </span>
                        <span className={s.sectionLine} />
                    </div>
                    {character.equipments.length === 0 ? (
                        <p className={s.none}>{t("none")}</p>
                    ) : (
                        character.equipments.map((item) => (
                            <div key={item.id} className={s.listRow}>
                                <Link
                                    to={`/storymap/items/${item.id}`}
                                    className={s.listName}
                                >
                                    {item.name}
                                </Link>
                            </div>
                        ))
                    )}
                </div>
            </div>
        </div>
    );
}
