using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEconomyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "City_EconomicSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EconomicSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuildId",
                table: "EducationRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorporationId",
                table: "Apprenticeships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuildId",
                table: "Apprenticeships",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExchangeRate = table.Column<double>(type: "float", nullable: false),
                    BackingType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Currencies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExtractionMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsSustainable = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractionMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtractionMethods_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExtractionMethods_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmploymentRate = table.Column<double>(type: "float", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Industries_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Industries_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxationSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeTaxRate = table.Column<double>(type: "float", nullable: false),
                    CorporateTaxRate = table.Column<double>(type: "float", nullable: false),
                    TradeTariffRate = table.Column<double>(type: "float", nullable: false),
                    HasFlatTax = table.Column<bool>(type: "bit", nullable: false),
                    HasWealthTax = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxationSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxationSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TaxationSystems_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TradeRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MainGoods = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeRoutes_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TradeRoutes_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankingSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InterestRate = table.Column<double>(type: "float", nullable: false),
                    AllowsLoans = table.Column<bool>(type: "bit", nullable: false),
                    HasStateControl = table.Column<bool>(type: "bit", nullable: false),
                    SupportsForeignInvestment = table.Column<bool>(type: "bit", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankingSystems_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankingSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BankingSystems_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NaturalResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    MarketValue = table.Column<double>(type: "float", nullable: false),
                    IsRenewable = table.Column<bool>(type: "bit", nullable: false),
                    IsStrategicResource = table.Column<bool>(type: "bit", nullable: false),
                    ExtractionMethodId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaturalResources_ExtractionMethods_ExtractionMethodId",
                        column: x => x.ExtractionMethodId,
                        principalTable: "ExtractionMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NaturalResources_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NaturalResources_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimaryActivity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsGovernmentSanctioned = table.Column<bool>(type: "bit", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: true),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    LegalSystemId = table.Column<int>(type: "int", nullable: true),
                    EducationSystemId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guilds_EducationSystems_EducationSystemId",
                        column: x => x.EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guilds_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Guilds_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guilds_LegalSystems_LegalSystemId",
                        column: x => x.LegalSystemId,
                        principalTable: "LegalSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guilds_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guilds_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TradeRouteLocations",
                columns: table => new
                {
                    TradeRouteId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRouteLocations", x => new { x.TradeRouteId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_TradeRouteLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeRouteLocations_TradeRoutes_TradeRouteId",
                        column: x => x.TradeRouteId,
                        principalTable: "TradeRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Corporations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndustrySector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Revenue = table.Column<double>(type: "float", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    IsPubliclyTraded = table.Column<bool>(type: "bit", nullable: false),
                    IsStateOwned = table.Column<bool>(type: "bit", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: true),
                    BankingSystemId = table.Column<int>(type: "int", nullable: true),
                    ParentCorporationId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporations", x => x.Id);
                    table.CheckConstraint("CK_Corporations_Parent_NotSelf", "[ParentCorporationId] <> [Id]");
                    table.ForeignKey(
                        name: "FK_Corporations_BankingSystems_BankingSystemId",
                        column: x => x.BankingSystemId,
                        principalTable: "BankingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporations_Corporations_ParentCorporationId",
                        column: x => x.ParentCorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Corporations_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporations_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporations_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EconomicSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMarketDriven = table.Column<bool>(type: "bit", nullable: false),
                    HasStateControl = table.Column<bool>(type: "bit", nullable: false),
                    IsFeudal = table.Column<bool>(type: "bit", nullable: false),
                    AllowsCorporations = table.Column<bool>(type: "bit", nullable: false),
                    AllowsGuilds = table.Column<bool>(type: "bit", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: true),
                    BankingSystemId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_BankingSystems_BankingSystemId",
                        column: x => x.BankingSystemId,
                        principalTable: "BankingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NaturalResourceLocations",
                columns: table => new
                {
                    NaturalResourceId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalResourceLocations", x => new { x.NaturalResourceId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_NaturalResourceLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NaturalResourceLocations_NaturalResources_NaturalResourceId",
                        column: x => x.NaturalResourceId,
                        principalTable: "NaturalResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeRouteResources",
                columns: table => new
                {
                    TradeRouteId = table.Column<int>(type: "int", nullable: false),
                    NaturalResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRouteResources", x => new { x.TradeRouteId, x.NaturalResourceId });
                    table.ForeignKey(
                        name: "FK_TradeRouteResources_NaturalResources_NaturalResourceId",
                        column: x => x.NaturalResourceId,
                        principalTable: "NaturalResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeRouteResources_TradeRoutes_TradeRouteId",
                        column: x => x.TradeRouteId,
                        principalTable: "TradeRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildFactions",
                columns: table => new
                {
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildFactions", x => new { x.GuildId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_GuildFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildFactions_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildProfessions",
                columns: table => new
                {
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildProfessions", x => new { x.GuildId, x.ProfessionId });
                    table.ForeignKey(
                        name: "FK_GuildProfessions_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildProfessions_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RankTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RankLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HasLeadershipAuthority = table.Column<bool>(type: "bit", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildRanks_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildRanks_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GuildRanks_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildSocialClasses",
                columns: table => new
                {
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    SocialClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildSocialClasses", x => new { x.GuildId, x.SocialClassId });
                    table.ForeignKey(
                        name: "FK_GuildSocialClasses_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildSocialClasses_SocialClasses_SocialClassId",
                        column: x => x.SocialClassId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporateLeaderships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    IsMajorShareholder = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateLeaderships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CorporationFactions",
                columns: table => new
                {
                    CorporationId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationFactions", x => new { x.CorporationId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_CorporationFactions_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporationProfessions",
                columns: table => new
                {
                    CorporationId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationProfessions", x => new { x.CorporationId, x.ProfessionId });
                    table.ForeignKey(
                        name: "FK_CorporationProfessions_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationProfessions_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_EconomicSystemId",
                table: "Locations",
                column: "City_EconomicSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EconomicSystemId",
                table: "Locations",
                column: "EconomicSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_GuildId",
                table: "EducationRecords",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_CorporationId",
                table: "Apprenticeships",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_GuildId",
                table: "Apprenticeships",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_BankingSystems_CurrencyId",
                table: "BankingSystems",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankingSystems_HistoryId",
                table: "BankingSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BankingSystems_WorldId_Name",
                table: "BankingSystems",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CorporateLeaderships_CharacterId",
                table: "CorporateLeaderships",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateLeaderships_CorporationId",
                table: "CorporateLeaderships",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateLeaderships_HistoryId",
                table: "CorporateLeaderships",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateLeaderships_ProfessionId",
                table: "CorporateLeaderships",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateLeaderships_WorldId_Name",
                table: "CorporateLeaderships",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CorporationFactions_FactionId",
                table: "CorporationFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationProfessions_ProfessionId",
                table: "CorporationProfessions",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_BankingSystemId",
                table: "Corporations",
                column: "BankingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_HistoryId",
                table: "Corporations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_IndustryId",
                table: "Corporations",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_ParentCorporationId",
                table: "Corporations",
                column: "ParentCorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_TaxationSystemId",
                table: "Corporations",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_WorldId_Name",
                table: "Corporations",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_HistoryId",
                table: "Currencies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_WorldId_Name",
                table: "Currencies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_EconomicSystems_BankingSystemId",
                table: "EconomicSystems",
                column: "BankingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_EconomicSystems_HistoryId",
                table: "EconomicSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EconomicSystems_TaxationSystemId",
                table: "EconomicSystems",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_EconomicSystems_WorldId_Name",
                table: "EconomicSystems",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ExtractionMethods_HistoryId",
                table: "ExtractionMethods",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractionMethods_WorldId_Name",
                table: "ExtractionMethods",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildFactions_FactionId",
                table: "GuildFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildProfessions_ProfessionId",
                table: "GuildProfessions",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildRanks_GuildId",
                table: "GuildRanks",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildRanks_HistoryId",
                table: "GuildRanks",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildRanks_WorldId_Name",
                table: "GuildRanks",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_EducationSystemId",
                table: "Guilds",
                column: "EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_HistoryId",
                table: "Guilds",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_IndustryId",
                table: "Guilds",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_LegalSystemId",
                table: "Guilds",
                column: "LegalSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_TaxationSystemId",
                table: "Guilds",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_WorldId_Name",
                table: "Guilds",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildSocialClasses_SocialClassId",
                table: "GuildSocialClasses",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_HistoryId",
                table: "Industries",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_WorldId_Name",
                table: "Industries",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResourceLocations_LocationId",
                table: "NaturalResourceLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResources_ExtractionMethodId",
                table: "NaturalResources",
                column: "ExtractionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResources_HistoryId",
                table: "NaturalResources",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResources_WorldId_Name",
                table: "NaturalResources",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxationSystems_HistoryId",
                table: "TaxationSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxationSystems_WorldId_Name",
                table: "TaxationSystems",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeRouteLocations_LocationId",
                table: "TradeRouteLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRouteResources_NaturalResourceId",
                table: "TradeRouteResources",
                column: "NaturalResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRoutes_HistoryId",
                table: "TradeRoutes",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRoutes_WorldId_Name",
                table: "TradeRoutes",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Apprenticeships_Corporations_CorporationId",
                table: "Apprenticeships",
                column: "CorporationId",
                principalTable: "Corporations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Apprenticeships_Guilds_GuildId",
                table: "Apprenticeships",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationRecords_Guilds_GuildId",
                table: "EducationRecords",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_EconomicSystems_City_EconomicSystemId",
                table: "Locations",
                column: "City_EconomicSystemId",
                principalTable: "EconomicSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_EconomicSystems_EconomicSystemId",
                table: "Locations",
                column: "EconomicSystemId",
                principalTable: "EconomicSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apprenticeships_Corporations_CorporationId",
                table: "Apprenticeships");

            migrationBuilder.DropForeignKey(
                name: "FK_Apprenticeships_Guilds_GuildId",
                table: "Apprenticeships");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationRecords_Guilds_GuildId",
                table: "EducationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_EconomicSystems_City_EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_EconomicSystems_EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "CorporateLeaderships");

            migrationBuilder.DropTable(
                name: "CorporationFactions");

            migrationBuilder.DropTable(
                name: "CorporationProfessions");

            migrationBuilder.DropTable(
                name: "EconomicSystems");

            migrationBuilder.DropTable(
                name: "GuildFactions");

            migrationBuilder.DropTable(
                name: "GuildProfessions");

            migrationBuilder.DropTable(
                name: "GuildRanks");

            migrationBuilder.DropTable(
                name: "GuildSocialClasses");

            migrationBuilder.DropTable(
                name: "NaturalResourceLocations");

            migrationBuilder.DropTable(
                name: "TradeRouteLocations");

            migrationBuilder.DropTable(
                name: "TradeRouteResources");

            migrationBuilder.DropTable(
                name: "Corporations");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "NaturalResources");

            migrationBuilder.DropTable(
                name: "TradeRoutes");

            migrationBuilder.DropTable(
                name: "BankingSystems");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "TaxationSystems");

            migrationBuilder.DropTable(
                name: "ExtractionMethods");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Locations_City_EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_EducationRecords_GuildId",
                table: "EducationRecords");

            migrationBuilder.DropIndex(
                name: "IX_Apprenticeships_CorporationId",
                table: "Apprenticeships");

            migrationBuilder.DropIndex(
                name: "IX_Apprenticeships_GuildId",
                table: "Apprenticeships");

            migrationBuilder.DropColumn(
                name: "City_EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "EconomicSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "EducationRecords");

            migrationBuilder.DropColumn(
                name: "CorporationId",
                table: "Apprenticeships");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Apprenticeships");
        }
    }
}
