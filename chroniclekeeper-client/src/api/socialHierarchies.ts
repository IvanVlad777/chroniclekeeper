import api from "../services/api";
import {
    SocialHierarchyCreateDto,
    SocialHierarchyDetailsDto,
    SocialHierarchyDto,
    SocialHierarchyUpdateDto,
} from "../interfaces/loreInterfaces";

/** Društvene hijerarhije, opcionalno filtrirane po svijetu. */
export const getSocialHierarchies = async (
    worldId?: number
): Promise<SocialHierarchyDto[]> => {
    const response = await api.get<SocialHierarchyDto[]>("/social-hierarchies", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getSocialHierarchyById = async (
    id: number
): Promise<SocialHierarchyDetailsDto> => {
    const response = await api.get<SocialHierarchyDetailsDto>(
        `/social-hierarchies/${id}`
    );
    return response.data;
};

export const createSocialHierarchy = async (
    data: SocialHierarchyCreateDto
): Promise<SocialHierarchyDto> => {
    const response = await api.post<SocialHierarchyDto>(
        "/social-hierarchies",
        data
    );
    return response.data;
};

export const updateSocialHierarchy = async (
    id: number,
    data: SocialHierarchyUpdateDto
): Promise<SocialHierarchyDto> => {
    const response = await api.put<SocialHierarchyDto>(
        `/social-hierarchies/${id}`,
        data
    );
    return response.data;
};

export const deleteSocialHierarchy = async (id: number): Promise<void> => {
    await api.delete(`/social-hierarchies/${id}`);
};
