import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createCreature,
    deleteCreature,
    getCreatureById,
    getCreatures,
    updateCreature,
} from "../../../../api/creatures";
import { getHistories } from "../../../../api/histories";
import {
    ArtificialOrigin,
    CreatureDto,
    CreatureSubtype,
    CreatureType,
    CreatureUpdateDto,
    CropType,
    DietType,
    HistoryDto,
    LeafType,
    PlantType,
    CreatureRarity,
    SoilType,
    SunlightRequirement,
    TemperatureRange,
    artificialOrigins,
    creatureRarities,
    creatureSubtypes,
    creatureTypes,
    cropTypes,
    dietTypes,
    leafTypes,
    plantTypes,
    soilTypes,
    sunlightRequirements,
    temperatureRanges,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    subtype: CreatureSubtype;
    type: CreatureType;
    averageLifespan: string;
    height: string;
    weight: string;
    isSentient: boolean;
    isArtificial: boolean;
    artificialOrigin: ArtificialOrigin | "";
    parentCreatureId: string;
    historyId: string;

    scientificName: string;
    isMedicinal: boolean;
    isPoisonous: boolean;
    isBioluminescent: boolean;
    isSymbiotic: boolean;
    specialProperties: string;
    mythologicalSignificance: string;

    isDomesticated: boolean;

    diet: DietType;
    numberOfLegs: string;
    hasWings: boolean;
    hasMultipleHeads: boolean;
    hasRegeneration: boolean;
    isSacred: boolean;
    isMythical: boolean;
    isEndangered: boolean;
    intelligence: string;
    specialAbilities: string;
    isPackAnimal: boolean;
    isAggressive: boolean;

    plantType: PlantType;
    sunlight: SunlightRequirement;
    preferredSoil: SoilType;
    temperatureRange: TemperatureRange;
    rarity: CreatureRarity;
    isCarnivorous: boolean;
    hasRegenerativeProperties: boolean;
    canMove: boolean;
    isParasitic: boolean;

    maxHeight: string;
    lifespan: string;
    leafType: LeafType;

    yieldPerHectare: string;
    cropType: CropType;

    isEdible: boolean;
    isHallucinogenic: boolean;
    hasMutagenicProperties: boolean;
    canCommunicate: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    subtype: "Animal",
    type: "Animal",
    averageLifespan: "",
    height: "",
    weight: "",
    isSentient: false,
    isArtificial: false,
    artificialOrigin: "",
    parentCreatureId: "",
    historyId: "",

    scientificName: "",
    isMedicinal: false,
    isPoisonous: false,
    isBioluminescent: false,
    isSymbiotic: false,
    specialProperties: "",
    mythologicalSignificance: "",

    isDomesticated: false,

    diet: "Herbivore",
    numberOfLegs: "",
    hasWings: false,
    hasMultipleHeads: false,
    hasRegeneration: false,
    isSacred: false,
    isMythical: false,
    isEndangered: false,
    intelligence: "",
    specialAbilities: "",
    isPackAnimal: false,
    isAggressive: false,

    plantType: "Tree",
    sunlight: "FullSun",
    preferredSoil: "Loamy",
    temperatureRange: "Temperate",
    rarity: "Common",
    isCarnivorous: false,
    hasRegenerativeProperties: false,
    canMove: false,
    isParasitic: false,

    maxHeight: "",
    lifespan: "",
    leafType: "Deciduous",

    yieldPerHectare: "",
    cropType: "Grain",

    isEdible: false,
    isHallucinogenic: false,
    hasMutagenicProperties: false,
    canCommunicate: false,
};

const toNum = (v: string): number | null => (v.trim() ? Number(v) : null);

function toDto(f: FormState): CreatureUpdateDto {
    const isPlantFamily = f.subtype === "Plant" || f.subtype === "Tree" || f.subtype === "Crop";
    const isAnimalOrFungus = f.subtype === "Animal" || f.subtype === "Fungus";

    return {
        name: f.name.trim(),
        description: f.description,
        type: f.type,
        averageLifespan: toNum(f.averageLifespan) ?? 0,
        height: toNum(f.height) ?? 0,
        weight: toNum(f.weight) ?? 0,
        isSentient: f.isSentient,
        isArtificial: f.isArtificial,
        artificialOrigin: f.isArtificial && f.artificialOrigin ? f.artificialOrigin : null,
        parentCreatureId: toNum(f.parentCreatureId),
        historyId: toNum(f.historyId),

        scientificName: isPlantFamily || f.subtype === "Fungus" ? f.scientificName : null,
        isMedicinal: isPlantFamily || f.subtype === "Fungus" ? f.isMedicinal : null,
        isPoisonous: isPlantFamily || f.subtype === "Fungus" ? f.isPoisonous : null,
        isBioluminescent: isPlantFamily || f.subtype === "Fungus" ? f.isBioluminescent : null,
        isSymbiotic: isAnimalOrFungus || isPlantFamily ? f.isSymbiotic : null,
        specialProperties: isPlantFamily || f.subtype === "Fungus" ? f.specialProperties : null,
        mythologicalSignificance: isPlantFamily || f.subtype === "Fungus" ? f.mythologicalSignificance : null,

        isDomesticated: f.subtype === "Animal" || f.subtype === "Crop" ? f.isDomesticated : null,

        diet: f.subtype === "Animal" ? f.diet : null,
        numberOfLegs: f.subtype === "Animal" ? toNum(f.numberOfLegs) : null,
        hasWings: f.subtype === "Animal" ? f.hasWings : null,
        hasMultipleHeads: f.subtype === "Animal" ? f.hasMultipleHeads : null,
        hasRegeneration: f.subtype === "Animal" ? f.hasRegeneration : null,
        isSacred: f.subtype === "Animal" ? f.isSacred : null,
        isMythical: f.subtype === "Animal" ? f.isMythical : null,
        isEndangered: f.subtype === "Animal" ? f.isEndangered : null,
        intelligence: f.subtype === "Animal" ? f.intelligence : null,
        specialAbilities: f.subtype === "Animal" ? f.specialAbilities : null,
        isPackAnimal: f.subtype === "Animal" ? f.isPackAnimal : null,
        isAggressive: f.subtype === "Animal" ? f.isAggressive : null,

        plantType: isPlantFamily ? f.plantType : null,
        sunlight: isPlantFamily ? f.sunlight : null,
        preferredSoil: isPlantFamily ? f.preferredSoil : null,
        temperatureRange: isPlantFamily ? f.temperatureRange : null,
        rarity: isPlantFamily ? f.rarity : null,
        isCarnivorous: isPlantFamily ? f.isCarnivorous : null,
        hasRegenerativeProperties: isPlantFamily ? f.hasRegenerativeProperties : null,
        canMove: isPlantFamily ? f.canMove : null,
        isParasitic: isPlantFamily ? f.isParasitic : null,

        maxHeight: f.subtype === "Tree" ? toNum(f.maxHeight) : null,
        lifespan: f.subtype === "Tree" ? toNum(f.lifespan) : null,
        leafType: f.subtype === "Tree" ? f.leafType : null,

        yieldPerHectare: f.subtype === "Crop" ? toNum(f.yieldPerHectare) : null,
        cropType: f.subtype === "Crop" ? f.cropType : null,

        isEdible: f.subtype === "Fungus" ? f.isEdible : null,
        isHallucinogenic: f.subtype === "Fungus" ? f.isHallucinogenic : null,
        hasMutagenicProperties: f.subtype === "Fungus" ? f.hasMutagenicProperties : null,
        canCommunicate: f.subtype === "Fungus" ? f.canCommunicate : null,
    };
}

/** Zajednička forma za /creatures/new i /creatures/:id/edit — subtype selektira TPH podtip. */
export default function CreatureForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("creature");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [parentCandidates, setParentCandidates] = useState<CreatureDto[]>([]);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        Promise.all([
            getCreatures({ worldId: selectedWorld.id }),
            getHistories(selectedWorld.id),
        ])
            .then(async ([creatures, historiesData]) => {
                if (cancelled) return;
                setParentCandidates(creatures);
                setHistories(historiesData);
                if (isEdit) {
                    const c = await getCreatureById(editId);
                    if (cancelled) return;
                    setForm({
                        name: c.name ?? "",
                        description: c.description ?? "",
                        subtype: (c.subtype as CreatureSubtype) ?? "Animal",
                        type: c.type ?? "Animal",
                        averageLifespan: String(c.averageLifespan ?? ""),
                        height: String(c.height ?? ""),
                        weight: String(c.weight ?? ""),
                        isSentient: c.isSentient ?? false,
                        isArtificial: c.isArtificial ?? false,
                        artificialOrigin: c.artificialOrigin ?? "",
                        parentCreatureId: c.parentCreatureId != null ? String(c.parentCreatureId) : "",
                        historyId: c.historyId != null ? String(c.historyId) : "",

                        scientificName: c.scientificName ?? "",
                        isMedicinal: c.isMedicinal ?? false,
                        isPoisonous: c.isPoisonous ?? false,
                        isBioluminescent: c.isBioluminescent ?? false,
                        isSymbiotic: c.isSymbiotic ?? false,
                        specialProperties: c.specialProperties ?? "",
                        mythologicalSignificance: c.mythologicalSignificance ?? "",

                        isDomesticated: c.isDomesticated ?? false,

                        diet: c.diet ?? "Herbivore",
                        numberOfLegs: c.numberOfLegs != null ? String(c.numberOfLegs) : "",
                        hasWings: c.hasWings ?? false,
                        hasMultipleHeads: c.hasMultipleHeads ?? false,
                        hasRegeneration: c.hasRegeneration ?? false,
                        isSacred: c.isSacred ?? false,
                        isMythical: c.isMythical ?? false,
                        isEndangered: c.isEndangered ?? false,
                        intelligence: c.intelligence ?? "",
                        specialAbilities: c.specialAbilities ?? "",
                        isPackAnimal: c.isPackAnimal ?? false,
                        isAggressive: c.isAggressive ?? false,

                        plantType: c.plantType ?? "Tree",
                        sunlight: c.sunlight ?? "FullSun",
                        preferredSoil: c.preferredSoil ?? "Loamy",
                        temperatureRange: c.temperatureRange ?? "Temperate",
                        rarity: c.rarity ?? "Common",
                        isCarnivorous: c.isCarnivorous ?? false,
                        hasRegenerativeProperties: c.hasRegenerativeProperties ?? false,
                        canMove: c.canMove ?? false,
                        isParasitic: c.isParasitic ?? false,

                        maxHeight: c.maxHeight != null ? String(c.maxHeight) : "",
                        lifespan: c.lifespan != null ? String(c.lifespan) : "",
                        leafType: c.leafType ?? "Deciduous",

                        yieldPerHectare: c.yieldPerHectare != null ? String(c.yieldPerHectare) : "",
                        cropType: c.cropType ?? "Grain",

                        isEdible: c.isEdible ?? false,
                        isHallucinogenic: c.isHallucinogenic ?? false,
                        hasMutagenicProperties: c.hasMutagenicProperties ?? false,
                        canCommunicate: c.canCommunicate ?? false,
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load creature form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateCreature(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createCreature({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                    subtype: form.subtype,
                });
                targetId = created.id;
            }
            navigate(`/storymap/creatures/${targetId}`);
        } catch (err) {
            console.error("Failed to save creature:", err);
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
            await deleteCreature(editId);
            navigate("/storymap/creatures");
        } catch (err) {
            console.error("Failed to delete creature:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="🐾"
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

    const isPlantFamily = form.subtype === "Plant" || form.subtype === "Tree" || form.subtype === "Crop";

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.subtype")} required>
                        <OrnateSelect
                            value={form.subtype}
                            disabled={isEdit}
                            onChange={(e) =>
                                set("subtype", e.target.value as CreatureSubtype)
                            }
                        >
                            {creatureSubtypes.map((subtype) => (
                                <option key={subtype} value={subtype}>
                                    {t(`subtypes.${subtype}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={6}
                            maxLength={4000}
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>

                    <OrnateField label={t("columns.type")}>
                        <OrnateSelect
                            value={form.type}
                            onChange={(e) => set("type", e.target.value as CreatureType)}
                        >
                            {creatureTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.averageLifespan")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.averageLifespan}
                                onChange={(e) => set("averageLifespan", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.height")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.height}
                                onChange={(e) => set("height", e.target.value)}
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.weight")}>
                        <OrnateTextInput
                            type="number"
                            min={0}
                            value={form.weight}
                            onChange={(e) => set("weight", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.checkboxGrid}>
                        <OrnateCheckbox
                            label={t("fields.isSentient")}
                            checked={form.isSentient}
                            onChange={(e) => set("isSentient", e.target.checked)}
                        />
                        <OrnateCheckbox
                            label={t("fields.isArtificial")}
                            checked={form.isArtificial}
                            onChange={(e) => set("isArtificial", e.target.checked)}
                        />
                    </div>
                    {form.isArtificial && (
                        <OrnateField label={t("fields.artificialOrigin")}>
                            <OrnateSelect
                                value={form.artificialOrigin}
                                onChange={(e) =>
                                    set("artificialOrigin", e.target.value as ArtificialOrigin)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {artificialOrigins.map((o) => (
                                    <option key={o} value={o}>
                                        {t(`artificialOrigins.${o}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    )}
                    <OrnateField label={t("fields.parentCreature")}>
                        <OrnateSelect
                            value={form.parentCreatureId}
                            onChange={(e) => set("parentCreatureId", e.target.value)}
                        >
                            <option value="">{t("none")}</option>
                            {parentCandidates
                                .filter((c) => c.id !== editId)
                                .map((c) => (
                                    <option key={c.id} value={c.id}>
                                        {c.name}
                                    </option>
                                ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("none")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>

                <div className={s.col}>
                    {form.subtype === "Animal" && (
                        <>
                            <h2 className={s.sectionTitle}>{t("subtypes.Animal")}</h2>
                            <OrnateField label={t("fields.diet")}>
                                <OrnateSelect
                                    value={form.diet}
                                    onChange={(e) => set("diet", e.target.value as DietType)}
                                >
                                    {dietTypes.map((d) => (
                                        <option key={d} value={d}>
                                            {t(`dietTypes.${d}`)}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.numberOfLegs")}>
                                <OrnateTextInput
                                    type="number"
                                    min={0}
                                    value={form.numberOfLegs}
                                    onChange={(e) => set("numberOfLegs", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.intelligence")}>
                                <OrnateTextInput
                                    value={form.intelligence}
                                    maxLength={500}
                                    onChange={(e) => set("intelligence", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.specialAbilities")}>
                                <OrnateTextInput
                                    value={form.specialAbilities}
                                    maxLength={500}
                                    onChange={(e) => set("specialAbilities", e.target.value)}
                                />
                            </OrnateField>
                            <div className={s.checkboxGrid}>
                                <OrnateCheckbox label={t("fields.isDomesticated")} checked={form.isDomesticated} onChange={(e) => set("isDomesticated", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.hasWings")} checked={form.hasWings} onChange={(e) => set("hasWings", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.hasMultipleHeads")} checked={form.hasMultipleHeads} onChange={(e) => set("hasMultipleHeads", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.hasRegeneration")} checked={form.hasRegeneration} onChange={(e) => set("hasRegeneration", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isSacred")} checked={form.isSacred} onChange={(e) => set("isSacred", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isMythical")} checked={form.isMythical} onChange={(e) => set("isMythical", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isEndangered")} checked={form.isEndangered} onChange={(e) => set("isEndangered", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isPackAnimal")} checked={form.isPackAnimal} onChange={(e) => set("isPackAnimal", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isAggressive")} checked={form.isAggressive} onChange={(e) => set("isAggressive", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isSymbiotic")} checked={form.isSymbiotic} onChange={(e) => set("isSymbiotic", e.target.checked)} />
                            </div>
                        </>
                    )}

                    {isPlantFamily && (
                        <>
                            <h2 className={s.sectionTitle}>{t(`subtypes.${form.subtype}`)}</h2>
                            <OrnateField label={t("fields.plantType")}>
                                <OrnateSelect
                                    value={form.plantType}
                                    onChange={(e) => set("plantType", e.target.value as PlantType)}
                                >
                                    {plantTypes.map((pt) => (
                                        <option key={pt} value={pt}>
                                            {t(`plantTypes.${pt}`)}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.scientificName")}>
                                <OrnateTextInput
                                    value={form.scientificName}
                                    maxLength={100}
                                    onChange={(e) => set("scientificName", e.target.value)}
                                />
                            </OrnateField>
                            <div className={s.row2}>
                                <OrnateField label={t("fields.sunlight")}>
                                    <OrnateSelect
                                        value={form.sunlight}
                                        onChange={(e) => set("sunlight", e.target.value as SunlightRequirement)}
                                    >
                                        {sunlightRequirements.map((sr) => (
                                            <option key={sr} value={sr}>
                                                {t(`sunlightRequirements.${sr}`)}
                                            </option>
                                        ))}
                                    </OrnateSelect>
                                </OrnateField>
                                <OrnateField label={t("fields.preferredSoil")}>
                                    <OrnateSelect
                                        value={form.preferredSoil}
                                        onChange={(e) => set("preferredSoil", e.target.value as SoilType)}
                                    >
                                        {soilTypes.map((st) => (
                                            <option key={st} value={st}>
                                                {t(`soilTypes.${st}`)}
                                            </option>
                                        ))}
                                    </OrnateSelect>
                                </OrnateField>
                            </div>
                            <div className={s.row2}>
                                <OrnateField label={t("fields.temperatureRange")}>
                                    <OrnateSelect
                                        value={form.temperatureRange}
                                        onChange={(e) => set("temperatureRange", e.target.value as TemperatureRange)}
                                    >
                                        {temperatureRanges.map((tr) => (
                                            <option key={tr} value={tr}>
                                                {t(`temperatureRanges.${tr}`)}
                                            </option>
                                        ))}
                                    </OrnateSelect>
                                </OrnateField>
                                <OrnateField label={t("fields.rarity")}>
                                    <OrnateSelect
                                        value={form.rarity}
                                        onChange={(e) => set("rarity", e.target.value as CreatureRarity)}
                                    >
                                        {creatureRarities.map((r) => (
                                            <option key={r} value={r}>
                                                {t(`rarities.${r}`)}
                                            </option>
                                        ))}
                                    </OrnateSelect>
                                </OrnateField>
                            </div>
                            <OrnateField label={t("fields.specialProperties")}>
                                <OrnateTextInput
                                    value={form.specialProperties}
                                    maxLength={500}
                                    onChange={(e) => set("specialProperties", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.mythologicalSignificance")}>
                                <OrnateTextInput
                                    value={form.mythologicalSignificance}
                                    maxLength={500}
                                    onChange={(e) => set("mythologicalSignificance", e.target.value)}
                                />
                            </OrnateField>
                            <div className={s.checkboxGrid}>
                                <OrnateCheckbox label={t("fields.isMedicinal")} checked={form.isMedicinal} onChange={(e) => set("isMedicinal", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isPoisonous")} checked={form.isPoisonous} onChange={(e) => set("isPoisonous", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isBioluminescent")} checked={form.isBioluminescent} onChange={(e) => set("isBioluminescent", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isCarnivorous")} checked={form.isCarnivorous} onChange={(e) => set("isCarnivorous", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.hasRegenerativeProperties")} checked={form.hasRegenerativeProperties} onChange={(e) => set("hasRegenerativeProperties", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.canMove")} checked={form.canMove} onChange={(e) => set("canMove", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isSymbiotic")} checked={form.isSymbiotic} onChange={(e) => set("isSymbiotic", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isParasitic")} checked={form.isParasitic} onChange={(e) => set("isParasitic", e.target.checked)} />
                            </div>

                            {form.subtype === "Tree" && (
                                <>
                                    <h3 className={s.sectionTitle}>{t("subtypes.Tree")}</h3>
                                    <div className={s.row2}>
                                        <OrnateField label={t("fields.maxHeight")}>
                                            <OrnateTextInput type="number" min={0} value={form.maxHeight} onChange={(e) => set("maxHeight", e.target.value)} />
                                        </OrnateField>
                                        <OrnateField label={t("fields.lifespan")}>
                                            <OrnateTextInput type="number" min={0} value={form.lifespan} onChange={(e) => set("lifespan", e.target.value)} />
                                        </OrnateField>
                                    </div>
                                    <OrnateField label={t("fields.leafType")}>
                                        <OrnateSelect value={form.leafType} onChange={(e) => set("leafType", e.target.value as LeafType)}>
                                            {leafTypes.map((lt) => (
                                                <option key={lt} value={lt}>{t(`leafTypes.${lt}`)}</option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                </>
                            )}

                            {form.subtype === "Crop" && (
                                <>
                                    <h3 className={s.sectionTitle}>{t("subtypes.Crop")}</h3>
                                    <OrnateField label={t("fields.yieldPerHectare")}>
                                        <OrnateTextInput type="number" min={0} value={form.yieldPerHectare} onChange={(e) => set("yieldPerHectare", e.target.value)} />
                                    </OrnateField>
                                    <OrnateField label={t("fields.cropType")}>
                                        <OrnateSelect value={form.cropType} onChange={(e) => set("cropType", e.target.value as CropType)}>
                                            {cropTypes.map((ct) => (
                                                <option key={ct} value={ct}>{t(`cropTypes.${ct}`)}</option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                    <OrnateCheckbox label={t("fields.isDomesticated")} checked={form.isDomesticated} onChange={(e) => set("isDomesticated", e.target.checked)} />
                                </>
                            )}
                        </>
                    )}

                    {form.subtype === "Fungus" && (
                        <>
                            <h2 className={s.sectionTitle}>{t("subtypes.Fungus")}</h2>
                            <OrnateField label={t("fields.scientificName")}>
                                <OrnateTextInput
                                    value={form.scientificName}
                                    maxLength={100}
                                    onChange={(e) => set("scientificName", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.specialProperties")}>
                                <OrnateTextInput
                                    value={form.specialProperties}
                                    maxLength={500}
                                    onChange={(e) => set("specialProperties", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.mythologicalSignificance")}>
                                <OrnateTextInput
                                    value={form.mythologicalSignificance}
                                    maxLength={500}
                                    onChange={(e) => set("mythologicalSignificance", e.target.value)}
                                />
                            </OrnateField>
                            <div className={s.checkboxGrid}>
                                <OrnateCheckbox label={t("fields.isMedicinal")} checked={form.isMedicinal} onChange={(e) => set("isMedicinal", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isPoisonous")} checked={form.isPoisonous} onChange={(e) => set("isPoisonous", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isEdible")} checked={form.isEdible} onChange={(e) => set("isEdible", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isHallucinogenic")} checked={form.isHallucinogenic} onChange={(e) => set("isHallucinogenic", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isBioluminescent")} checked={form.isBioluminescent} onChange={(e) => set("isBioluminescent", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.hasMutagenicProperties")} checked={form.hasMutagenicProperties} onChange={(e) => set("hasMutagenicProperties", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.isSymbiotic")} checked={form.isSymbiotic} onChange={(e) => set("isSymbiotic", e.target.checked)} />
                                <OrnateCheckbox label={t("fields.canCommunicate")} checked={form.canCommunicate} onChange={(e) => set("canCommunicate", e.target.checked)} />
                            </div>
                        </>
                    )}
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button variant="danger" disabled={busy} onClick={onDelete}>
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
                                ? `/storymap/creatures/${editId}`
                                : "/storymap/creatures"
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
