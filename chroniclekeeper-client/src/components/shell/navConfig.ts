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
    { key: "timelines", glyph: "⌛", to: "/storymap/timelines" },
    { key: "tags", glyph: "❧", to: "/storymap/tags" },
    { key: "notes", glyph: "✎", to: "/storymap/notes" },
];
