import api from "../services/api";
import {
    PrivilegeLawCreateDto,
    PrivilegeLawDto,
    PrivilegeLawUpdateDto,
} from "../interfaces/loreInterfaces";

/** Zakoni o privilegijama, filtrirani po svijetu i/ili društvenom sloju. */
export const getPrivilegeLaws = async (
    worldId?: number,
    socialClassId?: number
): Promise<PrivilegeLawDto[]> => {
    const response = await api.get<PrivilegeLawDto[]>("/privilege-laws", {
        params: { worldId, socialClassId },
    });
    return response.data;
};

export const getPrivilegeLawById = async (
    id: number
): Promise<PrivilegeLawDto> => {
    const response = await api.get<PrivilegeLawDto>(`/privilege-laws/${id}`);
    return response.data;
};

export const createPrivilegeLaw = async (
    data: PrivilegeLawCreateDto
): Promise<PrivilegeLawDto> => {
    const response = await api.post<PrivilegeLawDto>("/privilege-laws", data);
    return response.data;
};

export const updatePrivilegeLaw = async (
    id: number,
    data: PrivilegeLawUpdateDto
): Promise<PrivilegeLawDto> => {
    const response = await api.put<PrivilegeLawDto>(
        `/privilege-laws/${id}`,
        data
    );
    return response.data;
};

export const deletePrivilegeLaw = async (id: number): Promise<void> => {
    await api.delete(`/privilege-laws/${id}`);
};
