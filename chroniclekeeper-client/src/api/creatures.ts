import api from "../services/api";
import {
    CreatureCreateDto,
    CreatureDetailsDto,
    CreatureDto,
    CreatureUpdateDto,
} from "../interfaces/loreInterfaces";

/** Stvorenja (Animal/Plant/Tree/Crop/Fungus), opcionalno filtrirana po svijetu i/ili podtipu. */
export const getCreatures = async (params?: {
    worldId?: number;
    subtype?: string;
}): Promise<CreatureDto[]> => {
    const response = await api.get<CreatureDto[]>("/creatures", { params });
    return response.data;
};

export const getCreatureById = async (id: number): Promise<CreatureDetailsDto> => {
    const response = await api.get<CreatureDetailsDto>(`/creatures/${id}`);
    return response.data;
};

export const createCreature = async (
    data: CreatureCreateDto
): Promise<CreatureDto> => {
    const response = await api.post<CreatureDto>("/creatures", data);
    return response.data;
};

export const updateCreature = async (
    id: number,
    data: CreatureUpdateDto
): Promise<CreatureDto> => {
    const response = await api.put<CreatureDto>(`/creatures/${id}`, data);
    return response.data;
};

export const deleteCreature = async (id: number): Promise<void> => {
    await api.delete(`/creatures/${id}`);
};

export const addCreatureCity = async (
    creatureId: number,
    cityId: number
): Promise<void> => {
    await api.post(`/creatures/${creatureId}/cities/${cityId}`);
};

export const removeCreatureCity = async (
    creatureId: number,
    cityId: number
): Promise<void> => {
    await api.delete(`/creatures/${creatureId}/cities/${cityId}`);
};

export const addCreatureHabitat = async (
    creatureId: number,
    ecosystemId: number
): Promise<void> => {
    await api.post(`/creatures/${creatureId}/habitats/${ecosystemId}`);
};

export const removeCreatureHabitat = async (
    creatureId: number,
    ecosystemId: number
): Promise<void> => {
    await api.delete(`/creatures/${creatureId}/habitats/${ecosystemId}`);
};
