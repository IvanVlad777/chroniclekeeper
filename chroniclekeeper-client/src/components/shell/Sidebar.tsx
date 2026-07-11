import { useEffect, useRef, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { navEntries } from "./navConfig";
import { WorldSwitcher } from "./WorldSwitcher";
import { useAuth } from "../../hooks/useAuth";
import s from "./shell.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

/** Tip zapisa → odredište "+ Novi zapis" izbornika. */
const newEntryTargets: { key: string; glyph: string; to: string }[] = [
    { key: "characters", glyph: "♟", to: "/storymap/characters/new" },
    { key: "locations", glyph: "⚑", to: "/storymap/locations/new" },
    { key: "factions", glyph: "⚔", to: "/storymap/factions/new" },
    { key: "species", glyph: "⚘", to: "/storymap/species/new" },
    { key: "socialClasses", glyph: "⚖", to: "/storymap/social-classes/new" },
    { key: "nations", glyph: "♛", to: "/storymap/nations/new" },
    { key: "religions", glyph: "✤", to: "/storymap/religions/new" },
    { key: "languages", glyph: "✒", to: "/storymap/languages/new" },
    { key: "cultures", glyph: "☉", to: "/storymap/cultures/new" },
    { key: "timelines", glyph: "⌛", to: "/storymap/timelines/new" },
    { key: "tags", glyph: "❧", to: "/storymap/tags" },
    { key: "notes", glyph: "✎", to: "/storymap/notes" },
    {
        key: "politicalIdeologies",
        glyph: "◐",
        to: "/storymap/political-ideologies/new",
    },
    {
        key: "governmentSystems",
        glyph: "⌂",
        to: "/storymap/government-systems/new",
    },
    {
        key: "politicalParties",
        glyph: "✪",
        to: "/storymap/political-parties/new",
    },
    { key: "legalSystems", glyph: "⛨", to: "/storymap/legal-systems/new" },
    {
        key: "diplomaticAgreements",
        glyph: "⚜",
        to: "/storymap/diplomatic-agreements/new",
    },
    { key: "professions", glyph: "⚒", to: "/storymap/professions/new" },
    { key: "schools", glyph: "☰", to: "/storymap/schools/new" },
    { key: "universities", glyph: "⚛", to: "/storymap/universities/new" },
    {
        key: "educationSystems",
        glyph: "▦",
        to: "/storymap/education-systems/new",
    },
    { key: "libraries", glyph: "❖", to: "/storymap/libraries/new" },
    { key: "abilities", glyph: "✦", to: "/storymap/abilities/new" },
    { key: "items", glyph: "⚔", to: "/storymap/items/new" },
    { key: "histories", glyph: "⌛", to: "/storymap/histories/new" },
    { key: "contents", glyph: "📖", to: "/storymap/contents/new" },
];

export function Sidebar() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [menuOpen, setMenuOpen] = useState(false);
    const footerRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!menuOpen) return;
        const onDoc = (e: PointerEvent) => {
            if (
                footerRef.current &&
                !footerRef.current.contains(e.target as Node)
            ) {
                setMenuOpen(false);
            }
        };
        const onKey = (e: KeyboardEvent) =>
            e.key === "Escape" && setMenuOpen(false);
        document.addEventListener("pointerdown", onDoc);
        document.addEventListener("keydown", onKey);
        return () => {
            document.removeEventListener("pointerdown", onDoc);
            document.removeEventListener("keydown", onKey);
        };
    }, [menuOpen]);

    return (
        <aside className={s.sidebar}>
            <div className={s.worldSection}>
                <div className={s.sectionLabel}>{t("world")}</div>
                <WorldSwitcher />
            </div>

            <div className={s.ornament}>
                <span className={s.ornamentLine} />
                <span className={s.ornamentStar}>✦</span>
                <span className={s.ornamentLine} />
            </div>

            <nav className={s.nav}>
                {navEntries.map((entry) =>
                    entry.disabled ? (
                        <span
                            key={entry.key}
                            className={`${s.navItem} ${s.navItemDisabled}`}
                            aria-disabled="true"
                            title={t("shell.comingSoon")}
                        >
                            <span className={s.navGlyph}>{entry.glyph}</span>
                            {t(`nav.${entry.key}`)}
                            <span className={s.navSoon}>{t("shell.soon")}</span>
                        </span>
                    ) : (
                        <NavLink
                            key={entry.key}
                            to={entry.to!}
                            end={entry.end}
                            className={({ isActive }) =>
                                `${s.navItem} ${
                                    isActive ? s.navItemActive : ""
                                }`
                            }
                        >
                            <span className={s.navGlyph}>{entry.glyph}</span>
                            {t(`nav.${entry.key}`)}
                        </NavLink>
                    )
                )}
            </nav>

            <div className={s.sidebarFooter} ref={footerRef}>
                {menuOpen && (
                    <div className={s.newEntryMenu} role="menu">
                        {newEntryTargets.map((item) => (
                            <button
                                key={item.key}
                                type="button"
                                role="menuitem"
                                className={s.newEntryItem}
                                onClick={() => {
                                    setMenuOpen(false);
                                    navigate(item.to);
                                }}
                            >
                                <span className={s.newEntryItemGlyph}>
                                    {item.glyph}
                                </span>
                                {t(`nav.${item.key}`)}
                            </button>
                        ))}
                    </div>
                )}
                <button
                    type="button"
                    className={s.newEntryBtn}
                    disabled={!canCreate}
                    aria-haspopup="menu"
                    aria-expanded={menuOpen}
                    title={
                        canCreate
                            ? t("shell.newEntryTitle")
                            : t("shell.noPermission")
                    }
                    onClick={() => setMenuOpen((o) => !o)}
                >
                    <span className={s.newEntryPlus}>+</span>
                    {t("shell.newEntry")}
                </button>
            </div>
        </aside>
    );
}
