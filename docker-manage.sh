#!/bin/bash

# ChronicleKeeper Docker Management Script

echo "ChronicleKeeper Docker Management"
echo "================================="
echo ""
echo "1. Start all services"
echo "2. Start all services (rebuild)"
echo "3. Stop all services"
echo "4. Stop and remove volumes (DELETE ALL DATA)"
echo "5. Show logs"
echo "6. Show running containers"
echo "7. Open API shell"
echo "8. Open DB shell"
echo "9. Run database migrations"
echo "0. Exit"
echo ""

read -p "Choose an option: " choice

case $choice in
    1)
        echo "Starting all services..."
        docker-compose up -d
        ;;
    2)
        echo "Starting all services with rebuild..."
        docker-compose up --build -d
        ;;
    3)
        echo "Stopping all services..."
        docker-compose down
        ;;
    4)
        read -p "This will DELETE ALL DATA. Are you sure? (y/N): " confirm
        if [[ $confirm == [yY] ]]; then
            echo "Stopping and removing all data..."
            docker-compose down -v
        else
            echo "Cancelled."
        fi
        ;;
    5)
        echo "Showing logs..."
        docker-compose logs -f
        ;;
    6)
        echo "Running containers:"
        docker-compose ps
        ;;
    7)
        echo "Opening API shell..."
        docker exec -it chroniclekeeper-api bash
        ;;
    8)
        echo "Opening DB shell..."
        docker exec -it chroniclekeeper-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'ChronicleKeeper2024!'
        ;;
    9)
        echo "Running database migrations..."
        docker exec -it chroniclekeeper-api dotnet ef database update
        ;;
    0)
        echo "Goodbye!"
        exit 0
        ;;
    *)
        echo "Invalid option!"
        ;;
esac