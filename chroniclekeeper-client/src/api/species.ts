import api from "../services/api";
import {
    RaceCreateDto,
    RaceDto,
    RaceUpdateDto,
    SpeciesCreateDto,
    SpeciesDetailsDto,
    SpeciesDto,
    SpeciesUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vrste, opcionalno filtrirane po svijetu. */
export const getSpecies = async (worldId?: number): Promise<SpeciesDto[]> => {
    const response = await api.get<SpeciesDto[]>("/species", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj vrste s rasama. */
export const getSpeciesById = async (
    id: number
): Promise<SpeciesDetailsDto> => {
    const response = await api.get<SpeciesDetailsDto>(`/species/${id}`);
    return response.data;
};

export const createSpecies = async (
    data: SpeciesCreateDto
): Promise<SpeciesDto> => {
    const response = await api.post<SpeciesDto>("/species", data);
    return response.data;
};

export const updateSpecies = async (
    id: number,
    data: SpeciesUpdateDto
): Promise<SpeciesDto> => {
    const response = await api.put<SpeciesDto>(`/species/${id}`, data);
    return response.data;
};

export const deleteSpecies = async (id: number): Promise<void> => {
    await api.delete(`/species/${id}`);
};

/** Rase, opcionalno filtrirane po svijetu i/ili vrsti. */
export const getRaces = async (params?: {
    worldId?: number;
    speciesId?: number;
}): Promise<RaceDto[]> => {
    const response = await api.get<RaceDto[]>("/races", { params });
    return response.data;
};

export const createRace = async (data: RaceCreateDto): Promise<RaceDto> => {
    const response = await api.post<RaceDto>("/races", data);
    return response.data;
};

export const updateRace = async (
    id: number,
    data: RaceUpdateDto
): Promise<RaceDto> => {
    const response = await api.put<RaceDto>(`/races/${id}`, data);
    return response.data;
};

export const deleteRace = async (id: number): Promise<void> => {
    await api.delete(`/races/${id}`);
};
