import { createContext } from "react";
import { WorldDto } from "../../interfaces/loreInterfaces";

export interface WorldContextValue {
    /** Svjetovi prijavljenog korisnika. */
    worlds: WorldDto[];
    /** Trenutno odabrani svijet (null dok se ne učita / ako korisnik nema svjetova). */
    selectedWorld: WorldDto | null;
    selectWorld: (worldId: number) => void;
    loading: boolean;
    error: string | null;
    /** Ponovno dohvaćanje liste svjetova. */
    refresh: () => Promise<void>;
}

export const WorldContext = createContext<WorldContextValue | null>(null);
