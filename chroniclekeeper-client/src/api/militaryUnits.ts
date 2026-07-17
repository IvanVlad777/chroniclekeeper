import api from "../services/api";
import {
    MilitaryUnitCreateDto,
    MilitaryUnitDetailsDto,
    MilitaryUnitDto,
    MilitaryUnitUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojne postrojbe, filtrirane po svijetu i/ili vojsci. */
export const getMilitaryUnits = async (params?: {
    worldId?: number;
    armyId?: number;
}): Promise<MilitaryUnitDto[]> => {
    const response = await api.get<MilitaryUnitDto[]>("/military-units", {
        params,
    });
    return response.data;
};

export const getMilitaryUnitById = async (
    id: number
): Promise<MilitaryUnitDetailsDto> => {
    const response = await api.get<MilitaryUnitDetailsDto>(
        `/military-units/${id}`
    );
    return response.data;
};

export const createMilitaryUnit = async (
    data: MilitaryUnitCreateDto
): Promise<MilitaryUnitDto> => {
    const response = await api.post<MilitaryUnitDto>("/military-units", data);
    return response.data;
};

export const updateMilitaryUnit = async (
    id: number,
    data: MilitaryUnitUpdateDto
): Promise<MilitaryUnitDto> => {
    const response = await api.put<MilitaryUnitDto>(
        `/military-units/${id}`,
        data
    );
    return response.data;
};

export const deleteMilitaryUnit = async (id: number): Promise<void> => {
    await api.delete(`/military-units/${id}`);
};

// ----- Equipment links -----
export const addUnitEquipment = async (
    unitId: number,
    equipmentId: number
): Promise<void> => {
    await api.post(`/military-units/${unitId}/equipment/${equipmentId}`);
};

export const removeUnitEquipment = async (
    unitId: number,
    equipmentId: number
): Promise<void> => {
    await api.delete(`/military-units/${unitId}/equipment/${equipmentId}`);
};
