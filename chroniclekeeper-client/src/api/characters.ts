import api from "../services/api";
import {
    CharacterDetailsDto,
    CharacterDto,
} from "../interfaces/loreInterfaces";

/** Likovi, opcionalno filtrirani po svijetu. */
export const getCharacters = async (
    worldId?: number
): Promise<CharacterDto[]> => {
    const response = await api.get<CharacterDto[]>("/characters", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getCharacter = async (
    id: number
): Promise<CharacterDetailsDto> => {
    const response = await api.get<CharacterDetailsDto>(`/characters/${id}`);
    return response.data;
};
