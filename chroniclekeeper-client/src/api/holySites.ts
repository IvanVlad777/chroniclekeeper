import api from "../services/api";
import {
    HolySiteCreateDto,
    HolySiteDetailsDto,
    HolySiteDto,
    HolySiteUpdateDto,
} from "../interfaces/loreInterfaces";

export const getHolySites = async (
    worldId?: number
): Promise<HolySiteDto[]> => {
    const response = await api.get<HolySiteDto[]>("/holy-sites", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getHolySiteById = async (
    id: number
): Promise<HolySiteDetailsDto> => {
    const response = await api.get<HolySiteDetailsDto>(`/holy-sites/${id}`);
    return response.data;
};

export const createHolySite = async (
    data: HolySiteCreateDto
): Promise<HolySiteDto> => {
    const response = await api.post<HolySiteDto>("/holy-sites", data);
    return response.data;
};

export const updateHolySite = async (
    id: number,
    data: HolySiteUpdateDto
): Promise<HolySiteDto> => {
    const response = await api.put<HolySiteDto>(`/holy-sites/${id}`, data);
    return response.data;
};

export const deleteHolySite = async (id: number): Promise<void> => {
    await api.delete(`/holy-sites/${id}`);
};
