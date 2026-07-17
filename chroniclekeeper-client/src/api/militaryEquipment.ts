import api from "../services/api";
import {
    MilitaryEquipmentCreateDto,
    MilitaryEquipmentDetailsDto,
    MilitaryEquipmentDto,
    MilitaryEquipmentUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojna oprema, opcionalno filtrirana po svijetu. */
export const getMilitaryEquipment = async (
    worldId?: number
): Promise<MilitaryEquipmentDto[]> => {
    const response = await api.get<MilitaryEquipmentDto[]>("/military-equipment", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getMilitaryEquipmentById = async (
    id: number
): Promise<MilitaryEquipmentDetailsDto> => {
    const response = await api.get<MilitaryEquipmentDetailsDto>(
        `/military-equipment/${id}`
    );
    return response.data;
};

export const createMilitaryEquipment = async (
    data: MilitaryEquipmentCreateDto
): Promise<MilitaryEquipmentDto> => {
    const response = await api.post<MilitaryEquipmentDto>(
        "/military-equipment",
        data
    );
    return response.data;
};

export const updateMilitaryEquipment = async (
    id: number,
    data: MilitaryEquipmentUpdateDto
): Promise<MilitaryEquipmentDto> => {
    const response = await api.put<MilitaryEquipmentDto>(
        `/military-equipment/${id}`,
        data
    );
    return response.data;
};

export const deleteMilitaryEquipment = async (id: number): Promise<void> => {
    await api.delete(`/military-equipment/${id}`);
};
