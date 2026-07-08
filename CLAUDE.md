# ChronicleKeeper

Worldbuilding aplikacija za pisce: praćenje likova, lokacija, frakcija, vremenskih linija i ostalog lorea, organizirano po svjetovima.

## VAŽNO: Vizija i "mrtvi kod"

**Sve entity klase u `ChronicleKeeper.Core\Entities\` (~119 datoteka) dio su dugoročnog plana.**
Klase koje nisu mapirane u EF i navigacije koje su zakomentirane (`// TODO: Uncomment when X entity is revived`) **NISU mrtvi kod i NIKAD se ne brišu niti premještaju** — to je roadmap koji se oživljava postupno, faza po faza. Zakomentirani kod = buduća funkcionalnost.

Okvirne faze (redoslijed fleksibilan, po želji vlasnika projekta):

1. **Jezgra (implementirano)**: World, Character (+ obitelj, CharacterRelationship), Location, Faction (+ FactionMember), SapientSpecies/Race, Timeline/TimelineEvent, Tag, Note
2. Social: Nation, Religion, SocialClass, Culture, Language…
3. Professions / Education
4. Equipment / Items, Abilities
5. Content (Book, Chapter…), History (+ veza s Timeline)
6. Geography detalji: Creature hijerarhija (SapientSpecies se tada re-parenta na Creature), Ecosystems, Climate…

## Arhitektura

```
ChronicleKeeper.Core            → domena: Entities, Enums, DTOs, CQRS (MediatR), repository sučelja, Exceptions
ChronicleKeeper.Infrastructure  → EF Core: ApplicationDbContext, Configurations, Repositories, Migrations, DbSeeder
ChronicleKeeperAPI              → ASP.NET Core: Controllers, ServiceExtensions, Mapping (AutoMapper), Middleware
chroniclekeeper-client          → React + TypeScript + Vite
```

- **Code-first EF Core + migracije, samo SQL Server** (Docker mssql 2022). Nema database-first scaffolda — pregazio bi ručno pisane klase. Nema Npgsql-a.
- Shema se primjenjuje kroz `MigrateAsync()` u `Program.cs` (NE `EnsureCreated`).
- Auth: ASP.NET Identity + JWT; role: SuperAdmin, Admin, Editor, Moderator, Reader (seed u `DbSeeder`).

## Konvencije data modela

- Svi lore entiteti nasljeđuju **`LoreEntity`** (`Core\Entities\Base\LoreEntity.cs`): Id, Name, Description, **WorldId**, CreatedAt/UpdatedAt (timestampove stampa `ApplicationDbContext.SaveChangesAsync` — ne postavljati ručno).
- **`World` je aggregate root** (multi-world po korisniku, `OwnerId` → AspNetUsers). World namjerno NEMA kolekcijske navigacije.
- **Join entiteti** (CharacterRelationship, FactionMember, CharacterTag/LocationTag/FactionTag) NISU LoreEntity — nemaju WorldId (svijet im je impliciran kroz roditelja).
- Enumi se u bazu spremaju **kao string** (`HasConversion<string>().HasMaxLength(30)`).
- Fiktivni datumi (TimelineEvent.Date) su **string + SortOrder**, ne DateTime.

### DeleteBehavior pravila (SQL Server — izbjegavanje multiple cascade paths!)

- **`WorldId` FK je UVIJEK `Restrict`.** Brisanje svijeta ide isključivo kroz `WorldRepository.DeleteAsync` (uređeni ExecuteDelete slijed u transakciji). Ovo je temelj koji ubija sve cascade dijamante — ne mijenjati.
- Self-referencing FK (Character.FatherId/MotherId, Location.ParentLocationId) = `Restrict` + repository prije brisanja nulira/čisti (vidi `CharacterRepository.DeleteAsync`).
- Dva FK-a prema istoj tablici: samo jedan smije biti Cascade (CharacterRelationship: CharacterId=Cascade, RelatedCharacterId=Restrict).
- FK-ovi "u upotrebi" (Character.RaceId/SapientSpeciesId) = `Restrict` → friendly `DomainValidationException` u handlerima.
- Join tablice smiju Cascade s obje strane (roditelji nemaju zajedničkog kaskadirajućeg pretka).
- Reference koje samo "pokazuju" (Faction.LeaderId, HeadquartersId) = `SetNull`.

## Recept: oživljavanje novog entiteta (šablona)

1. U entity datoteci: naslijedi `LoreEntity`, odkomentiraj/dodaj veze **samo prema već mapiranim tipovima**; veze prema još-mrtvim tipovima ostavi zakomentirane s `// TODO: Uncomment when X entity is revived`.
2. Dodaj `Infrastructure\Configurations\XConfiguration.cs : LoreEntityConfiguration<X>` — odluči DeleteBehavior po pravilima gore, max-lengthove, indexe.
3. Dodaj `DbSet<X>` u `ApplicationDbContext`.
4. `dotnet ef migrations add AddX --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI`
5. **Pregledaj generiranu migraciju** — svaki `onDelete:` mora biti svjesna odluka.
6. Ako entitet sudjeluje u brisanju svijeta, dodaj ga na pravo mjesto u `WorldRepository.DeleteAsync`.
7. API sloj po Character šabloni: DTOs (`Core\DTOs\X\`) → CQRS (`Core\CQRS\Xs\{Commands,Queries,Handlers}`) → repository (sučelje u Core, impl. u Infrastructure, registracija u `DbSetup.cs`) → AutoMapper profil (`ChronicleKeeperAPI\Mapping\`) → controller. MediatR/AutoMapper se registriraju automatski (assembly scanning).

## Build & run

```bash
# Build (NE dotnet build na .sln — website projekt klijenta ga ruši)
dotnet build ChronicleKeeperAPI/ChronicleKeeperAPI.csproj

# Baza (Docker)
docker compose up -d db

# Migracije
dotnet ef migrations add <Ime> --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI
dotnet ef database update --project ChronicleKeeper.Infrastructure --startup-project ChronicleKeeperAPI

# OPREZ: baze stvorene starim EnsureCreated kodom (npr. lokalni SQLEXPRESS) nemaju
# __EFMigrationsHistory — MigrateAsync na njima pada ("There is already an object named...").
# Dev baze su disposable: dropati bazu (ili docker compose down -v) pa pustiti migracije.

# API (Swagger na rootu, http)
dotnet run --project ChronicleKeeperAPI

# Frontend
cd chroniclekeeper-client && npm run dev
```

Seed korisnici: `superadmin@chroniclekeeper.com` / `SuperAdmin@123`, `admin@chroniclekeeper.com` / `Admin@123`. Seed svijet: "Demo World".
