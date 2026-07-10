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
