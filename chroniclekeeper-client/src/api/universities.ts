import api from "../services/api";
import {
    UniversityCreateDto,
    UniversityDetailsDto,
    UniversityDto,
    UniversityMajorCreateDto,
    UniversityMajorDto,
    UniversityMajorUpdateDto,
    UniversityUpdateDto,
} from "../interfaces/loreInterfaces";

/** Sveučilišta, opcionalno filtrirana po svijetu i/ili sustavu obrazovanja. */
export const getUniversities = async (params?: {
    worldId?: number;
    educationSystemId?: number;
}): Promise<UniversityDto[]> => {
    const response = await api.get<UniversityDto[]>("/universities", { params });
    return response.data;
};

/** Detalj sveučilišta sa smjerovima i alumnima. */
export const getUniversityById = async (
    id: number
): Promise<UniversityDetailsDto> => {
    const response = await api.get<UniversityDetailsDto>(`/universities/${id}`);
    return response.data;
};

export const createUniversity = async (
    data: UniversityCreateDto
): Promise<UniversityDto> => {
    const response = await api.post<UniversityDto>("/universities", data);
    return response.data;
};

export const updateUniversity = async (
    id: number,
    data: UniversityUpdateDto
): Promise<UniversityDto> => {
    const response = await api.put<UniversityDto>(`/universities/${id}`, data);
    return response.data;
};

export const deleteUniversity = async (id: number): Promise<void> => {
    await api.delete(`/universities/${id}`);
};

// ----- University majors (inline-managed na University detalju) -----

export const getUniversityMajors = async (params?: {
    worldId?: number;
    universityId?: number;
}): Promise<UniversityMajorDto[]> => {
    const response = await api.get<UniversityMajorDto[]>("/university-majors", { params });
    return response.data;
};

export const createUniversityMajor = async (
    data: UniversityMajorCreateDto
): Promise<UniversityMajorDto> => {
    const response = await api.post<UniversityMajorDto>("/university-majors", data);
    return response.data;
};

export const updateUniversityMajor = async (
    id: number,
    data: UniversityMajorUpdateDto
): Promise<UniversityMajorDto> => {
    const response = await api.put<UniversityMajorDto>(`/university-majors/${id}`, data);
    return response.data;
};

export const deleteUniversityMajor = async (id: number): Promise<void> => {
    await api.delete(`/university-majors/${id}`);
};
