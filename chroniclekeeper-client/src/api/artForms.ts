import api from "../services/api";
import {
    ArtFormCreateDto,
    ArtFormDetailsDto,
    ArtFormDto,
    ArtFormUpdateDto,
} from "../interfaces/loreInterfaces";

export const getArtForms = async (worldId?: number): Promise<ArtFormDto[]> => {
    const response = await api.get<ArtFormDto[]>("/art-forms", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getArtFormById = async (
    id: number
): Promise<ArtFormDetailsDto> => {
    const response = await api.get<ArtFormDetailsDto>(`/art-forms/${id}`);
    return response.data;
};

export const createArtForm = async (
    data: ArtFormCreateDto
): Promise<ArtFormDto> => {
    const response = await api.post<ArtFormDto>("/art-forms", data);
    return response.data;
};

export const updateArtForm = async (
    id: number,
    data: ArtFormUpdateDto
): Promise<ArtFormDto> => {
    const response = await api.put<ArtFormDto>(`/art-forms/${id}`, data);
    return response.data;
};

export const deleteArtForm = async (id: number): Promise<void> => {
    await api.delete(`/art-forms/${id}`);
};
