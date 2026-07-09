import api from "../services/api";
import {
    CultureCreateDto,
    CultureDetailsDto,
    CultureDto,
    CultureUpdateDto,
} from "../interfaces/loreInterfaces";

/** Kulture, opcionalno filtrirane po svijetu. */
export const getCultures = async (
    worldId?: number
): Promise<CultureDto[]> => {
    const response = await api.get<CultureDto[]>("/cultures", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj kulture s jezikom i religijom. */
export const getCultureById = async (
    id: number
): Promise<CultureDetailsDto> => {
    const response = await api.get<CultureDetailsDto>(`/cultures/${id}`);
    return response.data;
};

export const createCulture = async (
    data: CultureCreateDto
): Promise<CultureDto> => {
    const response = await api.post<CultureDto>("/cultures", data);
    return response.data;
};

export const updateCulture = async (
    id: number,
    data: CultureUpdateDto
): Promise<CultureDto> => {
    const response = await api.put<CultureDto>(`/cultures/${id}`, data);
    return response.data;
};

export const deleteCulture = async (id: number): Promise<void> => {
    await api.delete(`/cultures/${id}`);
};
