export const themeList = ["ice", "forest", "lush", "sand", "stone"] as const;
export type ThemeName = (typeof themeList)[number];

export const variants = ["day", "night"] as const;
export type ThemeVariant = (typeof variants)[number];
