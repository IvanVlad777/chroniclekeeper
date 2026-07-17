import api from "../services/api";
import {
    HistoryCreateDto,
    HistoryDetailsDto,
    HistoryDto,
    HistoryUpdateDto,
} from "../interfaces/loreInterfaces";

/** Povijesti, opcionalno filtrirane po svijetu. */
export const getHistories = async (worldId?: number): Promise<HistoryDto[]> => {
    const response = await api.get<HistoryDto[]>("/histories", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getHistoryById = async (id: number): Promise<HistoryDetailsDto> => {
    const response = await api.get<HistoryDetailsDto>(`/histories/${id}`);
    return response.data;
};

export const createHistory = async (
    data: HistoryCreateDto
): Promise<HistoryDto> => {
    const response = await api.post<HistoryDto>("/histories", data);
    return response.data;
};

export const updateHistory = async (
    id: number,
    data: HistoryUpdateDto
): Promise<HistoryDto> => {
    const response = await api.put<HistoryDto>(`/histories/${id}`, data);
    return response.data;
};

export const deleteHistory = async (id: number): Promise<void> => {
    await api.delete(`/histories/${id}`);
};

/** Entity types that can carry a History link (mirrors the API enum). */
export type HistoryLinkTargetType =
    | "Character"
    | "Location"
    | "Faction"
    | "Nation"
    | "ClimateZone"
    | "ClimateDetail"
    | "Season"
    | "Creature"
    | "EconomicSystem"
    | "Currency"
    | "BankingSystem"
    | "TaxationSystem"
    | "TradeRoute"
    | "NaturalResource"
    | "ExtractionMethod"
    | "Industry"
    | "Guild"
    | "Corporation";

/** Attach a history to an entity (sets the entity's historyId). */
export const linkHistory = async (
    historyId: number,
    targetType: HistoryLinkTargetType,
    targetId: number
): Promise<void> => {
    await api.post(`/histories/${historyId}/links/${targetType}/${targetId}`);
};

/** Detach a history from an entity (clears the entity's historyId). */
export const unlinkHistory = async (
    historyId: number,
    targetType: HistoryLinkTargetType,
    targetId: number
): Promise<void> => {
    await api.delete(`/histories/${historyId}/links/${targetType}/${targetId}`);
};
