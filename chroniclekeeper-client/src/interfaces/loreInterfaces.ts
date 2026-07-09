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
}

export interface CharacterRelationshipDto {
    id: number;
    relatedCharacterId: number;
    relatedCharacterName: string;
    type: string;
    description: string;
    isSecret: boolean;
}

export interface CharacterDetailsDto extends CharacterDto {
    father?: ReferenceDto | null;
    mother?: ReferenceDto | null;
    species?: ReferenceDto | null;
    race?: ReferenceDto | null;
    factions: ReferenceDto[];
    tags: ReferenceDto[];
    relationships: CharacterRelationshipDto[];
}
