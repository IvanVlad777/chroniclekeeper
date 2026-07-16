import { useQueries } from "@tanstack/react-query";
import { useWorld } from "../../hooks/useWorld";
import type { NavEntry } from "./navConfig";
import { entryCountFetchers } from "./entryCountFetchers";

/**
 * Lazily fetches item counts for the given entries (the active domain only)
 * scoped to the selected world. Returns a map entryKey -> count | undefined
 * (undefined while loading, on error, or when no fetcher exists). First
 * TanStack Query use in the client — cached 60s via the shared client.
 */
export function useEntryCounts(entries: NavEntry[]): Record<string, number | undefined> {
    const { selectedWorld } = useWorld();
    const worldId = selectedWorld?.id;

    const countable = entries.filter((e) => entryCountFetchers[e.key]);

    const results = useQueries({
        queries: countable.map((e) => ({
            queryKey: ["entryCount", e.key, worldId],
            queryFn: () => entryCountFetchers[e.key](worldId!),
            enabled: worldId != null,
            select: (data: unknown[]) => data.length,
        })),
    });

    const counts: Record<string, number | undefined> = {};
    countable.forEach((e, i) => {
        counts[e.key] = results[i].data;
    });
    return counts;
}
