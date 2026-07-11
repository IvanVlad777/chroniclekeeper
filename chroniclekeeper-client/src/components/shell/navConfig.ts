export interface NavEntry {
    /** i18n ključ: t(`nav.${key}`) */
    key: string;
    glyph: string;
    /** Relativna ruta unutar /storymap; disabled stavke je nemaju. */
    to?: string;
    /** Ruta još nije izgrađena — stavka se prikazuje, ali je neaktivna. */
    disabled?: boolean;
    /** NavLink `end` — samo za Overview (aktivan i na indexu). */
    end?: boolean;
}

export const navEntries: NavEntry[] = [
    { key: "overview", glyph: "◈", to: "/storymap", end: true },
    { key: "characters", glyph: "♟", to: "/storymap/characters" },
    { key: "locations", glyph: "⚑", to: "/storymap/locations" },
    { key: "factions", glyph: "⚔", to: "/storymap/factions" },
    { key: "species", glyph: "⚘", to: "/storymap/species" },
    { key: "socialClasses", glyph: "⚖", to: "/storymap/social-classes" },
    { key: "nations", glyph: "♛", to: "/storymap/nations" },
    { key: "religions", glyph: "✤", to: "/storymap/religions" },
    { key: "languages", glyph: "✒", to: "/storymap/languages" },
    { key: "cultures", glyph: "☉", to: "/storymap/cultures" },
    { key: "timelines", glyph: "⌛", to: "/storymap/timelines" },
    { key: "tags", glyph: "❧", to: "/storymap/tags" },
    { key: "notes", glyph: "✎", to: "/storymap/notes" },
    {
        key: "politicalIdeologies",
        glyph: "◐",
        to: "/storymap/political-ideologies",
    },
    {
        key: "governmentSystems",
        glyph: "⌂",
        to: "/storymap/government-systems",
    },
    {
        key: "politicalParties",
        glyph: "✪",
        to: "/storymap/political-parties",
    },
    { key: "legalSystems", glyph: "⛨", to: "/storymap/legal-systems" },
    {
        key: "diplomaticAgreements",
        glyph: "⚜",
        to: "/storymap/diplomatic-agreements",
    },
    { key: "professions", glyph: "⚒", to: "/storymap/professions" },
    { key: "schools", glyph: "☰", to: "/storymap/schools" },
    { key: "universities", glyph: "⚛", to: "/storymap/universities" },
    {
        key: "educationSystems",
        glyph: "▦",
        to: "/storymap/education-systems",
    },
    { key: "libraries", glyph: "❖", to: "/storymap/libraries" },
];
