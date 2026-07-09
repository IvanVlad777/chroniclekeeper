import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { themeList, ThemeName, ThemeVariant } from "../../types/themeTypes";
import { loadStoredTheme, setTheme } from "../../theme/theme";
import s from "./shell.module.css";

/** Reprezentativna boja svake palete (iz Shell mockupa) — redoslijed prati themeList. */
const dotColors: Record<ThemeName, string> = {
    ice: "#8FB4C9",
    forest: "#6E8E63",
    lush: "#9C6B8E",
    sand: "#C6A36B",
    stone: "#8C8378",
};

export function ThemeDots() {
    const { t } = useTranslation();
    const [theme, setThemeState] = useState<ThemeName>("stone");
    const [mode, setMode] = useState<ThemeVariant>("night");

    useEffect(() => {
        const stored = loadStoredTheme();
        setThemeState(stored.theme);
        setMode(stored.mode);
    }, []);

    const pickTheme = (next: ThemeName) => {
        setThemeState(next);
        setTheme(next, mode);
    };

    const toggleMode = () => {
        const next: ThemeVariant = mode === "day" ? "night" : "day";
        setMode(next);
        setTheme(theme, next);
    };

    return (
        <div className={s.themeDots}>
            {themeList.map((name) => (
                <button
                    key={name}
                    type="button"
                    className={`${s.themeDot} ${
                        name === theme ? s.themeDotActive : ""
                    }`}
                    style={{ background: dotColors[name] }}
                    title={t(`theme.${name}`)}
                    aria-label={t(`theme.${name}`)}
                    aria-pressed={name === theme}
                    onClick={() => pickTheme(name)}
                />
            ))}
            <button
                type="button"
                className={s.modeBtn}
                title={mode === "day" ? t("theme.toNight") : t("theme.toDay")}
                aria-label={
                    mode === "day" ? t("theme.toNight") : t("theme.toDay")
                }
                onClick={toggleMode}
            >
                {mode === "day" ? "☾" : "☀"}
            </button>
        </div>
    );
}
