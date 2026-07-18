import api from "../services/api";
import {
    CustomCreateDto,
    CustomDetailsDto,
    CustomDto,
    CustomUpdateDto,
} from "../interfaces/loreInterfaces";

export const getCustoms = async (worldId?: number): Promise<CustomDto[]> => {
    const response = await api.get<CustomDto[]>("/customs", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCustomById = async (id: number): Promise<CustomDetailsDto> => {
    const response = await api.get<CustomDetailsDto>(`/customs/${id}`);
    return response.data;
};

export const createCustom = async (
    data: CustomCreateDto
): Promise<CustomDto> => {
    const response = await api.post<CustomDto>("/customs", data);
    return response.data;
};

export const updateCustom = async (
    id: number,
    data: CustomUpdateDto
): Promise<CustomDto> => {
    const response = await api.put<CustomDto>(`/customs/${id}`, data);
    return response.data;
};

export const deleteCustom = async (id: number): Promise<void> => {
    await api.delete(`/customs/${id}`);
};
