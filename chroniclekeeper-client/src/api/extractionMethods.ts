import api from "../services/api";
import {
    ExtractionMethodCreateDto,
    ExtractionMethodDetailsDto,
    ExtractionMethodDto,
    ExtractionMethodUpdateDto,
} from "../interfaces/loreInterfaces";

/** Metode ekstrakcije, opcionalno filtrirane po svijetu. */
export const getExtractionMethods = async (
    worldId?: number
): Promise<ExtractionMethodDto[]> => {
    const response = await api.get<ExtractionMethodDto[]>("/extraction-methods", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getExtractionMethodById = async (
    id: number
): Promise<ExtractionMethodDetailsDto> => {
    const response = await api.get<ExtractionMethodDetailsDto>(
        `/extraction-methods/${id}`
    );
    return response.data;
};

export const createExtractionMethod = async (
    data: ExtractionMethodCreateDto
): Promise<ExtractionMethodDto> => {
    const response = await api.post<ExtractionMethodDto>("/extraction-methods", data);
    return response.data;
};

export const updateExtractionMethod = async (
    id: number,
    data: ExtractionMethodUpdateDto
): Promise<ExtractionMethodDto> => {
    const response = await api.put<ExtractionMethodDto>(
        `/extraction-methods/${id}`,
        data
    );
    return response.data;
};

export const deleteExtractionMethod = async (id: number): Promise<void> => {
    await api.delete(`/extraction-methods/${id}`);
};
