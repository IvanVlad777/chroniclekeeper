import api from "../services/api";
import {
    ReligiousEducationCreateDto,
    ReligiousEducationDto,
    ReligiousEducationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vjerska obrazovanja, opcionalno filtrirana po svijetu/liku/religiji. */
export const getReligiousEducations = async (params?: {
    worldId?: number;
    characterId?: number;
    religionId?: number;
}): Promise<ReligiousEducationDto[]> => {
    const response = await api.get<ReligiousEducationDto[]>("/religious-educations", { params });
    return response.data;
};

export const createReligiousEducation = async (
    data: ReligiousEducationCreateDto
): Promise<ReligiousEducationDto> => {
    const response = await api.post<ReligiousEducationDto>("/religious-educations", data);
    return response.data;
};

export const updateReligiousEducation = async (
    id: number,
    data: ReligiousEducationUpdateDto
): Promise<ReligiousEducationDto> => {
    const response = await api.put<ReligiousEducationDto>(`/religious-educations/${id}`, data);
    return response.data;
};

export const deleteReligiousEducation = async (id: number): Promise<void> => {
    await api.delete(`/religious-educations/${id}`);
};
