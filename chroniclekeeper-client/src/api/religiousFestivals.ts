import api from "../services/api";
import {
    ReligiousFestivalCreateDto,
    ReligiousFestivalDetailsDto,
    ReligiousFestivalDto,
    ReligiousFestivalUpdateDto,
} from "../interfaces/loreInterfaces";

export const getReligiousFestivals = async (
    worldId?: number
): Promise<ReligiousFestivalDto[]> => {
    const response = await api.get<ReligiousFestivalDto[]>(
        "/religious-festivals",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getReligiousFestivalById = async (
    id: number
): Promise<ReligiousFestivalDetailsDto> => {
    const response = await api.get<ReligiousFestivalDetailsDto>(
        `/religious-festivals/${id}`
    );
    return response.data;
};

export const createReligiousFestival = async (
    data: ReligiousFestivalCreateDto
): Promise<ReligiousFestivalDto> => {
    const response = await api.post<ReligiousFestivalDto>(
        "/religious-festivals",
        data
    );
    return response.data;
};

export const updateReligiousFestival = async (
    id: number,
    data: ReligiousFestivalUpdateDto
): Promise<ReligiousFestivalDto> => {
    const response = await api.put<ReligiousFestivalDto>(
        `/religious-festivals/${id}`,
        data
    );
    return response.data;
};

export const deleteReligiousFestival = async (id: number): Promise<void> => {
    await api.delete(`/religious-festivals/${id}`);
};
