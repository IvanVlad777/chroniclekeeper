import api from "../services/api";
import {
    ClimateDetailCreateDto,
    ClimateDetailDto,
    ClimateDetailUpdateDto,
} from "../interfaces/loreInterfaces";

/** Klimatski detalji, opcionalno filtrirani po svijetu. */
export const getClimateDetails = async (
    worldId?: number
): Promise<ClimateDetailDto[]> => {
    const response = await api.get<ClimateDetailDto[]>("/climate-details", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getClimateDetailById = async (
    id: number
): Promise<ClimateDetailDto> => {
    const response = await api.get<ClimateDetailDto>(`/climate-details/${id}`);
    return response.data;
};

export const createClimateDetail = async (
    data: ClimateDetailCreateDto
): Promise<ClimateDetailDto> => {
    const response = await api.post<ClimateDetailDto>("/climate-details", data);
    return response.data;
};

export const updateClimateDetail = async (
    id: number,
    data: ClimateDetailUpdateDto
): Promise<ClimateDetailDto> => {
    const response = await api.put<ClimateDetailDto>(`/climate-details/${id}`, data);
    return response.data;
};

export const deleteClimateDetail = async (id: number): Promise<void> => {
    await api.delete(`/climate-details/${id}`);
};
