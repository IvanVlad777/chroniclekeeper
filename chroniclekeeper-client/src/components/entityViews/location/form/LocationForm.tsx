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
    createLocation,
    deleteLocation,
    getLocation,
    getLocations,
    updateLocation,
} from "../../../../api/locations";
import { getGovernmentSystems } from "../../../../api/governmentSystems";
import { getLegalSystems } from "../../../../api/legalSystems";
import { getEducationSystems } from "../../../../api/educationSystems";
import { getEconomicSystems } from "../../../../api/economicSystems";
import {
    GovernmentSystemDto,
    LegalSystemDto,
    EducationSystemDto,
    EconomicSystemDto,
    LocationDto,
    LocationType,
    LocationUpdateDto,
    locationTypes,
    ecosystemLocationTypes,
    desertTypes,
    forestTypes,
    caveTypes,
    grasslandTypes,
    DesertType,
    ForestType,
    CaveType,
    GrasslandType,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    type: LocationType;
    area: string;
    population: string;
    latitude: string;
    longitude: string;
    parentLocationId: string;
    historyId: string;
    continentSpecifics: string;
    regionSpecifics: string;
    governmentSystemId: string;
    legalSystemId: string;
    educationSystemId: string;
    economicSystemId: string;
    isCapital: boolean;
    districtType: string;
    uniqueFeatures: string;
    waterDepth: string;
    volume: string;
    maxDepth: string;
    isFreshwater: boolean;
    riverLength: string;
    sourceLocationId: string;
    mouthLocationId: string;
    maxElevation: string;
    prominence: string;
    mountainRangeLength: string;
    isSaltwater: boolean;
    desertKind: DesertType;
    forestKind: ForestType;
    caveDepth: string;
    caveKind: CaveType;
    grasslandKind: GrasslandType;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    type: "Other",
    area: "",
    population: "",
    latitude: "",
    longitude: "",
    parentLocationId: "",
    historyId: "",
    continentSpecifics: "",
    regionSpecifics: "",
    governmentSystemId: "",
    legalSystemId: "",
    educationSystemId: "",
    economicSystemId: "",
    isCapital: false,
    districtType: "",
    uniqueFeatures: "",
    waterDepth: "",
    volume: "",
    maxDepth: "",
    isFreshwater: true,
    riverLength: "",
    sourceLocationId: "",
    mouthLocationId: "",
    maxElevation: "",
    prominence: "",
    mountainRangeLength: "",
    isSaltwater: false,
    desertKind: desertTypes[0],
    forestKind: forestTypes[0],
    caveDepth: "",
    caveKind: caveTypes[0],
    grasslandKind: grasslandTypes[0],
};

const isWaterType = (t: LocationType) =>
    t === "Lake" || t === "Sea" || t === "Ocean" || t === "River";
const isEcosystemType = (t: LocationType) =>
    (ecosystemLocationTypes as readonly LocationType[]).includes(t);

const toNum = (v: string): number | null => (v.trim() ? Number(v) : null);
const toId = (v: string): number | null => (v ? Number(v) : null);
const toStr = (v: string): string | null => (v.trim() ? v : null);

function toDto(f: FormState): LocationUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        type: f.type,
        area: toNum(f.area),
        population: toNum(f.population),
        latitude: toNum(f.latitude),
        longitude: toNum(f.longitude),
        parentLocationId: toId(f.parentLocationId),
        historyId: toId(f.historyId),
        continentSpecifics: f.type === "Continent" ? toStr(f.continentSpecifics) : null,
        regionSpecifics: f.type === "Region" ? toStr(f.regionSpecifics) : null,
        governmentSystemId:
            f.type === "Country" || f.type === "City" ? toId(f.governmentSystemId) : null,
        legalSystemId:
            f.type === "Country" || f.type === "City" ? toId(f.legalSystemId) : null,
        educationSystemId:
            f.type === "Country" || f.type === "City" ? toId(f.educationSystemId) : null,
        economicSystemId:
            f.type === "Country" || f.type === "City" ? toId(f.economicSystemId) : null,
        isCapital: f.type === "City" ? f.isCapital : null,
        districtType: f.type === "District" ? toStr(f.districtType) : null,
        uniqueFeatures: isEcosystemType(f.type) ? toStr(f.uniqueFeatures) : null,
        waterDepth: isWaterType(f.type) ? toNum(f.waterDepth) : null,
        volume: f.type === "Lake" ? toNum(f.volume) : null,
        maxDepth: f.type === "Lake" ? toNum(f.maxDepth) : null,
        isFreshwater: f.type === "Lake" ? f.isFreshwater : null,
        riverLength: f.type === "River" ? toNum(f.riverLength) : null,
        sourceLocationId: f.type === "River" ? toId(f.sourceLocationId) : null,
        mouthLocationId: f.type === "River" ? toId(f.mouthLocationId) : null,
        maxElevation: f.type === "Mountain" ? toNum(f.maxElevation) : null,
        prominence: f.type === "Mountain" ? toNum(f.prominence) : null,
        mountainRangeLength: f.type === "MountainRange" ? toNum(f.mountainRangeLength) : null,
        isSaltwater: f.type === "Swamp" ? f.isSaltwater : null,
        desertKind: f.type === "Desert" ? f.desertKind : null,
        forestKind: f.type === "Forest" ? f.forestKind : null,
        caveDepth: f.type === "Cave" ? toNum(f.caveDepth) : null,
        caveKind: f.type === "Cave" ? f.caveKind : null,
        grasslandKind: f.type === "Grassland" ? f.grasslandKind : null,
    };
}

/** Zajednička forma za /locations/new i /locations/:id/edit. */
export default function LocationForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("location");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [form, setForm] = useState<FormState>(emptyForm);
    const [worldLocations, setWorldLocations] = useState<LocationDto[]>([]);
    const [governmentSystems, setGovernmentSystems] = useState<GovernmentSystemDto[]>([]);
    const [legalSystems, setLegalSystems] = useState<LegalSystemDto[]>([]);
    const [educationSystems, setEducationSystems] = useState<EducationSystemDto[]>([]);
    const [economicSystems, setEconomicSystems] = useState<EconomicSystemDto[]>([]);
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
            getLocations(selectedWorld.id),
            getGovernmentSystems(selectedWorld.id),
            getLegalSystems(selectedWorld.id),
            getEducationSystems(selectedWorld.id),
            getEconomicSystems(selectedWorld.id),
        ])
            .then(async ([locations, governmentSystemsData, legalSystemsData, educationSystemsData, economicSystemsData]) => {
                if (cancelled) return;
                setWorldLocations(locations);
                setGovernmentSystems(governmentSystemsData);
                setLegalSystems(legalSystemsData);
                setEducationSystems(educationSystemsData);
                setEconomicSystems(economicSystemsData);
                if (isEdit) {
                    const l = await getLocation(editId);
                    if (cancelled) return;
                    setForm({
                        name: l.name ?? "",
                        description: l.description ?? "",
                        type: l.type,
                        area: l.area != null ? String(l.area) : "",
                        population:
                            l.population != null ? String(l.population) : "",
                        latitude:
                            l.latitude != null ? String(l.latitude) : "",
                        longitude:
                            l.longitude != null ? String(l.longitude) : "",
                        parentLocationId: l.parentLocationId
                            ? String(l.parentLocationId)
                            : "",
                        historyId: l.historyId ? String(l.historyId) : "",
                        continentSpecifics: l.continentSpecifics ?? "",
                        regionSpecifics: l.regionSpecifics ?? "",
                        governmentSystemId: l.governmentSystemId
                            ? String(l.governmentSystemId)
                            : "",
                        legalSystemId: l.legalSystemId ? String(l.legalSystemId) : "",
                        educationSystemId: l.educationSystemId
                            ? String(l.educationSystemId)
                            : "",
                        economicSystemId: l.economicSystemId
                            ? String(l.economicSystemId)
                            : "",
                        isCapital: l.isCapital ?? false,
                        districtType: l.districtType ?? "",
                        uniqueFeatures: l.uniqueFeatures ?? "",
                        waterDepth: l.waterDepth != null ? String(l.waterDepth) : "",
                        volume: l.volume != null ? String(l.volume) : "",
                        maxDepth: l.maxDepth != null ? String(l.maxDepth) : "",
                        isFreshwater: l.isFreshwater ?? true,
                        riverLength: l.riverLength != null ? String(l.riverLength) : "",
                        sourceLocationId: l.sourceLocationId ? String(l.sourceLocationId) : "",
                        mouthLocationId: l.mouthLocationId ? String(l.mouthLocationId) : "",
                        maxElevation: l.maxElevation != null ? String(l.maxElevation) : "",
                        prominence: l.prominence != null ? String(l.prominence) : "",
                        mountainRangeLength:
                            l.mountainRangeLength != null ? String(l.mountainRangeLength) : "",
                        isSaltwater: l.isSaltwater ?? false,
                        desertKind: l.desertKind ?? desertTypes[0],
                        forestKind: l.forestKind ?? forestTypes[0],
                        caveDepth: l.caveDepth != null ? String(l.caveDepth) : "",
                        caveKind: l.caveKind ?? caveTypes[0],
                        grasslandKind: l.grasslandKind ?? grasslandTypes[0],
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load location form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    const parentOptions = worldLocations.filter((l) => l.id !== editId);

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
                await updateLocation(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createLocation({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/locations/${targetId}`);
        } catch (err) {
            console.error("Failed to save location:", err);
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
            await deleteLocation(editId);
            navigate("/storymap/locations");
        } catch (err) {
            console.error("Failed to delete location:", err);
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
                glyph="⚑"
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
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={10}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.type")}>
                        <OrnateSelect
                            value={form.type}
                            onChange={(e) =>
                                set("type", e.target.value as LocationType)
                            }
                        >
                            {locationTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>

                    {form.type === "Continent" && (
                        <OrnateField label={t("fields.continentSpecifics")}>
                            <OrnateTextArea
                                value={form.continentSpecifics}
                                rows={3}
                                onChange={(e) =>
                                    set("continentSpecifics", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {form.type === "Region" && (
                        <OrnateField label={t("fields.regionSpecifics")}>
                            <OrnateTextArea
                                value={form.regionSpecifics}
                                rows={3}
                                onChange={(e) =>
                                    set("regionSpecifics", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {form.type === "District" && (
                        <OrnateField label={t("fields.districtType")}>
                            <OrnateTextInput
                                value={form.districtType}
                                onChange={(e) =>
                                    set("districtType", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {isEcosystemType(form.type) && (
                        <OrnateField label={t("fields.uniqueFeatures")}>
                            <OrnateTextArea
                                value={form.uniqueFeatures}
                                rows={3}
                                onChange={(e) =>
                                    set("uniqueFeatures", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {isWaterType(form.type) && (
                        <OrnateField label={t("fields.waterDepth")}>
                            <OrnateTextInput
                                type="number"
                                step="0.01"
                                value={form.waterDepth}
                                onChange={(e) =>
                                    set("waterDepth", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {form.type === "Lake" && (
                        <>
                            <div className={s.row2}>
                                <OrnateField label={t("fields.volume")}>
                                    <OrnateTextInput
                                        type="number"
                                        step="0.01"
                                        value={form.volume}
                                        onChange={(e) =>
                                            set("volume", e.target.value)
                                        }
                                    />
                                </OrnateField>
                                <OrnateField label={t("fields.maxDepth")}>
                                    <OrnateTextInput
                                        type="number"
                                        step="0.01"
                                        value={form.maxDepth}
                                        onChange={(e) =>
                                            set("maxDepth", e.target.value)
                                        }
                                    />
                                </OrnateField>
                            </div>
                            <OrnateField label={t("fields.isFreshwater")}>
                                <OrnateCheckbox
                                    checked={form.isFreshwater}
                                    onChange={(e) =>
                                        set("isFreshwater", e.target.checked)
                                    }
                                />
                            </OrnateField>
                        </>
                    )}
                    {form.type === "River" && (
                        <>
                            <OrnateField label={t("fields.riverLength")}>
                                <OrnateTextInput
                                    type="number"
                                    step="0.01"
                                    value={form.riverLength}
                                    onChange={(e) =>
                                        set("riverLength", e.target.value)
                                    }
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.sourceLocation")}>
                                <OrnateSelect
                                    value={form.sourceLocationId}
                                    onChange={(e) =>
                                        set("sourceLocationId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {parentOptions.map((l) => (
                                        <option key={l.id} value={l.id}>
                                            {l.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.mouthLocation")}>
                                <OrnateSelect
                                    value={form.mouthLocationId}
                                    onChange={(e) =>
                                        set("mouthLocationId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {parentOptions.map((l) => (
                                        <option key={l.id} value={l.id}>
                                            {l.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                        </>
                    )}
                    {form.type === "Mountain" && (
                        <div className={s.row2}>
                            <OrnateField label={t("fields.maxElevation")}>
                                <OrnateTextInput
                                    type="number"
                                    step="0.01"
                                    value={form.maxElevation}
                                    onChange={(e) =>
                                        set("maxElevation", e.target.value)
                                    }
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.prominence")}>
                                <OrnateTextInput
                                    type="number"
                                    step="0.01"
                                    value={form.prominence}
                                    onChange={(e) =>
                                        set("prominence", e.target.value)
                                    }
                                />
                            </OrnateField>
                        </div>
                    )}
                    {form.type === "MountainRange" && (
                        <OrnateField label={t("fields.mountainRangeLength")}>
                            <OrnateTextInput
                                type="number"
                                step="0.01"
                                value={form.mountainRangeLength}
                                onChange={(e) =>
                                    set("mountainRangeLength", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
                    {form.type === "Swamp" && (
                        <OrnateField label={t("fields.isSaltwater")}>
                            <OrnateCheckbox
                                checked={form.isSaltwater}
                                onChange={(e) =>
                                    set("isSaltwater", e.target.checked)
                                }
                            />
                        </OrnateField>
                    )}
                    {form.type === "Desert" && (
                        <OrnateField label={t("fields.desertKind")}>
                            <OrnateSelect
                                value={form.desertKind}
                                onChange={(e) =>
                                    set("desertKind", e.target.value as DesertType)
                                }
                            >
                                {desertTypes.map((k) => (
                                    <option key={k} value={k}>
                                        {t(`desertKinds.${k}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    )}
                    {form.type === "Forest" && (
                        <OrnateField label={t("fields.forestKind")}>
                            <OrnateSelect
                                value={form.forestKind}
                                onChange={(e) =>
                                    set("forestKind", e.target.value as ForestType)
                                }
                            >
                                {forestTypes.map((k) => (
                                    <option key={k} value={k}>
                                        {t(`forestKinds.${k}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    )}
                    {form.type === "Cave" && (
                        <>
                            <OrnateField label={t("fields.caveDepth")}>
                                <OrnateTextInput
                                    type="number"
                                    step="0.01"
                                    value={form.caveDepth}
                                    onChange={(e) =>
                                        set("caveDepth", e.target.value)
                                    }
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.caveKind")}>
                                <OrnateSelect
                                    value={form.caveKind}
                                    onChange={(e) =>
                                        set("caveKind", e.target.value as CaveType)
                                    }
                                >
                                    {caveTypes.map((k) => (
                                        <option key={k} value={k}>
                                            {t(`caveKinds.${k}`)}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                        </>
                    )}
                    {form.type === "Grassland" && (
                        <OrnateField label={t("fields.grasslandKind")}>
                            <OrnateSelect
                                value={form.grasslandKind}
                                onChange={(e) =>
                                    set("grasslandKind", e.target.value as GrasslandType)
                                }
                            >
                                {grasslandTypes.map((k) => (
                                    <option key={k} value={k}>
                                        {t(`grasslandKinds.${k}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    )}
                    {(form.type === "Country" || form.type === "City") && (
                        <>
                            <OrnateField label={t("fields.governmentSystem")}>
                                <OrnateSelect
                                    value={form.governmentSystemId}
                                    onChange={(e) =>
                                        set("governmentSystemId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {governmentSystems.map((g) => (
                                        <option key={g.id} value={g.id}>
                                            {g.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.legalSystem")}>
                                <OrnateSelect
                                    value={form.legalSystemId}
                                    onChange={(e) =>
                                        set("legalSystemId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {legalSystems.map((l) => (
                                        <option key={l.id} value={l.id}>
                                            {l.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.educationSystem")}>
                                <OrnateSelect
                                    value={form.educationSystemId}
                                    onChange={(e) =>
                                        set("educationSystemId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {educationSystems.map((e) => (
                                        <option key={e.id} value={e.id}>
                                            {e.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                            <OrnateField label={t("fields.economicSystem")}>
                                <OrnateSelect
                                    value={form.economicSystemId}
                                    onChange={(e) =>
                                        set("economicSystemId", e.target.value)
                                    }
                                >
                                    <option value="">{t("none")}</option>
                                    {economicSystems.map((e) => (
                                        <option key={e.id} value={e.id}>
                                            {e.name}
                                        </option>
                                    ))}
                                </OrnateSelect>
                            </OrnateField>
                        </>
                    )}
                    {form.type === "City" && (
                        <OrnateField label={t("fields.isCapital")}>
                            <OrnateCheckbox
                                checked={form.isCapital}
                                onChange={(e) =>
                                    set("isCapital", e.target.checked)
                                }
                            />
                        </OrnateField>
                    )}

                    <OrnateField
                        label={t("fields.parent")}
                        hint={t("form.parentHint")}
                    >
                        <OrnateSelect
                            value={form.parentLocationId}
                            onChange={(e) =>
                                set("parentLocationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {parentOptions.map((l) => (
                                <option key={l.id} value={l.id}>
                                    {l.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.area")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="0.01"
                                value={form.area}
                                onChange={(e) => set("area", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.population")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="1"
                                value={form.population}
                                onChange={(e) =>
                                    set("population", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.latitude")}>
                            <OrnateTextInput
                                type="number"
                                step="0.0001"
                                value={form.latitude}
                                onChange={(e) =>
                                    set("latitude", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.longitude")}>
                            <OrnateTextInput
                                type="number"
                                step="0.0001"
                                value={form.longitude}
                                onChange={(e) =>
                                    set("longitude", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && (
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
                                ? `/storymap/locations/${editId}`
                                : "/storymap/locations"
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
