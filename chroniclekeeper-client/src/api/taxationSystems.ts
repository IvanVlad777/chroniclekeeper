import api from "../services/api";
import {
    TaxationSystemCreateDto,
    TaxationSystemDetailsDto,
    TaxationSystemDto,
    TaxationSystemUpdateDto,
} from "../interfaces/loreInterfaces";

/** Porezni sustavi, opcionalno filtrirani po svijetu. */
export const getTaxationSystems = async (
    worldId?: number
): Promise<TaxationSystemDto[]> => {
    const response = await api.get<TaxationSystemDto[]>("/taxation-systems", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getTaxationSystemById = async (
    id: number
): Promise<TaxationSystemDetailsDto> => {
    const response = await api.get<TaxationSystemDetailsDto>(`/taxation-systems/${id}`);
    return response.data;
};

export const createTaxationSystem = async (
    data: TaxationSystemCreateDto
): Promise<TaxationSystemDto> => {
    const response = await api.post<TaxationSystemDto>("/taxation-systems", data);
    return response.data;
};

export const updateTaxationSystem = async (
    id: number,
    data: TaxationSystemUpdateDto
): Promise<TaxationSystemDto> => {
    const response = await api.put<TaxationSystemDto>(`/taxation-systems/${id}`, data);
    return response.data;
};

export const deleteTaxationSystem = async (id: number): Promise<void> => {
    await api.delete(`/taxation-systems/${id}`);
};
