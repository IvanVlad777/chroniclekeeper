import api from "../services/api";
import {
    TradeSchoolCreateDto,
    TradeSchoolDetailsDto,
    TradeSchoolDto,
    TradeSchoolUpdateDto,
} from "../interfaces/loreInterfaces";

/** Strukovne škole, opcionalno filtrirane po svijetu i/ili sustavu obrazovanja. */
export const getTradeSchools = async (params?: {
    worldId?: number;
    educationSystemId?: number;
}): Promise<TradeSchoolDto[]> => {
    const response = await api.get<TradeSchoolDto[]>("/trade-schools", { params });
    return response.data;
};

/** Detalj strukovne škole s predmetima, alumnima, zanimanjima i naukovanjima. */
export const getTradeSchoolById = async (
    id: number
): Promise<TradeSchoolDetailsDto> => {
    const response = await api.get<TradeSchoolDetailsDto>(`/trade-schools/${id}`);
    return response.data;
};

export const createTradeSchool = async (
    data: TradeSchoolCreateDto
): Promise<TradeSchoolDto> => {
    const response = await api.post<TradeSchoolDto>("/trade-schools", data);
    return response.data;
};

export const updateTradeSchool = async (
    id: number,
    data: TradeSchoolUpdateDto
): Promise<TradeSchoolDto> => {
    const response = await api.put<TradeSchoolDto>(`/trade-schools/${id}`, data);
    return response.data;
};

export const deleteTradeSchool = async (id: number): Promise<void> => {
    await api.delete(`/trade-schools/${id}`);
};
