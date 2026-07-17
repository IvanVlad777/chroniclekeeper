import api from "../services/api";
import {
    MilitaryRankCreateDto,
    MilitaryRankDto,
    MilitaryRankUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojni činovi, filtrirani po svijetu i/ili postrojbi. */
export const getMilitaryRanks = async (params?: {
    worldId?: number;
    unitId?: number;
}): Promise<MilitaryRankDto[]> => {
    const response = await api.get<MilitaryRankDto[]>("/military-ranks", {
        params,
    });
    return response.data;
};

export const getMilitaryRankById = async (
    id: number
): Promise<MilitaryRankDto> => {
    const response = await api.get<MilitaryRankDto>(`/military-ranks/${id}`);
    return response.data;
};

export const createMilitaryRank = async (
    data: MilitaryRankCreateDto
): Promise<MilitaryRankDto> => {
    const response = await api.post<MilitaryRankDto>("/military-ranks", data);
    return response.data;
};

export const updateMilitaryRank = async (
    id: number,
    data: MilitaryRankUpdateDto
): Promise<MilitaryRankDto> => {
    const response = await api.put<MilitaryRankDto>(
        `/military-ranks/${id}`,
        data
    );
    return response.data;
};

export const deleteMilitaryRank = async (id: number): Promise<void> => {
    await api.delete(`/military-ranks/${id}`);
};
