import {
  themeList,
  ThemeName,
  ThemeVariant,
  variants,
} from "../types/themeTypes";

const DEFAULT_THEME: ThemeName = "stone";
const DEFAULT_MODE: ThemeVariant = "night";

export function setTheme(theme: ThemeName, mode: ThemeVariant) {
  document.documentElement.dataset.theme = theme;
  document.documentElement.dataset.mode = mode;
  localStorage.setItem("theme", `${theme}-${mode}`);
}

/**
 * Parsira spremljenu temu (podržava i legacy "theme-lush-day" format),
 * primjenjuje je na <html> i vraća vrijednosti za UI state.
 */
export function loadStoredTheme(): { theme: ThemeName; mode: ThemeVariant } {
  const saved = localStorage.getItem("theme") ?? "";
  const [theme, mode] = saved.replace(/^theme-/, "").split("-");

  const validTheme = themeList.includes(theme as ThemeName)
    ? (theme as ThemeName)
    : DEFAULT_THEME;
  const validMode = variants.includes(mode as ThemeVariant)
    ? (mode as ThemeVariant)
    : DEFAULT_MODE;

  setTheme(validTheme, validMode);
  return { theme: validTheme, mode: validMode };
}
