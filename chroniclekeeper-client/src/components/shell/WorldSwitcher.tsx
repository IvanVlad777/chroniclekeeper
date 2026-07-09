import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useWorld } from "../../hooks/useWorld";
import s from "./shell.module.css";

/**
 * Kartica aktivnog svijeta + popover s listom svih svjetova korisnika.
 * Zatvara se na vanjski klik i Escape.
 */
export function WorldSwitcher() {
    const { t } = useTranslation();
    const { worlds, selectedWorld, selectWorld, loading, error } = useWorld();
    const [open, setOpen] = useState(false);
    const ref = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!open) return;
        const onDoc = (e: PointerEvent) => {
            if (ref.current && !ref.current.contains(e.target as Node)) {
                setOpen(false);
            }
        };
        const onKey = (e: KeyboardEvent) => e.key === "Escape" && setOpen(false);
        document.addEventListener("pointerdown", onDoc);
        document.addEventListener("keydown", onKey);
        return () => {
            document.removeEventListener("pointerdown", onDoc);
            document.removeEventListener("keydown", onKey);
        };
    }, [open]);

    if (loading) {
        return <div className={s.worldEmpty}>…</div>;
    }
    if (error) {
        return <div className={s.worldEmpty}>{error}</div>;
    }
    if (worlds.length === 0) {
        return <div className={s.worldEmpty}>{t("noworlds")}</div>;
    }

    return (
        <div className={s.worldSwitcher} ref={ref}>
            <button
                type="button"
                className={`${s.worldCard} ${open ? s.worldCardOpen : ""}`}
                aria-haspopup="listbox"
                aria-expanded={open}
                onClick={() => setOpen((o) => !o)}
            >
                <span className={s.worldThumb} />
                <span className={s.worldInfo}>
                    <span className={s.worldName}>
                        {selectedWorld?.name ?? "—"}
                    </span>
                    {selectedWorld?.description && (
                        <span className={s.worldMeta}>
                            {selectedWorld.description}
                        </span>
                    )}
                </span>
                <span className={s.worldCaret}>{open ? "▴" : "▾"}</span>
            </button>

            {open && (
                <div className={s.worldMenu} role="listbox">
                    {worlds.map((w) => {
                        const active = w.id === selectedWorld?.id;
                        return (
                            <button
                                key={w.id}
                                type="button"
                                role="option"
                                aria-selected={active}
                                className={`${s.worldMenuItem} ${
                                    active ? s.worldMenuItemActive : ""
                                }`}
                                onClick={() => {
                                    selectWorld(w.id);
                                    setOpen(false);
                                }}
                            >
                                <span className={s.worldMenuThumb} />
                                <span className={s.worldInfo}>
                                    <span className={s.worldMenuName}>
                                        {w.name}
                                    </span>
                                </span>
                                {active && (
                                    <span className={s.worldMenuCheck}>✓</span>
                                )}
                            </button>
                        );
                    })}
                </div>
            )}
        </div>
    );
}
