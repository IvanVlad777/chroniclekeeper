import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { getCharacters } from "../../../api/characters";
import { getLocations } from "../../../api/locations";
import { getFactions } from "../../../api/factions";
import { getSpecies } from "../../../api/species";
import { useWorld } from "../../../hooks/useWorld";
import s from "./styles.module.css";

type RecentType = "character" | "location" | "faction" | "species";

interface RecentEntry {
    type: RecentType;
    id: number;
    name: string;
    updatedAt: string;
}

const detailPath: Record<RecentType, string> = {
    character: "/storymap/characters",
    location: "/storymap/locations",
    faction: "/storymap/factions",
    species: "/storymap/species",
};

interface Stats {
    characters: number;
    locations: number;
    factions: number;
    species: number;
}

export default function Overview() {
    const { t } = useTranslation("overview");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [stats, setStats] = useState<Stats | null>(null);
    const [recent, setRecent] = useState<RecentEntry[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setStats(null);
            setRecent([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        Promise.all([
            getCharacters(selectedWorld.id),
            getLocations(selectedWorld.id),
            getFactions(selectedWorld.id),
            getSpecies(selectedWorld.id),
        ])
            .then(([characters, locations, factions, species]) => {
                if (cancelled) return;
                setStats({
                    characters: characters.length,
                    locations: locations.length,
                    factions: factions.length,
                    species: species.length,
                });
                const merged: RecentEntry[] = [
                    ...characters.map((c) => ({
                        type: "character" as const,
                        id: c.id,
                        name: c.name,
                        updatedAt: c.updatedAt ?? "",
                    })),
                    ...locations.map((l) => ({
                        type: "location" as const,
                        id: l.id,
                        name: l.name,
                        updatedAt: l.updatedAt ?? "",
                    })),
                    ...factions.map((f) => ({
                        type: "faction" as const,
                        id: f.id,
                        name: f.name,
                        updatedAt: f.updatedAt ?? "",
                    })),
                    ...species.map((sp) => ({
                        type: "species" as const,
                        id: sp.id,
                        name: sp.name,
                        updatedAt: sp.updatedAt ?? "",
                    })),
                ];
                merged.sort((a, b) =>
                    b.updatedAt.localeCompare(a.updatedAt)
                );
                setRecent(merged.slice(0, 6));
            })
            .catch((err) => {
                console.error("Failed to load overview data:", err);
                if (!cancelled) setError("Failed to load");
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, reloadKey]);

    if (worldLoading || loading) return <LoadingSkeleton variant="block" />;
    if (error) return <ErrorState onRetry={refetch} detail={error} />;
    if (!selectedWorld || !stats) {
        return (
            <EmptyState
                glyph="✦"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const statCards: {
        key: keyof Stats;
        glyph: string;
        label: string;
        to: string;
    }[] = [
        {
            key: "characters",
            glyph: "♟",
            label: t("statCharacters"),
            to: "/storymap/characters",
        },
        {
            key: "locations",
            glyph: "⚑",
            label: t("statLocations"),
            to: "/storymap/locations",
        },
        {
            key: "factions",
            glyph: "⚔",
            label: t("statFactions"),
            to: "/storymap/factions",
        },
        {
            key: "species",
            glyph: "⚘",
            label: t("statSpecies"),
            to: "/storymap/species",
        },
    ];

    return (
        <div className={s.page}>
            <div className={s.header}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>{t("currentWorld")}</div>
                    <h1 className={s.worldName}>{selectedWorld.name}</h1>
                    {selectedWorld.description && (
                        <p className={s.meta}>{selectedWorld.description}</p>
                    )}
                </div>
            </div>

            <div className={s.stats}>
                {statCards.map((card) => (
                    <Link key={card.key} to={card.to} className={s.statCard}>
                        <div className={s.statTop}>
                            <span className={s.statGlyph}>{card.glyph}</span>
                            <span className={s.statLabel}>{card.label}</span>
                        </div>
                        <div className={s.statValue}>{stats[card.key]}</div>
                    </Link>
                ))}
            </div>

            <div className={s.panel}>
                <div className={s.panelHead}>
                    <span className={s.panelStar}>✦</span>
                    <span className={s.panelTitle}>{t("recentlyEdited")}</span>
                </div>
                {recent.length === 0 ? (
                    <p className={s.noRecent}>{t("noRecent")}</p>
                ) : (
                    recent.map((entry) => (
                        <Link
                            key={`${entry.type}-${entry.id}`}
                            to={`${detailPath[entry.type]}/${entry.id}`}
                            className={s.recentRow}
                        >
                            <span className={s.typeBadge}>
                                {t(`badges.${entry.type}`)}
                            </span>
                            <span className={s.recentName}>{entry.name}</span>
                            <span className={s.recentWhen}>
                                {entry.updatedAt
                                    ? new Date(
                                          entry.updatedAt
                                      ).toLocaleDateString()
                                    : ""}
                            </span>
                        </Link>
                    ))
                )}
            </div>
        </div>
    );
}
