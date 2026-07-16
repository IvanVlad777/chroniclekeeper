import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { navDomains, allEntries, frequentNewKeys } from "./navConfig";
import s from "./shell.module.css";

/**
 * Grouped "+ New entry" menu (mockup 1D): a "Frequent" chip row plus a
 * two-column grid of domains, each listing its creatable entries. Rendered
 * only for editors (the parent gates on role). Closes on outside click / Esc.
 */
export function NewEntryMenu() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [open, setOpen] = useState(false);
    const ref = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!open) return;
        const onDoc = (e: PointerEvent) => {
            if (ref.current && !ref.current.contains(e.target as Node)) {
                setOpen(false);
            }
        };
        const onKey = (e: KeyboardEvent) =>
            e.key === "Escape" && setOpen(false);
        document.addEventListener("pointerdown", onDoc);
        document.addEventListener("keydown", onKey);
        return () => {
            document.removeEventListener("pointerdown", onDoc);
            document.removeEventListener("keydown", onKey);
        };
    }, [open]);

    const go = (route: string) => {
        setOpen(false);
        navigate(route);
    };

    const frequent = frequentNewKeys
        .map((key) => allEntries.find((x) => x.entry.key === key)?.entry)
        .filter((e): e is NonNullable<typeof e> => !!e && !!e.newRoute);

    // Domains with at least one creatable entry (skip Overview).
    const domains = navDomains.filter((d) =>
        d.entries.some((e) => e.newRoute)
    );

    return (
        <div className={s.newEntry} ref={ref}>
            {open && (
                <div className={s.newEntryMenu} role="menu">
                    <div className={s.nemFrequent}>
                        <div className={s.nemSectionLabel}>
                            {t("newEntryMenu.frequent")}
                        </div>
                        <div className={s.nemChips}>
                            {frequent.map((entry) => (
                                <button
                                    key={entry.key}
                                    type="button"
                                    role="menuitem"
                                    className={s.nemChip}
                                    onClick={() => go(entry.newRoute!)}
                                >
                                    <span className={s.nemChipPlus}>+</span>
                                    {t(`nav.${entry.key}`)}
                                </button>
                            ))}
                        </div>
                    </div>

                    <div className={s.nemGrid}>
                        {domains.map((domain) => (
                            <div key={domain.key} className={s.nemDomain}>
                                <div className={s.nemDomainHead}>
                                    <span className={s.nemDomainGlyph}>
                                        {domain.glyph}
                                    </span>
                                    {t(`navGroups.${domain.key}`)}
                                </div>
                                <div className={s.nemDomainLinks}>
                                    {domain.entries
                                        .filter((e) => e.newRoute)
                                        .map((entry) => (
                                            <button
                                                key={entry.key}
                                                type="button"
                                                role="menuitem"
                                                className={s.nemLink}
                                                onClick={() =>
                                                    go(entry.newRoute!)
                                                }
                                            >
                                                {t(`nav.${entry.key}`)}
                                            </button>
                                        ))}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}

            <button
                type="button"
                className={s.newEntryBtn}
                aria-haspopup="menu"
                aria-expanded={open}
                title={t("shell.newEntryTitle")}
                onClick={() => setOpen((o) => !o)}
            >
                <span className={s.newEntryPlus}>+</span>
                {t("shell.newEntry")}
                <span className={s.newEntryCaret}>▾</span>
            </button>
        </div>
    );
}
