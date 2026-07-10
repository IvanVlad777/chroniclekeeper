import api from "../services/api";
import {
    PoliticalIdeologyCreateDto,
    PoliticalIdeologyDetailsDto,
    PoliticalIdeologyDto,
    PoliticalIdeologyUpdateDto,
} from "../interfaces/loreInterfaces";

export const getPoliticalIdeologies = async (
    worldId?: number
): Promise<PoliticalIdeologyDto[]> => {
    const response = await api.get<PoliticalIdeologyDto[]>(
        "/politicalideologies",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getPoliticalIdeologyById = async (
    id: number
): Promise<PoliticalIdeologyDetailsDto> => {
    const response = await api.get<PoliticalIdeologyDetailsDto>(
        `/politicalideologies/${id}`
    );
    return response.data;
};

export const createPoliticalIdeology = async (
    data: PoliticalIdeologyCreateDto
): Promise<PoliticalIdeologyDto> => {
    const response = await api.post<PoliticalIdeologyDto>(
        "/politicalideologies",
        data
    );
    return response.data;
};

export const updatePoliticalIdeology = async (
    id: number,
    data: PoliticalIdeologyUpdateDto
): Promise<PoliticalIdeologyDto> => {
    const response = await api.put<PoliticalIdeologyDto>(
        `/politicalideologies/${id}`,
        data
    );
    return response.data;
};

export const deletePoliticalIdeology = async (id: number): Promise<void> => {
    await api.delete(`/politicalideologies/${id}`);
};
