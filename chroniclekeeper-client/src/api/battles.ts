import api from "../services/api";
import {
    BattleCreateDto,
    BattleDetailsDto,
    BattleDto,
    BattleUpdateDto,
} from "../interfaces/loreInterfaces";

/** Bitke, opcionalno filtrirane po svijetu. */
export const getBattles = async (worldId?: number): Promise<BattleDto[]> => {
    const response = await api.get<BattleDto[]>("/battles", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getBattleById = async (id: number): Promise<BattleDetailsDto> => {
    const response = await api.get<BattleDetailsDto>(`/battles/${id}`);
    return response.data;
};

export const createBattle = async (
    data: BattleCreateDto
): Promise<BattleDto> => {
    const response = await api.post<BattleDto>("/battles", data);
    return response.data;
};

export const updateBattle = async (
    id: number,
    data: BattleUpdateDto
): Promise<BattleDto> => {
    const response = await api.put<BattleDto>(`/battles/${id}`, data);
    return response.data;
};

export const deleteBattle = async (id: number): Promise<void> => {
    await api.delete(`/battles/${id}`);
};
