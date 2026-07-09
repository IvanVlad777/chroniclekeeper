---
name: verify
description: Kako pokrenuti i end-to-end provjeriti ChronicleKeeper (API + React klijent) na ovom stroju.
---

# ChronicleKeeper — verifikacijski recept

## Pokretanje

```powershell
# 1. Baza (obično već radi): docker compose up -d db  →  container "chroniclekeeper-db"
docker ps --format "{{.Names}} {{.Status}}"

# 2. API (background) — sluša na http://localhost:5274, MigrateAsync + seed na startu
dotnet run --project ChronicleKeeperAPI

# 3. Klijent (background) — http://localhost:5173, vite proxy /api → :5274
cd chroniclekeeper-client; npm run dev
```

## Vožnja UI-ja (headless Edge, bez downloada browsera)

Playwright-core + sistemski Edge (`C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe`):

```powershell
cd <scratchpad>; npm i playwright-core
# chromium.launch({ executablePath: EDGE, headless: true })
```

Standardni prohod: `/` → redirect `/login` → krivi password (očekuj "Invalid credentials" u `[role=alert]`) →
login → `/storymap` Overview → Characters → klik na red → detalj → klik na disabled nav (URL se ne smije
promijeniti) → theme dot + ☾/☀ (provjeri `document.documentElement.dataset.theme/mode` + `localStorage.theme`)
→ HR toggle → logout → registracija sa slabim passwordom (očekuj Identity greške).

## Gotchas

- Seed računi: `superadmin@chroniclekeeper.com` / `SuperAdmin@123` (vlasnik "Demo World"),
  `admin@chroniclekeeper.com` / `Admin@123` (**nema svjetova** — dobar za empty-state, loš za data prohod).
- `dotnet build` na .sln ruši klijentski website projekt — builda se samo `ChronicleKeeperAPI/ChronicleKeeperAPI.csproj`.
- Frontend build/lint: `npm run build` (tsc + vite), `npm run lint` u `chroniclekeeper-client/`.
- Token je u sessionStorage — svaki novi browser context kreće odjavljen.
