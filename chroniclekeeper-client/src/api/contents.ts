import api from "../services/api";
import {
    ChapterCreateDto,
    ChapterDto,
    ChapterUpdateDto,
    ContentCreateDto,
    ContentDetailsDto,
    ContentDto,
    ContentUpdateDto,
    EpisodeCreateDto,
    EpisodeDto,
    EpisodeUpdateDto,
} from "../interfaces/loreInterfaces";

/** Sadržaji (Article/Book/Comic/Movie/Series), opcionalno filtrirani po svijetu i/ili tipu. */
export const getContents = async (params?: {
    worldId?: number;
    type?: string;
}): Promise<ContentDto[]> => {
    const response = await api.get<ContentDto[]>("/contents", { params });
    return response.data;
};

export const getContentById = async (id: number): Promise<ContentDetailsDto> => {
    const response = await api.get<ContentDetailsDto>(`/contents/${id}`);
    return response.data;
};

export const createContent = async (
    data: ContentCreateDto
): Promise<ContentDto> => {
    const response = await api.post<ContentDto>("/contents", data);
    return response.data;
};

export const updateContent = async (
    id: number,
    data: ContentUpdateDto
): Promise<ContentDto> => {
    const response = await api.put<ContentDto>(`/contents/${id}`, data);
    return response.data;
};

export const deleteContent = async (id: number): Promise<void> => {
    await api.delete(`/contents/${id}`);
};

// ----- Chapters (inline-managed na Book detalju) -----

export const getChapters = async (params?: {
    worldId?: number;
    bookId?: number;
}): Promise<ChapterDto[]> => {
    const response = await api.get<ChapterDto[]>("/chapters", { params });
    return response.data;
};

export const getChapterById = async (id: number): Promise<ChapterDto> => {
    const response = await api.get<ChapterDto>(`/chapters/${id}`);
    return response.data;
};

export const createChapter = async (
    data: ChapterCreateDto
): Promise<ChapterDto> => {
    const response = await api.post<ChapterDto>("/chapters", data);
    return response.data;
};

export const updateChapter = async (
    id: number,
    data: ChapterUpdateDto
): Promise<ChapterDto> => {
    const response = await api.put<ChapterDto>(`/chapters/${id}`, data);
    return response.data;
};

export const deleteChapter = async (id: number): Promise<void> => {
    await api.delete(`/chapters/${id}`);
};

// ----- Episodes (inline-managed na Series detalju) -----

export const getEpisodes = async (params?: {
    worldId?: number;
    seriesId?: number;
}): Promise<EpisodeDto[]> => {
    const response = await api.get<EpisodeDto[]>("/episodes", { params });
    return response.data;
};

export const getEpisodeById = async (id: number): Promise<EpisodeDto> => {
    const response = await api.get<EpisodeDto>(`/episodes/${id}`);
    return response.data;
};

export const createEpisode = async (
    data: EpisodeCreateDto
): Promise<EpisodeDto> => {
    const response = await api.post<EpisodeDto>("/episodes", data);
    return response.data;
};

export const updateEpisode = async (
    id: number,
    data: EpisodeUpdateDto
): Promise<EpisodeDto> => {
    const response = await api.put<EpisodeDto>(`/episodes/${id}`, data);
    return response.data;
};

export const deleteEpisode = async (id: number): Promise<void> => {
    await api.delete(`/episodes/${id}`);
};
