# chroniclekeeper-client

React + TypeScript + Vite frontend for ChronicleKeeper (worldbuilding app: characters, locations, factions, timelines, tags, notes — organized per world). Talks to `ChronicleKeeperAPI` — see root `../CLAUDE.md` for the domain model, API conventions, auth matrix and backend recipes. Visual source of truth for screens: the `../design/` folder (Grimoire mockups + original ornate component sources).

## Build, run, verify

```bash
npm run dev        # Vite dev server on :5173, proxies /api → http://localhost:5274
npm run build      # tsc -b && vite build — must stay clean
npm run lint
```

- **Check what's already running before starting servers** (Ivan usually has them up): a request to `http://localhost:5173` and `http://localhost:5173/api/...` tells you.
- **No test suite, by decision** — don't add Vitest/Jest unprompted. Nontrivial changes are verified by driving the real app: headless Edge + `playwright-core` per the recipe in `../.claude/skills/verify/SKILL.md`. Create test data in a throwaway world and delete it afterwards; keep "Demo World" pristine.
- Every phase of work ends with a clean `npm run build` + `npm run lint`.

## i18n — mandatory for every piece of UI text

This app ships in English and Croatian via `i18next` / `react-i18next` (`src/i18n/index.ts`).

- **Any user-facing text must be added to both `en` and `hr` locale files.** Never hardcode a visible string in a component — no exceptions for "temporary" or placeholder text.
- This includes labels, buttons, headings, empty/error/loading states, form field labels, validation messages, tooltips, and `aria-label`/`title` attributes users can see or have read to them.
- It does **not** include developer-only output: `console.log`/`console.error` messages, code comments, or identifiers. Those are English, untranslated.
- Locales live in `src/i18n/locales/{en,hr}/<namespace>.json`. One namespace per feature area (`common`, `auth`, `overview`, and one per entity: `character`, `location`, `faction`, `species`, `timeline`, `tag`, `note`). Same keys in both languages.
- New namespace → register it in **both** the `resources` map and the `ns` array in `src/i18n/index.ts`, and create both JSON files.
- Use `useTranslation("<namespace>")` and `t("key")`; reach into another namespace with `t("key", { ns: "common" })` (shared empty/error-state copy lives in `common.states.*`, tag-editor copy in `common.tagEditor.*`).
- **Enum values get label maps** (`types.*`, `relTypes.*` …) — the API sends enums as strings; never show the raw enum member.
- Croatian strings are natural Croatian, not literal machine translations.

## UI building blocks — reuse the ornate design system

`src/components/ornate/` (`Button`, `OrnateTextInput`, `OrnateTextArea`, `OrnateSelect`, `OrnateMultiSelect`, `OrnateCheckbox`, `OrnateField`, `OrnateDisplayBox`/`DisplayGrid`, `DataTable`, `StatusPill`, `Tag` — exported via `ornate/index.ts`), `src/components/feedback/` (`EmptyState`, `ErrorState`, `LoadingSkeleton`) and `src/components/tagging/TagEditor.tsx` are the required building blocks.

- Don't drop in raw `<input>`, `<select>`, `<textarea>`, or a hand-rolled table when an ornate equivalent exists — extend the ornate component if it's missing a variant, rather than bypassing it.
- List views: `DataTable` with a `columns` array (see `CharacterList.tsx`). Detail views: `OrnateDisplayBox`/`DisplayGrid` + section headers. Forms: `OrnateField` wrapping a control (it wires ids/aria automatically).
- Loading/empty/error states go through `LoadingSkeleton` / `EmptyState` / `ErrorState`, never ad hoc spinners.
- Tag attach/detach on any entity detail goes through the shared `TagEditor` (`targetType`: `"Character" | "Location" | "Faction"`) — don't reimplement it.
- Theming is CSS-variable based: `themes.css` (11 `--color-*` tokens, 5 palettes × day/night selected via `data-theme`/`data-mode` on `<html>`) + `ornate.vars.css` (geometry/fonts). `theme/theme.ts` owns switching + localStorage (`"stone-night"` format); an inline script in `index.html` applies the saved theme pre-paint (FOUC guard). **Never hardcode colors** — every color in component CSS is a `var(--color-*)`; all 10 themes must keep working.
- CSS Modules only (`*.module.css` per component), no inline `style={}`. Global classes come from `ornate.vars.css` (`.ornateLabel` — note `:global()` syntax does NOT work outside `*.module.css` files; use `composes: ornateLabel from global` inside modules).

## Data layer

- One API file per entity under `src/api/` (`characters.ts`, `locations.ts`, `factions.ts`, `species.ts`, `timelines.ts`, `tags.ts`, `notes.ts`, `worlds.ts`, `auth.ts`), all through the shared axios instance in `src/services/api.ts` (JWT bearer header + 401/403 handling — never create a second axios instance).
- Types mirroring backend DTOs live in `src/interfaces/loreInterfaces.ts` (lore) and `authInterfaces.ts` (auth) — field names/casing identical to the C# DTOs (`ChronicleKeeper.Core/DTOs`), hand-kept in sync. Enums are string-union types (`locationTypes`, `factionTypes`, `relationshipTypes` const arrays + derived types).
- API error → user message goes through `apiErrorMessage()` in `src/utils/apiError.ts` (handles ModelState, ProblemDetails, Identity arrays, DomainValidationException). Don't parse error shapes inline.
- **Direction: TanStack Query.** The current code uses a hand-rolled pattern (`useEffect` + `cancelled` flag + `reloadKey` state for refetch). When adding new data fetching, prefer `@tanstack/react-query` (`useQuery`/`useMutation`, query keys scoped by `worldId`) — install it on first use — and migrate existing screens opportunistically when touching them. Until a screen is migrated, follow its existing pattern consistently.
- World scoping: most data is per-`World`. Read the selected world from `useWorld()` (`WorldContext`, sessionStorage-persisted selection) and handle "no world selected" and "world still loading" explicitly (see any list view).

## Structure & the entity-vertical template

Feature UI lives under `src/components/entityViews/<entity>/{list,detail,form}/`, each folder with its own `styles.module.css`. All core verticals are built; **the `character` vertical is the template** for any new one. A complete vertical means:

1. Types in `loreInterfaces.ts` (Dto / DetailsDto / CreateDto / UpdateDto + enum unions).
2. `src/api/<entity>.ts` (CRUD + sub-resources).
3. `list/` — `DataTable` with search/sort, "+ New" action (role-gated), empty state with the same action.
4. `detail/` — Grimoire layout per the mockup in `../design/`, links to related entities, `TagEditor` where applicable, Edit button (role-gated).
5. `form/` — one component for both `/new` and `/:id/edit` (mode from `useParams`), delete button with `window.confirm` (role-gated), errors via `apiErrorMessage`.
6. Routes in `AppRoutes.tsx` under `/storymap/<entity>` — **`/new` before `/:id`**, plus `/:id/edit`.
7. Sidebar entry in `shell/navConfig.ts` (use `disabled: true` until the vertical exists) and, if creatable, an item in the Sidebar "+ New entry" menu.
8. i18n namespace (en + hr) registered in `src/i18n/index.ts`.

Role gating mirrors the API (see root CLAUDE.md matrix): `editorRoles = ["Editor", "Admin", "SuperAdmin"]` for create/edit/sub-resources; Character **delete** additionally requires Admin/SuperAdmin. Roles come from `useAuth().userInfo.roles`. Readers see a fully read-only app — hide mutating buttons, don't just disable the API call.

## Known gaps / deferred (don't "fix" casually)

- Tags page has no "entries with this tag" panel — needs a backend `GET /tags/{id}/attachments` endpoint first.
- `CharacterForm` does POST-then-PUT on create because `CharacterCreateDto` is a subset — goes away when the backend normalizes the Create DTO (see root CLAUDE.md).
- Notes/Tags selection is local state (no deep links) — intentional simplification.
- Guest/public world viewing is deferred until the core is complete (worlds are private per owner today).
- Family tree view and mobile drawer exist as mockups in `../design/` but aren't built.
- Google Fonts load from CDN (fine for local dev); self-hosting via @fontsource is a deploy-time concern.
