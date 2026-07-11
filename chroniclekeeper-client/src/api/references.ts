import api from "../services/api";
import {
    ContentReferenceLinkCreateDto,
    ContentReferenceLinkDto,
} from "../interfaces/loreInterfaces";

/** Reference (appearance-linkovi) — proslijedi točno jedan content-side i jedan entity-side id. */
export const getReferences = async (params?: {
    contentId?: number;
    chapterId?: number;
    episodeId?: number;
    characterId?: number;
    locationId?: number;
    factionId?: number;
    nationId?: number;
}): Promise<ContentReferenceLinkDto[]> => {
    const response = await api.get<ContentReferenceLinkDto[]>("/references", {
        params,
    });
    return response.data;
};

export const createReference = async (
    data: ContentReferenceLinkCreateDto
): Promise<ContentReferenceLinkDto> => {
    const response = await api.post<ContentReferenceLinkDto>(
        "/references",
        data
    );
    return response.data;
};

export const deleteReference = async (id: number): Promise<void> => {
    await api.delete(`/references/${id}`);
};
