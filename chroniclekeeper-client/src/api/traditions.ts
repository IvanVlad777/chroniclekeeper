import api from "../services/api";
import {
    TraditionCreateDto,
    TraditionDetailsDto,
    TraditionDto,
    TraditionUpdateDto,
} from "../interfaces/loreInterfaces";

export const getTraditions = async (
    worldId?: number
): Promise<TraditionDto[]> => {
    const response = await api.get<TraditionDto[]>("/traditions", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getTraditionById = async (
    id: number
): Promise<TraditionDetailsDto> => {
    const response = await api.get<TraditionDetailsDto>(`/traditions/${id}`);
    return response.data;
};

export const createTradition = async (
    data: TraditionCreateDto
): Promise<TraditionDto> => {
    const response = await api.post<TraditionDto>("/traditions", data);
    return response.data;
};

export const updateTradition = async (
    id: number,
    data: TraditionUpdateDto
): Promise<TraditionDto> => {
    const response = await api.put<TraditionDto>(`/traditions/${id}`, data);
    return response.data;
};

export const deleteTradition = async (id: number): Promise<void> => {
    await api.delete(`/traditions/${id}`);
};
