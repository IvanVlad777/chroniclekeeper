import api from "../services/api";
import {
    CulturalFestivalCreateDto,
    CulturalFestivalDetailsDto,
    CulturalFestivalDto,
    CulturalFestivalUpdateDto,
} from "../interfaces/loreInterfaces";

export const getCulturalFestivals = async (
    worldId?: number
): Promise<CulturalFestivalDto[]> => {
    const response = await api.get<CulturalFestivalDto[]>("/cultural-festivals", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCulturalFestivalById = async (
    id: number
): Promise<CulturalFestivalDetailsDto> => {
    const response = await api.get<CulturalFestivalDetailsDto>(
        `/cultural-festivals/${id}`
    );
    return response.data;
};

export const createCulturalFestival = async (
    data: CulturalFestivalCreateDto
): Promise<CulturalFestivalDto> => {
    const response = await api.post<CulturalFestivalDto>(
        "/cultural-festivals",
        data
    );
    return response.data;
};

export const updateCulturalFestival = async (
    id: number,
    data: CulturalFestivalUpdateDto
): Promise<CulturalFestivalDto> => {
    const response = await api.put<CulturalFestivalDto>(
        `/cultural-festivals/${id}`,
        data
    );
    return response.data;
};

export const deleteCulturalFestival = async (id: number): Promise<void> => {
    await api.delete(`/cultural-festivals/${id}`);
};
