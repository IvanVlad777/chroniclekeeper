import { NavLink } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { navEntries } from "./navConfig";
import { WorldSwitcher } from "./WorldSwitcher";
import s from "./shell.module.css";

export function Sidebar() {
    const { t } = useTranslation();

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
                            <span className={s.navSoon}>
                                {t("shell.soon")}
                            </span>
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

            <div className={s.sidebarFooter}>
                <button
                    type="button"
                    className={s.newEntryBtn}
                    disabled
                    title={t("shell.comingSoon")}
                >
                    <span className={s.newEntryPlus}>+</span>
                    {t("shell.newEntry")}
                </button>
            </div>
        </aside>
    );
}
