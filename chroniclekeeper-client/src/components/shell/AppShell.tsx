import { useEffect, useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { TopBar } from "./TopBar";
import { Rail } from "./Rail";
import { DomainPanel } from "./DomainPanel";
import { QuickJump } from "./QuickJump";
import { findDomainForPath, navDomains, type NavDomain } from "./navConfig";
import s from "./shell.module.css";

/** Main app frame: top bar + icon rail + domain panel + content (Outlet). */
export function AppShell() {
    const location = useLocation();
    const navigate = useNavigate();
    const [peekDomainKey, setPeekDomainKey] = useState<string | null>(null);
    const [paletteOpen, setPaletteOpen] = useState(false);

    const routeDomain = findDomainForPath(location.pathname);
    const peekDomain = peekDomainKey
        ? navDomains.find((d) => d.key === peekDomainKey)
        : undefined;
    const activeDomain = peekDomain ?? routeDomain;

    // Navigating anywhere snaps the rail + panel back to route truth.
    useEffect(() => {
        setPeekDomainKey(null);
    }, [location.pathname]);

    // Global ⌘K / Ctrl+K to open the quick-jump palette.
    useEffect(() => {
        const onKey = (e: KeyboardEvent) => {
            if (
                (e.ctrlKey || e.metaKey) &&
                !e.altKey &&
                e.key.toLowerCase() === "k"
            ) {
                e.preventDefault();
                setPaletteOpen(true);
            }
        };
        document.addEventListener("keydown", onKey);
        return () => document.removeEventListener("keydown", onKey);
    }, []);

    const handleRailSelect = (domain: NavDomain) => {
        // Childless Overview navigates directly; others just switch the panel.
        if (domain.key === "overview") {
            setPeekDomainKey(null);
            navigate(domain.entries[0].route);
        } else {
            setPeekDomainKey(domain.key);
        }
    };

    return (
        <div className={s.shell}>
            <TopBar onOpenPalette={() => setPaletteOpen(true)} />
            <div className={s.body}>
                <Rail
                    activeDomainKey={activeDomain.key}
                    onSelect={handleRailSelect}
                />
                <DomainPanel domain={activeDomain} />
                <main className={s.main}>
                    <Outlet />
                </main>
            </div>
            {paletteOpen && (
                <QuickJump onClose={() => setPaletteOpen(false)} />
            )}
        </div>
    );
}
