import api from "../services/api";
import {
    GuildCreateDto,
    GuildDetailsDto,
    GuildDto,
    GuildRankCreateDto,
    GuildRankDto,
    GuildRankUpdateDto,
    GuildUpdateDto,
} from "../interfaces/loreInterfaces";

/** Cehovi, opcionalno filtrirani po svijetu. */
export const getGuilds = async (worldId?: number): Promise<GuildDto[]> => {
    const response = await api.get<GuildDto[]>("/guilds", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getGuildById = async (id: number): Promise<GuildDetailsDto> => {
    const response = await api.get<GuildDetailsDto>(`/guilds/${id}`);
    return response.data;
};

export const createGuild = async (data: GuildCreateDto): Promise<GuildDto> => {
    const response = await api.post<GuildDto>("/guilds", data);
    return response.data;
};

export const updateGuild = async (
    id: number,
    data: GuildUpdateDto
): Promise<GuildDto> => {
    const response = await api.put<GuildDto>(`/guilds/${id}`, data);
    return response.data;
};

export const deleteGuild = async (id: number): Promise<void> => {
    await api.delete(`/guilds/${id}`);
};

export const addGuildFaction = async (
    guildId: number,
    factionId: number
): Promise<void> => {
    await api.post(`/guilds/${guildId}/factions/${factionId}`);
};

export const removeGuildFaction = async (
    guildId: number,
    factionId: number
): Promise<void> => {
    await api.delete(`/guilds/${guildId}/factions/${factionId}`);
};

export const addGuildProfession = async (
    guildId: number,
    professionId: number
): Promise<void> => {
    await api.post(`/guilds/${guildId}/professions/${professionId}`);
};

export const removeGuildProfession = async (
    guildId: number,
    professionId: number
): Promise<void> => {
    await api.delete(`/guilds/${guildId}/professions/${professionId}`);
};

export const addGuildSocialClass = async (
    guildId: number,
    socialClassId: number
): Promise<void> => {
    await api.post(`/guilds/${guildId}/social-classes/${socialClassId}`);
};

export const removeGuildSocialClass = async (
    guildId: number,
    socialClassId: number
): Promise<void> => {
    await api.delete(`/guilds/${guildId}/social-classes/${socialClassId}`);
};

// ----- Guild ranks (inline-managed na Guild detalju) -----

export const createGuildRank = async (
    data: GuildRankCreateDto
): Promise<GuildRankDto> => {
    const response = await api.post<GuildRankDto>("/guild-ranks", data);
    return response.data;
};

export const updateGuildRank = async (
    id: number,
    data: GuildRankUpdateDto
): Promise<GuildRankDto> => {
    const response = await api.put<GuildRankDto>(`/guild-ranks/${id}`, data);
    return response.data;
};

export const deleteGuildRank = async (id: number): Promise<void> => {
    await api.delete(`/guild-ranks/${id}`);
};
