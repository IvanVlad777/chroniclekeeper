import api from "../services/api";
import {
    FolkloreCreateDto,
    FolkloreDetailsDto,
    FolkloreDto,
    FolkloreUpdateDto,
} from "../interfaces/loreInterfaces";

export const getFolklore = async (worldId?: number): Promise<FolkloreDto[]> => {
    const response = await api.get<FolkloreDto[]>("/folklore", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getFolkloreById = async (
    id: number
): Promise<FolkloreDetailsDto> => {
    const response = await api.get<FolkloreDetailsDto>(`/folklore/${id}`);
    return response.data;
};

export const createFolklore = async (
    data: FolkloreCreateDto
): Promise<FolkloreDto> => {
    const response = await api.post<FolkloreDto>("/folklore", data);
    return response.data;
};

export const updateFolklore = async (
    id: number,
    data: FolkloreUpdateDto
): Promise<FolkloreDto> => {
    const response = await api.put<FolkloreDto>(`/folklore/${id}`, data);
    return response.data;
};

export const deleteFolklore = async (id: number): Promise<void> => {
    await api.delete(`/folklore/${id}`);
};

export const addFolkloreEvent = async (
    id: number,
    eventId: number
): Promise<void> => {
    await api.post(`/folklore/${id}/events/${eventId}`);
};

export const removeFolkloreEvent = async (
    id: number,
    eventId: number
): Promise<void> => {
    await api.delete(`/folklore/${id}/events/${eventId}`);
};

export const addFolkloreSpecies = async (
    id: number,
    speciesId: number
): Promise<void> => {
    await api.post(`/folklore/${id}/species/${speciesId}`);
};

export const removeFolkloreSpecies = async (
    id: number,
    speciesId: number
): Promise<void> => {
    await api.delete(`/folklore/${id}/species/${speciesId}`);
};
