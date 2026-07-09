import api from "../services/api";
import {
    FactionCreateDto,
    FactionDetailsDto,
    FactionDto,
    FactionMemberAddDto,
    FactionUpdateDto,
} from "../interfaces/loreInterfaces";

/** Frakcije, opcionalno filtrirane po svijetu. */
export const getFactions = async (worldId?: number): Promise<FactionDto[]> => {
    const response = await api.get<FactionDto[]>("/factions", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getFaction = async (id: number): Promise<FactionDetailsDto> => {
    const response = await api.get<FactionDetailsDto>(`/factions/${id}`);
    return response.data;
};

export const createFaction = async (
    data: FactionCreateDto
): Promise<FactionDto> => {
    const response = await api.post<FactionDto>("/factions", data);
    return response.data;
};

export const updateFaction = async (
    id: number,
    data: FactionUpdateDto
): Promise<FactionDto> => {
    const response = await api.put<FactionDto>(`/factions/${id}`, data);
    return response.data;
};

export const deleteFaction = async (id: number): Promise<void> => {
    await api.delete(`/factions/${id}`);
};

export const addFactionMember = async (
    factionId: number,
    data: FactionMemberAddDto
): Promise<void> => {
    await api.post(`/factions/${factionId}/members`, data);
};

export const removeFactionMember = async (
    factionId: number,
    characterId: number
): Promise<void> => {
    await api.delete(`/factions/${factionId}/members/${characterId}`);
};
