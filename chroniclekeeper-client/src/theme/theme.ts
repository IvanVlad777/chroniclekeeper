import {
  themeList,
  ThemeName,
  ThemeVariant,
  variants,
} from "../types/themeTypes";

export function setTheme(theme: ThemeName, variant: ThemeVariant) {
  const root = document.documentElement;
  const allVariants = themeList.flatMap((th) =>
    variants.map((v) => `theme-${th}-${v}`)
  );

  root.classList.remove(...allVariants);
  const selected = `theme-${theme}-${variant}`;
  root.classList.add(selected);
  localStorage.setItem("theme", selected);
}

export function loadStoredTheme() {
  const saved = localStorage.getItem("theme") || "theme-lush-day";
  document.documentElement.classList.add(saved);
}
