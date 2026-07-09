import api from "../services/api";
import {
    LanguageCreateDto,
    LanguageDetailsDto,
    LanguageDto,
    LanguageUpdateDto,
} from "../interfaces/loreInterfaces";

/** Jezici, opcionalno filtrirani po svijetu. */
export const getLanguages = async (
    worldId?: number
): Promise<LanguageDto[]> => {
    const response = await api.get<LanguageDto[]>("/languages", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj jezika s kulturama koje ga govore. */
export const getLanguageById = async (
    id: number
): Promise<LanguageDetailsDto> => {
    const response = await api.get<LanguageDetailsDto>(`/languages/${id}`);
    return response.data;
};

export const createLanguage = async (
    data: LanguageCreateDto
): Promise<LanguageDto> => {
    const response = await api.post<LanguageDto>("/languages", data);
    return response.data;
};

export const updateLanguage = async (
    id: number,
    data: LanguageUpdateDto
): Promise<LanguageDto> => {
    const response = await api.put<LanguageDto>(`/languages/${id}`, data);
    return response.data;
};

export const deleteLanguage = async (id: number): Promise<void> => {
    await api.delete(`/languages/${id}`);
};
