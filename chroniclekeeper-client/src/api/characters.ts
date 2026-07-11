import api from "../services/api";
import {
    CharacterCreateDto,
    CharacterDetailsDto,
    CharacterDto,
    CharacterRelationshipCreateDto,
    CharacterRelationshipDto,
    CharacterUpdateDto,
} from "../interfaces/loreInterfaces";

/** Likovi, opcionalno filtrirani po svijetu. */
export const getCharacters = async (
    worldId?: number
): Promise<CharacterDto[]> => {
    const response = await api.get<CharacterDto[]>("/characters", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCharacter = async (
    id: number
): Promise<CharacterDetailsDto> => {
    const response = await api.get<CharacterDetailsDto>(`/characters/${id}`);
    return response.data;
};

export const createCharacter = async (
    data: CharacterCreateDto
): Promise<CharacterDto> => {
    const response = await api.post<CharacterDto>("/characters", data);
    return response.data;
};

export const updateCharacter = async (
    id: number,
    data: CharacterUpdateDto
): Promise<CharacterDto> => {
    const response = await api.put<CharacterDto>(`/characters/${id}`, data);
    return response.data;
};

/** Brisanje je dopušteno samo Admin/SuperAdmin rolama (API vraća 403 inače). */
export const deleteCharacter = async (id: number): Promise<void> => {
    await api.delete(`/characters/${id}`);
};

export const addRelationship = async (
    characterId: number,
    data: CharacterRelationshipCreateDto
): Promise<CharacterRelationshipDto> => {
    const response = await api.post<CharacterRelationshipDto>(
        `/characters/${characterId}/relationships`,
        data
    );
    return response.data;
};

export const removeRelationship = async (
    characterId: number,
    relationshipId: number
): Promise<void> => {
    await api.delete(
        `/characters/${characterId}/relationships/${relationshipId}`
    );
};

export const addCharacterAbility = async (
    characterId: number,
    abilityId: number
): Promise<void> => {
    await api.post(`/characters/${characterId}/abilities/${abilityId}`);
};

export const removeCharacterAbility = async (
    characterId: number,
    abilityId: number
): Promise<void> => {
    await api.delete(`/characters/${characterId}/abilities/${abilityId}`);
};
