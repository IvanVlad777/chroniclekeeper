import api from "../services/api";
import { RaceDto, SpeciesDto } from "../interfaces/loreInterfaces";

/** Vrste, opcionalno filtrirane po svijetu. */
export const getSpecies = async (worldId?: number): Promise<SpeciesDto[]> => {
    const response = await api.get<SpeciesDto[]>("/species", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Rase, opcionalno filtrirane po svijetu i/ili vrsti. */
export const getRaces = async (params?: {
    worldId?: number;
    speciesId?: number;
}): Promise<RaceDto[]> => {
    const response = await api.get<RaceDto[]>("/races", { params });
    return response.data;
};
