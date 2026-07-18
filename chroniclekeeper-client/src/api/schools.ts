import api from "../services/api";
import {
    SchoolCreateDto,
    SchoolDetailsDto,
    SchoolDto,
    SchoolSubjectCreateDto,
    SchoolSubjectDto,
    SchoolSubjectUpdateDto,
    SchoolUpdateDto,
} from "../interfaces/loreInterfaces";

/** Škole (uklj. strukovne, preko schoolType), opcionalno filtrirane po svijetu i/ili sustavu obrazovanja. */
export const getSchools = async (params?: {
    worldId?: number;
    educationSystemId?: number;
}): Promise<SchoolDto[]> => {
    const response = await api.get<SchoolDto[]>("/schools", { params });
    return response.data;
};

/** Detalj škole s predmetima i alumnima. */
export const getSchoolById = async (id: number): Promise<SchoolDetailsDto> => {
    const response = await api.get<SchoolDetailsDto>(`/schools/${id}`);
    return response.data;
};

export const createSchool = async (
    data: SchoolCreateDto
): Promise<SchoolDto> => {
    const response = await api.post<SchoolDto>("/schools", data);
    return response.data;
};

export const updateSchool = async (
    id: number,
    data: SchoolUpdateDto
): Promise<SchoolDto> => {
    const response = await api.put<SchoolDto>(`/schools/${id}`, data);
    return response.data;
};

export const deleteSchool = async (id: number): Promise<void> => {
    await api.delete(`/schools/${id}`);
};

// ----- School subjects (inline-managed na School detalju) -----

export const getSchoolSubjects = async (params?: {
    worldId?: number;
    schoolId?: number;
}): Promise<SchoolSubjectDto[]> => {
    const response = await api.get<SchoolSubjectDto[]>("/school-subjects", { params });
    return response.data;
};

export const createSchoolSubject = async (
    data: SchoolSubjectCreateDto
): Promise<SchoolSubjectDto> => {
    const response = await api.post<SchoolSubjectDto>("/school-subjects", data);
    return response.data;
};

export const updateSchoolSubject = async (
    id: number,
    data: SchoolSubjectUpdateDto
): Promise<SchoolSubjectDto> => {
    const response = await api.put<SchoolSubjectDto>(`/school-subjects/${id}`, data);
    return response.data;
};

export const deleteSchoolSubject = async (id: number): Promise<void> => {
    await api.delete(`/school-subjects/${id}`);
};

export const addSchoolStudent = async (schoolId: number, characterId: number): Promise<void> => {
    await api.post(`/schools/${schoolId}/students/${characterId}`);
};
export const removeSchoolStudent = async (schoolId: number, characterId: number): Promise<void> => {
    await api.delete(`/schools/${schoolId}/students/${characterId}`);
};
export const addSchoolTeacher = async (schoolId: number, characterId: number): Promise<void> => {
    await api.post(`/schools/${schoolId}/teachers/${characterId}`);
};
export const removeSchoolTeacher = async (schoolId: number, characterId: number): Promise<void> => {
    await api.delete(`/schools/${schoolId}/teachers/${characterId}`);
};
