import { QueryClient } from "@tanstack/react-query";

/**
 * Shared TanStack Query client. Introduced for the new shell (entry counts,
 * quick-jump). Existing screens keep their manual fetch pattern for now and
 * are migrated opportunistically — see chroniclekeeper-client/CLAUDE.md.
 */
export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 60_000,
            refetchOnWindowFocus: false,
            retry: 1,
        },
    },
});
