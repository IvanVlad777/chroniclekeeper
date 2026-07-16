import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import type { TFunction } from "i18next";
import { allEntries } from "./navConfig";
import s from "./quickJump.module.css";

interface QuickJumpProps {
    onClose: () => void;
}

export interface PaletteResult {
    /** "section" today; "entity" reserved for a future backend search provider. */
    kind: "section";
    key: string;
    domainKey: string;
    label: string;
    /** Group heading (currently the domain name). */
    group: string;
    to: string;
    hint: string;
}

/** Result provider signature — lets an async entity search slot in later. */
type Provider = (query: string, t: TFunction) => PaletteResult[];

const sectionProvider: Provider = (query, t) => {
    const q = query.trim().toLowerCase();
    return allEntries
        .map(({ entry, domain }) => {
            const label = t(`nav.${entry.key}`);
            const group = t(`navGroups.${domain.key}`);
            return {
                kind: "section" as const,
                key: entry.key,
                domainKey: domain.key,
                label,
                group,
                to: entry.route,
                hint: group,
            };
        })
        .filter(
            (r) =>
                q === "" ||
                r.label.toLowerCase().includes(q) ||
                r.group.toLowerCase().includes(q)
        );
};

const providers: Provider[] = [sectionProvider];

/** ⌘K / Ctrl+K command palette — jump to any section (mockup 1C). */
export function QuickJump({ onClose }: QuickJumpProps) {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [query, setQuery] = useState("");
    const [active, setActive] = useState(0);
    const inputRef = useRef<HTMLInputElement>(null);
    const listRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        inputRef.current?.focus();
    }, []);

    const results = useMemo(
        () => providers.flatMap((p) => p(query, t)),
        [query, t]
    );

    // Keep the highlighted index in range as results change.
    useEffect(() => {
        setActive((a) => (a >= results.length ? 0 : a));
    }, [results.length]);

    // Scroll the active row into view.
    useEffect(() => {
        const el = listRef.current?.querySelector(`[data-idx="${active}"]`);
        el?.scrollIntoView({ block: "nearest" });
    }, [active]);

    const openResult = (r: PaletteResult | undefined) => {
        if (!r) return;
        onClose();
        navigate(r.to);
    };

    const onKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === "ArrowDown") {
            e.preventDefault();
            setActive((a) => (results.length ? (a + 1) % results.length : 0));
        } else if (e.key === "ArrowUp") {
            e.preventDefault();
            setActive((a) =>
                results.length ? (a - 1 + results.length) % results.length : 0
            );
        } else if (e.key === "Enter") {
            e.preventDefault();
            openResult(results[active]);
        } else if (e.key === "Escape") {
            e.preventDefault();
            onClose();
        } else if (e.key === "Tab") {
            e.preventDefault();
        }
    };

    // Group consecutive results by their group heading (allEntries is ordered).
    const groups: { name: string; items: { r: PaletteResult; idx: number }[] }[] =
        [];
    results.forEach((r, idx) => {
        const last = groups[groups.length - 1];
        if (last && last.name === r.group) last.items.push({ r, idx });
        else groups.push({ name: r.group, items: [{ r, idx }] });
    });

    return (
        <div
            className={s.scrim}
            onPointerDown={(e) => {
                if (e.target === e.currentTarget) onClose();
            }}
        >
            <div
                className={s.card}
                role="dialog"
                aria-modal="true"
                aria-label={t("quickJump.placeholder")}
            >
                <div className={s.inputRow}>
                    <span className={s.searchGlyph} aria-hidden="true">
                        ⌕
                    </span>
                    <input
                        ref={inputRef}
                        className={s.input}
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        onKeyDown={onKeyDown}
                        placeholder={t("quickJump.placeholder")}
                        role="combobox"
                        aria-expanded="true"
                        aria-controls="quickjump-list"
                        aria-activedescendant={
                            results.length ? `quickjump-opt-${active}` : undefined
                        }
                    />
                    <span className={s.escChip}>{t("quickJump.close")}</span>
                </div>

                <div className={s.results} id="quickjump-list" role="listbox" ref={listRef}>
                    {results.length === 0 ? (
                        <div className={s.noResults}>
                            {t("quickJump.noResults")}
                        </div>
                    ) : (
                        groups.map((group) => (
                            <div key={group.name} className={s.group}>
                                <div className={s.groupLabel}>{group.name}</div>
                                {group.items.map(({ r, idx }) => (
                                    <button
                                        key={r.key}
                                        type="button"
                                        id={`quickjump-opt-${idx}`}
                                        data-idx={idx}
                                        role="option"
                                        aria-selected={idx === active}
                                        className={`${s.row} ${
                                            idx === active ? s.rowActive : ""
                                        }`}
                                        onPointerEnter={() => setActive(idx)}
                                        onClick={() => openResult(r)}
                                    >
                                        <span className={s.rowPill}>
                                            {t("quickJump.section")}
                                        </span>
                                        <span className={s.rowLabel}>
                                            {r.label}
                                        </span>
                                        <span className={s.rowHint}>
                                            {r.hint}
                                        </span>
                                    </button>
                                ))}
                            </div>
                        ))
                    )}
                </div>

                <div className={s.footer}>
                    <span className={s.footerHint}>
                        <span className={s.kbd}>↑</span>
                        <span className={s.kbd}>↓</span>
                        {t("quickJump.navigate")}
                    </span>
                    <span className={s.footerHint}>
                        <span className={s.kbd}>↵</span>
                        {t("quickJump.open")}
                    </span>
                </div>
            </div>
        </div>
    );
}
