import { LocationType } from "../../../interfaces/loreInterfaces";

/** Glif po tipu lokacije (Grimoire mockup: ⚑ regija, ⌂ grad, ▲ ruševina/vrh). */
export const locationGlyphs: Record<LocationType, string> = {
    Continent: "❖",
    Region: "⚑",
    Country: "♔",
    City: "⌂",
    Town: "⌂",
    Village: "⌂",
    District: "▦",
    Building: "⌂",
    Landmark: "▲",
    Wilderness: "❧",
    Other: "✦",
};
