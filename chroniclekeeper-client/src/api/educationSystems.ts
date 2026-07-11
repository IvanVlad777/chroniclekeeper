import api from "../services/api";
import {
    EducationSystemCreateDto,
    EducationSystemDetailsDto,
    EducationSystemDto,
    EducationSystemUpdateDto,
} from "../interfaces/loreInterfaces";

/** Sustavi obrazovanja, opcionalno filtrirani po svijetu. */
export const getEducationSystems = async (
    worldId?: number
): Promise<EducationSystemDto[]> => {
    const response = await api.get<EducationSystemDto[]>("/education-systems", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj sustava obrazovanja sa školama i sveučilištima. */
export const getEducationSystemById = async (
    id: number
): Promise<EducationSystemDetailsDto> => {
    const response = await api.get<EducationSystemDetailsDto>(`/education-systems/${id}`);
    return response.data;
};

export const createEducationSystem = async (
    data: EducationSystemCreateDto
): Promise<EducationSystemDto> => {
    const response = await api.post<EducationSystemDto>("/education-systems", data);
    return response.data;
};

export const updateEducationSystem = async (
    id: number,
    data: EducationSystemUpdateDto
): Promise<EducationSystemDto> => {
    const response = await api.put<EducationSystemDto>(`/education-systems/${id}`, data);
    return response.data;
};

export const deleteEducationSystem = async (id: number): Promise<void> => {
    await api.delete(`/education-systems/${id}`);
};
