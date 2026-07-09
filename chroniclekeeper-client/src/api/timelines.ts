import api from "../services/api";
import {
    TimelineCreateDto,
    TimelineDetailsDto,
    TimelineDto,
    TimelineEventCreateDto,
    TimelineEventDto,
    TimelineEventUpdateDto,
    TimelineUpdateDto,
} from "../interfaces/loreInterfaces";

/** Vremenske linije, opcionalno filtrirane po svijetu. */
export const getTimelines = async (
    worldId?: number
): Promise<TimelineDto[]> => {
    const response = await api.get<TimelineDto[]>("/timelines", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj s eventima poredanima po sortOrder. */
export const getTimeline = async (
    id: number
): Promise<TimelineDetailsDto> => {
    const response = await api.get<TimelineDetailsDto>(`/timelines/${id}`);
    return response.data;
};

export const createTimeline = async (
    data: TimelineCreateDto
): Promise<TimelineDto> => {
    const response = await api.post<TimelineDto>("/timelines", data);
    return response.data;
};

export const updateTimeline = async (
    id: number,
    data: TimelineUpdateDto
): Promise<TimelineDto> => {
    const response = await api.put<TimelineDto>(`/timelines/${id}`, data);
    return response.data;
};

export const deleteTimeline = async (id: number): Promise<void> => {
    await api.delete(`/timelines/${id}`);
};

export const addTimelineEvent = async (
    timelineId: number,
    data: TimelineEventCreateDto
): Promise<TimelineEventDto> => {
    const response = await api.post<TimelineEventDto>(
        `/timelines/${timelineId}/events`,
        data
    );
    return response.data;
};

export const updateTimelineEvent = async (
    eventId: number,
    data: TimelineEventUpdateDto
): Promise<TimelineEventDto> => {
    const response = await api.put<TimelineEventDto>(
        `/timelines/events/${eventId}`,
        data
    );
    return response.data;
};

export const deleteTimelineEvent = async (eventId: number): Promise<void> => {
    await api.delete(`/timelines/events/${eventId}`);
};
