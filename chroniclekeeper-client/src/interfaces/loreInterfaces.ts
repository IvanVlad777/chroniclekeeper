// Tipovi koji odgovaraju API DTO-ima (ChronicleKeeper.Core/DTOs)

export interface ReferenceDto {
    id: number;
    name: string;
}

export interface WorldDto {
    id: number;
    name: string;
    description: string;
    ownerId: string;
    createdAt: string;
    updatedAt: string;
}

export interface WorldCreateDto {
    name: string;
    description: string;
}

export interface SpeciesDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    commonName: string;
    scientificName: string;
    isHumanoid: boolean;
    lifespan: string;
    createdAt: string;
    updatedAt: string;
}

export interface RaceDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    sapientSpeciesId: number;
    appearanceTraits: string;
    geneticFeatures: string;
    adaptations: string;
    createdAt: string;
    updatedAt: string;
}

export interface SpeciesDetailsDto extends SpeciesDto {
    races: RaceDto[];
}

export interface SpeciesCreateDto {
    name: string;
    description: string;
    worldId: number;
    commonName: string;
    scientificName: string;
    isHumanoid: boolean;
    lifespan: string;
}

export type SpeciesUpdateDto = Omit<SpeciesCreateDto, "worldId">;

/** Svijet rase izvodi se iz vrste — ne šalje se worldId. */
export interface RaceCreateDto {
    name: string;
    description: string;
    sapientSpeciesId: number;
    appearanceTraits: string;
    geneticFeatures: string;
    adaptations: string;
}

/** Vrsta rase se ne mijenja (backend invarijanta) — zato bez sapientSpeciesId. */
export type RaceUpdateDto = Omit<RaceCreateDto, "sapientSpeciesId">;

export interface SocialClassDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    isNoble: boolean;
    isMerchantClass: boolean;
    isOutcast: boolean;
    canOwnLand: boolean;
    canHoldOffice: boolean;
    hasTaxExemptions: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface SocialClassDetailsDto extends SocialClassDto {
    members: ReferenceDto[];
}

export interface SocialClassCreateDto {
    name: string;
    description: string;
    worldId: number;
    isNoble: boolean;
    isMerchantClass: boolean;
    isOutcast: boolean;
    canOwnLand: boolean;
    canHoldOffice: boolean;
    hasTaxExemptions: boolean;
}

export type SocialClassUpdateDto = Omit<SocialClassCreateDto, "worldId">;

export interface NationDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    population: number;
    createdAt: string;
    updatedAt: string;
}

export interface NationDetailsDto extends NationDto {
    citizens: ReferenceDto[];
}

export interface NationCreateDto {
    name: string;
    description: string;
    worldId: number;
    population: number;
}

export type NationUpdateDto = Omit<NationCreateDto, "worldId">;

export interface ReligionDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    coreBeliefs: string;
    practices: string;
    hasDeities: boolean;
    isStateReligion: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface ReligionDetailsDto extends ReligionDto {
    followers: ReferenceDto[];
}

export interface ReligionCreateDto {
    name: string;
    description: string;
    worldId: number;
    coreBeliefs: string;
    practices: string;
    hasDeities: boolean;
    isStateReligion: boolean;
}

export type ReligionUpdateDto = Omit<ReligionCreateDto, "worldId">;

export interface LanguageDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    writingSystem: string;
    isExtinct: boolean;
    dialects: string;
    createdAt: string;
    updatedAt: string;
}

export interface LanguageDetailsDto extends LanguageDto {
    cultures: ReferenceDto[];
}

export interface LanguageCreateDto {
    name: string;
    description: string;
    worldId: number;
    writingSystem: string;
    isExtinct: boolean;
    dialects: string;
}

export type LanguageUpdateDto = Omit<LanguageCreateDto, "worldId">;

export const xenophobiaLevels = [
    "VeryOpen",
    "Open",
    "Neutral",
    "Suspicious",
    "Extreme",
] as const;
export type XenophobiaLevel = (typeof xenophobiaLevels)[number];

export const technologicalLevels = [
    "Primitive",
    "Medieval",
    "Industrial",
    "Modern",
    "Advanced",
    "Futuristic",
    "PostHuman",
] as const;
export type TechnologicalLevel = (typeof technologicalLevels)[number];

export interface CultureDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    languageId: number;
    religionId?: number | null;
    commonValues: string;
    hasOralTradition: boolean;
    socialStructure: string;
    xenophobiaLevel: XenophobiaLevel;
    technologicalLevel: TechnologicalLevel;
    conflictResolution: string;
    createdAt: string;
    updatedAt: string;
}

export interface CultureDetailsDto extends CultureDto {
    language: ReferenceDto | null;
    religion: ReferenceDto | null;
}

export interface CultureCreateDto {
    name: string;
    description: string;
    worldId: number;
    languageId: number;
    religionId?: number | null;
    commonValues: string;
    hasOralTradition: boolean;
    socialStructure: string;
    xenophobiaLevel: XenophobiaLevel;
    technologicalLevel: TechnologicalLevel;
    conflictResolution: string;
}

export type CultureUpdateDto = Omit<CultureCreateDto, "worldId">;

export interface TimelineDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    createdAt: string;
    updatedAt: string;
}

export interface TimelineEventDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    timelineId: number;
    /** Slobodan in-world datum, npr. "Godina 512, Treće doba". */
    date: string;
    sortOrder: number;
    consequences: string;
    isMajorEvent: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface TimelineDetailsDto extends TimelineDto {
    /** Eventi poredani po sortOrder. */
    events: TimelineEventDto[];
}

export interface TimelineCreateDto {
    name: string;
    description: string;
    worldId: number;
}

export type TimelineUpdateDto = Omit<TimelineCreateDto, "worldId">;

/** Svijet eventa izvodi se iz timelinea — ne šalje se worldId. */
export interface TimelineEventCreateDto {
    name: string;
    description: string;
    date: string;
    sortOrder: number;
    consequences: string;
    isMajorEvent: boolean;
}

export type TimelineEventUpdateDto = TimelineEventCreateDto;

export interface NoteDto {
    id: number;
    /** Naslov bilješke. */
    name: string;
    description: string;
    worldId: number;
    content: string;
    createdAt: string;
    updatedAt: string;
}

export interface NoteCreateDto {
    name: string;
    description: string;
    worldId: number;
    content: string;
}

export type NoteUpdateDto = Omit<NoteCreateDto, "worldId">;

export interface CharacterDto {
    id: number;
    name: string;
    firstName: string;
    lastName: string;
    nickname: string;
    title: string;
    birthDate?: string | null;
    deathDate?: string | null;
    description: string;
    createdAt: string;
    updatedAt: string;
    height?: number | null;
    weight?: number | null;
    hairColor: string;
    eyeColor: string;
    specialPhysicalFeatures: string;
    isArtificial: boolean;
    worldId: number;
    fatherId?: number | null;
    motherId?: number | null;
    sapientSpeciesId?: number | null;
    raceId?: number | null;
    socialClassId?: number | null;
    nationId?: number | null;
    religionId?: number | null;
}

export const locationTypes = [
    "Continent",
    "Region",
    "Country",
    "City",
    "Town",
    "Village",
    "District",
    "Building",
    "Landmark",
    "Wilderness",
    "Other",
] as const;
export type LocationType = (typeof locationTypes)[number];

export interface LocationDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    type: LocationType;
    area?: number | null;
    population?: number | null;
    latitude?: number | null;
    longitude?: number | null;
    parentLocationId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface LocationDetailsDto extends LocationDto {
    parentLocation?: ReferenceDto | null;
    subLocations: ReferenceDto[];
    tags: ReferenceDto[];
}

export interface LocationCreateDto {
    name: string;
    description: string;
    worldId: number;
    type: LocationType;
    area?: number | null;
    population?: number | null;
    latitude?: number | null;
    longitude?: number | null;
    parentLocationId?: number | null;
}

export type LocationUpdateDto = Omit<LocationCreateDto, "worldId">;

export const factionTypes = [
    "CriminalSyndicate",
    "ReligiousSect",
    "PoliticalMovement",
    "MercenaryCompany",
    "TradeConsortium",
    "ResistanceGroup",
    "SecretSociety",
    "KnightlyOrder",
    "ScholarSociety",
    "ArcaneCoven",
    "TechnologicalUnion",
    "MilitaryAlliance",
    "PirateBrotherhood",
    "DiplomaticLeague",
    "RadicalExtremist",
    "Adventurers",
] as const;
export type FactionType = (typeof factionTypes)[number];

export interface FactionDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    type: FactionType;
    isSecretive: boolean;
    motto: string;
    leaderId?: number | null;
    headquartersId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface FactionMemberDto {
    id: number;
    characterId: number;
    characterName: string;
    role: string;
    isSecret: boolean;
}

export interface FactionDetailsDto extends FactionDto {
    leader?: ReferenceDto | null;
    headquarters?: ReferenceDto | null;
    members: FactionMemberDto[];
    tags: ReferenceDto[];
}

export interface FactionCreateDto {
    name: string;
    description: string;
    worldId: number;
    type: FactionType;
    isSecretive: boolean;
    motto: string;
    leaderId?: number | null;
    headquartersId?: number | null;
}

export type FactionUpdateDto = Omit<FactionCreateDto, "worldId">;

export interface FactionMemberAddDto {
    characterId: number;
    role: string;
    isSecret: boolean;
}

/** POST /characters — create prima samo osnovna polja; ostatak ide kroz PUT. */
export interface CharacterCreateDto {
    name: string;
    firstName: string;
    lastName: string;
    nickname: string;
    title: string;
    birthDate?: string | null;
    isArtificial: boolean;
    worldId: number;
    sapientSpeciesId?: number | null;
    raceId?: number | null;
    fatherId?: number | null;
    motherId?: number | null;
    socialClassId?: number | null;
    nationId?: number | null;
    religionId?: number | null;
}

/** PUT /characters/{id} — full replace: izostavljena polja se resetiraju. */
export interface CharacterUpdateDto {
    name: string;
    firstName: string;
    lastName: string;
    nickname: string;
    title: string;
    birthDate?: string | null;
    deathDate?: string | null;
    description: string;
    height?: number | null;
    weight?: number | null;
    hairColor: string;
    eyeColor: string;
    specialPhysicalFeatures: string;
    isArtificial: boolean;
    sapientSpeciesId?: number | null;
    raceId?: number | null;
    fatherId?: number | null;
    motherId?: number | null;
    socialClassId?: number | null;
    nationId?: number | null;
    religionId?: number | null;
}

/** Enumi putuju kao stringovi (globalni JsonStringEnumConverter na API-ju). */
export const relationshipTypes = [
    "Friend",
    "Enemy",
    "Rival",
    "Ally",
    "Mentor",
    "Student",
    "RomanticPartner",
    "Spouse",
    "ExPartner",
    "Other",
] as const;
export type RelationshipType = (typeof relationshipTypes)[number];

export interface CharacterRelationshipDto {
    id: number;
    relatedCharacterId: number;
    relatedCharacterName: string;
    type: string;
    description: string;
    isSecret: boolean;
}

export interface CharacterRelationshipCreateDto {
    relatedCharacterId: number;
    type: RelationshipType;
    description: string;
    isSecret: boolean;
}

export type TagTargetType = "Character" | "Location" | "Faction";

export interface TagDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    createdAt: string;
    updatedAt: string;
}

export interface TagCreateDto {
    name: string;
    description: string;
    worldId: number;
}

export interface TagUpdateDto {
    name: string;
    description: string;
}

export interface TagAttachDto {
    targetType: TagTargetType;
    targetId: number;
}

export interface CharacterDetailsDto extends CharacterDto {
    father?: ReferenceDto | null;
    mother?: ReferenceDto | null;
    species?: ReferenceDto | null;
    race?: ReferenceDto | null;
    socialClass?: ReferenceDto | null;
    nation?: ReferenceDto | null;
    religion?: ReferenceDto | null;
    factions: ReferenceDto[];
    tags: ReferenceDto[];
    relationships: CharacterRelationshipDto[];
}

export const electionSystems = [
    "DirectElection",
    "Parliamentary",
    "Hereditary",
    "Meritocratic",
    "DivineMandate",
    "Oligarchic",
    "NoElections",
] as const;
export type ElectionSystem = (typeof electionSystems)[number];

export const scaleLevels = ["Low", "Moderate", "High"] as const;
export type ScaleLevel = (typeof scaleLevels)[number];

export const punishmentMethods = [
    "DeathPenalty",
    "Imprisonment",
    "ForcedLabor",
    "Fines",
    "Exile",
    "CorporalPunishment",
    "Rehabilitation",
] as const;
export type PunishmentMethod = (typeof punishmentMethods)[number];

export interface PoliticalIdeologyDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    isAuthoritarian: boolean;
    isSocialist: boolean;
    isLiberal: boolean;
    isRadical: boolean;
    isMilitaristic: boolean;
    supportsFreeMarket: boolean;
    supportsPlannedEconomy: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface PoliticalIdeologyDetailsDto extends PoliticalIdeologyDto {
    affiliatedPoliticalParties: ReferenceDto[];
    affiliatedGovernmentSystems: ReferenceDto[];
}

export interface PoliticalIdeologyCreateDto {
    name: string;
    description: string;
    worldId: number;
    isAuthoritarian: boolean;
    isSocialist: boolean;
    isLiberal: boolean;
    isRadical: boolean;
    isMilitaristic: boolean;
    supportsFreeMarket: boolean;
    supportsPlannedEconomy: boolean;
}

export type PoliticalIdeologyUpdateDto = Omit<
    PoliticalIdeologyCreateDto,
    "worldId"
>;

export interface GovernmentSystemDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    isDemocratic: boolean;
    isMonarchic: boolean;
    isReligious: boolean;
    isFederal: boolean;
    isCentralized: boolean;
    politicalIdeologyId?: number | null;
    electionSystem: ElectionSystem;
    stabilityLevel: ScaleLevel;
    hasTermLimits: boolean;
    maxTermLength?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface GovernmentSystemDetailsDto extends GovernmentSystemDto {
    politicalIdeology: ReferenceDto | null;
    politicalParties: ReferenceDto[];
}

export interface GovernmentSystemCreateDto {
    name: string;
    description: string;
    worldId: number;
    isDemocratic: boolean;
    isMonarchic: boolean;
    isReligious: boolean;
    isFederal: boolean;
    isCentralized: boolean;
    politicalIdeologyId?: number | null;
    electionSystem: ElectionSystem;
    stabilityLevel: ScaleLevel;
    hasTermLimits: boolean;
    maxTermLength?: number | null;
}

export type GovernmentSystemUpdateDto = Omit<
    GovernmentSystemCreateDto,
    "worldId"
>;

export interface PoliticalPartyDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    ideologyDescription: string;
    politicalIdeologyId: number;
    governmentSystemId?: number | null;
    influenceLevel: ScaleLevel;
    isBanned: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface PoliticalPartyDetailsDto extends PoliticalPartyDto {
    politicalIdeology: ReferenceDto | null;
    governmentSystem: ReferenceDto | null;
}

export interface PoliticalPartyCreateDto {
    name: string;
    description: string;
    worldId: number;
    ideologyDescription: string;
    politicalIdeologyId: number;
    governmentSystemId?: number | null;
    influenceLevel: ScaleLevel;
    isBanned: boolean;
}

export type PoliticalPartyUpdateDto = Omit<PoliticalPartyCreateDto, "worldId">;

export interface LegalSystemDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    laws: string;
    judicialIndependence: ScaleLevel;
    punishmentMethods: PunishmentMethod;
    createdAt: string;
    updatedAt: string;
}

export type LegalSystemDetailsDto = LegalSystemDto;

export interface LegalSystemCreateDto {
    name: string;
    description: string;
    worldId: number;
    laws: string;
    judicialIndependence: ScaleLevel;
    punishmentMethods: PunishmentMethod;
}

export type LegalSystemUpdateDto = Omit<LegalSystemCreateDto, "worldId">;

export interface DiplomaticAgreementDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    agreementType: string;
    signedDate: string;
    terminationDate?: string | null;
    durationYears?: number | null;
    terms: string;
    isUnequal: boolean;
    firstNationId: number;
    secondNationId: number;
    createdAt: string;
    updatedAt: string;
}

export interface DiplomaticAgreementDetailsDto extends DiplomaticAgreementDto {
    firstNation: ReferenceDto;
    secondNation: ReferenceDto;
}

export interface DiplomaticAgreementCreateDto {
    name: string;
    description: string;
    worldId: number;
    agreementType: string;
    signedDate: string;
    terminationDate?: string | null;
    durationYears?: number | null;
    terms: string;
    isUnequal: boolean;
    firstNationId: number;
    secondNationId: number;
}

export type DiplomaticAgreementUpdateDto = Omit<
    DiplomaticAgreementCreateDto,
    "worldId"
>;
