import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { CharacterDto } from "../../../interfaces/loreInterfaces";
import { getCharacters } from "../../../api/characters";
import { useWorld } from "../../../hooks/useWorld";
import s from "./styles.module.css";

// Stats za Locations/Factions/Species čekaju svoje API vertikale — do tada "—".
const pendingStats = ["locations", "factions", "species"] as const;

export default function Overview() {
    const { t } = useTranslation("overview");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [characters, setCharacters] = useState<CharacterDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setCharacters([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getCharacters(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setCharacters(data);
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
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="✦"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const recent = [...characters]
        .sort((a, b) => (b.updatedAt ?? "").localeCompare(a.updatedAt ?? ""))
        .slice(0, 5);

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
                <Link to="/storymap/characters" className={s.statCard}>
                    <div className={s.statTop}>
                        <span className={s.statGlyph}>♟</span>
                        <span className={s.statLabel}>
                            {t("statCharacters")}
                        </span>
                    </div>
                    <div className={s.statValue}>{characters.length}</div>
                </Link>
                {pendingStats.map((key) => (
                    <div key={key} className={s.statCard}>
                        <div className={s.statTop}>
                            <span className={s.statGlyph}>
                                {key === "locations"
                                    ? "⚑"
                                    : key === "factions"
                                    ? "⚔"
                                    : "⚘"}
                            </span>
                            <span className={s.statLabel}>
                                {t(
                                    `stat${
                                        key.charAt(0).toUpperCase() +
                                        key.slice(1)
                                    }`
                                )}
                            </span>
                        </div>
                        <div className={`${s.statValue} ${s.statSoon}`}>
                            —
                            <span className={s.statSoonNote}>
                                {t("shell.soon", { ns: "common" })}
                            </span>
                        </div>
                    </div>
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
                    recent.map((c) => (
                        <Link
                            key={c.id}
                            to={`/storymap/characters/${c.id}`}
                            className={s.recentRow}
                        >
                            <span className={s.typeBadge}>
                                {t("characterBadge")}
                            </span>
                            <span className={s.recentName}>{c.name}</span>
                            <span className={s.recentWhen}>
                                {c.updatedAt
                                    ? new Date(
                                          c.updatedAt
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
