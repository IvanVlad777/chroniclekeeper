import api from "../services/api";
import {
    HobbyCreateDto,
    HobbyDetailsDto,
    HobbyDto,
    HobbyUpdateDto,
} from "../interfaces/loreInterfaces";

/** Hobiji, opcionalno filtrirani po svijetu. */
export const getHobbies = async (worldId?: number): Promise<HobbyDto[]> => {
    const response = await api.get<HobbyDto[]>("/hobbies", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getHobbyById = async (id: number): Promise<HobbyDetailsDto> => {
    const response = await api.get<HobbyDetailsDto>(`/hobbies/${id}`);
    return response.data;
};

export const createHobby = async (
    data: HobbyCreateDto
): Promise<HobbyDto> => {
    const response = await api.post<HobbyDto>("/hobbies", data);
    return response.data;
};

export const updateHobby = async (
    id: number,
    data: HobbyUpdateDto
): Promise<HobbyDto> => {
    const response = await api.put<HobbyDto>(`/hobbies/${id}`, data);
    return response.data;
};

export const deleteHobby = async (id: number): Promise<void> => {
    await api.delete(`/hobbies/${id}`);
};
