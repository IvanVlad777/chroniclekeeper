import { NavLink } from "react-router-dom";
import { useTranslation } from "react-i18next";
import type { NavDomain } from "./navConfig";
import { WorldSwitcher } from "./WorldSwitcher";
import { NewEntryMenu } from "./NewEntryMenu";
import { useEntryCounts } from "./useEntryCounts";
import { useAuth } from "../../hooks/useAuth";
import { editorRoles } from "./roles";
import s from "./shell.module.css";

interface DomainPanelProps {
    domain: NavDomain;
}

/** The 236px panel: world switcher, the active domain's entries, "+ New entry". */
export function DomainPanel({ domain }: DomainPanelProps) {
    const { t } = useTranslation();
    const { userInfo } = useAuth();
    const counts = useEntryCounts(domain.entries);

    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    return (
        <aside className={s.panel}>
            <div className={s.worldSection}>
                <WorldSwitcher />
            </div>

            <div className={s.ornament}>
                <span className={s.ornamentLine} />
                <span className={s.ornamentStar}>✦</span>
                <span className={s.ornamentLine} />
            </div>

            <div className={s.domainTitle}>{t(`navGroups.${domain.key}`)}</div>

            <nav className={s.nav}>
                {domain.entries.map((entry) => {
                    const count = counts[entry.key];
                    return (
                        <NavLink
                            key={entry.key}
                            to={entry.route}
                            end={entry.end}
                            className={({ isActive }) =>
                                `${s.navItem} ${isActive ? s.navItemActive : ""}`
                            }
                        >
                            <span className={s.navItemLabel}>
                                {t(`nav.${entry.key}`)}
                            </span>
                            {count != null && (
                                <span className={s.navCount}>{count}</span>
                            )}
                        </NavLink>
                    );
                })}
            </nav>

            {canCreate && (
                <div className={s.panelFooter}>
                    <NewEntryMenu />
                </div>
            )}
        </aside>
    );
}
