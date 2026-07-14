import api from "../services/api";
import {
    CurrencyCreateDto,
    CurrencyDetailsDto,
    CurrencyDto,
    CurrencyUpdateDto,
} from "../interfaces/loreInterfaces";

/** Valute, opcionalno filtrirane po svijetu. */
export const getCurrencies = async (worldId?: number): Promise<CurrencyDto[]> => {
    const response = await api.get<CurrencyDto[]>("/currencies", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCurrencyById = async (id: number): Promise<CurrencyDetailsDto> => {
    const response = await api.get<CurrencyDetailsDto>(`/currencies/${id}`);
    return response.data;
};

export const createCurrency = async (data: CurrencyCreateDto): Promise<CurrencyDto> => {
    const response = await api.post<CurrencyDto>("/currencies", data);
    return response.data;
};

export const updateCurrency = async (
    id: number,
    data: CurrencyUpdateDto
): Promise<CurrencyDto> => {
    const response = await api.put<CurrencyDto>(`/currencies/${id}`, data);
    return response.data;
};

export const deleteCurrency = async (id: number): Promise<void> => {
    await api.delete(`/currencies/${id}`);
};
