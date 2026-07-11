import api from "../services/api";
import {
    ClimateZoneCreateDto,
    ClimateZoneDetailsDto,
    ClimateZoneDto,
    ClimateZoneUpdateDto,
    WeatherPatternCreateDto,
    WeatherPatternDto,
    WeatherPatternUpdateDto,
} from "../interfaces/loreInterfaces";

/** Klimatske zone, opcionalno filtrirane po svijetu. */
export const getClimateZones = async (
    worldId?: number
): Promise<ClimateZoneDto[]> => {
    const response = await api.get<ClimateZoneDto[]>("/climate-zones", {
        params: worldId ? { worldId } : undefined,
    });
    return response.data;
};

/** Detalj klimatske zone s poviješću, detaljima, sezonama, lokacijama i vremenskim obrascima. */
export const getClimateZoneById = async (
    id: number
): Promise<ClimateZoneDetailsDto> => {
    const response = await api.get<ClimateZoneDetailsDto>(`/climate-zones/${id}`);
    return response.data;
};

export const createClimateZone = async (
    data: ClimateZoneCreateDto
): Promise<ClimateZoneDto> => {
    const response = await api.post<ClimateZoneDto>("/climate-zones", data);
    return response.data;
};

export const updateClimateZone = async (
    id: number,
    data: ClimateZoneUpdateDto
): Promise<ClimateZoneDto> => {
    const response = await api.put<ClimateZoneDto>(`/climate-zones/${id}`, data);
    return response.data;
};

export const deleteClimateZone = async (id: number): Promise<void> => {
    await api.delete(`/climate-zones/${id}`);
};

export const addClimateZoneDetail = async (
    climateZoneId: number,
    climateDetailId: number
): Promise<void> => {
    await api.post(`/climate-zones/${climateZoneId}/details/${climateDetailId}`);
};

export const removeClimateZoneDetail = async (
    climateZoneId: number,
    climateDetailId: number
): Promise<void> => {
    await api.delete(`/climate-zones/${climateZoneId}/details/${climateDetailId}`);
};

export const addClimateZoneSeason = async (
    climateZoneId: number,
    seasonId: number
): Promise<void> => {
    await api.post(`/climate-zones/${climateZoneId}/seasons/${seasonId}`);
};

export const removeClimateZoneSeason = async (
    climateZoneId: number,
    seasonId: number
): Promise<void> => {
    await api.delete(`/climate-zones/${climateZoneId}/seasons/${seasonId}`);
};

export const addClimateZoneLocation = async (
    climateZoneId: number,
    locationId: number
): Promise<void> => {
    await api.post(`/climate-zones/${climateZoneId}/locations/${locationId}`);
};

export const removeClimateZoneLocation = async (
    climateZoneId: number,
    locationId: number
): Promise<void> => {
    await api.delete(`/climate-zones/${climateZoneId}/locations/${locationId}`);
};

// ----- Weather patterns (inline-managed na ClimateZone detalju) -----

export const getWeatherPatterns = async (params?: {
    worldId?: number;
    climateZoneId?: number;
}): Promise<WeatherPatternDto[]> => {
    const response = await api.get<WeatherPatternDto[]>("/weather-patterns", { params });
    return response.data;
};

export const createWeatherPattern = async (
    data: WeatherPatternCreateDto
): Promise<WeatherPatternDto> => {
    const response = await api.post<WeatherPatternDto>("/weather-patterns", data);
    return response.data;
};

export const updateWeatherPattern = async (
    id: number,
    data: WeatherPatternUpdateDto
): Promise<WeatherPatternDto> => {
    const response = await api.put<WeatherPatternDto>(`/weather-patterns/${id}`, data);
    return response.data;
};

export const deleteWeatherPattern = async (id: number): Promise<void> => {
    await api.delete(`/weather-patterns/${id}`);
};
