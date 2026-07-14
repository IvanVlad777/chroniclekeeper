import api from "../services/api";
import {
    TradeRouteCreateDto,
    TradeRouteDetailsDto,
    TradeRouteDto,
    TradeRouteUpdateDto,
} from "../interfaces/loreInterfaces";

/** Trgovačke rute, opcionalno filtrirane po svijetu. */
export const getTradeRoutes = async (worldId?: number): Promise<TradeRouteDto[]> => {
    const response = await api.get<TradeRouteDto[]>("/trade-routes", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getTradeRouteById = async (
    id: number
): Promise<TradeRouteDetailsDto> => {
    const response = await api.get<TradeRouteDetailsDto>(`/trade-routes/${id}`);
    return response.data;
};

export const createTradeRoute = async (
    data: TradeRouteCreateDto
): Promise<TradeRouteDto> => {
    const response = await api.post<TradeRouteDto>("/trade-routes", data);
    return response.data;
};

export const updateTradeRoute = async (
    id: number,
    data: TradeRouteUpdateDto
): Promise<TradeRouteDto> => {
    const response = await api.put<TradeRouteDto>(`/trade-routes/${id}`, data);
    return response.data;
};

export const deleteTradeRoute = async (id: number): Promise<void> => {
    await api.delete(`/trade-routes/${id}`);
};

export const addTradeRouteLocation = async (
    tradeRouteId: number,
    locationId: number
): Promise<void> => {
    await api.post(`/trade-routes/${tradeRouteId}/locations/${locationId}`);
};

export const removeTradeRouteLocation = async (
    tradeRouteId: number,
    locationId: number
): Promise<void> => {
    await api.delete(`/trade-routes/${tradeRouteId}/locations/${locationId}`);
};

export const addTradeRouteResource = async (
    tradeRouteId: number,
    resourceId: number
): Promise<void> => {
    await api.post(`/trade-routes/${tradeRouteId}/resources/${resourceId}`);
};

export const removeTradeRouteResource = async (
    tradeRouteId: number,
    resourceId: number
): Promise<void> => {
    await api.delete(`/trade-routes/${tradeRouteId}/resources/${resourceId}`);
};
