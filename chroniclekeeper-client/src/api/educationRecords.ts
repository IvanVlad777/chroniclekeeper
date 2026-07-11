import api from "../services/api";
import {
    EducationRecordCreateDto,
    EducationRecordDto,
    EducationRecordUpdateDto,
} from "../interfaces/loreInterfaces";

/** Obrazovni zapisi, opcionalno filtrirani po svijetu/liku/školi/sveučilištu. */
export const getEducationRecords = async (params?: {
    worldId?: number;
    characterId?: number;
    schoolId?: number;
    universityId?: number;
}): Promise<EducationRecordDto[]> => {
    const response = await api.get<EducationRecordDto[]>("/education-records", { params });
    return response.data;
};

export const createEducationRecord = async (
    data: EducationRecordCreateDto
): Promise<EducationRecordDto> => {
    const response = await api.post<EducationRecordDto>("/education-records", data);
    return response.data;
};

export const updateEducationRecord = async (
    id: number,
    data: EducationRecordUpdateDto
): Promise<EducationRecordDto> => {
    const response = await api.put<EducationRecordDto>(`/education-records/${id}`, data);
    return response.data;
};

export const deleteEducationRecord = async (id: number): Promise<void> => {
    await api.delete(`/education-records/${id}`);
};
