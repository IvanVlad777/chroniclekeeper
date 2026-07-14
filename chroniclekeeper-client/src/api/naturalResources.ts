import api from "../services/api";
import {
    NaturalResourceCreateDto,
    NaturalResourceDetailsDto,
    NaturalResourceDto,
    NaturalResourceUpdateDto,
} from "../interfaces/loreInterfaces";

/** Prirodni resursi, opcionalno filtrirani po svijetu. */
export const getNaturalResources = async (
    worldId?: number
): Promise<NaturalResourceDto[]> => {
    const response = await api.get<NaturalResourceDto[]>("/natural-resources", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getNaturalResourceById = async (
    id: number
): Promise<NaturalResourceDetailsDto> => {
    const response = await api.get<NaturalResourceDetailsDto>(
        `/natural-resources/${id}`
    );
    return response.data;
};

export const createNaturalResource = async (
    data: NaturalResourceCreateDto
): Promise<NaturalResourceDto> => {
    const response = await api.post<NaturalResourceDto>("/natural-resources", data);
    return response.data;
};

export const updateNaturalResource = async (
    id: number,
    data: NaturalResourceUpdateDto
): Promise<NaturalResourceDto> => {
    const response = await api.put<NaturalResourceDto>(
        `/natural-resources/${id}`,
        data
    );
    return response.data;
};

export const deleteNaturalResource = async (id: number): Promise<void> => {
    await api.delete(`/natural-resources/${id}`);
};

export const addNaturalResourceLocation = async (
    naturalResourceId: number,
    locationId: number
): Promise<void> => {
    await api.post(`/natural-resources/${naturalResourceId}/locations/${locationId}`);
};

export const removeNaturalResourceLocation = async (
    naturalResourceId: number,
    locationId: number
): Promise<void> => {
    await api.delete(`/natural-resources/${naturalResourceId}/locations/${locationId}`);
};
