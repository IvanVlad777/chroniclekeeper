import { useCallback, useEffect, useState } from "react";
import { WorldContext } from "./WorldContext";
import { WorldDto } from "../../interfaces/loreInterfaces";
import { getMyWorlds } from "../../api/worlds";

const STORAGE_KEY = "selectedWorldId";

/**
 * Drži svjetove prijavljenog korisnika i odabrani svijet (pamti se u sessionStorage).
 * Montirati unutar ProtectedRoute — dohvat kreće odmah i traži valjan token.
 */
export const WorldProvider = ({ children }: { children: React.ReactNode }) => {
    const [worlds, setWorlds] = useState<WorldDto[]>([]);
    const [selectedWorldId, setSelectedWorldId] = useState<number | null>(() => {
        const stored = sessionStorage.getItem(STORAGE_KEY);
        return stored ? Number(stored) : null;
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const refresh = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await getMyWorlds();
            setWorlds(data);
        } catch (err) {
            console.error("Failed to load worlds:", err);
            setError("Failed to load worlds");
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        void refresh();
    }, [refresh]);

    // Ako odabrani svijet ne postoji (prvi login, obrisan svijet) — uzmi prvi s liste
    useEffect(() => {
        if (worlds.length === 0) return;
        if (!worlds.some((w) => w.id === selectedWorldId)) {
            setSelectedWorldId(worlds[0].id);
            sessionStorage.setItem(STORAGE_KEY, String(worlds[0].id));
        }
    }, [worlds, selectedWorldId]);

    const selectWorld = (worldId: number) => {
        setSelectedWorldId(worldId);
        sessionStorage.setItem(STORAGE_KEY, String(worldId));
    };

    const selectedWorld = worlds.find((w) => w.id === selectedWorldId) ?? null;

    return (
        <WorldContext.Provider
            value={{ worlds, selectedWorld, selectWorld, loading, error, refresh }}
        >
            {children}
        </WorldContext.Provider>
    );
};
