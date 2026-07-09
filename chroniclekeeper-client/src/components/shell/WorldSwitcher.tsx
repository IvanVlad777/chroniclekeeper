import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useWorld } from "../../hooks/useWorld";
import { useAuth } from "../../hooks/useAuth";
import { WorldDto } from "../../interfaces/loreInterfaces";
import { CreateWorldDialog } from "./CreateWorldDialog";
import s from "./shell.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

/**
 * Kartica aktivnog svijeta + popover s listom svih svjetova korisnika
 * i opcijom stvaranja novog. Zatvara se na vanjski klik i Escape.
 */
export function WorldSwitcher() {
    const { t } = useTranslation();
    const { worlds, selectedWorld, selectWorld, loading, error, refresh } =
        useWorld();
    const { userInfo } = useAuth();
    const [open, setOpen] = useState(false);
    const [creating, setCreating] = useState(false);
    const ref = useRef<HTMLDivElement>(null);

    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

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

    const handleCreated = async (world: WorldDto) => {
        setCreating(false);
        setOpen(false);
        await refresh();
        selectWorld(world.id);
    };

    if (loading) {
        return <div className={s.worldEmpty}>…</div>;
    }
    if (error) {
        return <div className={s.worldEmpty}>{error}</div>;
    }
    if (worlds.length === 0) {
        return (
            <>
                {canCreate ? (
                    <button
                        type="button"
                        className={s.worldCard}
                        onClick={() => setCreating(true)}
                    >
                        <span className={s.worldThumb} />
                        <span className={s.worldInfo}>
                            <span className={s.worldName}>
                                {t("worldDialog.createFirst")}
                            </span>
                            <span className={s.worldMeta}>{t("noworlds")}</span>
                        </span>
                        <span className={s.worldCaret}>+</span>
                    </button>
                ) : (
                    <div className={s.worldEmpty}>{t("noworlds")}</div>
                )}
                {creating && (
                    <CreateWorldDialog
                        onCreated={handleCreated}
                        onClose={() => setCreating(false)}
                    />
                )}
            </>
        );
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
                    {canCreate && (
                        <button
                            type="button"
                            className={s.worldCreateRow}
                            onClick={() => {
                                setOpen(false);
                                setCreating(true);
                            }}
                        >
                            <span className={s.worldCreatePlus}>+</span>
                            {t("worldDialog.createRow")}
                        </button>
                    )}
                </div>
            )}

            {creating && (
                <CreateWorldDialog
                    onCreated={handleCreated}
                    onClose={() => setCreating(false)}
                />
            )}
        </div>
    );
}
