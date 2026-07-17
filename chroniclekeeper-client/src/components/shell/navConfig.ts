/**
 * Grouped navigation config — the single source of truth for the shell.
 * The 12-domain icon rail + domain panel and the grouped "+ New entry" menu
 * are all driven from `navDomains`. Labels are i18n keys: domain names resolve
 * via t(`navGroups.${domain.key}`), entries via t(`nav.${entry.key}`).
 */

export interface NavEntry {
    /** i18n key: t(`nav.${key}`) */
    key: string;
    /** Absolute route, e.g. "/storymap/characters". */
    route: string;
    /** NavLink `end` — only for Overview (active on the index route). */
    end?: boolean;
    /**
     * Where the "+ New entry" menu links for this entry. Omitted => not
     * creatable (won't appear in the menu). Tags/Notes point at their list
     * page (they create inline, no /new route) — same as the old menu.
     */
    newRoute?: string;
    /** Extra route prefixes owned by this entry (for active-domain matching). */
    matchExtra?: string[];
}

export interface NavDomain {
    /** i18n key: t(`navGroups.${key}`) */
    key: string;
    /** Rail glyph. */
    glyph: string;
    entries: NavEntry[];
}

/** Helper to keep entry definitions terse and consistent. */
const entry = (
    key: string,
    route: string,
    opts?: Partial<Omit<NavEntry, "key" | "route">>
): NavEntry => ({ key, route, newRoute: `${route}/new`, ...opts });

export const navDomains: NavDomain[] = [
    {
        key: "overview",
        glyph: "◈",
        entries: [
            {
                key: "overview",
                route: "/storymap",
                end: true,
                matchExtra: ["/storymap/overview"],
            },
        ],
    },
    {
        key: "people",
        glyph: "♟",
        entries: [
            entry("characters", "/storymap/characters"),
            entry("species", "/storymap/species"),
            entry("professions", "/storymap/professions"),
            entry("abilities", "/storymap/abilities"),
        ],
    },
    {
        key: "places",
        glyph: "⚑",
        entries: [
            entry("locations", "/storymap/locations"),
            entry("climateZones", "/storymap/climate-zones"),
            entry("climateDetails", "/storymap/climate-details"),
            entry("seasons", "/storymap/seasons"),
        ],
    },
    {
        key: "nature",
        glyph: "❦",
        entries: [entry("creatures", "/storymap/creatures")],
    },
    {
        key: "society",
        glyph: "⚔",
        entries: [
            entry("nations", "/storymap/nations"),
            entry("factions", "/storymap/factions"),
            entry("religions", "/storymap/religions"),
            entry("cultures", "/storymap/cultures"),
            entry("languages", "/storymap/languages"),
            entry("socialClasses", "/storymap/social-classes"),
        ],
    },
    {
        key: "politics",
        glyph: "⚖",
        entries: [
            entry("politicalIdeologies", "/storymap/political-ideologies"),
            entry("governmentSystems", "/storymap/government-systems"),
            entry("politicalParties", "/storymap/political-parties"),
            entry("legalSystems", "/storymap/legal-systems"),
            entry("diplomaticAgreements", "/storymap/diplomatic-agreements"),
        ],
    },
    {
        key: "economy",
        glyph: "⚒",
        entries: [
            entry("economicSystems", "/storymap/economic-systems"),
            entry("bankingSystems", "/storymap/banking-systems"),
            entry("taxationSystems", "/storymap/taxation-systems"),
            entry("currencies", "/storymap/currencies"),
            entry("tradeRoutes", "/storymap/trade-routes"),
            entry("naturalResources", "/storymap/natural-resources"),
            entry("extractionMethods", "/storymap/extraction-methods"),
            entry("industries", "/storymap/industries"),
            entry("guilds", "/storymap/guilds"),
            entry("corporations", "/storymap/corporations"),
        ],
    },
    {
        key: "military",
        glyph: "⚔",
        entries: [
            entry("armies", "/storymap/armies", {
                matchExtra: ["/storymap/military-units"],
            }),
            entry("militaryOrganizations", "/storymap/military-organizations"),
            entry("militaryDoctrines", "/storymap/military-doctrines"),
            entry("battles", "/storymap/battles"),
            entry("militaryEquipment", "/storymap/military-equipment"),
        ],
    },
    {
        key: "knowledge",
        glyph: "❖",
        entries: [
            entry("educationSystems", "/storymap/education-systems"),
            entry("schools", "/storymap/schools"),
            entry("universities", "/storymap/universities"),
            entry("libraries", "/storymap/libraries"),
        ],
    },
    {
        key: "chronicle",
        glyph: "⌛",
        entries: [
            entry("histories", "/storymap/histories"),
            entry("timelines", "/storymap/timelines"),
        ],
    },
    {
        key: "artifacts",
        glyph: "⚗",
        entries: [entry("items", "/storymap/items")],
    },
    {
        key: "media",
        glyph: "✇",
        entries: [
            entry("contents", "/storymap/contents", {
                matchExtra: ["/storymap/chapters", "/storymap/episodes"],
            }),
        ],
    },
    {
        key: "workbench",
        glyph: "✎",
        entries: [
            // Tags/Notes create inline on their list page — no /new route.
            entry("tags", "/storymap/tags", { newRoute: "/storymap/tags" }),
            entry("notes", "/storymap/notes", { newRoute: "/storymap/notes" }),
        ],
    },
];

/** Flat list of every entry (for path matching and quick-jump). */
export const allEntries: { entry: NavEntry; domain: NavDomain }[] =
    navDomains.flatMap((domain) =>
        domain.entries.map((entry) => ({ entry, domain }))
    );

/** Keys shown as the "Frequent" chips in the "+ New entry" menu (mockup 1D). */
export const frequentNewKeys = [
    "characters",
    "locations",
    "factions",
    "notes",
    "timelines",
    "items",
];

/**
 * Which domain owns the given pathname — longest matching route (or matchExtra)
 * prefix wins. Falls back to the Overview domain.
 */
export function findDomainForPath(pathname: string): NavDomain {
    let best: { domain: NavDomain; len: number } | null = null;
    for (const { entry, domain } of allEntries) {
        const prefixes = [entry.route, ...(entry.matchExtra ?? [])];
        for (const prefix of prefixes) {
            // Overview's "/storymap" must match exactly, not as a prefix of all.
            const matches = entry.end
                ? pathname === prefix
                : pathname === prefix || pathname.startsWith(prefix + "/");
            if (matches && prefix.length > (best?.len ?? -1)) {
                best = { domain, len: prefix.length };
            }
        }
    }
    return best?.domain ?? navDomains[0];
}
