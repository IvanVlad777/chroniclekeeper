import api from "../services/api";
import {
    DiplomaticAgreementCreateDto,
    DiplomaticAgreementDetailsDto,
    DiplomaticAgreementDto,
    DiplomaticAgreementUpdateDto,
} from "../interfaces/loreInterfaces";

export const getDiplomaticAgreements = async (
    worldId?: number
): Promise<DiplomaticAgreementDto[]> => {
    const response = await api.get<DiplomaticAgreementDto[]>(
        "/diplomaticagreements",
        { params: worldId ? { worldId } : undefined }
    );
    return response.data;
};

export const getDiplomaticAgreementById = async (
    id: number
): Promise<DiplomaticAgreementDetailsDto> => {
    const response = await api.get<DiplomaticAgreementDetailsDto>(
        `/diplomaticagreements/${id}`
    );
    return response.data;
};

export const createDiplomaticAgreement = async (
    data: DiplomaticAgreementCreateDto
): Promise<DiplomaticAgreementDto> => {
    const response = await api.post<DiplomaticAgreementDto>(
        "/diplomaticagreements",
        data
    );
    return response.data;
};

export const updateDiplomaticAgreement = async (
    id: number,
    data: DiplomaticAgreementUpdateDto
): Promise<DiplomaticAgreementDto> => {
    const response = await api.put<DiplomaticAgreementDto>(
        `/diplomaticagreements/${id}`,
        data
    );
    return response.data;
};

export const deleteDiplomaticAgreement = async (id: number): Promise<void> => {
    await api.delete(`/diplomaticagreements/${id}`);
};
