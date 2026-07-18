import api from "../services/api";
import {
    ArchitectureStyleCreateDto,
    ArchitectureStyleDetailsDto,
    ArchitectureStyleDto,
    ArchitectureStyleUpdateDto,
} from "../interfaces/loreInterfaces";

export const getArchitectureStyles = async (
    worldId?: number
): Promise<ArchitectureStyleDto[]> => {
    const response = await api.get<ArchitectureStyleDto[]>(
        "/architecture-styles",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getArchitectureStyleById = async (
    id: number
): Promise<ArchitectureStyleDetailsDto> => {
    const response = await api.get<ArchitectureStyleDetailsDto>(
        `/architecture-styles/${id}`
    );
    return response.data;
};

export const createArchitectureStyle = async (
    data: ArchitectureStyleCreateDto
): Promise<ArchitectureStyleDto> => {
    const response = await api.post<ArchitectureStyleDto>(
        "/architecture-styles",
        data
    );
    return response.data;
};

export const updateArchitectureStyle = async (
    id: number,
    data: ArchitectureStyleUpdateDto
): Promise<ArchitectureStyleDto> => {
    const response = await api.put<ArchitectureStyleDto>(
        `/architecture-styles/${id}`,
        data
    );
    return response.data;
};

export const deleteArchitectureStyle = async (id: number): Promise<void> => {
    await api.delete(`/architecture-styles/${id}`);
};

export const addArchitectureStyleLocation = async (
    id: number,
    locationId: number
): Promise<void> => {
    await api.post(`/architecture-styles/${id}/locations/${locationId}`);
};

export const removeArchitectureStyleLocation = async (
    id: number,
    locationId: number
): Promise<void> => {
    await api.delete(`/architecture-styles/${id}/locations/${locationId}`);
};
