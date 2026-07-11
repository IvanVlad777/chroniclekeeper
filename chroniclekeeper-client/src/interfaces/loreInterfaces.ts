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
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface NationDetailsDto extends NationDto {
    citizens: ReferenceDto[];
    history?: ReferenceDto | null;
}

export interface NationCreateDto {
    name: string;
    description: string;
    worldId: number;
    population: number;
    historyId?: number | null;
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
    nations: ReferenceDto[];
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
    nations: ReferenceDto[];
    practicedBySpecies: ReferenceDto[];
    influencedSocialClasses: ReferenceDto[];
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
    historyId?: number | null;
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
    history?: ReferenceDto | null;
}

export interface TimelineCreateDto {
    name: string;
    description: string;
    worldId: number;
    historyId?: number | null;
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
    professionId?: number | null;
    historyId?: number | null;
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
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface LocationDetailsDto extends LocationDto {
    parentLocation?: ReferenceDto | null;
    subLocations: ReferenceDto[];
    tags: ReferenceDto[];
    history?: ReferenceDto | null;
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
    historyId?: number | null;
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
    historyId?: number | null;
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
    history?: ReferenceDto | null;
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
    historyId?: number | null;
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
    professionId?: number | null;
    historyId?: number | null;
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
    professionId?: number | null;
    historyId?: number | null;
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
    profession?: ReferenceDto | null;
    history?: ReferenceDto | null;
    factions: ReferenceDto[];
    tags: ReferenceDto[];
    relationships: CharacterRelationshipDto[];
    educations: EducationRecordDto[];
    religiousEducations: ReligiousEducationDto[];
    abilities: ReferenceDto[];
    equipments: ReferenceDto[];
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
    factions: ReferenceDto[];
    nations: ReferenceDto[];
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

// ----- Professions -----

export interface ProfessionDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    requiredSkills: string;
    workEnvironment: string;
    createdAt: string;
    updatedAt: string;
}

export interface JobRankDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    professionId: number;
    rankTitle: string;
    rankLevel: number;
    responsibilities: string;
    createdAt: string;
    updatedAt: string;
}

export interface ApprenticeshipDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    professionId: number;
    tradeSchoolId?: number | null;
    tradeSchoolName?: string | null;
    durationYears: number;
    isPaid: boolean;
    skillsTaught: string;
    createdAt: string;
    updatedAt: string;
}

export interface SpecialisationDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    professionId?: number | null;
    field: string;
    createdAt: string;
    updatedAt: string;
}

export interface ProfessionDetailsDto extends ProfessionDto {
    jobRanks: JobRankDto[];
    apprenticeships: ApprenticeshipDto[];
    specialisations: SpecialisationDto[];
    practicedBySpecies: ReferenceDto[];
    socialClasses: ReferenceDto[];
    tradeSchools: ReferenceDto[];
    characters: ReferenceDto[];
}

export interface ProfessionCreateDto {
    name: string;
    description: string;
    worldId: number;
    requiredSkills: string;
    workEnvironment: string;
}

export type ProfessionUpdateDto = Omit<ProfessionCreateDto, "worldId">;

/** Svijet ranga se izvodi iz zanimanja — ne šalje se worldId. */
export interface JobRankCreateDto {
    name: string;
    description: string;
    professionId: number;
    rankTitle: string;
    rankLevel: number;
    responsibilities: string;
}

export type JobRankUpdateDto = Omit<JobRankCreateDto, "professionId">;

/** Svijet naukovanja se izvodi iz zanimanja — ne šalje se worldId. */
export interface ApprenticeshipCreateDto {
    name: string;
    description: string;
    professionId: number;
    tradeSchoolId?: number | null;
    durationYears: number;
    isPaid: boolean;
    skillsTaught: string;
}

export type ApprenticeshipUpdateDto = Omit<ApprenticeshipCreateDto, "professionId">;

/** Svijet specijalizacije se izvodi iz zanimanja — ne šalje se worldId. */
export interface SpecialisationCreateDto {
    name: string;
    description: string;
    professionId: number;
    field: string;
}

export type SpecialisationUpdateDto = Omit<SpecialisationCreateDto, "professionId">;

// ----- Education -----

export interface EducationSystemDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    isStateControlled: boolean;
    allowsPrivateInstitutions: boolean;
    includesReligiousEducation: boolean;
    supportsGuildTraining: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface EducationSystemDetailsDto extends EducationSystemDto {
    schools: ReferenceDto[];
    universities: ReferenceDto[];
}

export interface EducationSystemCreateDto {
    name: string;
    description: string;
    worldId: number;
    isStateControlled: boolean;
    allowsPrivateInstitutions: boolean;
    includesReligiousEducation: boolean;
    supportsGuildTraining: boolean;
}

export type EducationSystemUpdateDto = Omit<EducationSystemCreateDto, "worldId">;

export type SchoolType = "School" | "TradeSchool";

export interface SchoolSubjectDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    schoolId: number;
    subjectName: string;
    isMandatory: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface SchoolDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    educationSystemId: number;
    isPublic: boolean;
    isReligious: boolean;
    /** Read-only TPH diskriminator: "School" ili "TradeSchool". */
    schoolType: SchoolType;
    createdAt: string;
    updatedAt: string;
}

export interface SchoolDetailsDto extends SchoolDto {
    subjects: SchoolSubjectDto[];
    alumni: ReferenceDto[];
}

/** Svijet škole se izvodi iz sustava obrazovanja — ne šalje se worldId. */
export interface SchoolCreateDto {
    name: string;
    description: string;
    educationSystemId: number;
    isPublic: boolean;
    isReligious: boolean;
}

export type SchoolUpdateDto = Omit<SchoolCreateDto, "educationSystemId">;

export interface TradeSchoolDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    educationSystemId: number;
    isPublic: boolean;
    isReligious: boolean;
    specialization: string;
    durationYears: number;
    isGovernmentRecognized: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface TradeSchoolDetailsDto extends TradeSchoolDto {
    subjects: SchoolSubjectDto[];
    alumni: ReferenceDto[];
    trainedProfessions: ReferenceDto[];
    apprenticeships: ReferenceDto[];
}

/** Svijet strukovne škole se izvodi iz sustava obrazovanja — ne šalje se worldId. */
export interface TradeSchoolCreateDto {
    name: string;
    description: string;
    educationSystemId: number;
    isPublic: boolean;
    isReligious: boolean;
    specialization: string;
    durationYears: number;
    isGovernmentRecognized: boolean;
}

export type TradeSchoolUpdateDto = Omit<TradeSchoolCreateDto, "educationSystemId">;

/** Svijet predmeta se izvodi iz škole — ne šalje se worldId. */
export interface SchoolSubjectCreateDto {
    name: string;
    description: string;
    schoolId: number;
    subjectName: string;
    isMandatory: boolean;
}

export type SchoolSubjectUpdateDto = Omit<SchoolSubjectCreateDto, "schoolId">;

export interface UniversityMajorDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    universityId: number;
    majorName: string;
    degreeLevel: string;
    createdAt: string;
    updatedAt: string;
}

export interface UniversityDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    educationSystemId: number;
    focusesOnScience: boolean;
    focusesOnMagic: boolean;
    focusesOnPhilosophy: boolean;
    focusesOnMilitaryStudies: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface UniversityDetailsDto extends UniversityDto {
    majors: UniversityMajorDto[];
    alumni: ReferenceDto[];
}

/** Svijet sveučilišta se izvodi iz sustava obrazovanja — ne šalje se worldId. */
export interface UniversityCreateDto {
    name: string;
    description: string;
    educationSystemId: number;
    focusesOnScience: boolean;
    focusesOnMagic: boolean;
    focusesOnPhilosophy: boolean;
    focusesOnMilitaryStudies: boolean;
}

export type UniversityUpdateDto = Omit<UniversityCreateDto, "educationSystemId">;

/** Svijet smjera se izvodi iz sveučilišta — ne šalje se worldId. */
export interface UniversityMajorCreateDto {
    name: string;
    description: string;
    universityId: number;
    majorName: string;
    degreeLevel: string;
}

export type UniversityMajorUpdateDto = Omit<UniversityMajorCreateDto, "universityId">;

export interface LibraryDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    isPublic: boolean;
    focusesOnMagic: boolean;
    focusesOnHistory: boolean;
    universityId?: number | null;
    university?: ReferenceDto | null;
    locationId?: number | null;
    location?: ReferenceDto | null;
    createdAt: string;
    updatedAt: string;
}

export type LibraryDetailsDto = LibraryDto;

export interface LibraryCreateDto {
    name: string;
    description: string;
    worldId: number;
    isPublic: boolean;
    focusesOnMagic: boolean;
    focusesOnHistory: boolean;
    universityId?: number | null;
    locationId?: number | null;
}

export type LibraryUpdateDto = Omit<LibraryCreateDto, "worldId">;

export interface EducationRecordDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    characterId?: number | null;
    schoolId?: number | null;
    universityId?: number | null;
    startDate: string;
    endDate?: string | null;
    degree: string;
    createdAt: string;
    updatedAt: string;
}

export interface EducationRecordCreateDto {
    name: string;
    description: string;
    worldId: number;
    characterId?: number | null;
    schoolId?: number | null;
    universityId?: number | null;
    startDate: string;
    endDate?: string | null;
    degree: string;
}

export type EducationRecordUpdateDto = Omit<EducationRecordCreateDto, "worldId">;

export interface ReligiousEducationDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    characterId?: number | null;
    religionId: number;
    startDate: string;
    completionDate?: string | null;
    ordained: boolean;
    createdAt: string;
    updatedAt: string;
}

/** Svijet vjerskog obrazovanja se izvodi iz religije — ne šalje se worldId. */
export interface ReligiousEducationCreateDto {
    name: string;
    description: string;
    religionId: number;
    characterId?: number | null;
    startDate: string;
    completionDate?: string | null;
    ordained: boolean;
}

export type ReligiousEducationUpdateDto = Omit<ReligiousEducationCreateDto, "religionId">;

// ----- Abilities -----

export const abilityTypes = ["Physical", "Magical", "Mental", "Technical"] as const;
export type AbilityType = (typeof abilityTypes)[number];

export const abilityRanks = ["Beginner", "Expert", "Master"] as const;
export type AbilityRank = (typeof abilityRanks)[number];

export interface AbilityDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    type: string;
    createdAt: string;
    updatedAt: string;
}

export interface AbilityCreateDto {
    name: string;
    description: string;
    worldId: number;
    type: string;
}

export type AbilityUpdateDto = Omit<AbilityCreateDto, "worldId">;

export interface AbilityLevelDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    abilityId: number;
    rank: string;
    createdAt: string;
    updatedAt: string;
}

/** Svijet razine se izvodi iz sposobnosti — ne šalje se worldId. */
export interface AbilityLevelCreateDto {
    name: string;
    description: string;
    abilityId: number;
    rank: string;
}

export type AbilityLevelUpdateDto = Omit<AbilityLevelCreateDto, "abilityId">;

// ----- Equipment -----

export const itemCategories = ["Weapon", "Armor", "Tool", "MagicalItem"] as const;
export type ItemCategory = (typeof itemCategories)[number];

export const itemRarities = ["Common", "Uncommon", "Rare", "Legendary", "Mythical"] as const;
export type ItemRarity = (typeof itemRarities)[number];

export const ownershipTransferReasons = ["Stolen", "Inherited", "Gifted", "Lost", "Found"] as const;
export type OwnershipTransferReason = (typeof ownershipTransferReasons)[number];

export interface ItemDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    category: string;
    isUnique: boolean;
    material: string;
    specialProperties: string;
    rarity: string;
    currentOwnerId?: number | null;
    storedAtId?: number | null;
    factionId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface ItemDetailsDto extends ItemDto {
    currentOwner?: ReferenceDto | null;
    storedAt?: ReferenceDto | null;
    faction?: ReferenceDto | null;
    ownershipHistory: OwnershipHistoryDto[];
}

export interface ItemCreateDto {
    name: string;
    description: string;
    worldId: number;
    category: string;
    isUnique: boolean;
    material: string;
    specialProperties: string;
    rarity: string;
    currentOwnerId?: number | null;
    storedAtId?: number | null;
    factionId?: number | null;
}

export type ItemUpdateDto = Omit<ItemCreateDto, "worldId">;

export interface OwnershipHistoryDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    itemId: number;
    previousOwnerId?: number | null;
    newOwnerId?: number | null;
    dateAcquired: string;
    transferReason: string;
    createdAt: string;
    updatedAt: string;
}

/** Svijet zapisa se izvodi iz predmeta — ne šalje se worldId. */
export interface OwnershipHistoryCreateDto {
    name: string;
    description: string;
    itemId: number;
    previousOwnerId?: number | null;
    newOwnerId?: number | null;
    dateAcquired: string;
    transferReason: string;
}

export type OwnershipHistoryUpdateDto = Omit<OwnershipHistoryCreateDto, "itemId">;

// ----- History -----

export interface HistoryDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    summary: string;
    isOfficial: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface HistoryDetailsDto extends HistoryDto {
    timelines: ReferenceDto[];
}

export interface HistoryCreateDto {
    name: string;
    description: string;
    worldId: number;
    summary: string;
    isOfficial: boolean;
}

export type HistoryUpdateDto = Omit<HistoryCreateDto, "worldId">;

// ----- Content -----

export const contentTypes = ["Article", "Book", "Comic", "Movie", "Series"] as const;
export type ContentType = (typeof contentTypes)[number];

/** Ravni DTO za Article/Book/Comic/Movie/Series TPH obitelj — samo polja relevantna za `type` su popunjena. */
export interface ContentDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    type: string;
    source?: string | null;
    publishDate?: string | null;
    author?: string | null;
    releaseDate?: string | null;
    issueNumber?: number | null;
    director?: string | null;
    durationMinutes?: number | null;
    prequelId?: number | null;
    creator?: string | null;
    seasons?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface ContentReferenceEntryDto {
    id: number;
    note: string;
    entityType: string;
    entityId: number;
    entityName: string;
}

export interface ContentDetailsDto extends ContentDto {
    /** Popunjeno samo kad je type === "Book". */
    chapters: ReferenceDto[];
    /** Popunjeno samo kad je type === "Series". */
    episodes: ReferenceDto[];
    /** Popunjeno samo kad je type === "Movie". */
    prequel?: ReferenceDto | null;
    sequels: ReferenceDto[];
    references: ContentReferenceEntryDto[];
}

export interface ContentCreateDto {
    name: string;
    description: string;
    worldId: number;
    type: string;
    source?: string | null;
    publishDate?: string | null;
    author?: string | null;
    releaseDate?: string | null;
    issueNumber?: number | null;
    director?: string | null;
    durationMinutes?: number | null;
    prequelId?: number | null;
    creator?: string | null;
    seasons?: number | null;
}

export type ContentUpdateDto = Omit<ContentCreateDto, "worldId" | "type">;

// ----- Chapter -----

export interface ChapterDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    bookId: number;
    order: number;
    createdAt: string;
    updatedAt: string;
}

/** Svijet poglavlja se izvodi iz knjige — ne šalje se worldId. */
export interface ChapterCreateDto {
    name: string;
    description: string;
    bookId: number;
    order: number;
}

export type ChapterUpdateDto = Omit<ChapterCreateDto, "bookId">;

// ----- Episode -----

export interface EpisodeDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    seriesId: number;
    season: number;
    order: number;
    createdAt: string;
    updatedAt: string;
}

/** Svijet epizode se izvodi iz serije — ne šalje se worldId. */
export interface EpisodeCreateDto {
    name: string;
    description: string;
    seriesId: number;
    season: number;
    order: number;
}

export type EpisodeUpdateDto = Omit<EpisodeCreateDto, "seriesId">;

// ----- Content reference links (Reference entitet) -----

export interface ContentReferenceLinkDto {
    id: number;
    note: string;
    contentId?: number | null;
    chapterId?: number | null;
    episodeId?: number | null;
    characterId?: number | null;
    locationId?: number | null;
    factionId?: number | null;
    nationId?: number | null;
}

export type ContentReferenceLinkCreateDto = Omit<ContentReferenceLinkDto, "id">;
export type ContentReferenceLinkUpdateDto = ContentReferenceLinkCreateDto;

// ----- Climate -----

export const climateZoneTypes = [
    "Tropical",
    "Arid",
    "Temperate",
    "Continental",
    "Polar",
] as const;
export type ClimateZoneType = (typeof climateZoneTypes)[number];

export const windDirections = [
    "North",
    "South",
    "East",
    "West",
    "Northeast",
    "Northwest",
    "Southeast",
    "Southwest",
    "Variable",
] as const;
export type WindDirection = (typeof windDirections)[number];

export const notableWeatherPhenomena = [
    "None",
    "Tornado",
    "Hurricane",
    "Monsoon",
    "Sandstorm",
    "Blizzard",
    "Thunderstorm",
    "Drought",
    "Fog",
] as const;
export type NotableWeatherPhenomena = (typeof notableWeatherPhenomena)[number];

export const weatherPatternTypes = [
    "Normal",
    "Monsoon",
    "DesertStorm",
    "HurricaneSeason",
    "ArcticBlast",
    "HeatWave",
] as const;
export type WeatherPatternType = (typeof weatherPatternTypes)[number];

export const weatherFrequencies = ["Rare", "Seasonal", "Frequent", "Constant"] as const;
export type WeatherFrequency = (typeof weatherFrequencies)[number];

export const weatherEffects = [
    "None",
    "Flooding",
    "Drought",
    "HighWinds",
    "ExtremeCold",
    "ExtremeHeat",
] as const;
export type WeatherEffect = (typeof weatherEffects)[number];

export interface WeatherPatternDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    climateZoneId: number;
    patternType: WeatherPatternType;
    frequency: WeatherFrequency;
    effects: WeatherEffect;
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

/** Svijet vremenskog obrasca se izvodi iz klimatske zone — ne šalje se worldId. */
export interface WeatherPatternCreateDto {
    name: string;
    description: string;
    climateZoneId: number;
    patternType: WeatherPatternType;
    frequency: WeatherFrequency;
    effects: WeatherEffect;
    historyId?: number | null;
}

export type WeatherPatternUpdateDto = Omit<WeatherPatternCreateDto, "climateZoneId">;

export interface ClimateZoneDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    zoneType: ClimateZoneType;
    averageTemperature: number;
    averageHumidity: number;
    averagePrecipitation: number;
    hasDistinctSeasons: boolean;
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface ClimateZoneDetailsDto extends ClimateZoneDto {
    history: ReferenceDto | null;
    climates: ReferenceDto[];
    seasons: ReferenceDto[];
    locations: ReferenceDto[];
    weatherPatterns: WeatherPatternDto[];
}

export interface ClimateZoneCreateDto {
    name: string;
    description: string;
    worldId: number;
    zoneType: ClimateZoneType;
    averageTemperature: number;
    averageHumidity: number;
    averagePrecipitation: number;
    hasDistinctSeasons: boolean;
    historyId?: number | null;
}

export type ClimateZoneUpdateDto = Omit<ClimateZoneCreateDto, "worldId">;

export interface ClimateDetailDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    averageTemperature: number;
    humidity: number;
    precipitation: number;
    windSpeed: number;
    windDirection: WindDirection;
    isExtremeClimate: boolean;
    notableWeatherPhenomena: NotableWeatherPhenomena;
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface ClimateDetailCreateDto {
    name: string;
    description: string;
    worldId: number;
    averageTemperature: number;
    humidity: number;
    precipitation: number;
    windSpeed: number;
    windDirection: WindDirection;
    isExtremeClimate: boolean;
    notableWeatherPhenomena: NotableWeatherPhenomena;
    historyId?: number | null;
}

export type ClimateDetailUpdateDto = Omit<ClimateDetailCreateDto, "worldId">;

export interface SeasonDto {
    id: number;
    name: string;
    description: string;
    worldId: number;
    durationInDays: number;
    typicalTemperature: number;
    typicalPrecipitation: number;
    historyId?: number | null;
    createdAt: string;
    updatedAt: string;
}

export interface SeasonCreateDto {
    name: string;
    description: string;
    worldId: number;
    durationInDays: number;
    typicalTemperature: number;
    typicalPrecipitation: number;
    historyId?: number | null;
}

export type SeasonUpdateDto = Omit<SeasonCreateDto, "worldId">;
