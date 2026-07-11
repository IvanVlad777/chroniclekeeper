import api from "../services/api";
import {
    ItemCreateDto,
    ItemDetailsDto,
    ItemDto,
    ItemUpdateDto,
    OwnershipHistoryCreateDto,
    OwnershipHistoryDto,
    OwnershipHistoryUpdateDto,
} from "../interfaces/loreInterfaces";

/** Predmeti, opcionalno filtrirani po svijetu / trenutnom vlasniku / frakciji. */
export const getItems = async (params?: {
    worldId?: number;
    currentOwnerId?: number;
    factionId?: number;
}): Promise<ItemDto[]> => {
    const response = await api.get<ItemDto[]>("/items", { params });
    return response.data;
};

export const getItemById = async (id: number): Promise<ItemDetailsDto> => {
    const response = await api.get<ItemDetailsDto>(`/items/${id}`);
    return response.data;
};

export const createItem = async (
    data: ItemCreateDto
): Promise<ItemDto> => {
    const response = await api.post<ItemDto>("/items", data);
    return response.data;
};

export const updateItem = async (
    id: number,
    data: ItemUpdateDto
): Promise<ItemDto> => {
    const response = await api.put<ItemDto>(`/items/${id}`, data);
    return response.data;
};

export const deleteItem = async (id: number): Promise<void> => {
    await api.delete(`/items/${id}`);
};

// ----- Ownership history (inline-managed na Item detalju) -----

export const getOwnershipHistories = async (params?: {
    worldId?: number;
    itemId?: number;
}): Promise<OwnershipHistoryDto[]> => {
    const response = await api.get<OwnershipHistoryDto[]>(
        "/ownership-history",
        { params }
    );
    return response.data;
};

export const createOwnershipHistory = async (
    data: OwnershipHistoryCreateDto
): Promise<OwnershipHistoryDto> => {
    const response = await api.post<OwnershipHistoryDto>(
        "/ownership-history",
        data
    );
    return response.data;
};

export const updateOwnershipHistory = async (
    id: number,
    data: OwnershipHistoryUpdateDto
): Promise<OwnershipHistoryDto> => {
    const response = await api.put<OwnershipHistoryDto>(
        `/ownership-history/${id}`,
        data
    );
    return response.data;
};

export const deleteOwnershipHistory = async (id: number): Promise<void> => {
    await api.delete(`/ownership-history/${id}`);
};
