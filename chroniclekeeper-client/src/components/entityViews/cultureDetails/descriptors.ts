import type { SelectOption } from "../../ornate";
import { getReligions } from "../../../api/religions";
import { getLocations } from "../../../api/locations";
import * as customs from "../../../api/customs";
import * as artForms from "../../../api/artForms";
import * as cuisines from "../../../api/cuisines";
import * as clothing from "../../../api/clothing";
import * as traditions from "../../../api/traditions";
import * as architectureStyles from "../../../api/architectureStyles";
import * as folklore from "../../../api/folklore";
import * as myths from "../../../api/myths";
import * as culturalFestivals from "../../../api/culturalFestivals";
import * as culturalInstitutions from "../../../api/culturalInstitutions";

export type FieldKind = "text" | "textarea" | "bool" | "number" | "select";

export interface FieldDef {
    /** DTO property key (create/update). */
    key: string;
    kind: FieldKind;
    /** For select FK fields — loads options for the current world. */
    loadOptions?: (worldId: number) => Promise<SelectOption[]>;
}

/** A generic culture-detail child (any of the 10). Scalar fields are read
 * dynamically via a `Record<string, unknown>` cast at the call site. */
export interface CultureDetailDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    cultureId?: number | null;
}

export interface Descriptor {
    /** Identity + i18n subkey (sections.<key>). */
    key: string;
    /** Own detail route base, when items link out for M:N management. */
    detailRoute?: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    list: (worldId?: number) => Promise<any[]>;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    create: (data: any) => Promise<any>;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    update: (id: number, data: any) => Promise<any>;
    remove: (id: number) => Promise<void>;
    /** Whether cultureId is optional (Custom, CulturalInstitution). */
    optionalCulture?: boolean;
    /** Scalar/FK fields beyond name + description. */
    fields: FieldDef[];
}

const loadReligions = (worldId: number): Promise<SelectOption[]> =>
    getReligions(worldId).then((rs) =>
        rs.map((r) => ({ value: String(r.id), label: r.name }))
    );

const loadLocations = (worldId: number): Promise<SelectOption[]> =>
    getLocations(worldId).then((ls) =>
        ls.map((l) => ({ value: String(l.id), label: l.name }))
    );

export const descriptors: Descriptor[] = [
    {
        key: "custom",
        optionalCulture: true,
        list: customs.getCustoms,
        create: customs.createCustom,
        update: customs.updateCustom,
        remove: customs.deleteCustom,
        fields: [{ key: "isUniversal", kind: "bool" }],
    },
    {
        key: "artForm",
        list: artForms.getArtForms,
        create: artForms.createArtForm,
        update: artForms.updateArtForm,
        remove: artForms.deleteArtForm,
        fields: [
            { key: "type", kind: "text" },
            { key: "notableArtists", kind: "text" },
            { key: "historicalInfluences", kind: "textarea" },
        ],
    },
    {
        key: "cuisine",
        list: cuisines.getCuisines,
        create: cuisines.createCuisine,
        update: cuisines.updateCuisine,
        remove: cuisines.deleteCuisine,
        fields: [
            { key: "mainIngredients", kind: "text" },
            { key: "cookingMethods", kind: "text" },
            { key: "typicalDishes", kind: "text" },
            { key: "isVegetarian", kind: "bool" },
        ],
    },
    {
        key: "clothing",
        list: clothing.getClothing,
        create: clothing.createClothing,
        update: clothing.updateClothing,
        remove: clothing.deleteClothing,
        fields: [
            { key: "clothingType", kind: "text" },
            { key: "materials", kind: "text" },
            { key: "designFeatures", kind: "text" },
            { key: "specialProperties", kind: "text" },
            { key: "isRitualistic", kind: "bool" },
            { key: "isArmor", kind: "bool" },
        ],
    },
    {
        key: "tradition",
        list: traditions.getTraditions,
        create: traditions.createTradition,
        update: traditions.updateTradition,
        remove: traditions.deleteTradition,
        fields: [
            { key: "practice", kind: "textarea" },
            { key: "isReligious", kind: "bool" },
            { key: "religionId", kind: "select", loadOptions: loadReligions },
        ],
    },
    {
        key: "architectureStyle",
        detailRoute: "/storymap/architecture-styles",
        list: architectureStyles.getArchitectureStyles,
        create: architectureStyles.createArchitectureStyle,
        update: architectureStyles.updateArchitectureStyle,
        remove: architectureStyles.deleteArchitectureStyle,
        fields: [
            { key: "materialsUsed", kind: "text" },
            { key: "designFeatures", kind: "text" },
            { key: "isFortified", kind: "bool" },
        ],
    },
    {
        key: "folklore",
        detailRoute: "/storymap/folklore",
        list: folklore.getFolklore,
        create: folklore.createFolklore,
        update: folklore.updateFolklore,
        remove: folklore.deleteFolklore,
        fields: [
            { key: "story", kind: "textarea" },
            { key: "moral", kind: "text" },
            { key: "isHistorical", kind: "bool" },
        ],
    },
    {
        key: "myth",
        list: myths.getMyths,
        create: myths.createMyth,
        update: myths.updateMyth,
        remove: myths.deleteMyth,
        fields: [
            { key: "creationStory", kind: "textarea" },
            { key: "symbolism", kind: "textarea" },
            { key: "hasReligiousConnections", kind: "bool" },
            { key: "religionId", kind: "select", loadOptions: loadReligions },
        ],
    },
    {
        key: "culturalFestival",
        list: culturalFestivals.getCulturalFestivals,
        create: culturalFestivals.createCulturalFestival,
        update: culturalFestivals.updateCulturalFestival,
        remove: culturalFestivals.deleteCulturalFestival,
        fields: [
            { key: "startDate", kind: "text" },
            { key: "durationDays", kind: "number" },
            { key: "activities", kind: "textarea" },
            { key: "isNationalHoliday", kind: "bool" },
            { key: "locationId", kind: "select", loadOptions: loadLocations },
        ],
    },
    {
        key: "culturalInstitution",
        optionalCulture: true,
        list: culturalInstitutions.getCulturalInstitutions,
        create: culturalInstitutions.createCulturalInstitution,
        update: culturalInstitutions.updateCulturalInstitution,
        remove: culturalInstitutions.deleteCulturalInstitution,
        fields: [
            { key: "institutionType", kind: "text" },
            { key: "isGovernmentFunded", kind: "bool" },
        ],
    },
];

export const descriptorByKey = (key: string): Descriptor | undefined =>
    descriptors.find((d) => d.key === key);
