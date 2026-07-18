import api from "../services/api";
import {
    CuisineCreateDto,
    CuisineDetailsDto,
    CuisineDto,
    CuisineUpdateDto,
} from "../interfaces/loreInterfaces";

export const getCuisines = async (worldId?: number): Promise<CuisineDto[]> => {
    const response = await api.get<CuisineDto[]>("/cuisines", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCuisineById = async (
    id: number
): Promise<CuisineDetailsDto> => {
    const response = await api.get<CuisineDetailsDto>(`/cuisines/${id}`);
    return response.data;
};

export const createCuisine = async (
    data: CuisineCreateDto
): Promise<CuisineDto> => {
    const response = await api.post<CuisineDto>("/cuisines", data);
    return response.data;
};

export const updateCuisine = async (
    id: number,
    data: CuisineUpdateDto
): Promise<CuisineDto> => {
    const response = await api.put<CuisineDto>(`/cuisines/${id}`, data);
    return response.data;
};

export const deleteCuisine = async (id: number): Promise<void> => {
    await api.delete(`/cuisines/${id}`);
};
