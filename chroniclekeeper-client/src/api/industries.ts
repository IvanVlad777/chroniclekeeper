import api from "../services/api";
import {
    IndustryCreateDto,
    IndustryDetailsDto,
    IndustryDto,
    IndustryUpdateDto,
} from "../interfaces/loreInterfaces";

/** Industrije, opcionalno filtrirane po svijetu. */
export const getIndustries = async (worldId?: number): Promise<IndustryDto[]> => {
    const response = await api.get<IndustryDto[]>("/industries", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getIndustryById = async (id: number): Promise<IndustryDetailsDto> => {
    const response = await api.get<IndustryDetailsDto>(`/industries/${id}`);
    return response.data;
};

export const createIndustry = async (data: IndustryCreateDto): Promise<IndustryDto> => {
    const response = await api.post<IndustryDto>("/industries", data);
    return response.data;
};

export const updateIndustry = async (
    id: number,
    data: IndustryUpdateDto
): Promise<IndustryDto> => {
    const response = await api.put<IndustryDto>(`/industries/${id}`, data);
    return response.data;
};

export const deleteIndustry = async (id: number): Promise<void> => {
    await api.delete(`/industries/${id}`);
};
