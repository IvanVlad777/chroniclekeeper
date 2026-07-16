import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../hooks/useAuth";
import { ThemeDots } from "./ThemeDots";
import { LangToggle } from "./LangToggle";
import s from "./shell.module.css";

interface TopBarProps {
    onOpenPalette: () => void;
}

/** True on Mac-like platforms, where the palette shortcut is ⌘K not Ctrl+K. */
const isMac =
    typeof navigator !== "undefined" &&
    /Mac|iPhone|iPad|iPod/.test(navigator.platform || navigator.userAgent);

export function TopBar({ onOpenPalette }: TopBarProps) {
    const { t } = useTranslation();
    const { userInfo, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login");
    };

    return (
        <header className={s.topbar}>
            <div className={s.logo}>
                <span className={s.logoGlyph}>❦</span>
                <span className={s.logoText}>
                    Chronicle<span className={s.logoAccent}>Keeper</span>
                </span>
            </div>
            <button
                type="button"
                className={s.searchBox}
                onClick={onOpenPalette}
                title={t("shell.searchPlaceholder")}
            >
                <span className={s.searchGlyph} aria-hidden="true">
                    ⌕
                </span>
                <span className={s.searchText}>
                    {t("shell.searchPlaceholder")}
                </span>
                <span className={s.searchKbd}>{isMac ? "⌘K" : "Ctrl K"}</span>
            </button>
            <div className={s.spacer} />
            <ThemeDots />
            <span className={s.divider} />
            <LangToggle />
            <span className={s.divider} />
            <div className={s.user}>
                <span className={s.avatar}>
                    {userInfo?.email?.charAt(0) ?? "?"}
                </span>
                <span className={s.userName}>{userInfo?.email}</span>
                <button
                    type="button"
                    className={s.logoutBtn}
                    title={t("logout")}
                    aria-label={t("logout")}
                    onClick={handleLogout}
                >
                    ↷
                </button>
            </div>
        </header>
    );
}
