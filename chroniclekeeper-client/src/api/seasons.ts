import api from "../services/api";
import { SeasonCreateDto, SeasonDto, SeasonUpdateDto } from "../interfaces/loreInterfaces";

/** Godišnja doba, opcionalno filtrirana po svijetu. */
export const getSeasons = async (worldId?: number): Promise<SeasonDto[]> => {
    const response = await api.get<SeasonDto[]>("/seasons", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getSeasonById = async (id: number): Promise<SeasonDto> => {
    const response = await api.get<SeasonDto>(`/seasons/${id}`);
    return response.data;
};

export const createSeason = async (data: SeasonCreateDto): Promise<SeasonDto> => {
    const response = await api.post<SeasonDto>("/seasons", data);
    return response.data;
};

export const updateSeason = async (
    id: number,
    data: SeasonUpdateDto
): Promise<SeasonDto> => {
    const response = await api.put<SeasonDto>(`/seasons/${id}`, data);
    return response.data;
};

export const deleteSeason = async (id: number): Promise<void> => {
    await api.delete(`/seasons/${id}`);
};
