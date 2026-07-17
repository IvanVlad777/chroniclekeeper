import api from "../services/api";
import {
    ArmyCreateDto,
    ArmyDetailsDto,
    ArmyDto,
    ArmyUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vojske, opcionalno filtrirane po svijetu. */
export const getArmies = async (worldId?: number): Promise<ArmyDto[]> => {
    const response = await api.get<ArmyDto[]>("/armies", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getArmyById = async (id: number): Promise<ArmyDetailsDto> => {
    const response = await api.get<ArmyDetailsDto>(`/armies/${id}`);
    return response.data;
};

export const createArmy = async (data: ArmyCreateDto): Promise<ArmyDto> => {
    const response = await api.post<ArmyDto>("/armies", data);
    return response.data;
};

export const updateArmy = async (
    id: number,
    data: ArmyUpdateDto
): Promise<ArmyDto> => {
    const response = await api.put<ArmyDto>(`/armies/${id}`, data);
    return response.data;
};

export const deleteArmy = async (id: number): Promise<void> => {
    await api.delete(`/armies/${id}`);
};

// ----- Battle links -----
export const addArmyBattle = async (
    armyId: number,
    battleId: number
): Promise<void> => {
    await api.post(`/armies/${armyId}/battles/${battleId}`);
};

export const removeArmyBattle = async (
    armyId: number,
    battleId: number
): Promise<void> => {
    await api.delete(`/armies/${armyId}/battles/${battleId}`);
};
