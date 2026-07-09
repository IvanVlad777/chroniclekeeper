# chroniclekeeper-client

React + TypeScript + Vite frontend for ChronicleKeeper (worldbuilding app: characters, locations, factions, timelines, etc., organized per world). Talks to `ChronicleKeeperAPI` (see root `../CLAUDE.md` for the domain model and backend conventions).

## i18n — mandatory for every new piece of UI text

This app ships in English and Croatian via `i18next` / `react-i18next` (`src/i18n/index.ts`).

- **Any new user-facing text must be added to both `en` and `hr` locale files.** Never hardcode a visible string in a component — no exceptions for "temporary" or placeholder text.
- This includes labels, buttons, headings, empty/error/loading states, form field labels, validation messages, tooltips, and `aria-label`/`title` attributes users can see or have read to them.
- It does **not** include developer-only output: `console.log`/`console.error` messages, code comments, or internal variable/prop names. Those stay in English, untranslated.
- Locales live in `src/i18n/locales/{en,hr}/<namespace>.json`. Namespaces map to feature areas (`common`, `auth`, `overview`, `character`, ...). One JSON file per namespace per language, same keys in both languages.
- If you add a new namespace, register it in **both** the `resources` map and the `ns` array in `src/i18n/index.ts`, and create both the `en` and `hr` JSON files.
- Use `useTranslation("<namespace>")` and `t("key")`; reach into another namespace with `t("key", { ns: "common" })` (see `CharacterList.tsx` for the pattern of pulling shared empty-state copy from `common`).
- When adding a Croatian string, use natural Croatian, not a literal machine translation of the English copy.

## UI building blocks — reuse the ornate design system

`src/components/ornate/` (`Button`, `OrnateTextInput`, `OrnateTextArea`, `OrnateSelect`, `OrnateMultiSelect`, `OrnateField`, `OrnateDisplayBox`, `DataTable`, `StatusPill`, `Tag`, exported via `ornate/index.ts`) and `src/components/feedback/` (`EmptyState`, `ErrorState`, `LoadingSkeleton`) are the required building blocks for new screens.

- Don't drop in raw `<input>`, `<select>`, `<textarea>`, or a hand-rolled table when an ornate equivalent exists — extend the ornate component if it's missing a variant, rather than bypassing it.
- List views use `DataTable` with a `columns` array (see `CharacterList.tsx`); detail views use `OrnateDisplayBox`/`OrnateField` (see `CharacterDetails.tsx`).
- Loading/empty/error states go through `LoadingSkeleton` / `EmptyState` / `ErrorState`, not ad hoc spinners or conditionals.
- Theming is CSS-variable based (`themes.css`, `ornate.vars.css`, `theme/theme.ts`) with `data-theme`/`data-mode` on `<html>`; don't hardcode colors in component styles.

## Structure & conventions

- Feature UI lives under `src/components/entityViews/<entity>/{list,detail,form}/` (e.g. `character/list/CharacterList.tsx`, `character/detail/CharacterDetails.tsx`, `character/form/CharacterForm.tsx`), each with its own `styles.module.css`. All entity verticals are built (character, location, faction, species, timeline, tag, note); follow the `character` vertical as the template when adding a new one (`navConfig.ts` holds the sidebar nav; use `disabled: true` for entries whose vertical isn't built yet). Cross-entity tag attach/detach goes through the shared `src/components/tagging/TagEditor.tsx`.
- API calls: one file per entity under `src/api/` (`characters.ts`, `worlds.ts`, `auth.ts`), using the shared `axios` instance in `src/services/api.ts` (handles the JWT bearer header and 401/403 handling — don't create a second axios instance).
- Types mirroring backend DTOs go in `src/interfaces/loreInterfaces.ts` (lore entities) or `src/interfaces/authInterfaces.ts` (auth) — keep field names/casing identical to the C# DTOs (`ChronicleKeeper.Core/DTOs`) since these are hand-kept in sync, not generated.
- CSS Modules only (`*.module.css` + a `cx()` helper), no inline `style={}` and no global class soup.
- World scoping: most data is per-`World`; read the selected world from `useWorld()` (`src/hooks/useWorld.ts` / `WorldContext`), and handle the "no world selected" and "world still loading" states explicitly (see `CharacterList.tsx`).

## Build & run

```bash
cd chroniclekeeper-client
npm run dev       # Vite dev server
npm run build      # tsc -b && vite build
npm run lint
```

There is no test suite configured yet (no test script/framework in `package.json`) — don't invent one unprompted.
