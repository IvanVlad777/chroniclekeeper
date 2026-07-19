import { useEffect, useMemo, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createCharacter,
    deleteCharacter,
    getCharacter,
    getCharacters,
    updateCharacter,
} from "../../../../api/characters";
import { getRaces, getSpecies } from "../../../../api/species";
import { getSocialClasses } from "../../../../api/socialClasses";
import { getNations } from "../../../../api/nations";
import { getReligions } from "../../../../api/religions";
import { getProfessions } from "../../../../api/professions";
import {
    BackgroundInfo,
    CharacterUpdateDto,
    PersonalityInfo,
    RaceDto,
    emptyBackgroundInfo,
    emptyPersonalityInfo,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { EntityPicker, type EntityOption } from "../../../quickCreate/EntityPicker";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const adminRoles = ["Admin", "SuperAdmin"];

interface FormState {
    name: string;
    firstName: string;
    lastName: string;
    nickname: string;
    title: string;
    birthDate: string; // slobodan fiktivni datum (npr. "Godina 1024") ili ""
    deathDate: string;
    description: string;
    height: string;
    weight: string;
    hairColor: string;
    eyeColor: string;
    specialPhysicalFeatures: string;
    isArtificial: boolean;
    sapientSpeciesId: string; // select vrijednosti su stringovi; "" = nije odabrano
    raceId: string;
    socialClassId: string;
    nationId: string;
    religionId: string;
    professionId: string;
    historyId: string;
    fatherId: string;
    motherId: string;
    background: BackgroundInfo;
    personality: PersonalityInfo;
}

const emptyForm: FormState = {
    name: "",
    firstName: "",
    lastName: "",
    nickname: "",
    title: "",
    birthDate: "",
    deathDate: "",
    description: "",
    height: "",
    weight: "",
    hairColor: "",
    eyeColor: "",
    specialPhysicalFeatures: "",
    isArtificial: false,
    sapientSpeciesId: "",
    raceId: "",
    socialClassId: "",
    nationId: "",
    religionId: "",
    professionId: "",
    historyId: "",
    fatherId: "",
    motherId: "",
    background: { ...emptyBackgroundInfo },
    personality: { ...emptyPersonalityInfo },
};

const toId = (v: string): number | null => (v ? Number(v) : null);
const toNum = (v: string): number | null => (v.trim() ? Number(v) : null);
const toDate = (v: string): string | null => (v ? v : null);

function toUpdateDto(f: FormState): CharacterUpdateDto {
    return {
        name: f.name.trim(),
        firstName: f.firstName.trim(),
        lastName: f.lastName.trim(),
        nickname: f.nickname.trim(),
        title: f.title.trim(),
        birthDate: toDate(f.birthDate),
        deathDate: toDate(f.deathDate),
        description: f.description,
        height: toNum(f.height),
        weight: toNum(f.weight),
        hairColor: f.hairColor.trim(),
        eyeColor: f.eyeColor.trim(),
        specialPhysicalFeatures: f.specialPhysicalFeatures,
        isArtificial: f.isArtificial,
        sapientSpeciesId: toId(f.sapientSpeciesId),
        raceId: toId(f.raceId),
        socialClassId: toId(f.socialClassId),
        nationId: toId(f.nationId),
        religionId: toId(f.religionId),
        professionId: toId(f.professionId),
        historyId: toId(f.historyId),
        fatherId: toId(f.fatherId),
        motherId: toId(f.motherId),
        background: f.background,
        personality: f.personality,
    };
}

/** Zajednička forma za /characters/new i /characters/:id/edit. */
export default function CharacterForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("character");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => adminRoles.includes(r)) ?? false;
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [speciesOptions, setSpeciesOptions] = useState<EntityOption[]>([]);
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [socialClassOptions, setSocialClassOptions] = useState<EntityOption[]>(
        []
    );
    const [nationOptions, setNationOptions] = useState<EntityOption[]>([]);
    const [religionOptions, setReligionOptions] = useState<EntityOption[]>([]);
    const [professionOptions, setProfessionOptions] = useState<EntityOption[]>(
        []
    );
    const [characterOptions, setCharacterOptions] = useState<EntityOption[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    const setBg = <K extends keyof BackgroundInfo>(
        key: K,
        value: BackgroundInfo[K]
    ) =>
        setForm((f) => ({
            ...f,
            background: { ...f.background, [key]: value },
        }));

    const setPers = <K extends keyof PersonalityInfo>(
        key: K,
        value: PersonalityInfo[K]
    ) =>
        setForm((f) => ({
            ...f,
            personality: { ...f.personality, [key]: value },
        }));

    // Učitavanje šifrarnika (vrste, rase, likovi za roditelje) + lika u edit modu
    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        Promise.all([
            getSpecies(selectedWorld.id),
            getRaces({ worldId: selectedWorld.id }),
            getCharacters(selectedWorld.id),
            getSocialClasses(selectedWorld.id),
            getNations(selectedWorld.id),
            getReligions(selectedWorld.id),
            getProfessions(selectedWorld.id),
        ])
            .then(async ([speciesData, racesData, charactersData, socialClassData, nationsData, religionsData, professionsData]) => {
                if (cancelled) return;
                setSpeciesOptions(speciesData.map((sp) => ({ value: sp.id, label: sp.name })));
                setRaces(racesData);
                setCharacterOptions(charactersData.map((c) => ({ value: c.id, label: c.name })));
                setSocialClassOptions(
                    socialClassData.map((sc) => ({
                        value: sc.id,
                        label: sc.name,
                    }))
                );
                setNationOptions(
                    nationsData.map((n) => ({ value: n.id, label: n.name }))
                );
                setReligionOptions(
                    religionsData.map((r) => ({ value: r.id, label: r.name }))
                );
                setProfessionOptions(
                    professionsData.map((p) => ({ value: p.id, label: p.name }))
                );

                if (isEdit) {
                    const c = await getCharacter(editId);
                    if (cancelled) return;
                    setForm({
                        name: c.name ?? "",
                        firstName: c.firstName ?? "",
                        lastName: c.lastName ?? "",
                        nickname: c.nickname ?? "",
                        title: c.title ?? "",
                        birthDate: c.birthDate ?? "",
                        deathDate: c.deathDate ?? "",
                        description: c.description ?? "",
                        height: c.height != null ? String(c.height) : "",
                        weight: c.weight != null ? String(c.weight) : "",
                        hairColor: c.hairColor ?? "",
                        eyeColor: c.eyeColor ?? "",
                        specialPhysicalFeatures:
                            c.specialPhysicalFeatures ?? "",
                        isArtificial: c.isArtificial,
                        sapientSpeciesId: c.sapientSpeciesId
                            ? String(c.sapientSpeciesId)
                            : "",
                        raceId: c.raceId ? String(c.raceId) : "",
                        socialClassId: c.socialClassId
                            ? String(c.socialClassId)
                            : "",
                        nationId: c.nationId ? String(c.nationId) : "",
                        religionId: c.religionId ? String(c.religionId) : "",
                        professionId: c.professionId
                            ? String(c.professionId)
                            : "",
                        historyId: c.historyId ? String(c.historyId) : "",
                        fatherId: c.fatherId ? String(c.fatherId) : "",
                        motherId: c.motherId ? String(c.motherId) : "",
                        background: {
                            ...emptyBackgroundInfo,
                            ...(c.background ?? {}),
                        },
                        personality: {
                            ...emptyPersonalityInfo,
                            ...(c.personality ?? {}),
                        },
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load character form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    // Rase filtrirane po odabranoj vrsti
    const raceOptions = useMemo(() => {
        if (!form.sapientSpeciesId) return [];
        return races.filter(
            (r) => r.sapientSpeciesId === Number(form.sapientSpeciesId)
        );
    }, [races, form.sapientSpeciesId]);

    const addCharacterOption = (c: { id: number; name: string }) =>
        setCharacterOptions((prev) => [...prev, { value: c.id, label: c.name }]);

    const onSpeciesChange = (value: string) => {
        setForm((f) => ({
            ...f,
            sapientSpeciesId: value,
            // rasa mora pripadati vrsti — očisti ako više ne odgovara
            raceId:
                value &&
                races.some(
                    (r) =>
                        r.id === Number(f.raceId) &&
                        r.sapientSpeciesId === Number(value)
                )
                    ? f.raceId
                    : "",
        }));
    };

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim() || !form.firstName.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateCharacter(editId, toUpdateDto(form));
                targetId = editId;
            } else {
                // Create takes the full field set (mirrors update + worldId) — no follow-up PUT.
                const created = await createCharacter({
                    ...toUpdateDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/characters/${targetId}`);
        } catch (err) {
            console.error("Failed to save character:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteCharacter(editId);
            navigate("/storymap/characters");
        } catch (err) {
            console.error("Failed to delete character:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="♟"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }
    if (loadError) {
        return (
            <ErrorState
                onRetry={() => setReloadKey((k) => k + 1)}
                detail={loadError}
            />
        );
    }

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("firstname")} required>
                            <OrnateTextInput
                                value={form.firstName}
                                maxLength={50}
                                onChange={(e) =>
                                    set("firstName", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("lastname")}>
                            <OrnateTextInput
                                value={form.lastName}
                                maxLength={50}
                                onChange={(e) =>
                                    set("lastName", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("nickname")}>
                            <OrnateTextInput
                                value={form.nickname}
                                maxLength={50}
                                onChange={(e) =>
                                    set("nickname", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("columns.title")}>
                            <OrnateTextInput
                                value={form.title}
                                maxLength={100}
                                onChange={(e) => set("title", e.target.value)}
                            />
                        </OrnateField>
                    </div>
                    <OrnateField
                        label={t("description")}
                        hint={t("form.descriptionHint")}
                    >
                        <OrnateTextArea
                            value={form.description}
                            rows={6}
                            maxLength={1000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("features")}>
                        <OrnateTextArea
                            value={form.specialPhysicalFeatures}
                            rows={4}
                            maxLength={500}
                            onChange={(e) =>
                                set("specialPhysicalFeatures", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("species")}>
                        <EntityPicker
                            kind="species"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.sapientSpeciesId}
                            options={speciesOptions}
                            onChange={onSpeciesChange}
                            onCreated={(sp) =>
                                setSpeciesOptions((prev) => [
                                    ...prev,
                                    { value: sp.id, label: sp.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField
                        label={t("race")}
                        hint={
                            !form.sapientSpeciesId
                                ? t("form.raceHint")
                                : undefined
                        }
                    >
                        <EntityPicker
                            kind="race"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            disabled={!form.sapientSpeciesId}
                            value={form.raceId}
                            options={raceOptions.map((r) => ({
                                value: r.id,
                                label: r.name,
                            }))}
                            createContext={{
                                sapientSpeciesId: form.sapientSpeciesId
                                    ? Number(form.sapientSpeciesId)
                                    : undefined,
                            }}
                            onChange={(v) => set("raceId", v)}
                            onCreated={(r) =>
                                setRaces((prev) => [
                                    ...prev,
                                    {
                                        id: r.id,
                                        name: r.name,
                                        sapientSpeciesId: Number(
                                            form.sapientSpeciesId
                                        ),
                                    } as unknown as RaceDto,
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("socialClass")}>
                        <EntityPicker
                            kind="socialClass"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.socialClassId}
                            options={socialClassOptions}
                            onChange={(v) => set("socialClassId", v)}
                            onCreated={(sc) =>
                                setSocialClassOptions((prev) => [
                                    ...prev,
                                    { value: sc.id, label: sc.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("nation")}>
                        <EntityPicker
                            kind="nation"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.nationId}
                            options={nationOptions}
                            onChange={(v) => set("nationId", v)}
                            onCreated={(n) =>
                                setNationOptions((prev) => [
                                    ...prev,
                                    { value: n.id, label: n.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("religion")}>
                        <EntityPicker
                            kind="religion"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.religionId}
                            options={religionOptions}
                            onChange={(v) => set("religionId", v)}
                            onCreated={(r) =>
                                setReligionOptions((prev) => [
                                    ...prev,
                                    { value: r.id, label: r.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("profession.label")}>
                        <EntityPicker
                            kind="profession"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.professionId}
                            options={professionOptions}
                            onChange={(v) => set("professionId", v)}
                            onCreated={(p) =>
                                setProfessionOptions((prev) => [
                                    ...prev,
                                    { value: p.id, label: p.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("father")}>
                        <EntityPicker
                            kind="character"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            excludeValue={editId ?? undefined}
                            value={form.fatherId}
                            options={characterOptions}
                            onChange={(v) => set("fatherId", v)}
                            onCreated={addCharacterOption}
                        />
                    </OrnateField>
                    <OrnateField label={t("mother")}>
                        <EntityPicker
                            kind="character"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            excludeValue={editId ?? undefined}
                            value={form.motherId}
                            options={characterOptions}
                            onChange={(v) => set("motherId", v)}
                            onCreated={addCharacterOption}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField
                            label={t("form.birthdate")}
                            hint={t("form.dateHint")}
                        >
                            <OrnateTextInput
                                value={form.birthDate}
                                maxLength={100}
                                placeholder={t("form.datePlaceholder")}
                                onChange={(e) =>
                                    set("birthDate", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("form.deathdate")}>
                            <OrnateTextInput
                                value={form.deathDate}
                                maxLength={100}
                                placeholder={t("form.datePlaceholder")}
                                onChange={(e) =>
                                    set("deathDate", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("height")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={300}
                                step="0.1"
                                value={form.height}
                                onChange={(e) => set("height", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("weight")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={1000}
                                step="0.1"
                                value={form.weight}
                                onChange={(e) => set("weight", e.target.value)}
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("haircolor")}>
                            <OrnateTextInput
                                value={form.hairColor}
                                maxLength={50}
                                onChange={(e) =>
                                    set("hairColor", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("eyecolor")}>
                            <OrnateTextInput
                                value={form.eyeColor}
                                maxLength={50}
                                onChange={(e) =>
                                    set("eyeColor", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("form.isArtificial")}
                        checked={form.isArtificial}
                        onChange={(e) => set("isArtificial", e.target.checked)}
                    />
                </div>
            </div>

            <div className={s.grid}>
                <div className={s.col}>
                    <span className={s.legend}>{t("background.legend")}</span>
                    <OrnateField label={t("background.familyStatus")}>
                        <OrnateTextInput
                            value={form.background.familyStatus}
                            maxLength={200}
                            onChange={(e) =>
                                setBg("familyStatus", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("background.childhood")}>
                        <OrnateTextArea
                            value={form.background.childhood}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) =>
                                setBg("childhood", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("background.upbringing")}>
                        <OrnateTextArea
                            value={form.background.upbringing}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) =>
                                setBg("upbringing", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("background.isImmigrant")}
                        checked={form.background.isImmigrant}
                        onChange={(e) =>
                            setBg("isImmigrant", e.target.checked)
                        }
                    />
                    <OrnateField label={t("background.migrationHistory")}>
                        <OrnateTextArea
                            value={form.background.migrationHistory}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) =>
                                setBg("migrationHistory", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <span className={s.legend}>{t("personality.legend")}</span>
                    <OrnateField label={t("personality.personalityTraits")}>
                        <OrnateTextArea
                            value={form.personality.personalityTraits}
                            rows={2}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("personalityTraits", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.motivations")}>
                        <OrnateTextArea
                            value={form.personality.motivations}
                            rows={2}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("motivations", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.virtues")}>
                        <OrnateTextInput
                            value={form.personality.virtues}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("virtues", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.flaws")}>
                        <OrnateTextInput
                            value={form.personality.flaws}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("flaws", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.fears")}>
                        <OrnateTextInput
                            value={form.personality.fears}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("fears", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.ambitions")}>
                        <OrnateTextInput
                            value={form.personality.ambitions}
                            maxLength={2000}
                            onChange={(e) =>
                                setPers("ambitions", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("personality.psychologicalProfile")}>
                        <OrnateTextArea
                            value={form.personality.psychologicalProfile}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) =>
                                setPers(
                                    "psychologicalProfile",
                                    e.target.value
                                )
                            }
                        />
                    </OrnateField>
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button
                        variant="danger"
                        disabled={busy}
                        onClick={onDelete}
                    >
                        {t("form.delete")}
                    </Button>
                )}
                <span className={s.footerSpacer} />
                <Button
                    variant="ghost"
                    disabled={busy}
                    onClick={() =>
                        navigate(
                            isEdit
                                ? `/storymap/characters/${editId}`
                                : "/storymap/characters"
                        )
                    }
                >
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>
                    {busy ? t("form.saving") : t("form.save")}
                </Button>
            </div>
        </form>
    );
}
