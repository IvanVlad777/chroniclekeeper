import api from "../services/api";
import {
    LibraryCreateDto,
    LibraryDetailsDto,
    LibraryDto,
    LibraryUpdateDto,
} from "../interfaces/loreInterfaces";

/** Knjižnice, opcionalno filtrirane po svijetu. */
export const getLibraries = async (worldId?: number): Promise<LibraryDto[]> => {
    const response = await api.get<LibraryDto[]>("/libraries", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getLibraryById = async (id: number): Promise<LibraryDetailsDto> => {
    const response = await api.get<LibraryDetailsDto>(`/libraries/${id}`);
    return response.data;
};

export const createLibrary = async (
    data: LibraryCreateDto
): Promise<LibraryDto> => {
    const response = await api.post<LibraryDto>("/libraries", data);
    return response.data;
};

export const updateLibrary = async (
    id: number,
    data: LibraryUpdateDto
): Promise<LibraryDto> => {
    const response = await api.put<LibraryDto>(`/libraries/${id}`, data);
    return response.data;
};

export const deleteLibrary = async (id: number): Promise<void> => {
    await api.delete(`/libraries/${id}`);
};

export const addLibraryScholar = async (libraryId: number, characterId: number): Promise<void> => {
    await api.post(`/libraries/${libraryId}/scholars/${characterId}`);
};
export const removeLibraryScholar = async (libraryId: number, characterId: number): Promise<void> => {
    await api.delete(`/libraries/${libraryId}/scholars/${characterId}`);
};
