import api from "../services/api";
import {
    ApprenticeshipCreateDto,
    ApprenticeshipDto,
    ApprenticeshipUpdateDto,
    JobRankCreateDto,
    JobRankDto,
    JobRankUpdateDto,
    ProfessionCreateDto,
    ProfessionDetailsDto,
    ProfessionDto,
    ProfessionUpdateDto,
    SpecialisationCreateDto,
    SpecialisationDto,
    SpecialisationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Zanimanja, opcionalno filtrirana po svijetu. */
export const getProfessions = async (
    worldId?: number
): Promise<ProfessionDto[]> => {
    const response = await api.get<ProfessionDto[]>("/professions", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj zanimanja s rangovima, naukovanjima, specijalizacijama i cross-linkovima. */
export const getProfessionById = async (
    id: number
): Promise<ProfessionDetailsDto> => {
    const response = await api.get<ProfessionDetailsDto>(`/professions/${id}`);
    return response.data;
};

export const createProfession = async (
    data: ProfessionCreateDto
): Promise<ProfessionDto> => {
    const response = await api.post<ProfessionDto>("/professions", data);
    return response.data;
};

export const updateProfession = async (
    id: number,
    data: ProfessionUpdateDto
): Promise<ProfessionDto> => {
    const response = await api.put<ProfessionDto>(`/professions/${id}`, data);
    return response.data;
};

export const deleteProfession = async (id: number): Promise<void> => {
    await api.delete(`/professions/${id}`);
};

export const addProfessionSpecies = async (
    professionId: number,
    speciesId: number
): Promise<void> => {
    await api.post(`/professions/${professionId}/species/${speciesId}`);
};

export const removeProfessionSpecies = async (
    professionId: number,
    speciesId: number
): Promise<void> => {
    await api.delete(`/professions/${professionId}/species/${speciesId}`);
};

export const addProfessionSocialClass = async (
    professionId: number,
    socialClassId: number
): Promise<void> => {
    await api.post(`/professions/${professionId}/social-classes/${socialClassId}`);
};

export const removeProfessionSocialClass = async (
    professionId: number,
    socialClassId: number
): Promise<void> => {
    await api.delete(`/professions/${professionId}/social-classes/${socialClassId}`);
};

export const addProfessionTradeSchool = async (
    professionId: number,
    tradeSchoolId: number
): Promise<void> => {
    await api.post(`/professions/${professionId}/trade-schools/${tradeSchoolId}`);
};

export const removeProfessionTradeSchool = async (
    professionId: number,
    tradeSchoolId: number
): Promise<void> => {
    await api.delete(`/professions/${professionId}/trade-schools/${tradeSchoolId}`);
};

// ----- Job ranks (inline-managed na Profession detalju) -----

export const getJobRanks = async (params?: {
    worldId?: number;
    professionId?: number;
}): Promise<JobRankDto[]> => {
    const response = await api.get<JobRankDto[]>("/job-ranks", { params });
    return response.data;
};

export const createJobRank = async (
    data: JobRankCreateDto
): Promise<JobRankDto> => {
    const response = await api.post<JobRankDto>("/job-ranks", data);
    return response.data;
};

export const updateJobRank = async (
    id: number,
    data: JobRankUpdateDto
): Promise<JobRankDto> => {
    const response = await api.put<JobRankDto>(`/job-ranks/${id}`, data);
    return response.data;
};

export const deleteJobRank = async (id: number): Promise<void> => {
    await api.delete(`/job-ranks/${id}`);
};

// ----- Apprenticeships (inline-managed na Profession detalju) -----

export const getApprenticeships = async (params?: {
    worldId?: number;
    professionId?: number;
}): Promise<ApprenticeshipDto[]> => {
    const response = await api.get<ApprenticeshipDto[]>("/apprenticeships", { params });
    return response.data;
};

export const createApprenticeship = async (
    data: ApprenticeshipCreateDto
): Promise<ApprenticeshipDto> => {
    const response = await api.post<ApprenticeshipDto>("/apprenticeships", data);
    return response.data;
};

export const updateApprenticeship = async (
    id: number,
    data: ApprenticeshipUpdateDto
): Promise<ApprenticeshipDto> => {
    const response = await api.put<ApprenticeshipDto>(`/apprenticeships/${id}`, data);
    return response.data;
};

export const deleteApprenticeship = async (id: number): Promise<void> => {
    await api.delete(`/apprenticeships/${id}`);
};

// ----- Specialisations (inline-managed na Profession detalju) -----

export const getSpecialisations = async (params?: {
    worldId?: number;
    professionId?: number;
}): Promise<SpecialisationDto[]> => {
    const response = await api.get<SpecialisationDto[]>("/specialisations", { params });
    return response.data;
};

export const createSpecialisation = async (
    data: SpecialisationCreateDto
): Promise<SpecialisationDto> => {
    const response = await api.post<SpecialisationDto>("/specialisations", data);
    return response.data;
};

export const updateSpecialisation = async (
    id: number,
    data: SpecialisationUpdateDto
): Promise<SpecialisationDto> => {
    const response = await api.put<SpecialisationDto>(`/specialisations/${id}`, data);
    return response.data;
};

export const deleteSpecialisation = async (id: number): Promise<void> => {
    await api.delete(`/specialisations/${id}`);
};
