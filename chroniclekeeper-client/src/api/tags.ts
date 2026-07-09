import api from "../services/api";
import {
    TagAttachDto,
    TagCreateDto,
    TagDto,
    TagTargetType,
    TagUpdateDto,
} from "../interfaces/loreInterfaces";

/** Tagovi, opcionalno filtrirani po svijetu. */
export const getTags = async (worldId?: number): Promise<TagDto[]> => {
    const response = await api.get<TagDto[]>("/tags", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getTag = async (id: number): Promise<TagDto> => {
    const response = await api.get<TagDto>(`/tags/${id}`);
    return response.data;
};

export const createTag = async (data: TagCreateDto): Promise<TagDto> => {
    const response = await api.post<TagDto>("/tags", data);
    return response.data;
};

export const updateTag = async (
    id: number,
    data: TagUpdateDto
): Promise<TagDto> => {
    const response = await api.put<TagDto>(`/tags/${id}`, data);
    return response.data;
};

export const deleteTag = async (id: number): Promise<void> => {
    await api.delete(`/tags/${id}`);
};

/** Zakači tag na Character/Location/Faction. */
export const attachTag = async (
    tagId: number,
    data: TagAttachDto
): Promise<void> => {
    await api.post(`/tags/${tagId}/attachments`, data);
};

export const detachTag = async (
    tagId: number,
    targetType: TagTargetType,
    targetId: number
): Promise<void> => {
    await api.delete(`/tags/${tagId}/attachments/${targetType}/${targetId}`);
};
