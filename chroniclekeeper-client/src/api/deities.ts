import api from "../services/api";
import {
    DeityCreateDto,
    DeityDetailsDto,
    DeityDto,
    DeityUpdateDto,
} from "../interfaces/loreInterfaces";

export const getDeities = async (worldId?: number): Promise<DeityDto[]> => {
    const response = await api.get<DeityDto[]>("/deities", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getDeityById = async (id: number): Promise<DeityDetailsDto> => {
    const response = await api.get<DeityDetailsDto>(`/deities/${id}`);
    return response.data;
};

export const createDeity = async (data: DeityCreateDto): Promise<DeityDto> => {
    const response = await api.post<DeityDto>("/deities", data);
    return response.data;
};

export const updateDeity = async (
    id: number,
    data: DeityUpdateDto
): Promise<DeityDto> => {
    const response = await api.put<DeityDto>(`/deities/${id}`, data);
    return response.data;
};

export const deleteDeity = async (id: number): Promise<void> => {
    await api.delete(`/deities/${id}`);
};

export const addDeityOrder = async (
    id: number,
    orderId: number
): Promise<void> => {
    await api.post(`/deities/${id}/orders/${orderId}`);
};
export const removeDeityOrder = async (
    id: number,
    orderId: number
): Promise<void> => {
    await api.delete(`/deities/${id}/orders/${orderId}`);
};

export const addDeityAlly = async (
    id: number,
    alliedDeityId: number
): Promise<void> => {
    await api.post(`/deities/${id}/allies/${alliedDeityId}`);
};
export const removeDeityAlly = async (
    id: number,
    alliedDeityId: number
): Promise<void> => {
    await api.delete(`/deities/${id}/allies/${alliedDeityId}`);
};

export const addDeityRival = async (
    id: number,
    rivalDeityId: number
): Promise<void> => {
    await api.post(`/deities/${id}/rivals/${rivalDeityId}`);
};
export const removeDeityRival = async (
    id: number,
    rivalDeityId: number
): Promise<void> => {
    await api.delete(`/deities/${id}/rivals/${rivalDeityId}`);
};
