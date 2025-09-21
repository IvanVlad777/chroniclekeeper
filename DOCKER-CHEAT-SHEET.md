# ChronicleKeeper Docker - Vodič za korištenje

## 🐳 Što je Docker?
Docker pakira aplikaciju s EVERYTHING što joj treba:
- OS, Runtime, Bazu podataka, Biblioteke
- "Radi kod mene" → "Radi svugdje"
- Nema instalacija SQL Server-a!

## 📁 Struktura koju smo stvorili

```
ChronicleKeeper/
├── docker-compose.yml          # Orkestracija svih servisa
├── ChronicleKeeperAPI/
│   └── Dockerfile             # Recipe za .NET API kontejner
├── chroniclekeeper-client/
│   ├── Dockerfile             # Recipe za React kontejner
│   └── nginx.conf             # Web server konfiguracija
├── .dockerignore              # Što ne kopirati u kontejnere
└── DOCKER-README.md           # Kompletna dokumentacija
```

## 🚀 OSNOVNI WORKFLOW

### Prvi put - Potpuna instalacija
```powershell
cd f:\Projekti\ChronicleKeeper
docker-compose up --build
```

### Svakodnevni development (NAJBOLJI pristup)
```powershell
# Terminal 1: Samo baza u Dockeru
docker-compose up db

# Terminal 2: API lokalno (brži development)
cd ChronicleKeeperAPI
dotnet run

# Terminal 3: React lokalno (hot reload)
cd chroniclekeeper-client
npm run dev
```

## 🔄 PROMJENE U KODU

### Scenario 1: Mijenjam React kod
```powershell
# Ako koristim hybrid (preporučeno):
# Ništa! Vite automatski reload-a promjene

# Ako koristim Docker:
docker-compose up --build client
```

### Scenario 2: Mijenjam .NET API kod
```powershell
# Ako koristim hybrid (preporučeno):
# Ništa! dotnet run automatski kompajlira

# Ako koristim Docker:
docker-compose up --build api
```

### Scenario 3: Mijenjam bazu/migracije
```powershell
# Ako je baza u Dockeru:
docker exec chroniclekeeper-api dotnet ef database update

# Ili ako je API lokaln:
cd ChronicleKeeperAPI
dotnet ef database update
```

## 🛠️ KORISNE NAREDBE

### Osnovne Docker naredbe
```powershell
# Status kontejnera
docker-compose ps

# Svi logovi
docker-compose logs

# Logovi samo API-ja
docker-compose logs api

# Real-time logovi
docker-compose logs -f

# Zaustavi sve
docker-compose down

# OPREZ: Obriši sve podatke
docker-compose down -v
```

### Development naredbe
```powershell
# Restart servisa
docker-compose restart api
docker-compose restart db

# Rebuild samo jednog servisa
docker-compose up --build --no-deps api

# Otvori shell u kontejneru
docker exec -it chroniclekeeper-api bash
docker exec -it chroniclekeeper-db bash
```

### Pristup aplikaciji
- React App: http://localhost:3000 (ili 5173 lokalno)
- API: http://localhost:5000
- Swagger API docs: http://localhost:5000/swagger
- SQL Server: localhost:1433 (sa/ChronicleKeeper2024!)

## 🔧 ČESTI PROBLEMI

### "Port already in use"
```powershell
# Provjeri što koristi port
netstat -ano | findstr :3000
netstat -ano | findstr :5000

# Ili promijeni port u docker-compose.yml
# "3001:80" umjesto "3000:80"
```

### "Cannot connect to database"
```powershell
# Provjeri status
docker-compose ps

# Provjeri logove baze
docker-compose logs db

# Čekaj 30-60 sekundi da se SQL Server pokrene
```

### "Build failed"
```powershell
# Očisti cache
docker system prune -a

# Rebuild sve
docker-compose build --no-cache
docker-compose up
```

### Hot reload ne radi
- Koristi hybrid pristup (lokalni API + React)
- Docker je sporiji za development

## ⚡ BRZI SAVJETI

### Za svakodnevni rad:
1. `docker-compose up db` - pokreni samo bazu
2. `dotnet run` u API direktoriju
3. `npm run dev` u client direktoriju
4. Kod promjene se automatski učitavaju!

### Za finalno testiranje:
1. `docker-compose down` - zaustavi sve
2. `docker-compose up --build` - pokreni kao produkcija

### Za deploy:
- Sve je spremno za produkciju!
- Samo promijeni portove i environment varijable

## 📞 Connection Stringovi

### Docker (automatski):
```
Server=db;Database=ChronicleKeeperDB;User Id=sa;Password=ChronicleKeeper2024!;TrustServerCertificate=True;
```

### Lokalni SQL Server:
```
Server=localhost\\SQLEXPRESS;Database=ChronicleKeeperDB;Integrated Security=True;TrustServerCertificate=True;
```

## 💡 GLAVNE PREDNOSTI

✅ Nema instalacija SQL Server-a
✅ Isti kod radi svugdje
✅ Jedan command pokreće sve
✅ Lako skaliranje
✅ Production ready
✅ Izolacija servisa
✅ Backup/restore jednostavan

## 🎯 NAJVAŽNIJE

**Development workflow:**
```powershell
# Jednom dnevno:
docker-compose up db

# U drugim terminalima:
dotnet run     # (u API direktoriju)
npm run dev    # (u client direktoriju)

# Hot reload + brze promjene!
```

**Production test:**
```powershell
docker-compose up --build
```

**Sve radi na: localhost:3000** 🎉