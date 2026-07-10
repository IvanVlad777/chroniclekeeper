import api from "../services/api";
import {
    PoliticalPartyCreateDto,
    PoliticalPartyDetailsDto,
    PoliticalPartyDto,
    PoliticalPartyUpdateDto,
} from "../interfaces/loreInterfaces";

export const getPoliticalParties = async (
    worldId?: number
): Promise<PoliticalPartyDto[]> => {
    const response = await api.get<PoliticalPartyDto[]>("/politicalparties", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getPoliticalPartyById = async (
    id: number
): Promise<PoliticalPartyDetailsDto> => {
    const response = await api.get<PoliticalPartyDetailsDto>(
        `/politicalparties/${id}`
    );
    return response.data;
};

export const createPoliticalParty = async (
    data: PoliticalPartyCreateDto
): Promise<PoliticalPartyDto> => {
    const response = await api.post<PoliticalPartyDto>(
        "/politicalparties",
        data
    );
    return response.data;
};

export const updatePoliticalParty = async (
    id: number,
    data: PoliticalPartyUpdateDto
): Promise<PoliticalPartyDto> => {
    const response = await api.put<PoliticalPartyDto>(
        `/politicalparties/${id}`,
        data
    );
    return response.data;
};

export const deletePoliticalParty = async (id: number): Promise<void> => {
    await api.delete(`/politicalparties/${id}`);
};

export const addPoliticalPartyFaction = async (
    politicalPartyId: number,
    factionId: number
): Promise<void> => {
    await api.post(`/politicalparties/${politicalPartyId}/factions/${factionId}`);
};

export const removePoliticalPartyFaction = async (
    politicalPartyId: number,
    factionId: number
): Promise<void> => {
    await api.delete(`/politicalparties/${politicalPartyId}/factions/${factionId}`);
};

export const addPoliticalPartyNation = async (
    politicalPartyId: number,
    nationId: number
): Promise<void> => {
    await api.post(`/politicalparties/${politicalPartyId}/nations/${nationId}`);
};

export const removePoliticalPartyNation = async (
    politicalPartyId: number,
    nationId: number
): Promise<void> => {
    await api.delete(`/politicalparties/${politicalPartyId}/nations/${nationId}`);
};
