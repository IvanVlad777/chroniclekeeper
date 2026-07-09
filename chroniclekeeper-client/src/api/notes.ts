import api from "../services/api";
import {
    NoteCreateDto,
    NoteDto,
    NoteUpdateDto,
} from "../interfaces/loreInterfaces";

/** Bilješke, opcionalno filtrirane po svijetu. */
export const getNotes = async (worldId?: number): Promise<NoteDto[]> => {
    const response = await api.get<NoteDto[]>("/notes", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

export const getNote = async (id: number): Promise<NoteDto> => {
    const response = await api.get<NoteDto>(`/notes/${id}`);
    return response.data;
};

export const createNote = async (data: NoteCreateDto): Promise<NoteDto> => {
    const response = await api.post<NoteDto>("/notes", data);
    return response.data;
};

export const updateNote = async (
    id: number,
    data: NoteUpdateDto
): Promise<NoteDto> => {
    const response = await api.put<NoteDto>(`/notes/${id}`, data);
    return response.data;
};

export const deleteNote = async (id: number): Promise<void> => {
    await api.delete(`/notes/${id}`);
};
