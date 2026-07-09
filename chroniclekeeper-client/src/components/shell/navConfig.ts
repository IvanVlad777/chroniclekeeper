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
    { key: "locations", glyph: "⚑", disabled: true },
    { key: "factions", glyph: "⚔", disabled: true },
    { key: "species", glyph: "⚘", disabled: true },
    { key: "timelines", glyph: "⌛", disabled: true },
    { key: "tags", glyph: "❧", disabled: true },
    { key: "notes", glyph: "✎", disabled: true },
];
