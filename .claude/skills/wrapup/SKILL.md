---
name: wrapup
description: After implementation work (new entity, endpoint, UI vertical...) — build, migrate, frontend build/lint, E2E-drive the real app in a throwaway world, clean up, then propose a commit message. Use before reporting work as done.
---

# ChronicleKeeper — Wrapup: verify, test, propose commit

This is the standard post-implementation pass. Goal: don't ask the user to approve each
individual verification step — the steps below are pre-authorized because they're local,
reversible, and don't touch anything important (no real user data, no commits, no pushes).
Only ask when a step falls outside this scope.

## Pre-authorized — do these without asking

- `dotnet build ChronicleKeeperAPI/ChronicleKeeperAPI.csproj` (never the `.sln`)
- `dotnet ef migrations add <Name> --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI`
  and `dotnet ef database update ...` — against the local dev DB in Docker
- `npm run build` / `npm run lint` in `chroniclekeeper-client`
- Checking/starting/stopping **local dev** processes (API `dotnet run --project ChronicleKeeperAPI`,
  client `npm run dev`) when actually needed (e.g. a DLL lock is blocking a build) — check what's
  already running first (`docker ps`, curl the ports), only stop/start what's necessary
- Creating a throwaway world via the UI or API, driving the new feature through it end-to-end
  (headless Edge + playwright-core, see `.claude/skills/verify/SKILL.md`), then deleting that
  world and everything in it afterward
- Reading logs, read-only checks (`curl`, `docker ps`, `git status`, `git diff`)

## Still requires confirmation — never do these silently

- Anything touching "Demo World" or other real/seed data
- `git commit`, `git push`, or any git write operation
- Destructive operations (`git reset --hard`, force-push, dropping the dev DB, `docker compose down -v`)
- Installing/upgrading packages beyond what the feature strictly needs
- Anything outside the local dev environment

## Procedure

1. **Backend build** — `dotnet build ChronicleKeeperAPI/ChronicleKeeperAPI.csproj`. If it fails
   only with `MSB3027`/`MSB3021` (file lock), the API dev server is holding the DLLs — stop it,
   build, remember to bring it back up in step 5.
2. **Migration** (only if the entity model changed) — `dotnet ef migrations add <Name> ...`,
   review the generated `Up()`/`Down()` (DeleteBehavior must match the rules in root `CLAUDE.md`),
   then `dotnet ef database update ...`.
3. **Frontend build + lint** — `npm run build` and `npm run lint` in `chroniclekeeper-client`,
   must come back clean.
4. **Review the diff before trusting it** — `git status` / `git diff --stat` across both halves
   of the repo; skim new/changed files to confirm they follow established patterns (DTO shape,
   DeleteBehavior, i18n coverage, role gating) — especially if a subagent wrote the code.
5. **Bring dev servers back up** — `docker ps` must show `chroniclekeeper-db` up, the API must
   respond on :5274, the client on :5173 (start whichever isn't running).
6. **Drive the real app** (headless Edge + playwright-core; login/harness mechanics are in
   `.claude/skills/verify/SKILL.md`):
   - Log in as `superadmin@chroniclekeeper.com` / `SuperAdmin@123`.
   - Create a throwaway world (distinctive name, e.g. `E2E <feature> <timestamp>`) via the UI
     dialog, then resolve its id via `GET /api/worlds/mine` for later cleanup.
   - Drive the new/changed feature through real UI actions — create/read/update, the specific
     business rule under test (e.g. an in-use delete block) — asserting on rendered page
     content, not just HTTP status codes.
   - **Always clean up**: delete anything created, then `DELETE /api/worlds/{id}` the throwaway
     world via `page.evaluate(fetch(...))` with the session token, and confirm via
     `GET /api/worlds/mine` that only real worlds remain.
7. **Propose a commit message** — end by printing a suggested commit message (1-2 sentences,
   focused on "why" not "what", matching the style of recent commits from `git log`).
   **Never run `git add` or `git commit`** — that's always the user's call.

## Gotchas (collected from real E2E runs)

- Route order: `/new` must come before `/:id` in `AppRoutes.tsx`, or the new route never matches.
- Required-field labels render as `"Label*"` (no space) — `getByLabel("Name", { exact: true })`
  will time out; anchor a regex at the start instead (`/^Name/`) or use substring matching.
- The login submit button is labeled "Enter the archive", not "Sign in" — "Sign in" is only the
  tab label.
- Fields in the world-creation dialog live inside `role="dialog"` — scope locators to it to avoid
  ambiguity when the same label exists elsewhere on the page.
- `admin@chroniclekeeper.com` owns no worlds — useless for data-flow E2E, good only for
  empty-state checks.
- `dotnet build` on the `.sln` breaks the client website project — only build
  `ChronicleKeeperAPI/ChronicleKeeperAPI.csproj`.
