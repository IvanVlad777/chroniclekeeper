---
name: afk
description: Invoke this the moment a plan is approved and Ivan is stepping away. From invocation to the final commit-message proposal, execute the entire plan with zero questions and zero confirmation prompts — build, implement, migrate, verify, E2E-test, clean up, propose a commit message. Ivan reviews the code afterward; he is not present during execution.
---

# ChronicleKeeper — AFK: unattended execution from "go" to commit message

Ivan's workflow: he participates at the **start** (planning, answering clarifying
questions) and at the **end** (reviewing the finished code). Everything in between — the
actual implementation — he is literally away from the keyboard for. Invoking this skill is
the signal that the "away" stretch has started. **From this point until you print the
final proposed commit message, do not stop for anything that has a reasonable default.**

## The rule

No `AskUserQuestion` calls. No "should I proceed?" pauses. No stopping to report interim
progress and wait. If the plan leaves something unspecified — a field name, a glyph, which
of two reasonable patterns to follow, how to phase a large change — pick the option most
consistent with the existing codebase and keep going. Note the call in your final summary;
don't ask about it mid-flight.

If you hit a build error, a failing migration, a lint failure, a flaky E2E step: fix it and
continue. That is implementation work, not a decision point.

**The only case where you may stop and ask:** the plan itself is impossible, self
contradictory, or depends on information genuinely only Ivan has (a business decision with
no reasonable default, not an engineering judgment call). This should be rare — most things
that feel like judgment calls have a reasonable default; use it.

## Still never do these, even AFK

- `git add` / `git commit` / `git push`, or any git write operation.
- Anything touching real data — "Demo World" or other non-throwaway seed content.
- Destructive operations: `git reset --hard`, force-push, dropping the dev database,
  `docker compose down -v`.
- Installing or upgrading dependencies beyond what the plan strictly needs.
- Anything outside the local dev environment.

These are the only boundary. Everything else — starting/stopping local dev processes,
running builds, creating and applying EF migrations against the local dev DB, creating and
deleting throwaway worlds for testing, running scripts from the scratchpad — is
pre-authorized. Just do it.

## Procedure

1. **Implement the plan.** Follow the established patterns already in the codebase (or the
   specific files/templates the plan names). Work through it fully before moving to
   verification — don't check in partway.
2. **Backend build** — `dotnet build ChronicleKeeperAPI/ChronicleKeeperAPI.csproj` (never
   the `.sln`). If it fails only with `MSB3027`/`MSB3021` (file lock), the API dev server is
   holding the DLLs — stop it, rebuild, remember to bring it back up in step 5.
3. **Migration** (if the entity model changed) — `dotnet ef migrations add <Name>
   --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI`, review
   the generated `Up()`/`Down()` (`DeleteBehavior` must match the rules in root
   `CLAUDE.md`), then `dotnet ef database update ...`. If something's off (e.g. a missing
   `HasMaxLength`), fix the EF config and regenerate the migration before applying — don't
   apply a migration you haven't checked.
4. **Frontend build + lint** — `npm run build` and `npm run lint` in
   `chroniclekeeper-client`, must come back clean.
5. **Bring dev servers up** — `docker ps` must show `chroniclekeeper-db` up, the API must
   respond on :5274, the client on :5173 (start whichever isn't running).
6. **Review the diff before trusting it** — `git status` / `git diff --stat`; skim
   new/changed files for pattern consistency (DTO shape, DeleteBehavior, i18n coverage,
   role gating) — especially anything a subagent wrote.
7. **Drive the real app** (headless Edge + playwright-core; mechanics in
   `.claude/skills/verify/SKILL.md`):
   - Log in as `superadmin@chroniclekeeper.com` / `SuperAdmin@123`.
   - Create a throwaway world (distinctive name, e.g. `E2E <feature> <timestamp>`), resolve
     its id via `GET /api/worlds/mine` for cleanup.
   - Drive the new/changed feature through real UI actions, asserting on rendered page
     content, not just HTTP status.
   - **Always clean up**: delete what you created, `DELETE /api/worlds/{id}` the throwaway
     world, confirm via `GET /api/worlds/mine` that only real worlds remain.
8. **Propose a commit message** — 1-2 sentences, "why" not "what", matching the style of
   recent `git log` entries. **Never run `git add` or `git commit`.** Printing this message
   is the signal that the AFK stretch is over and Ivan's review can begin.

## Gotchas (collected from real runs)

- Route order: `/new` before `/:id` in `AppRoutes.tsx`, or the new route never matches.
- Required-field labels render as `"Label*"` (no space) — `getByLabel("Name", { exact: true })`
  times out; anchor a regex at the start (`/^Name/`) or match the literal `"Name*"`. Watch
  for ambiguous matches too (e.g. a top-nav language-toggle group also named "Language" —
  scope the locator or match the exact rendered text).
- The login submit button is "Enter the archive", not "Sign in" — "Sign in" is only the tab
  label.
- World-creation dialog fields live inside `role="dialog"` — scope locators to it.
- `admin@chroniclekeeper.com` owns no worlds — useless for data-flow E2E, fine for
  empty-state checks.
- `dotnet build` on the `.sln` breaks the client website project — only build
  `ChronicleKeeperAPI/ChronicleKeeperAPI.csproj`.

## On permission prompts

This skill controls *your* behavior — it cannot suppress the harness's own tool-permission
prompts for Bash commands outside the project's allowlist (`.claude/settings.json`). Routine
commands used in this procedure (`dotnet build/ef/run`, `npm run`, `node`, `docker ps`,
`curl`, `git status/diff/log`, `tasklist`/`taskkill` for local dev processes) are already
allowlisted there. If Ivan wants genuinely zero interruptions for a specific AFK stretch —
including prompts for a Bash pattern not yet covered — that's a session permission-mode
choice on his end (switching to a more permissive mode before stepping away), not something
this skill file can grant.
