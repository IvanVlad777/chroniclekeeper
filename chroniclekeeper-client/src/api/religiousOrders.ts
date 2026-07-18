import api from "../services/api";
import {
    ReligiousOrderCreateDto,
    ReligiousOrderDetailsDto,
    ReligiousOrderDto,
    ReligiousOrderUpdateDto,
} from "../interfaces/loreInterfaces";

export const getReligiousOrders = async (
    worldId?: number
): Promise<ReligiousOrderDto[]> => {
    const response = await api.get<ReligiousOrderDto[]>("/religious-orders", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getReligiousOrderById = async (
    id: number
): Promise<ReligiousOrderDetailsDto> => {
    const response = await api.get<ReligiousOrderDetailsDto>(
        `/religious-orders/${id}`
    );
    return response.data;
};

export const createReligiousOrder = async (
    data: ReligiousOrderCreateDto
): Promise<ReligiousOrderDto> => {
    const response = await api.post<ReligiousOrderDto>(
        "/religious-orders",
        data
    );
    return response.data;
};

export const updateReligiousOrder = async (
    id: number,
    data: ReligiousOrderUpdateDto
): Promise<ReligiousOrderDto> => {
    const response = await api.put<ReligiousOrderDto>(
        `/religious-orders/${id}`,
        data
    );
    return response.data;
};

export const deleteReligiousOrder = async (id: number): Promise<void> => {
    await api.delete(`/religious-orders/${id}`);
};

export const addReligiousOrderFaction = async (
    id: number,
    factionId: number
): Promise<void> => {
    await api.post(`/religious-orders/${id}/factions/${factionId}`);
};

export const removeReligiousOrderFaction = async (
    id: number,
    factionId: number
): Promise<void> => {
    await api.delete(`/religious-orders/${id}/factions/${factionId}`);
};
