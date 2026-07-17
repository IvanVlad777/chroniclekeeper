import api from "../services/api";
import {
    MilitaryOrganizationCreateDto,
    MilitaryOrganizationDetailsDto,
    MilitaryOrganizationDto,
    MilitaryOrganizationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojne organizacije, opcionalno filtrirane po svijetu. */
export const getMilitaryOrganizations = async (
    worldId?: number
): Promise<MilitaryOrganizationDto[]> => {
    const response = await api.get<MilitaryOrganizationDto[]>(
        "/military-organizations",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getMilitaryOrganizationById = async (
    id: number
): Promise<MilitaryOrganizationDetailsDto> => {
    const response = await api.get<MilitaryOrganizationDetailsDto>(
        `/military-organizations/${id}`
    );
    return response.data;
};

export const createMilitaryOrganization = async (
    data: MilitaryOrganizationCreateDto
): Promise<MilitaryOrganizationDto> => {
    const response = await api.post<MilitaryOrganizationDto>(
        "/military-organizations",
        data
    );
    return response.data;
};

export const updateMilitaryOrganization = async (
    id: number,
    data: MilitaryOrganizationUpdateDto
): Promise<MilitaryOrganizationDto> => {
    const response = await api.put<MilitaryOrganizationDto>(
        `/military-organizations/${id}`,
        data
    );
    return response.data;
};

export const deleteMilitaryOrganization = async (id: number): Promise<void> => {
    await api.delete(`/military-organizations/${id}`);
};

// ----- Country links -----
export const addOrganizationCountry = async (
    organizationId: number,
    countryId: number
): Promise<void> => {
    await api.post(
        `/military-organizations/${organizationId}/countries/${countryId}`
    );
};

export const removeOrganizationCountry = async (
    organizationId: number,
    countryId: number
): Promise<void> => {
    await api.delete(
        `/military-organizations/${organizationId}/countries/${countryId}`
    );
};

// ----- Faction links -----
export const addOrganizationFaction = async (
    organizationId: number,
    factionId: number
): Promise<void> => {
    await api.post(
        `/military-organizations/${organizationId}/factions/${factionId}`
    );
};

export const removeOrganizationFaction = async (
    organizationId: number,
    factionId: number
): Promise<void> => {
    await api.delete(
        `/military-organizations/${organizationId}/factions/${factionId}`
    );
};
