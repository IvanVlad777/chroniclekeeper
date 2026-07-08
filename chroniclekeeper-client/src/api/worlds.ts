import api from "../services/api";
import { WorldDto } from "../interfaces/loreInterfaces";

/** Svjetovi prijavljenog korisnika. */
export const getMyWorlds = async (): Promise<WorldDto[]> => {
    const response = await api.get<WorldDto[]>("/worlds/mine");
    return response.data;
};

export const getWorld = async (id: number): Promise<WorldDto> => {
    const response = await api.get<WorldDto>(`/worlds/${id}`);
    return response.data;
};
