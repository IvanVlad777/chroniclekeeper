# ChronicleKeeper Docker Setup

Ovaj projekt je konfiguriran da se pokreće pomoću Dockera s tri glavna servisa:
- **SQL Server** - baza podataka
- **ChronicleKeeper API** - .NET backend
- **ChronicleKeeper Client** - React frontend

## Preduvjeti

- Docker Desktop za Windows
- Git

## Kako pokrenuti projekt

1. **Kloniraj repozitorij** (ako još nisi):
   ```bash
   git clone <your-repo-url>
   cd ChronicleKeeper
   ```

2. **Pokreni Docker Compose**:
   ```bash
   docker-compose up --build
   ```

3. **Pristupi aplikaciji**:
   - Frontend (React): http://localhost:3000
   - Backend API: http://localhost:5000
   - SQL Server: localhost:1433

## Korisne Docker naredbe

### Osnovne naredbe
```bash
# Pokreni sve servise
docker-compose up

# Pokreni u pozadini
docker-compose up -d

# Pokreni s rebuild
docker-compose up --build

# Zaustavi sve servise
docker-compose down

# Zaustavi i obriši volumene (sve podatke)
docker-compose down -v
```

### Development naredbe
```bash
# Vidi logove
docker-compose logs

# Vidi logove samo API-ja
docker-compose logs api

# Otvori shell u kontejneru
docker exec -it chroniclekeeper-api bash
docker exec -it chroniclekeeper-db bash

# Restartaj samo jedan servis
docker-compose restart api
```

### Baza podataka
```bash
# Pristupi SQL Server bazi
docker exec -it chroniclekeeper-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'ChronicleKeeper2024!'

# Backup baze
docker exec chroniclekeeper-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'ChronicleKeeper2024!' -Q "BACKUP DATABASE ChronicleKeeperDB TO DISK = '/var/opt/mssql/data/ChronicleKeeperDB.bak'"
```

## Konfiguracija

### Environment varijable
- `SA_PASSWORD`: Lozinka za SQL Server SA korisnika
- `ASPNETCORE_ENVIRONMENT`: .NET environment (Development/Production)

### Portovi
- **3000**: React frontend
- **5000**: .NET API
- **1433**: SQL Server

### Volumeni
- `sqlserver_data`: Trajno spremanje SQL Server podataka
- `api_logs`: Logovi API aplikacije

## Migracije baze

Nakon prvog pokretanja, možda ćete trebati pokrenuti migracije:

```bash
# Otvori shell u API kontejneru
docker exec -it chroniclekeeper-api bash

# Pokreni migracije
dotnet ef database update
```

## Troubleshooting

### SQL Server se ne povezuje
- Provjeri jesu li svi kontejneri pokrenuti: `docker-compose ps`
- Provjeri logove: `docker-compose logs db`
- Čekaj da se SQL Server potpuno pokrene (može potrajati 30-60 sekundi)

### Build errors
- Obriši sve i rebuildiraj: `docker-compose down && docker-compose up --build`
- Obriši Docker cache: `docker system prune -a`

### Port already in use
- Promijeni portove u `docker-compose.yml` ili zaustavi druge aplikacije