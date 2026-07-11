import api from "../services/api";
import {
    AbilityCreateDto,
    AbilityDto,
    AbilityLevelCreateDto,
    AbilityLevelDto,
    AbilityLevelUpdateDto,
    AbilityUpdateDto,
} from "../interfaces/loreInterfaces";

/** Sposobnosti, opcionalno filtrirane po svijetu. */
export const getAbilities = async (
    worldId?: number
): Promise<AbilityDto[]> => {
    const response = await api.get<AbilityDto[]>("/abilities", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getAbilityById = async (id: number): Promise<AbilityDto> => {
    const response = await api.get<AbilityDto>(`/abilities/${id}`);
    return response.data;
};

export const createAbility = async (
    data: AbilityCreateDto
): Promise<AbilityDto> => {
    const response = await api.post<AbilityDto>("/abilities", data);
    return response.data;
};

export const updateAbility = async (
    id: number,
    data: AbilityUpdateDto
): Promise<AbilityDto> => {
    const response = await api.put<AbilityDto>(`/abilities/${id}`, data);
    return response.data;
};

export const deleteAbility = async (id: number): Promise<void> => {
    await api.delete(`/abilities/${id}`);
};

// ----- Ability levels (inline-managed na Ability detalju) -----

export const getAbilityLevels = async (params?: {
    worldId?: number;
    abilityId?: number;
}): Promise<AbilityLevelDto[]> => {
    const response = await api.get<AbilityLevelDto[]>("/ability-levels", {
        params,
    });
    return response.data;
};

export const createAbilityLevel = async (
    data: AbilityLevelCreateDto
): Promise<AbilityLevelDto> => {
    const response = await api.post<AbilityLevelDto>(
        "/ability-levels",
        data
    );
    return response.data;
};

export const updateAbilityLevel = async (
    id: number,
    data: AbilityLevelUpdateDto
): Promise<AbilityLevelDto> => {
    const response = await api.put<AbilityLevelDto>(
        `/ability-levels/${id}`,
        data
    );
    return response.data;
};

export const deleteAbilityLevel = async (id: number): Promise<void> => {
    await api.delete(`/ability-levels/${id}`);
};
