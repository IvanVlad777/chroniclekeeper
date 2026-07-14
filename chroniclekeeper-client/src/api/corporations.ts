import api from "../services/api";
import {
    CorporateLeadershipCreateDto,
    CorporateLeadershipDto,
    CorporateLeadershipUpdateDto,
    CorporationCreateDto,
    CorporationDetailsDto,
    CorporationDto,
    CorporationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Korporacije, opcionalno filtrirane po svijetu. */
export const getCorporations = async (
    worldId?: number
): Promise<CorporationDto[]> => {
    const response = await api.get<CorporationDto[]>("/corporations", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCorporationById = async (
    id: number
): Promise<CorporationDetailsDto> => {
    const response = await api.get<CorporationDetailsDto>(`/corporations/${id}`);
    return response.data;
};

export const createCorporation = async (
    data: CorporationCreateDto
): Promise<CorporationDto> => {
    const response = await api.post<CorporationDto>("/corporations", data);
    return response.data;
};

export const updateCorporation = async (
    id: number,
    data: CorporationUpdateDto
): Promise<CorporationDto> => {
    const response = await api.put<CorporationDto>(`/corporations/${id}`, data);
    return response.data;
};

export const deleteCorporation = async (id: number): Promise<void> => {
    await api.delete(`/corporations/${id}`);
};

export const addCorporationFaction = async (
    corporationId: number,
    factionId: number
): Promise<void> => {
    await api.post(`/corporations/${corporationId}/factions/${factionId}`);
};

export const removeCorporationFaction = async (
    corporationId: number,
    factionId: number
): Promise<void> => {
    await api.delete(`/corporations/${corporationId}/factions/${factionId}`);
};

export const addCorporationProfession = async (
    corporationId: number,
    professionId: number
): Promise<void> => {
    await api.post(`/corporations/${corporationId}/professions/${professionId}`);
};

export const removeCorporationProfession = async (
    corporationId: number,
    professionId: number
): Promise<void> => {
    await api.delete(`/corporations/${corporationId}/professions/${professionId}`);
};

// ----- Corporate leadership (inline-managed na Corporation detalju) -----

export const createCorporateLeadership = async (
    data: CorporateLeadershipCreateDto
): Promise<CorporateLeadershipDto> => {
    const response = await api.post<CorporateLeadershipDto>(
        "/corporate-leaderships",
        data
    );
    return response.data;
};

export const updateCorporateLeadership = async (
    id: number,
    data: CorporateLeadershipUpdateDto
): Promise<CorporateLeadershipDto> => {
    const response = await api.put<CorporateLeadershipDto>(
        `/corporate-leaderships/${id}`,
        data
    );
    return response.data;
};

export const deleteCorporateLeadership = async (id: number): Promise<void> => {
    await api.delete(`/corporate-leaderships/${id}`);
};
