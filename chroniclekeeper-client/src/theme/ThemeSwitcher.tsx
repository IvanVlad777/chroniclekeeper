import { useEffect, useState } from "react";
import { themeList, ThemeName, ThemeVariant } from "../types/themeTypes";
import { loadStoredTheme, setTheme } from "./theme";
import "./theme-switcher.css";

export function ThemeSwitcher() {
    const [currentTheme, setCurrentTheme] = useState<ThemeName>("lush");
    const [mode, setMode] = useState<ThemeVariant>("day");

    useEffect(() => {
        const saved = localStorage.getItem("theme");
        if (saved) {
            const [, themeName, themeVariant] = saved.split("-");
            setCurrentTheme(themeName as ThemeName);
            setMode(themeVariant as ThemeVariant);
        }
        loadStoredTheme();
    }, []);

    const handleThemeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const theme = e.target.value as ThemeName;
        setCurrentTheme(theme);
        setTheme(theme, mode);
    };

    const handleModeToggle = () => {
        const newMode = mode === "day" ? "night" : "day";
        setMode(newMode);
        setTheme(currentTheme, newMode);
    };

    return (
        <div
            className="theme-switcher"
            //className="flex items-center gap-2 text-sm"
        >
            <select
                value={currentTheme}
                onChange={handleThemeChange}
                className="theme-select"
                // className="px-2 py-1 rounded border border-[var(--color-accent)] bg-[var(--color-surface)] text-[var(--color-text)]"
            >
                {themeList.map((theme) => (
                    <option key={theme} value={theme}>
                        {theme.charAt(0).toUpperCase() + theme.slice(1)}
                    </option>
                ))}
            </select>

            <button
                onClick={handleModeToggle}
                className="theme-btn"
                // className="px-3 py-1 rounded bg-[var(--color-accent)] text-white hover:opacity-90 transition"
            >
                {mode === "day" ? "🌙" : "☀️"}
            </button>
        </div>
    );
}

// export function ThemeSwitcher() {
//   const [currentTheme, setCurrentTheme] = useState<ThemeName>("lush");
//   const [mode, setMode] = useState<ThemeVariant>("day");

//   // Učitaj prethodno spremljenu temu na mount
//   useEffect(() => {
//     const saved = localStorage.getItem("theme");
//     if (saved) {
//       const [, themeName, themeVariant] = saved.split("-");
//       setCurrentTheme(themeName as ThemeName);
//       setMode(themeVariant as ThemeVariant);
//     }
//     loadStoredTheme();
//   }, []);

//   // Postavi novu temu
//   const handleThemeChange = (theme: ThemeName) => {
//     setCurrentTheme(theme);
//     setTheme(theme, mode);
//   };

//   // Promijeni između "day" i "night" načina
//   const handleModeToggle = () => {
//     const newMode = mode === "day" ? "night" : "day";
//     setMode(newMode);
//     setTheme(currentTheme, newMode);
//   };

//   return (
//     <div className="space-y-4 p-6 rounded-xl shadow-lg border bg-[var(--color-surface)] text-[var(--color-text)]">
//       <h2 className="text-xl font-semibold">Odaberi temu</h2>
//       <div className="flex flex-wrap gap-2">
//         {themeList.map((theme) => (
//           <button
//             key={theme}
//             onClick={() => handleThemeChange(theme)}
//             className={`
//               px-4 py-2 rounded
//               ${
//                 currentTheme === theme
//                   ? "bg-[var(--color-primary)] text-white"
//                   : "bg-gray-200 text-black"
//               }
//               hover:opacity-80 transition
//             `}
//           >
//             {theme}
//           </button>
//         ))}
//       </div>

//       <div className="pt-4">
//         <button
//           onClick={handleModeToggle}
//           className="px-4 py-2 rounded bg-[var(--color-accent)] text-white hover:opacity-90 transition"
//         >
//           Prebaci na {mode === "day" ? "noćnu" : "dnevnu"} temu
//         </button>
//       </div>

//       <p className="text-sm pt-2 text-[var(--color-text)] opacity-70">
//         Aktivna tema: <strong>{currentTheme}</strong> – <strong>{mode}</strong>
//       </p>
//     </div>
//   );
// }
