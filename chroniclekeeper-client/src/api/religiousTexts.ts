import api from "../services/api";
import {
    ReligiousTextCreateDto,
    ReligiousTextDetailsDto,
    ReligiousTextDto,
    ReligiousTextUpdateDto,
} from "../interfaces/loreInterfaces";

export const getReligiousTexts = async (
    worldId?: number
): Promise<ReligiousTextDto[]> => {
    const response = await api.get<ReligiousTextDto[]>("/religious-texts", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getReligiousTextById = async (
    id: number
): Promise<ReligiousTextDetailsDto> => {
    const response = await api.get<ReligiousTextDetailsDto>(
        `/religious-texts/${id}`
    );
    return response.data;
};

export const createReligiousText = async (
    data: ReligiousTextCreateDto
): Promise<ReligiousTextDto> => {
    const response = await api.post<ReligiousTextDto>("/religious-texts", data);
    return response.data;
};

export const updateReligiousText = async (
    id: number,
    data: ReligiousTextUpdateDto
): Promise<ReligiousTextDto> => {
    const response = await api.put<ReligiousTextDto>(
        `/religious-texts/${id}`,
        data
    );
    return response.data;
};

export const deleteReligiousText = async (id: number): Promise<void> => {
    await api.delete(`/religious-texts/${id}`);
};
