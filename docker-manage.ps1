# ChronicleKeeper Docker Management Script for Windows

Write-Host "ChronicleKeeper Docker Management" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""
Write-Host "1. Start all services"
Write-Host "2. Start all services (rebuild)"
Write-Host "3. Stop all services"
Write-Host "4. Stop and remove volumes (DELETE ALL DATA)" -ForegroundColor Red
Write-Host "5. Show logs"
Write-Host "6. Show running containers"
Write-Host "7. Open API shell"
Write-Host "8. Open DB shell"
Write-Host "9. Run database migrations"
Write-Host "0. Exit"
Write-Host ""

$choice = Read-Host "Choose an option"

switch ($choice) {
    1 {
        Write-Host "Starting all services..." -ForegroundColor Yellow
        docker-compose up -d
    }
    2 {
        Write-Host "Starting all services with rebuild..." -ForegroundColor Yellow
        docker-compose up --build -d
    }
    3 {
        Write-Host "Stopping all services..." -ForegroundColor Yellow
        docker-compose down
    }
    4 {
        $confirm = Read-Host "This will DELETE ALL DATA. Are you sure? (y/N)"
        if ($confirm -eq "y" -or $confirm -eq "Y") {
            Write-Host "Stopping and removing all data..." -ForegroundColor Red
            docker-compose down -v
        } else {
            Write-Host "Cancelled." -ForegroundColor Green
        }
    }
    5 {
        Write-Host "Showing logs..." -ForegroundColor Yellow
        docker-compose logs -f
    }
    6 {
        Write-Host "Running containers:" -ForegroundColor Yellow
        docker-compose ps
    }
    7 {
        Write-Host "Opening API shell..." -ForegroundColor Yellow
        docker exec -it chroniclekeeper-api bash
    }
    8 {
        Write-Host "Opening DB shell..." -ForegroundColor Yellow
        docker exec -it chroniclekeeper-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'ChronicleKeeper2024!'
    }
    9 {
        Write-Host "Running database migrations..." -ForegroundColor Yellow
        docker exec -it chroniclekeeper-api dotnet ef database update
    }
    0 {
        Write-Host "Goodbye!" -ForegroundColor Green
        exit 0
    }
    default {
        Write-Host "Invalid option!" -ForegroundColor Red
    }
}