import api from "../services/api";
import {
    SocialClassCreateDto,
    SocialClassDetailsDto,
    SocialClassDto,
    SocialClassUpdateDto,
} from "../interfaces/loreInterfaces";

/** Društveni slojevi, opcionalno filtrirani po svijetu. */
export const getSocialClasses = async (
    worldId?: number
): Promise<SocialClassDto[]> => {
    const response = await api.get<SocialClassDto[]>("/social-classes", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj društvenog sloja s članovima. */
export const getSocialClassById = async (
    id: number
): Promise<SocialClassDetailsDto> => {
    const response = await api.get<SocialClassDetailsDto>(
        `/social-classes/${id}`
    );
    return response.data;
};

export const createSocialClass = async (
    data: SocialClassCreateDto
): Promise<SocialClassDto> => {
    const response = await api.post<SocialClassDto>("/social-classes", data);
    return response.data;
};

export const updateSocialClass = async (
    id: number,
    data: SocialClassUpdateDto
): Promise<SocialClassDto> => {
    const response = await api.put<SocialClassDto>(
        `/social-classes/${id}`,
        data
    );
    return response.data;
};

export const deleteSocialClass = async (id: number): Promise<void> => {
    await api.delete(`/social-classes/${id}`);
};
