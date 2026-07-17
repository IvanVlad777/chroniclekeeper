import api from "../services/api";
import {
    MilitaryDoctrineCreateDto,
    MilitaryDoctrineDetailsDto,
    MilitaryDoctrineDto,
    MilitaryDoctrineUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojne doktrine, opcionalno filtrirane po svijetu. */
export const getMilitaryDoctrines = async (
    worldId?: number
): Promise<MilitaryDoctrineDto[]> => {
    const response = await api.get<MilitaryDoctrineDto[]>("/military-doctrines", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getMilitaryDoctrineById = async (
    id: number
): Promise<MilitaryDoctrineDetailsDto> => {
    const response = await api.get<MilitaryDoctrineDetailsDto>(
        `/military-doctrines/${id}`
    );
    return response.data;
};

export const createMilitaryDoctrine = async (
    data: MilitaryDoctrineCreateDto
): Promise<MilitaryDoctrineDto> => {
    const response = await api.post<MilitaryDoctrineDto>(
        "/military-doctrines",
        data
    );
    return response.data;
};

export const updateMilitaryDoctrine = async (
    id: number,
    data: MilitaryDoctrineUpdateDto
): Promise<MilitaryDoctrineDto> => {
    const response = await api.put<MilitaryDoctrineDto>(
        `/military-doctrines/${id}`,
        data
    );
    return response.data;
};

export const deleteMilitaryDoctrine = async (id: number): Promise<void> => {
    await api.delete(`/military-doctrines/${id}`);
};
