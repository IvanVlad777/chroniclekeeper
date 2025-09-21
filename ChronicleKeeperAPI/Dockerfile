# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ChronicleKeeper.sln .

# Copy project files
COPY ChronicleKeeperAPI/ChronicleKeeperAPI.csproj ChronicleKeeperAPI/
COPY ChronicleKeeper.Core/ChronicleKeeper.Core.csproj ChronicleKeeper.Core/
COPY ChronicleKeeper.Infrastructure/ChronicleKeeper.Infrastructure.csproj ChronicleKeeper.Infrastructure/

# Restore dependencies
RUN dotnet restore ChronicleKeeperAPI/ChronicleKeeperAPI.csproj

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/ChronicleKeeperAPI
RUN dotnet build ChronicleKeeperAPI.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish ChronicleKeeperAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

# Set entry point
ENTRYPOINT ["dotnet", "ChronicleKeeperAPI.dll"]