import api from "../services/api";
import {
    LocationCreateDto,
    LocationDetailsDto,
    LocationDto,
    LocationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Lokacije, opcionalno filtrirane po svijetu. */
export const getLocations = async (
    worldId?: number
): Promise<LocationDto[]> => {
    const response = await api.get<LocationDto[]>("/locations", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getLocation = async (
    id: number
): Promise<LocationDetailsDto> => {
    const response = await api.get<LocationDetailsDto>(`/locations/${id}`);
    return response.data;
};

export const createLocation = async (
    data: LocationCreateDto
): Promise<LocationDto> => {
    const response = await api.post<LocationDto>("/locations", data);
    return response.data;
};

export const updateLocation = async (
    id: number,
    data: LocationUpdateDto
): Promise<LocationDto> => {
    const response = await api.put<LocationDto>(`/locations/${id}`, data);
    return response.data;
};

export const deleteLocation = async (id: number): Promise<void> => {
    await api.delete(`/locations/${id}`);
};

export const addRegionNativeSpecies = async (
    regionId: number,
    speciesId: number
): Promise<void> => {
    await api.post(`/locations/${regionId}/native-species/${speciesId}`);
};

export const removeRegionNativeSpecies = async (
    regionId: number,
    speciesId: number
): Promise<void> => {
    await api.delete(`/locations/${regionId}/native-species/${speciesId}`);
};
