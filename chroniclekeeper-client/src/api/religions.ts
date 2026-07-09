import api from "../services/api";
import {
    ReligionCreateDto,
    ReligionDetailsDto,
    ReligionDto,
    ReligionUpdateDto,
} from "../interfaces/loreInterfaces";

/** Religije, opcionalno filtrirane po svijetu. */
export const getReligions = async (
    worldId?: number
): Promise<ReligionDto[]> => {
    const response = await api.get<ReligionDto[]>("/religions", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj religije sa sljedbenicima. */
export const getReligionById = async (
    id: number
): Promise<ReligionDetailsDto> => {
    const response = await api.get<ReligionDetailsDto>(`/religions/${id}`);
    return response.data;
};

export const createReligion = async (
    data: ReligionCreateDto
): Promise<ReligionDto> => {
    const response = await api.post<ReligionDto>("/religions", data);
    return response.data;
};

export const updateReligion = async (
    id: number,
    data: ReligionUpdateDto
): Promise<ReligionDto> => {
    const response = await api.put<ReligionDto>(`/religions/${id}`, data);
    return response.data;
};

export const deleteReligion = async (id: number): Promise<void> => {
    await api.delete(`/religions/${id}`);
};
