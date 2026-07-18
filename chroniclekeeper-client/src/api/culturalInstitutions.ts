import api from "../services/api";
import {
    CulturalInstitutionCreateDto,
    CulturalInstitutionDetailsDto,
    CulturalInstitutionDto,
    CulturalInstitutionUpdateDto,
} from "../interfaces/loreInterfaces";

export const getCulturalInstitutions = async (
    worldId?: number
): Promise<CulturalInstitutionDto[]> => {
    const response = await api.get<CulturalInstitutionDto[]>(
        "/cultural-institutions",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getCulturalInstitutionById = async (
    id: number
): Promise<CulturalInstitutionDetailsDto> => {
    const response = await api.get<CulturalInstitutionDetailsDto>(
        `/cultural-institutions/${id}`
    );
    return response.data;
};

export const createCulturalInstitution = async (
    data: CulturalInstitutionCreateDto
): Promise<CulturalInstitutionDto> => {
    const response = await api.post<CulturalInstitutionDto>(
        "/cultural-institutions",
        data
    );
    return response.data;
};

export const updateCulturalInstitution = async (
    id: number,
    data: CulturalInstitutionUpdateDto
): Promise<CulturalInstitutionDto> => {
    const response = await api.put<CulturalInstitutionDto>(
        `/cultural-institutions/${id}`,
        data
    );
    return response.data;
};

export const deleteCulturalInstitution = async (id: number): Promise<void> => {
    await api.delete(`/cultural-institutions/${id}`);
};

export const addCulturalInstitutionArtist = async (institutionId: number, characterId: number): Promise<void> => {
    await api.post(`/cultural-institutions/${institutionId}/artists/${characterId}`);
};
export const removeCulturalInstitutionArtist = async (institutionId: number, characterId: number): Promise<void> => {
    await api.delete(`/cultural-institutions/${institutionId}/artists/${characterId}`);
};
