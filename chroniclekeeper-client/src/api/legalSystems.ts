import api from "../services/api";
import {
    LegalSystemCreateDto,
    LegalSystemDetailsDto,
    LegalSystemDto,
    LegalSystemUpdateDto,
} from "../interfaces/loreInterfaces";

export const getLegalSystems = async (
    worldId?: number
): Promise<LegalSystemDto[]> => {
    const response = await api.get<LegalSystemDto[]>("/legalsystems", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getLegalSystemById = async (
    id: number
): Promise<LegalSystemDetailsDto> => {
    const response = await api.get<LegalSystemDetailsDto>(
        `/legalsystems/${id}`
    );
    return response.data;
};

export const createLegalSystem = async (
    data: LegalSystemCreateDto
): Promise<LegalSystemDto> => {
    const response = await api.post<LegalSystemDto>("/legalsystems", data);
    return response.data;
};

export const updateLegalSystem = async (
    id: number,
    data: LegalSystemUpdateDto
): Promise<LegalSystemDto> => {
    const response = await api.put<LegalSystemDto>(
        `/legalsystems/${id}`,
        data
    );
    return response.data;
};

export const deleteLegalSystem = async (id: number): Promise<void> => {
    await api.delete(`/legalsystems/${id}`);
};
