# ChronicleKeeper

Worldbuilding app for writers: characters, locations, factions, timelines, tags, notes and other lore, organized per world. ASP.NET Core API + React client. The frontend has its own instructions in `chroniclekeeper-client/CLAUDE.md` — read that one for any client work.

## CRITICAL: Vision and "dead code"

**Every entity class in `ChronicleKeeper.Core\Entities\` (~119 files) is part of the long-term plan.**
Classes not mapped in EF and navigations that are commented out (`// TODO: Uncomment when X entity is revived`) are **NOT dead code and must NEVER be deleted or moved** — they are a roadmap that gets revived gradually, phase by phase. Commented-out code = future functionality.

Rough phases (order is flexible — the project owner decides per phase, don't assume a sequence):

1. **Core (implemented, full stack)**: World, Character (+ family, CharacterRelationship), Location, Faction (+ FactionMember), SapientSpecies/Race, Timeline/TimelineEvent, Tag, Note
2. Social: Nation, Religion, SocialClass, Culture, Language…
3. Professions / Education
4. Equipment / Items, Abilities
5. Content (Book, Chapter…), History (+ link to Timeline)
6. Geography details: Creature hierarchy (SapientSpecies re-parents onto Creature then), Ecosystems, Climate…

## Architecture

```
ChronicleKeeper.Core            → domain: Entities, Enums, DTOs, CQRS (MediatR), repository interfaces, Exceptions
ChronicleKeeper.Infrastructure  → EF Core: ApplicationDbContext, Configurations, Repositories, Migrations, DbSeeder
ChronicleKeeperAPI              → ASP.NET Core: Controllers, ServiceExtensions, Mapping (AutoMapper), Middleware
chroniclekeeper-client          → React + TypeScript + Vite (see its own CLAUDE.md)
design/                         → exported Claude Design project ("Grimoire") — mockups + component sources;
                                  the visual source of truth for new screens. Never build a screen blind if a
                                  mockup exists here.
```

- **Code-first EF Core + migrations, SQL Server only** (Docker mssql 2022). No database-first scaffolding — it would overwrite hand-written classes. No Npgsql.
- Schema is applied via `MigrateAsync()` in `Program.cs` (NOT `EnsureCreated`).
- Auth: ASP.NET Identity + JWT; roles: SuperAdmin, Admin, Editor, Moderator, Reader (seeded in `DbSeeder`).

## Dev environment

**Long-term goal: everything (db + API + client) runs in Docker via compose.** That setup is not fully wired yet and is not a current priority. Today's working setup:

- Database: Docker container `chroniclekeeper-db` (`docker compose up -d db`), SQL Server on `localhost,1433`.
- API: `dotnet run --project ChronicleKeeperAPI` → `http://localhost:5274` (Swagger at root). A Docker API variant may exist on :5000 — the Vite proxy targets **5274**.
- Client: `npm run dev` in `chroniclekeeper-client` → `http://localhost:5173`, proxies `/api` → `:5274`.

**Before starting anything, check what is already running** (Ivan often has servers up): `docker ps`, then a quick request to `http://localhost:5173/api/...` or `http://localhost:5274`. Don't kill or restart services that are already serving.

Dev databases are disposable: databases created by the old `EnsureCreated` code (e.g. local SQLEXPRESS) have no `__EFMigrationsHistory` and crash `MigrateAsync` ("There is already an object named…") — drop them (or `docker compose down -v`) and let migrations recreate.

## Build & commands

```bash
# Build — NEVER dotnet build on the .sln (the client website project breaks it)
dotnet build ChronicleKeeperAPI/ChronicleKeeperAPI.csproj

# Migrations (never pass --nologo to dotnet ef)
dotnet ef migrations add <Name> --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI
dotnet ef database update --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI
```

Seed users: `superadmin@chroniclekeeper.com` / `SuperAdmin@123` (owns the seed "Demo World"), `admin@chroniclekeeper.com` / `Admin@123` (owns **no** worlds — good for empty states, useless for data flows). Seed world: "Demo World".

## Data model conventions

- All lore entities inherit **`LoreEntity`** (`Core\Entities\Base\LoreEntity.cs`): Id, Name, Description, **WorldId**, CreatedAt/UpdatedAt (timestamps are stamped by `ApplicationDbContext.SaveChangesAsync` — never set them manually).
- **`World` is the aggregate root** (multi-world per user, `OwnerId` → AspNetUsers). World intentionally has NO collection navigations.
- **Join entities** (CharacterRelationship, FactionMember, CharacterTag/LocationTag/FactionTag) are NOT LoreEntity — no WorldId (their world is implied through the parent).
- Enums are stored in the DB **as strings** (`HasConversion<string>().HasMaxLength(30)`) and serialized in the API **as strings** (global `JsonStringEnumConverter` in `Program.cs`). The client mirrors them as string-union types.
- Fictional dates (TimelineEvent.Date) are **string + SortOrder**, not DateTime.
- Note: `FactionType` has no "Guild" member — use TradeConsortium etc.

### DeleteBehavior rules (SQL Server — avoiding multiple cascade paths!)

- **The `WorldId` FK is ALWAYS `Restrict`.** Deleting a world goes exclusively through `WorldRepository.DeleteAsync` (ordered ExecuteDelete sequence in a transaction). This is the foundation that kills every cascade diamond — do not change it.
- Self-referencing FKs (Character.FatherId/MotherId, Location.ParentLocationId) = `Restrict` + the repository nulls/cleans them before delete (see `CharacterRepository.DeleteAsync`).
- Two FKs to the same table: only one may Cascade (CharacterRelationship: CharacterId=Cascade, RelatedCharacterId=Restrict).
- "In use" FKs (Character.RaceId/SapientSpeciesId) = `Restrict` → friendly `DomainValidationException` in handlers.
- Join tables may Cascade from both sides (the parents share no cascading ancestor).
- Pointer-only references (Faction.LeaderId, HeadquartersId) = `SetNull`.

## API conventions

- REST routes: `api/<plural>` (`api/characters`, `api/species` + `api/races` share one controller file). Sub-resources: `POST/DELETE api/characters/{id}/relationships`, `api/factions/{id}/members`, `api/tags/{id}/attachments/{targetType}/{targetId}`, `api/timelines/{id}/events` (+ `PUT/DELETE api/timelines/events/{eventId}`).
- DTO pattern per entity: `XDto` (list), `XDetailsDto` (detail with related `ReferenceDto`s), `XCreateDto`, `XUpdateDto` (usually `Create` minus `WorldId`, via inheritance or `Omit`).
- **Create DTOs take the full field set** — everything `Update` accepts (minus id/world derivation). Known violation: `CharacterCreateDto` only takes basic fields, which forces the client into a POST-then-PUT dance; **normalize it the next time character DTOs are touched**, then remove the follow-up PUT in `CharacterForm.tsx`.
- PUT is **full replace** — omitted fields reset. Accepted behavior; clients must send complete payloads.
- Validation errors come back as `BadRequest(ModelState)`; Identity errors as a plain array `[{code, description}]`; domain rules as `DomainValidationException`. The client parses all of these in `src/utils/apiError.ts` — reuse those shapes, don't invent new error formats.
- Authorization matrix (mirror it in client role gating):
  - read = any authenticated user; `GET /api/worlds` (all worlds) is Admin/SuperAdmin-only, regular users use `/api/worlds/mine`
  - create/update + sub-resource add/remove = Editor, Admin, SuperAdmin
  - delete = Editor+ for everything **except Character delete (Admin/SuperAdmin only)**
  - ownership/tenancy: non-owners get 403 on other users' worlds; cross-world references are rejected. Known gap: per-entity world-ownership is not yet enforced as a pipeline behavior (only World write ops check ownership).

## Recipe: reviving a new entity (template)

1. In the entity file: inherit `LoreEntity`, uncomment/add navigations **only toward already-mapped types**; leave navigations to still-dormant types commented with `// TODO: Uncomment when X entity is revived`.
2. Add `Infrastructure\Configurations\XConfiguration.cs : LoreEntityConfiguration<X>` — decide DeleteBehavior per the rules above, max lengths, indexes.
3. Add `DbSet<X>` to `ApplicationDbContext`.
4. `dotnet ef migrations add AddX --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI`
5. **Review the generated migration** — every `onDelete:` must be a conscious decision.
6. If the entity participates in world deletion, add it in the right position in `WorldRepository.DeleteAsync`.
7. API layer per the Character template: DTOs (`Core\DTOs\X\`) → CQRS (`Core\CQRS\Xs\{Commands,Queries,Handlers}`) → repository (interface in Core, impl. in Infrastructure, registration in `DbSetup.cs`) → AutoMapper profile (`ChronicleKeeperAPI\Mapping\`) → controller. MediatR/AutoMapper register automatically (assembly scanning).
8. Client vertical per `chroniclekeeper-client/CLAUDE.md` (the character vertical is the template).

## Workflow & style

- **Code comments and identifiers in English.** Older Croatian comments exist — translate them opportunistically when you touch a file, don't do a bulk sweep. UI text is never hardcoded — it goes through i18n (en + hr, see client CLAUDE.md). Conversation with Ivan is in Croatian.
- **Ivan commits himself.** Never commit or push unless he explicitly asks. Leave the working tree clean and report what changed.
- **No test framework, by decision.** Don't add xUnit/NUnit/Vitest unprompted. Changes are verified against the running app: build clean + drive the real UI/API (see `.claude/skills/verify/SKILL.md` for the E2E recipe with headless Edge + playwright-core). If you create test data, clean it up (create a temp world, delete it afterwards) — keep "Demo World" pristine.
- Known NuGet vulnerability warnings (AutoMapper 12.0.1, Microsoft.AspNetCore.Identity 2.3.0) are acknowledged — upgrade when convenient, don't block on them.
- Deployment: none yet, local development only. Don't introduce production concerns (secrets management, CORS hardening, CI) unprompted.
