import api from "../services/api";
import {
    MythCreateDto,
    MythDetailsDto,
    MythDto,
    MythUpdateDto,
} from "../interfaces/loreInterfaces";

export const getMyths = async (worldId?: number): Promise<MythDto[]> => {
    const response = await api.get<MythDto[]>("/myths", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getMythById = async (id: number): Promise<MythDetailsDto> => {
    const response = await api.get<MythDetailsDto>(`/myths/${id}`);
    return response.data;
};

export const createMyth = async (data: MythCreateDto): Promise<MythDto> => {
    const response = await api.post<MythDto>("/myths", data);
    return response.data;
};

export const updateMyth = async (
    id: number,
    data: MythUpdateDto
): Promise<MythDto> => {
    const response = await api.put<MythDto>(`/myths/${id}`, data);
    return response.data;
};

export const deleteMyth = async (id: number): Promise<void> => {
    await api.delete(`/myths/${id}`);
};
