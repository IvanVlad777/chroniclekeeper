import api from "../services/api";
import {
    MutationCreateDto,
    MutationDetailsDto,
    MutationDto,
    MutationUpdateDto,
} from "../interfaces/loreInterfaces";

/** Mutacije, opcionalno filtrirane po svijetu. */
export const getMutations = async (
    worldId?: number
): Promise<MutationDto[]> => {
    const response = await api.get<MutationDto[]>("/mutations", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getMutationById = async (
    id: number
): Promise<MutationDetailsDto> => {
    const response = await api.get<MutationDetailsDto>(`/mutations/${id}`);
    return response.data;
};

export const createMutation = async (
    data: MutationCreateDto
): Promise<MutationDto> => {
    const response = await api.post<MutationDto>("/mutations", data);
    return response.data;
};

export const updateMutation = async (
    id: number,
    data: MutationUpdateDto
): Promise<MutationDto> => {
    const response = await api.put<MutationDto>(`/mutations/${id}`, data);
    return response.data;
};

export const deleteMutation = async (id: number): Promise<void> => {
    await api.delete(`/mutations/${id}`);
};
