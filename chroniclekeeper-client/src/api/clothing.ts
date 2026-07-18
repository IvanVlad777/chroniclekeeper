import api from "../services/api";
import {
    ClothingCreateDto,
    ClothingDetailsDto,
    ClothingDto,
    ClothingUpdateDto,
} from "../interfaces/loreInterfaces";

export const getClothing = async (worldId?: number): Promise<ClothingDto[]> => {
    const response = await api.get<ClothingDto[]>("/clothing", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getClothingById = async (
    id: number
): Promise<ClothingDetailsDto> => {
    const response = await api.get<ClothingDetailsDto>(`/clothing/${id}`);
    return response.data;
};

export const createClothing = async (
    data: ClothingCreateDto
): Promise<ClothingDto> => {
    const response = await api.post<ClothingDto>("/clothing", data);
    return response.data;
};

export const updateClothing = async (
    id: number,
    data: ClothingUpdateDto
): Promise<ClothingDto> => {
    const response = await api.put<ClothingDto>(`/clothing/${id}`, data);
    return response.data;
};

export const deleteClothing = async (id: number): Promise<void> => {
    await api.delete(`/clothing/${id}`);
};
