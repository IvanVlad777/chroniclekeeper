import api from "../services/api";
import {
    NationCreateDto,
    NationDetailsDto,
    NationDto,
    NationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Nacije, opcionalno filtrirane po svijetu. */
export const getNations = async (worldId?: number): Promise<NationDto[]> => {
    const response = await api.get<NationDto[]>("/nations", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj nacije s državljanima. */
export const getNationById = async (
    id: number
): Promise<NationDetailsDto> => {
    const response = await api.get<NationDetailsDto>(`/nations/${id}`);
    return response.data;
};

export const createNation = async (
    data: NationCreateDto
): Promise<NationDto> => {
    const response = await api.post<NationDto>("/nations", data);
    return response.data;
};

export const updateNation = async (
    id: number,
    data: NationUpdateDto
): Promise<NationDto> => {
    const response = await api.put<NationDto>(`/nations/${id}`, data);
    return response.data;
};

export const deleteNation = async (id: number): Promise<void> => {
    await api.delete(`/nations/${id}`);
};
