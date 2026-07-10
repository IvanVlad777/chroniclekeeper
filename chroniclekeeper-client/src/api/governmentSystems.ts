import api from "../services/api";
import {
    GovernmentSystemCreateDto,
    GovernmentSystemDetailsDto,
    GovernmentSystemDto,
    GovernmentSystemUpdateDto,
} from "../interfaces/loreInterfaces";

export const getGovernmentSystems = async (
    worldId?: number
): Promise<GovernmentSystemDto[]> => {
    const response = await api.get<GovernmentSystemDto[]>(
        "/governmentsystems",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getGovernmentSystemById = async (
    id: number
): Promise<GovernmentSystemDetailsDto> => {
    const response = await api.get<GovernmentSystemDetailsDto>(
        `/governmentsystems/${id}`
    );
    return response.data;
};

export const createGovernmentSystem = async (
    data: GovernmentSystemCreateDto
): Promise<GovernmentSystemDto> => {
    const response = await api.post<GovernmentSystemDto>(
        "/governmentsystems",
        data
    );
    return response.data;
};

export const updateGovernmentSystem = async (
    id: number,
    data: GovernmentSystemUpdateDto
): Promise<GovernmentSystemDto> => {
    const response = await api.put<GovernmentSystemDto>(
        `/governmentsystems/${id}`,
        data
    );
    return response.data;
};

export const deleteGovernmentSystem = async (id: number): Promise<void> => {
    await api.delete(`/governmentsystems/${id}`);
};
