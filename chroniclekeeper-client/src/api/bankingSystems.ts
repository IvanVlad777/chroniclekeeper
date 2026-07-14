import api from "../services/api";
import {
    BankingSystemCreateDto,
    BankingSystemDetailsDto,
    BankingSystemDto,
    BankingSystemUpdateDto,
} from "../interfaces/loreInterfaces";

/** Bankarski sustavi, opcionalno filtrirani po svijetu. */
export const getBankingSystems = async (
    worldId?: number
): Promise<BankingSystemDto[]> => {
    const response = await api.get<BankingSystemDto[]>("/banking-systems", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getBankingSystemById = async (
    id: number
): Promise<BankingSystemDetailsDto> => {
    const response = await api.get<BankingSystemDetailsDto>(`/banking-systems/${id}`);
    return response.data;
};

export const createBankingSystem = async (
    data: BankingSystemCreateDto
): Promise<BankingSystemDto> => {
    const response = await api.post<BankingSystemDto>("/banking-systems", data);
    return response.data;
};

export const updateBankingSystem = async (
    id: number,
    data: BankingSystemUpdateDto
): Promise<BankingSystemDto> => {
    const response = await api.put<BankingSystemDto>(`/banking-systems/${id}`, data);
    return response.data;
};

export const deleteBankingSystem = async (id: number): Promise<void> => {
    await api.delete(`/banking-systems/${id}`);
};
