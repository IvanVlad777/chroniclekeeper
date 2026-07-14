import api from "../services/api";
import {
    EconomicSystemCreateDto,
    EconomicSystemDetailsDto,
    EconomicSystemDto,
    EconomicSystemUpdateDto,
} from "../interfaces/loreInterfaces";

/** Ekonomski sustavi, opcionalno filtrirani po svijetu. */
export const getEconomicSystems = async (
    worldId?: number
): Promise<EconomicSystemDto[]> => {
    const response = await api.get<EconomicSystemDto[]>("/economic-systems", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getEconomicSystemById = async (
    id: number
): Promise<EconomicSystemDetailsDto> => {
    const response = await api.get<EconomicSystemDetailsDto>(`/economic-systems/${id}`);
    return response.data;
};

export const createEconomicSystem = async (
    data: EconomicSystemCreateDto
): Promise<EconomicSystemDto> => {
    const response = await api.post<EconomicSystemDto>("/economic-systems", data);
    return response.data;
};

export const updateEconomicSystem = async (
    id: number,
    data: EconomicSystemUpdateDto
): Promise<EconomicSystemDto> => {
    const response = await api.put<EconomicSystemDto>(`/economic-systems/${id}`, data);
    return response.data;
};

export const deleteEconomicSystem = async (id: number): Promise<void> => {
    await api.delete(`/economic-systems/${id}`);
};
