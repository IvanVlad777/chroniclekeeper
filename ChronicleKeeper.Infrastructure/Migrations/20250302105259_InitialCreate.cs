using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seasons = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Content_BookId",
                        column: x => x.BookId,
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episode_Content_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abilities_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    BattleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterRelationships_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClimateDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    Precipitation = table.Column<double>(type: "float", nullable: false),
                    WindSpeed = table.Column<double>(type: "float", nullable: false),
                    WindDirection = table.Column<int>(type: "int", nullable: false),
                    IsExtremeClimate = table.Column<bool>(type: "bit", nullable: false),
                    NotableWeatherPhenomena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateDetails_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClimateZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ZoneType = table.Column<int>(type: "int", nullable: false),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    AverageHumidity = table.Column<double>(type: "float", nullable: false),
                    AveragePrecipitation = table.Column<double>(type: "float", nullable: false),
                    HasDistinctSeasons = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateZones_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangeRate = table.Column<double>(type: "float", nullable: false),
                    BackingType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EducationSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsStateControlled = table.Column<bool>(type: "bit", nullable: false),
                    AllowsPrivateInstitutions = table.Column<bool>(type: "bit", nullable: false),
                    IncludesReligiousEducation = table.Column<bool>(type: "bit", nullable: false),
                    SupportsGuildTraining = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExtractionMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MethodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSustainable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractionMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtractionMethods_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsSecretive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hobbies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    WritingSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExtinct = table.Column<bool>(type: "bit", nullable: false),
                    Dialects = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Laws = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JudicialIndependence = table.Column<int>(type: "int", nullable: false),
                    PunishmentMethods = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MilitaryDoctrines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Strategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Philosophy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrioritizesInfantry = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesCavalry = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesArtillery = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesNavalForces = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesAirForces = table.Column<bool>(type: "bit", nullable: false),
                    RequiresHeavyIndustry = table.Column<bool>(type: "bit", nullable: false),
                    UsesMercenaries = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryDoctrines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryDoctrines_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MilitaryEquipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    EquipmentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryEquipment_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OwnershipHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnershipHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnershipHistories_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PoliticalIdeologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsAuthoritarian = table.Column<bool>(type: "bit", nullable: false),
                    IsSocialist = table.Column<bool>(type: "bit", nullable: false),
                    IsLiberal = table.Column<bool>(type: "bit", nullable: false),
                    IsRadical = table.Column<bool>(type: "bit", nullable: false),
                    IsMilitaristic = table.Column<bool>(type: "bit", nullable: false),
                    SupportsFreeMarket = table.Column<bool>(type: "bit", nullable: false),
                    SupportsPlannedEconomy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalIdeologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliticalIdeologies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkEnvironment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReligiousFestivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Traditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPilgrimageEvent = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    HolySiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousFestivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousFestivals_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    TypicalTemperature = table.Column<double>(type: "float", nullable: false),
                    TypicalPrecipitation = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SocialHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsCasteSystem = table.Column<bool>(type: "bit", nullable: false),
                    AllowsUpwardMobility = table.Column<bool>(type: "bit", nullable: false),
                    AllowsIntermarriage = table.Column<bool>(type: "bit", nullable: false),
                    EnforcesLegalSeparation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialHierarchies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialHierarchies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaxationSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IncomeTaxRate = table.Column<double>(type: "float", nullable: false),
                    CorporateTaxRate = table.Column<double>(type: "float", nullable: false),
                    TradeTariffRate = table.Column<double>(type: "float", nullable: false),
                    HasFlatTax = table.Column<bool>(type: "bit", nullable: false),
                    HasWealthTax = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxationSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxationSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Timelines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timelines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timelines_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeRoutes_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterId = table.Column<int>(type: "int", nullable: true),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    EpisodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reference_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reference_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reference_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbilityLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbilityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityLevels_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "Abilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbilityLevels_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClimateDetailClimateZone",
                columns: table => new
                {
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    ClimatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDetailClimateZone", x => new { x.ClimateZoneId, x.ClimatesId });
                    table.ForeignKey(
                        name: "FK_ClimateDetailClimateZone_ClimateDetails_ClimatesId",
                        column: x => x.ClimatesId,
                        principalTable: "ClimateDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClimateDetailClimateZone_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    PatternType = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Effects = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherPatterns_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherPatterns_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankingSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    SystemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestRate = table.Column<double>(type: "float", nullable: false),
                    AllowsLoans = table.Column<bool>(type: "bit", nullable: false),
                    HasStateControl = table.Column<bool>(type: "bit", nullable: false),
                    SupportsForeignInvestment = table.Column<bool>(type: "bit", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankingSystems_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankingSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    EducationSystemId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationYears = table.Column<int>(type: "int", nullable: true),
                    IsGovernmentRecognized = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_EducationSystems_EducationSystemId",
                        column: x => x.EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schools_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    FocusesOnScience = table.Column<bool>(type: "bit", nullable: false),
                    FocusesOnMagic = table.Column<bool>(type: "bit", nullable: false),
                    FocusesOnPhilosophy = table.Column<bool>(type: "bit", nullable: false),
                    FocusesOnMilitaryStudies = table.Column<bool>(type: "bit", nullable: false),
                    EducationSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Universities_EducationSystems_EducationSystemId",
                        column: x => x.EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Universities_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NaturalResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    MarketValue = table.Column<double>(type: "float", nullable: false),
                    IsRenewable = table.Column<bool>(type: "bit", nullable: false),
                    IsStrategicResource = table.Column<bool>(type: "bit", nullable: false),
                    ExtractionMethodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaturalResources_ExtractionMethods_ExtractionMethodId",
                        column: x => x.ExtractionMethodId,
                        principalTable: "ExtractionMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NaturalResources_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MilitaryOrganizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilitaryDoctrineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizations_MilitaryDoctrines_MilitaryDoctrineId",
                        column: x => x.MilitaryDoctrineId,
                        principalTable: "MilitaryDoctrines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GovernmentSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsDemocratic = table.Column<bool>(type: "bit", nullable: false),
                    IsMonarchic = table.Column<bool>(type: "bit", nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    IsFederal = table.Column<bool>(type: "bit", nullable: false),
                    IsCentralized = table.Column<bool>(type: "bit", nullable: false),
                    PoliticalIdeologyId = table.Column<int>(type: "int", nullable: true),
                    ElectionSystem = table.Column<int>(type: "int", nullable: false),
                    StabilityLevel = table.Column<int>(type: "int", nullable: false),
                    HasTermLimits = table.Column<bool>(type: "bit", nullable: false),
                    MaxTermLength = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernmentSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GovernmentSystems_PoliticalIdeologies_PoliticalIdeologyId",
                        column: x => x.PoliticalIdeologyId,
                        principalTable: "PoliticalIdeologies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    RankTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RankLevel = table.Column<int>(type: "int", nullable: false),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRanks_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobRanks_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialisations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialisations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Specialisations_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClimateZoneSeason",
                columns: table => new
                {
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    SeasonsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZoneSeason", x => new { x.ClimateZoneId, x.SeasonsId });
                    table.ForeignKey(
                        name: "FK_ClimateZoneSeason_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClimateZoneSeason_Seasons_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    SocialHierarchyId = table.Column<int>(type: "int", nullable: true),
                    Population = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Nations_SocialHierarchies_SocialHierarchyId",
                        column: x => x.SocialHierarchyId,
                        principalTable: "SocialHierarchies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SocialClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsNoble = table.Column<bool>(type: "bit", nullable: false),
                    IsMerchantClass = table.Column<bool>(type: "bit", nullable: false),
                    IsOutcast = table.Column<bool>(type: "bit", nullable: false),
                    CanOwnLand = table.Column<bool>(type: "bit", nullable: false),
                    CanHoldOffice = table.Column<bool>(type: "bit", nullable: false),
                    HasTaxExemptions = table.Column<bool>(type: "bit", nullable: false),
                    SocialHierarchyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialClasses_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocialClasses_SocialHierarchies_SocialHierarchyId",
                        column: x => x.SocialHierarchyId,
                        principalTable: "SocialHierarchies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EconomicSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsMarketDriven = table.Column<bool>(type: "bit", nullable: false),
                    HasStateControl = table.Column<bool>(type: "bit", nullable: false),
                    IsFeudal = table.Column<bool>(type: "bit", nullable: false),
                    AllowsCorporations = table.Column<bool>(type: "bit", nullable: false),
                    AllowsGuilds = table.Column<bool>(type: "bit", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: false),
                    BankingSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_BankingSystems_BankingSystemId",
                        column: x => x.BankingSystemId,
                        principalTable: "BankingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EconomicSystems_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EconomicSystems_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionTradeSchool",
                columns: table => new
                {
                    TradeSchoolsId = table.Column<int>(type: "int", nullable: false),
                    TrainedProfessionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionTradeSchool", x => new { x.TradeSchoolsId, x.TrainedProfessionsId });
                    table.ForeignKey(
                        name: "FK_ProfessionTradeSchool_Professions_TrainedProfessionsId",
                        column: x => x.TrainedProfessionsId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionTradeSchool_Schools_TradeSchoolsId",
                        column: x => x.TradeSchoolsId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversityMajors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniversityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityMajors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniversityMajors_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UniversityMajors_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalResourceTradeRoute",
                columns: table => new
                {
                    ExportRoutesId = table.Column<int>(type: "int", nullable: false),
                    ResourcesTradedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalResourceTradeRoute", x => new { x.ExportRoutesId, x.ResourcesTradedId });
                    table.ForeignKey(
                        name: "FK_NaturalResourceTradeRoute_NaturalResources_ResourcesTradedId",
                        column: x => x.ResourcesTradedId,
                        principalTable: "NaturalResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NaturalResourceTradeRoute_TradeRoutes_ExportRoutesId",
                        column: x => x.ExportRoutesId,
                        principalTable: "TradeRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionMilitaryOrganization",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    MilitaryConnectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionMilitaryOrganization", x => new { x.FactionsId, x.MilitaryConnectionsId });
                    table.ForeignKey(
                        name: "FK_FactionMilitaryOrganization_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionMilitaryOrganization_MilitaryOrganizations_MilitaryConnectionsId",
                        column: x => x.MilitaryConnectionsId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IdeologyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoliticalIdeologyId = table.Column<int>(type: "int", nullable: false),
                    InfluenceLevel = table.Column<int>(type: "int", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    GovernmentSystemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_GovernmentSystems_GovernmentSystemId",
                        column: x => x.GovernmentSystemId,
                        principalTable: "GovernmentSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PoliticalParties_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PoliticalParties_PoliticalIdeologies_PoliticalIdeologyId",
                        column: x => x.PoliticalIdeologyId,
                        principalTable: "PoliticalIdeologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageNation",
                columns: table => new
                {
                    LanguagesSpokenId = table.Column<int>(type: "int", nullable: false),
                    NationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageNation", x => new { x.LanguagesSpokenId, x.NationsId });
                    table.ForeignKey(
                        name: "FK_LanguageNation_Languages_LanguagesSpokenId",
                        column: x => x.LanguagesSpokenId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageNation_Nations_NationsId",
                        column: x => x.NationsId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    CoreBeliefs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Practices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDeities = table.Column<bool>(type: "bit", nullable: false),
                    IsStateReligion = table.Column<bool>(type: "bit", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Religions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Religions_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeLaws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    GrantsLegalImmunity = table.Column<bool>(type: "bit", nullable: false),
                    GrantsLandOwnershipRights = table.Column<bool>(type: "bit", nullable: false),
                    AllowsPrivateArmies = table.Column<bool>(type: "bit", nullable: false),
                    SocialClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegeLaws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivilegeLaws_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivilegeLaws_SocialClasses_SocialClassId",
                        column: x => x.SocialClassId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionSocialClass",
                columns: table => new
                {
                    SocialClassesId = table.Column<int>(type: "int", nullable: false),
                    TypicalProfessionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionSocialClass", x => new { x.SocialClassesId, x.TypicalProfessionsId });
                    table.ForeignKey(
                        name: "FK_ProfessionSocialClass_Professions_TypicalProfessionsId",
                        column: x => x.TypicalProfessionsId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionSocialClass_SocialClasses_SocialClassesId",
                        column: x => x.SocialClassesId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionPoliticalParty",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    PoliticalConnectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPoliticalParty", x => new { x.FactionsId, x.PoliticalConnectionsId });
                    table.ForeignKey(
                        name: "FK_FactionPoliticalParty_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionPoliticalParty_PoliticalParties_PoliticalConnectionsId",
                        column: x => x.PoliticalConnectionsId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NationPoliticalParty",
                columns: table => new
                {
                    NationsId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationPoliticalParty", x => new { x.NationsId, x.PoliticalPartiesId });
                    table.ForeignKey(
                        name: "FK_NationPoliticalParty_Nations_NationsId",
                        column: x => x.NationsId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NationPoliticalParty_PoliticalParties_PoliticalPartiesId",
                        column: x => x.PoliticalPartiesId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AverageLifespan = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    IsSentient = table.Column<bool>(type: "bit", nullable: false),
                    IsArtificial = table.Column<bool>(type: "bit", nullable: false),
                    ArtificialOrigin = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FungusId = table.Column<int>(type: "int", nullable: true),
                    PlantId = table.Column<int>(type: "int", nullable: true),
                    Diet = table.Column<int>(type: "int", nullable: true),
                    IsDomesticated = table.Column<bool>(type: "bit", nullable: true),
                    NumberOfLegs = table.Column<int>(type: "int", nullable: true),
                    HasWings = table.Column<bool>(type: "bit", nullable: true),
                    HasMultipleHeads = table.Column<bool>(type: "bit", nullable: true),
                    HasRegeneration = table.Column<bool>(type: "bit", nullable: true),
                    IsSacred = table.Column<bool>(type: "bit", nullable: true),
                    IsMythical = table.Column<bool>(type: "bit", nullable: true),
                    IsEndangered = table.Column<bool>(type: "bit", nullable: true),
                    Intelligence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialAbilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPackAnimal = table.Column<bool>(type: "bit", nullable: true),
                    IsAggressive = table.Column<bool>(type: "bit", nullable: true),
                    IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    AnimalId = table.Column<int>(type: "int", nullable: true),
                    ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMedicinal = table.Column<bool>(type: "bit", nullable: true),
                    IsPoisonous = table.Column<bool>(type: "bit", nullable: true),
                    IsEdible = table.Column<bool>(type: "bit", nullable: true),
                    IsHallucinogenic = table.Column<bool>(type: "bit", nullable: true),
                    IsBioluminescent = table.Column<bool>(type: "bit", nullable: true),
                    HasMutagenicProperties = table.Column<bool>(type: "bit", nullable: true),
                    Fungus_IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    CanCommunicate = table.Column<bool>(type: "bit", nullable: true),
                    SpecialProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MythologicalSignificance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fungus_AnimalId = table.Column<int>(type: "int", nullable: true),
                    FungusId1 = table.Column<int>(type: "int", nullable: true),
                    PlantType = table.Column<int>(type: "int", nullable: true),
                    Plant_ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_IsMedicinal = table.Column<bool>(type: "bit", nullable: true),
                    Plant_IsPoisonous = table.Column<bool>(type: "bit", nullable: true),
                    Sunlight = table.Column<int>(type: "int", nullable: true),
                    PreferredSoil = table.Column<int>(type: "int", nullable: true),
                    TemperatureRange = table.Column<int>(type: "int", nullable: true),
                    Rarity = table.Column<int>(type: "int", nullable: true),
                    Plant_IsBioluminescent = table.Column<bool>(type: "bit", nullable: true),
                    IsCarnivorous = table.Column<bool>(type: "bit", nullable: true),
                    HasRegenerativeProperties = table.Column<bool>(type: "bit", nullable: true),
                    CanMove = table.Column<bool>(type: "bit", nullable: true),
                    Plant_SpecialProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_MythologicalSignificance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant_IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    IsParasitic = table.Column<bool>(type: "bit", nullable: true),
                    Plant_AnimalId = table.Column<int>(type: "int", nullable: true),
                    PlantId1 = table.Column<int>(type: "int", nullable: true),
                    YieldPerHectare = table.Column<double>(type: "float", nullable: true),
                    CropType = table.Column<int>(type: "int", nullable: true),
                    Crop_IsDomesticated = table.Column<bool>(type: "bit", nullable: true),
                    MaxHeight = table.Column<double>(type: "float", nullable: true),
                    Lifespan = table.Column<int>(type: "int", nullable: true),
                    LeafType = table.Column<int>(type: "int", nullable: true),
                    CommonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SapientSpecies_ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHumanoid = table.Column<bool>(type: "bit", nullable: true),
                    SapientType = table.Column<int>(type: "int", nullable: true),
                    ReligionId1 = table.Column<int>(type: "int", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorshipMethods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMonotheistic = table.Column<bool>(type: "bit", nullable: true),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    DeityType = table.Column<int>(type: "int", nullable: true),
                    IsImmortal = table.Column<bool>(type: "bit", nullable: true),
                    CanManifestPhysically = table.Column<bool>(type: "bit", nullable: true),
                    GrantsPowers = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_FungusId",
                        column: x => x.FungusId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_FungusId1",
                        column: x => x.FungusId1,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_Fungus_AnimalId",
                        column: x => x.Fungus_AnimalId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_PlantId1",
                        column: x => x.PlantId1,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_Plant_AnimalId",
                        column: x => x.Plant_AnimalId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Creatures_Religions_ReligionId1",
                        column: x => x.ReligionId1,
                        principalTable: "Religions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    CommonValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasOralTradition = table.Column<bool>(type: "bit", nullable: false),
                    SocialStructure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XenophobiaLevel = table.Column<int>(type: "int", nullable: false),
                    TechnologicalLevel = table.Column<int>(type: "int", nullable: false),
                    ConflictResolution = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cultures_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cultures_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cultures_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReligiousOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Beliefs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMilitant = table.Column<bool>(type: "bit", nullable: false),
                    IsSecretive = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousOrders_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReligiousOrders_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeityDeity",
                columns: table => new
                {
                    AlliedDeitiesId = table.Column<int>(type: "int", nullable: false),
                    RivalDeitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeityDeity", x => new { x.AlliedDeitiesId, x.RivalDeitiesId });
                    table.ForeignKey(
                        name: "FK_DeityDeity_Creatures_AlliedDeitiesId",
                        column: x => x.AlliedDeitiesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeityDeity_Creatures_RivalDeitiesId",
                        column: x => x.RivalDeitiesId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mutations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    Effect = table.Column<int>(type: "int", nullable: false),
                    MutantCreatureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mutations_Creatures_MutantCreatureId",
                        column: x => x.MutantCreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mutations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProfessionSapientSpecies",
                columns: table => new
                {
                    FrequentOccupationsId = table.Column<int>(type: "int", nullable: false),
                    PracticedBySpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionSapientSpecies", x => new { x.FrequentOccupationsId, x.PracticedBySpeciesId });
                    table.ForeignKey(
                        name: "FK_ProfessionSapientSpecies_Creatures_PracticedBySpeciesId",
                        column: x => x.PracticedBySpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionSapientSpecies_Professions_FrequentOccupationsId",
                        column: x => x.FrequentOccupationsId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    SapientSpeciesId = table.Column<int>(type: "int", nullable: false),
                    AppearanceTraits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneticFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adaptations = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Creatures_SapientSpeciesId",
                        column: x => x.SapientSpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReligiousTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    DeityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Creatures_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArchitectureStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MaterialsUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFortified = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchitectureStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchitectureStyles_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArchitectureStyles_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArtForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotableArtists = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoricalInfluences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtForms_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtForms_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cuisines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MainIngredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CookingMethods = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    TypicalDishes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuisines_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuisines_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CulturalClothings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ClothingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Materials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRitualistic = table.Column<bool>(type: "bit", nullable: false),
                    IsArmor = table.Column<bool>(type: "bit", nullable: false),
                    SpecialProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalClothings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalClothings_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CulturalClothings_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CultureNation",
                columns: table => new
                {
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureNation", x => new { x.CultureId, x.NationId });
                    table.ForeignKey(
                        name: "FK_CultureNation_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureNation_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultureSapientSpecies",
                columns: table => new
                {
                    CulturesId = table.Column<int>(type: "int", nullable: false),
                    PracticedBySpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureSapientSpecies", x => new { x.CulturesId, x.PracticedBySpeciesId });
                    table.ForeignKey(
                        name: "FK_CultureSapientSpecies_Creatures_PracticedBySpeciesId",
                        column: x => x.PracticedBySpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureSapientSpecies_Cultures_CulturesId",
                        column: x => x.CulturesId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultureSocialClass",
                columns: table => new
                {
                    CulturesId = table.Column<int>(type: "int", nullable: false),
                    InfluencedSocialClassesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureSocialClass", x => new { x.CulturesId, x.InfluencedSocialClassesId });
                    table.ForeignKey(
                        name: "FK_CultureSocialClass_Cultures_CulturesId",
                        column: x => x.CulturesId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureSocialClass_SocialClasses_InfluencedSocialClassesId",
                        column: x => x.InfluencedSocialClassesId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsUniversal = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customs_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customs_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Folklores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHistorical = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folklores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folklores_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folklores_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Myths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    CreationStory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbolism = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasReligiousConnections = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    DeityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Myths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Myths_Creatures_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Myths_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Myths_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Myths_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Traditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Practice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Traditions_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Traditions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Traditions_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeityReligiousOrder",
                columns: table => new
                {
                    DeitiesId = table.Column<int>(type: "int", nullable: false),
                    OrdersDedicatedToDeityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeityReligiousOrder", x => new { x.DeitiesId, x.OrdersDedicatedToDeityId });
                    table.ForeignKey(
                        name: "FK_DeityReligiousOrder_Creatures_DeitiesId",
                        column: x => x.DeitiesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeityReligiousOrder_ReligiousOrders_OrdersDedicatedToDeityId",
                        column: x => x.OrdersDedicatedToDeityId,
                        principalTable: "ReligiousOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionReligiousOrder",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    ReligiousConnectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionReligiousOrder", x => new { x.FactionsId, x.ReligiousConnectionsId });
                    table.ForeignKey(
                        name: "FK_FactionReligiousOrder_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionReligiousOrder_ReligiousOrders_ReligiousConnectionsId",
                        column: x => x.ReligiousConnectionsId,
                        principalTable: "ReligiousOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ordained = table.Column<bool>(type: "bit", nullable: false),
                    ReligiousOrderId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousEducations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReligiousEducations_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReligiousEducations_ReligiousOrders_ReligiousOrderId",
                        column: x => x.ReligiousOrderId,
                        principalTable: "ReligiousOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<double>(type: "float", nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ParentLocationId = table.Column<int>(type: "int", nullable: true),
                    ArchitectureStyleId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    IsCapital = table.Column<bool>(type: "bit", nullable: true),
                    City_GovernmentSystemId = table.Column<int>(type: "int", nullable: true),
                    City_LegalSystemId = table.Column<int>(type: "int", nullable: true),
                    City_EconomicSystemId = table.Column<int>(type: "int", nullable: true),
                    City_EducationSystemId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    ContinentSpecifics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernmentSystemId = table.Column<int>(type: "int", nullable: true),
                    CapitalCityId = table.Column<int>(type: "int", nullable: true),
                    PrimaryNationId = table.Column<int>(type: "int", nullable: true),
                    LegalSystemId = table.Column<int>(type: "int", nullable: true),
                    EconomicSystemId = table.Column<int>(type: "int", nullable: true),
                    EducationSystemId = table.Column<int>(type: "int", nullable: true),
                    DistrictType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    UniqueFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depth = table.Column<double>(type: "float", nullable: true),
                    CaveEcosystem_Type = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    ForestEcosystem_Type = table.Column<int>(type: "int", nullable: true),
                    GrasslandEcosystem_Type = table.Column<int>(type: "int", nullable: true),
                    MaxElevation = table.Column<double>(type: "float", nullable: true),
                    Prominence = table.Column<double>(type: "float", nullable: true),
                    Length = table.Column<double>(type: "float", nullable: true),
                    IsSaltwater = table.Column<bool>(type: "bit", nullable: true),
                    WaterEcosystem_Depth = table.Column<double>(type: "float", nullable: true),
                    WaterEcosystem_Type = table.Column<int>(type: "int", nullable: true),
                    Volume = table.Column<double>(type: "float", nullable: true),
                    MaxDepth = table.Column<double>(type: "float", nullable: true),
                    IsFreshwater = table.Column<bool>(type: "bit", nullable: true),
                    RiverEcosystem_Length = table.Column<double>(type: "float", nullable: true),
                    LandmarkType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionSpecifics = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_ArchitectureStyles_ArchitectureStyleId",
                        column: x => x.ArchitectureStyleId,
                        principalTable: "ArchitectureStyles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_EconomicSystems_City_EconomicSystemId",
                        column: x => x.City_EconomicSystemId,
                        principalTable: "EconomicSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_EconomicSystems_EconomicSystemId",
                        column: x => x.EconomicSystemId,
                        principalTable: "EconomicSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_EducationSystems_City_EducationSystemId",
                        column: x => x.City_EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_EducationSystems_EducationSystemId",
                        column: x => x.EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_GovernmentSystems_City_GovernmentSystemId",
                        column: x => x.City_GovernmentSystemId,
                        principalTable: "GovernmentSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_GovernmentSystems_GovernmentSystemId",
                        column: x => x.GovernmentSystemId,
                        principalTable: "GovernmentSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_LegalSystems_City_LegalSystemId",
                        column: x => x.City_LegalSystemId,
                        principalTable: "LegalSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_LegalSystems_LegalSystemId",
                        column: x => x.LegalSystemId,
                        principalTable: "LegalSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FolkloreSapientSpecies",
                columns: table => new
                {
                    FolkloreId = table.Column<int>(type: "int", nullable: false),
                    OriginatedFromSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolkloreSapientSpecies", x => new { x.FolkloreId, x.OriginatedFromSpeciesId });
                    table.ForeignKey(
                        name: "FK_FolkloreSapientSpecies_Creatures_OriginatedFromSpeciesId",
                        column: x => x.OriginatedFromSpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolkloreSapientSpecies_Folklores_FolkloreId",
                        column: x => x.FolkloreId,
                        principalTable: "Folklores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Armies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsStandingArmy = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    MilitaryOrganizationId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armies_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Armies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Armies_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Armies_MilitaryOrganizations_MilitaryOrganizationId",
                        column: x => x.MilitaryOrganizationId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CityCreature",
                columns: table => new
                {
                    CitiesItInhabitsId = table.Column<int>(type: "int", nullable: false),
                    CreatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCreature", x => new { x.CitiesItInhabitsId, x.CreatureId });
                    table.ForeignKey(
                        name: "FK_CityCreature_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCreature_Locations_CitiesItInhabitsId",
                        column: x => x.CitiesItInhabitsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityCulture",
                columns: table => new
                {
                    PredominantCulturesId = table.Column<int>(type: "int", nullable: false),
                    PredominantInCitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCulture", x => new { x.PredominantCulturesId, x.PredominantInCitiesId });
                    table.ForeignKey(
                        name: "FK_CityCulture_Cultures_PredominantCulturesId",
                        column: x => x.PredominantCulturesId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCulture_Locations_PredominantInCitiesId",
                        column: x => x.PredominantInCitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityNation",
                columns: table => new
                {
                    CitiesId = table.Column<int>(type: "int", nullable: false),
                    NationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityNation", x => new { x.CitiesId, x.NationsId });
                    table.ForeignKey(
                        name: "FK_CityNation_Locations_CitiesId",
                        column: x => x.CitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityNation_Nations_NationsId",
                        column: x => x.NationsId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityPoliticalParty",
                columns: table => new
                {
                    CitiesId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityPoliticalParty", x => new { x.CitiesId, x.PoliticalPartiesId });
                    table.ForeignKey(
                        name: "FK_CityPoliticalParty_Locations_CitiesId",
                        column: x => x.CitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityPoliticalParty_PoliticalParties_PoliticalPartiesId",
                        column: x => x.PoliticalPartiesId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityReligion",
                columns: table => new
                {
                    InCitiesId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityReligion", x => new { x.InCitiesId, x.ReligionId });
                    table.ForeignKey(
                        name: "FK_CityReligion_Locations_InCitiesId",
                        column: x => x.InCitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityReligion_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClimateZoneLocation",
                columns: table => new
                {
                    ClimateConditionsId = table.Column<int>(type: "int", nullable: false),
                    LocationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZoneLocation", x => new { x.ClimateConditionsId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_ClimateZoneLocation_ClimateZones_ClimateConditionsId",
                        column: x => x.ClimateConditionsId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClimateZoneLocation_Locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryCulture",
                columns: table => new
                {
                    PredominantCulturesId = table.Column<int>(type: "int", nullable: false),
                    PredominantInCountriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryCulture", x => new { x.PredominantCulturesId, x.PredominantInCountriesId });
                    table.ForeignKey(
                        name: "FK_CountryCulture_Cultures_PredominantCulturesId",
                        column: x => x.PredominantCulturesId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryCulture_Locations_PredominantInCountriesId",
                        column: x => x.PredominantInCountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryFaction",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    OperatesInCountriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryFaction", x => new { x.FactionsId, x.OperatesInCountriesId });
                    table.ForeignKey(
                        name: "FK_CountryFaction_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryFaction_Locations_OperatesInCountriesId",
                        column: x => x.OperatesInCountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryMilitaryOrganization",
                columns: table => new
                {
                    CountriesId = table.Column<int>(type: "int", nullable: false),
                    MilitaryOrganizationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryMilitaryOrganization", x => new { x.CountriesId, x.MilitaryOrganizationsId });
                    table.ForeignKey(
                        name: "FK_CountryMilitaryOrganization_Locations_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryMilitaryOrganization_MilitaryOrganizations_MilitaryOrganizationsId",
                        column: x => x.MilitaryOrganizationsId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryNation",
                columns: table => new
                {
                    CountriesId = table.Column<int>(type: "int", nullable: false),
                    NationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryNation", x => new { x.CountriesId, x.NationsId });
                    table.ForeignKey(
                        name: "FK_CountryNation_Locations_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryNation_Nations_NationsId",
                        column: x => x.NationsId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryPoliticalParty",
                columns: table => new
                {
                    CountriesId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryPoliticalParty", x => new { x.CountriesId, x.PoliticalPartiesId });
                    table.ForeignKey(
                        name: "FK_CountryPoliticalParty_Locations_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryPoliticalParty_PoliticalParties_PoliticalPartiesId",
                        column: x => x.PoliticalPartiesId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryReligion",
                columns: table => new
                {
                    InCountriesId = table.Column<int>(type: "int", nullable: false),
                    ReligionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryReligion", x => new { x.InCountriesId, x.ReligionsId });
                    table.ForeignKey(
                        name: "FK_CountryReligion_Locations_InCountriesId",
                        column: x => x.InCountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryReligion_Religions_ReligionsId",
                        column: x => x.ReligionsId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureEcosystem",
                columns: table => new
                {
                    HabitantsId = table.Column<int>(type: "int", nullable: false),
                    NaturalHabitatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureEcosystem", x => new { x.HabitantsId, x.NaturalHabitatsId });
                    table.ForeignKey(
                        name: "FK_CreatureEcosystem_Creatures_NaturalHabitatsId",
                        column: x => x.NaturalHabitatsId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureEcosystem_Locations_HabitantsId",
                        column: x => x.HabitantsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CulturalFestivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNationalHoliday = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalFestivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalFestivals_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CulturalFestivals_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CulturalFestivals_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CulturalInstitutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    InstitutionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGovernmentFunded = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalInstitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HolySites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Significance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPilgrimageDestination = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    DeityId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolySites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolySites_Creatures_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HolySites_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HolySites_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolySites_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentRate = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Industries_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Industries_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Industries_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    FocusesOnMagic = table.Column<bool>(type: "bit", nullable: false),
                    FocusesOnHistory = table.Column<bool>(type: "bit", nullable: false),
                    UniversityId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libraries_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Libraries_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Libraries_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LocationNaturalResource",
                columns: table => new
                {
                    LocationsId = table.Column<int>(type: "int", nullable: false),
                    NaturalResourcesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationNaturalResource", x => new { x.LocationsId, x.NaturalResourcesId });
                    table.ForeignKey(
                        name: "FK_LocationNaturalResource_Locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationNaturalResource_NaturalResources_NaturalResourcesId",
                        column: x => x.NaturalResourcesId,
                        principalTable: "NaturalResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationTradeRoute",
                columns: table => new
                {
                    LocationsId = table.Column<int>(type: "int", nullable: false),
                    TradeRoutesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTradeRoute", x => new { x.LocationsId, x.TradeRoutesId });
                    table.ForeignKey(
                        name: "FK_LocationTradeRoute_Locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationTradeRoute_TradeRoutes_TradeRoutesId",
                        column: x => x.TradeRoutesId,
                        principalTable: "TradeRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionSapientSpecies",
                columns: table => new
                {
                    NativeRegionsId = table.Column<int>(type: "int", nullable: false),
                    OriginOfSapientSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionSapientSpecies", x => new { x.NativeRegionsId, x.OriginOfSapientSpeciesId });
                    table.ForeignKey(
                        name: "FK_RegionSapientSpecies_Creatures_OriginOfSapientSpeciesId",
                        column: x => x.OriginOfSapientSpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionSapientSpecies_Locations_NativeRegionsId",
                        column: x => x.NativeRegionsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Consequences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMajorEvent = table.Column<bool>(type: "bit", nullable: false),
                    TimelineId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    BattleId = table.Column<int>(type: "int", nullable: true),
                    FolkloreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Folklores_FolkloreId",
                        column: x => x.FolkloreId,
                        principalTable: "Folklores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Timelines_TimelineId",
                        column: x => x.TimelineId,
                        principalTable: "Timelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmyBattle",
                columns: table => new
                {
                    BattlesId = table.Column<int>(type: "int", nullable: false),
                    ParticipatingArmiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmyBattle", x => new { x.BattlesId, x.ParticipatingArmiesId });
                    table.ForeignKey(
                        name: "FK_ArmyBattle_Armies_ParticipatingArmiesId",
                        column: x => x.ParticipatingArmiesId,
                        principalTable: "Armies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmyBattle_Battles_BattlesId",
                        column: x => x.BattlesId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    UnitType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    IsElite = table.Column<bool>(type: "bit", nullable: false),
                    BelongsToArmyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryUnits_Armies_BelongsToArmyId",
                        column: x => x.BelongsToArmyId,
                        principalTable: "Armies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryUnits_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Corporations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IndustrySector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revenue = table.Column<double>(type: "float", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    IsPubliclyTraded = table.Column<bool>(type: "bit", nullable: false),
                    IsStateOwned = table.Column<bool>(type: "bit", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: true),
                    BankingSystemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corporations_BankingSystems_BankingSystemId",
                        column: x => x.BankingSystemId,
                        principalTable: "BankingSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Corporations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Corporations_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Corporations_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    GuildType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryActivity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGovernmentSanctioned = table.Column<bool>(type: "bit", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: true),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    LegalSystemId = table.Column<int>(type: "int", nullable: true),
                    EducationSystemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guilds_EducationSystems_EducationSystemId",
                        column: x => x.EducationSystemId,
                        principalTable: "EducationSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guilds_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guilds_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guilds_LegalSystems_LegalSystemId",
                        column: x => x.LegalSystemId,
                        principalTable: "LegalSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guilds_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeathDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    HairColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EyeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialPhysicalFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SapientSpeciesId = table.Column<int>(type: "int", nullable: false),
                    IsArtificial = table.Column<bool>(type: "bit", nullable: false),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    MotherId = table.Column<int>(type: "int", nullable: true),
                    NationId = table.Column<int>(type: "int", nullable: true),
                    SocialClassId = table.Column<int>(type: "int", nullable: true),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    Background_FamilyStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Background_Childhood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Background_Upbringing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Background_IsImmigrant = table.Column<bool>(type: "bit", nullable: false),
                    Background_MigrationHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_PersonalityTraits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_Motivations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_Virtues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_Flaws = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_PsychologicalProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_Fears = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personality_Ambitions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    CulturalInstitutionId = table.Column<int>(type: "int", nullable: true),
                    TimelineEventId = table.Column<int>(type: "int", nullable: true),
                    UniversityMajorId = table.Column<int>(type: "int", nullable: true),
                    UniversityMajorId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Characters_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Characters_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Creatures_SapientSpeciesId",
                        column: x => x.SapientSpeciesId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_CulturalInstitutions_CulturalInstitutionId",
                        column: x => x.CulturalInstitutionId,
                        principalTable: "CulturalInstitutions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_SocialClasses_SocialClassId",
                        column: x => x.SocialClassId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_TimelineEvents_TimelineEventId",
                        column: x => x.TimelineEventId,
                        principalTable: "TimelineEvents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_UniversityMajors_UniversityMajorId",
                        column: x => x.UniversityMajorId,
                        principalTable: "UniversityMajors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_UniversityMajors_UniversityMajorId1",
                        column: x => x.UniversityMajorId1,
                        principalTable: "UniversityMajors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MilitaryEquipmentMilitaryUnit",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    MilitaryUnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryEquipmentMilitaryUnit", x => new { x.EquipmentId, x.MilitaryUnitsId });
                    table.ForeignKey(
                        name: "FK_MilitaryEquipmentMilitaryUnit_MilitaryEquipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "MilitaryEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryEquipmentMilitaryUnit_MilitaryUnits_MilitaryUnitsId",
                        column: x => x.MilitaryUnitsId,
                        principalTable: "MilitaryUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    RankTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RankLevel = table.Column<int>(type: "int", nullable: false),
                    MilitaryUnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryRanks_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MilitaryRanks_MilitaryUnits_MilitaryUnitId",
                        column: x => x.MilitaryUnitId,
                        principalTable: "MilitaryUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityCorporation",
                columns: table => new
                {
                    CorporationsId = table.Column<int>(type: "int", nullable: false),
                    PresentInCitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCorporation", x => new { x.CorporationsId, x.PresentInCitiesId });
                    table.ForeignKey(
                        name: "FK_CityCorporation_Corporations_CorporationsId",
                        column: x => x.CorporationsId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCorporation_Locations_PresentInCitiesId",
                        column: x => x.PresentInCitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporateLeaderships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    IsMajorShareholder = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateLeaderships", x => x.Id);
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CorporateLeaderships_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CorporationCountry",
                columns: table => new
                {
                    CorporationsId = table.Column<int>(type: "int", nullable: false),
                    PresentInCountriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationCountry", x => new { x.CorporationsId, x.PresentInCountriesId });
                    table.ForeignKey(
                        name: "FK_CorporationCountry_Corporations_CorporationsId",
                        column: x => x.CorporationsId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationCountry_Locations_PresentInCountriesId",
                        column: x => x.PresentInCountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporationFaction",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    FinancialBackersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationFaction", x => new { x.FactionsId, x.FinancialBackersId });
                    table.ForeignKey(
                        name: "FK_CorporationFaction_Corporations_FinancialBackersId",
                        column: x => x.FinancialBackersId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationFaction_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporationProfession",
                columns: table => new
                {
                    CorporationsId = table.Column<int>(type: "int", nullable: false),
                    MemberProfessionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationProfession", x => new { x.CorporationsId, x.MemberProfessionsId });
                    table.ForeignKey(
                        name: "FK_CorporationProfession_Corporations_CorporationsId",
                        column: x => x.CorporationsId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationProfession_Professions_MemberProfessionsId",
                        column: x => x.MemberProfessionsId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apprenticeships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    DurationYears = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    SkillsTaught = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: true),
                    CorporationId = table.Column<int>(type: "int", nullable: true),
                    TradeSchoolId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apprenticeships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Schools_TradeSchoolId",
                        column: x => x.TradeSchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CityGuild",
                columns: table => new
                {
                    GuildsId = table.Column<int>(type: "int", nullable: false),
                    PresentInCitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityGuild", x => new { x.GuildsId, x.PresentInCitiesId });
                    table.ForeignKey(
                        name: "FK_CityGuild_Guilds_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityGuild_Locations_PresentInCitiesId",
                        column: x => x.PresentInCitiesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryGuild",
                columns: table => new
                {
                    GuildsId = table.Column<int>(type: "int", nullable: false),
                    PresentInCountriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryGuild", x => new { x.GuildsId, x.PresentInCountriesId });
                    table.ForeignKey(
                        name: "FK_CountryGuild_Guilds_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryGuild_Locations_PresentInCountriesId",
                        column: x => x.PresentInCountriesId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionGuild",
                columns: table => new
                {
                    ConnectedGuildsId = table.Column<int>(type: "int", nullable: false),
                    FactionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionGuild", x => new { x.ConnectedGuildsId, x.FactionsId });
                    table.ForeignKey(
                        name: "FK_FactionGuild_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionGuild_Guilds_ConnectedGuildsId",
                        column: x => x.ConnectedGuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildProfession",
                columns: table => new
                {
                    GuildsId = table.Column<int>(type: "int", nullable: false),
                    MemberProfessionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildProfession", x => new { x.GuildsId, x.MemberProfessionsId });
                    table.ForeignKey(
                        name: "FK_GuildProfession_Guilds_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildProfession_Professions_MemberProfessionsId",
                        column: x => x.MemberProfessionsId,
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    RankTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RankLevel = table.Column<int>(type: "int", nullable: false),
                    HasLeadershipAuthority = table.Column<bool>(type: "bit", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false)
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuildSocialClass",
                columns: table => new
                {
                    GuildsId = table.Column<int>(type: "int", nullable: false),
                    SocialClassesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildSocialClass", x => new { x.GuildsId, x.SocialClassesId });
                    table.ForeignKey(
                        name: "FK_GuildSocialClass_Guilds_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildSocialClass_SocialClasses_SocialClassesId",
                        column: x => x.SocialClassesId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityCharacter",
                columns: table => new
                {
                    AbilitiesId = table.Column<int>(type: "int", nullable: false),
                    CharactersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityCharacter", x => new { x.AbilitiesId, x.CharactersId });
                    table.ForeignKey(
                        name: "FK_AbilityCharacter_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityCharacter_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterClothing",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ClothingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClothing", x => new { x.CharacterId, x.ClothingId });
                    table.ForeignKey(
                        name: "FK_CharacterClothing_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClothing_CulturalClothings_ClothingId",
                        column: x => x.ClothingId,
                        principalTable: "CulturalClothings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFaction",
                columns: table => new
                {
                    FactionsId = table.Column<int>(type: "int", nullable: false),
                    MembersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFaction", x => new { x.FactionsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_CharacterFaction_Characters_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFaction_Factions_FactionsId",
                        column: x => x.FactionsId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterHobby",
                columns: table => new
                {
                    HobbiesId = table.Column<int>(type: "int", nullable: false),
                    PractitionersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterHobby", x => new { x.HobbiesId, x.PractitionersId });
                    table.ForeignKey(
                        name: "FK_CharacterHobby_Characters_PractitionersId",
                        column: x => x.PractitionersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterHobby_Hobbies_HobbiesId",
                        column: x => x.HobbiesId,
                        principalTable: "Hobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpecialisation",
                columns: table => new
                {
                    ExpertsId = table.Column<int>(type: "int", nullable: false),
                    SpecialisationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpecialisation", x => new { x.ExpertsId, x.SpecialisationsId });
                    table.ForeignKey(
                        name: "FK_CharacterSpecialisation_Characters_ExpertsId",
                        column: x => x.ExpertsId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpecialisation_Specialisations_SpecialisationsId",
                        column: x => x.SpecialisationsId,
                        principalTable: "Specialisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: true),
                    GuildId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationRecords_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationRecords_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationRecords_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationRecords_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationRecords_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactionType = table.Column<int>(type: "int", nullable: false),
                    IsUnique = table.Column<bool>(type: "bit", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentOwnerId = table.Column<int>(type: "int", nullable: true),
                    StoredAtId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Characters_CurrentOwnerId",
                        column: x => x.CurrentOwnerId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Locations_StoredAtId",
                        column: x => x.StoredAtId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_HistoryId",
                table: "Abilities",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityCharacter_CharactersId",
                table: "AbilityCharacter",
                column: "CharactersId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityLevels_AbilityId",
                table: "AbilityLevels",
                column: "AbilityId",
                unique: true,
                filter: "[AbilityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityLevels_HistoryId",
                table: "AbilityLevels",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_CorporationId",
                table: "Apprenticeships",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_GuildId",
                table: "Apprenticeships",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_HistoryId",
                table: "Apprenticeships",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_ProfessionId",
                table: "Apprenticeships",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_TradeSchoolId",
                table: "Apprenticeships",
                column: "TradeSchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyles_CultureId",
                table: "ArchitectureStyles",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyles_HistoryId",
                table: "ArchitectureStyles",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_CityId",
                table: "Armies",
                column: "CityId",
                unique: true,
                filter: "[CityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_FactionId",
                table: "Armies",
                column: "FactionId",
                unique: true,
                filter: "[FactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_HistoryId",
                table: "Armies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_MilitaryOrganizationId",
                table: "Armies",
                column: "MilitaryOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmyBattle_ParticipatingArmiesId",
                table: "ArmyBattle",
                column: "ParticipatingArmiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtForms_CultureId",
                table: "ArtForms",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtForms_HistoryId",
                table: "ArtForms",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BankingSystems_CurrencyId",
                table: "BankingSystems",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankingSystems_HistoryId",
                table: "BankingSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_HistoryId",
                table: "Battles",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_BookId",
                table: "Chapter",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClothing_ClothingId",
                table: "CharacterClothing",
                column: "ClothingId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFaction_MembersId",
                table: "CharacterFaction",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterHobby_PractitionersId",
                table: "CharacterHobby",
                column: "PractitionersId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationships_HistoryId",
                table: "CharacterRelationships",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterId",
                table: "Characters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CulturalInstitutionId",
                table: "Characters",
                column: "CulturalInstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FatherId",
                table: "Characters",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_HistoryId",
                table: "Characters",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_MotherId",
                table: "Characters",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_NationId",
                table: "Characters",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ProfessionId",
                table: "Characters",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ReligionId",
                table: "Characters",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SapientSpeciesId",
                table: "Characters",
                column: "SapientSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SocialClassId",
                table: "Characters",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_TimelineEventId",
                table: "Characters",
                column: "TimelineEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UniversityMajorId",
                table: "Characters",
                column: "UniversityMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UniversityMajorId1",
                table: "Characters",
                column: "UniversityMajorId1");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpecialisation_SpecialisationsId",
                table: "CharacterSpecialisation",
                column: "SpecialisationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCorporation_PresentInCitiesId",
                table: "CityCorporation",
                column: "PresentInCitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCreature_CreatureId",
                table: "CityCreature",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCulture_PredominantInCitiesId",
                table: "CityCulture",
                column: "PredominantInCitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CityGuild_PresentInCitiesId",
                table: "CityGuild",
                column: "PresentInCitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CityNation_NationsId",
                table: "CityNation",
                column: "NationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CityPoliticalParty_PoliticalPartiesId",
                table: "CityPoliticalParty",
                column: "PoliticalPartiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CityReligion_ReligionId",
                table: "CityReligion",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDetailClimateZone_ClimatesId",
                table: "ClimateDetailClimateZone",
                column: "ClimatesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDetails_HistoryId",
                table: "ClimateDetails",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZoneLocation_LocationsId",
                table: "ClimateZoneLocation",
                column: "LocationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZones_HistoryId",
                table: "ClimateZones",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZoneSeason_SeasonsId",
                table: "ClimateZoneSeason",
                column: "SeasonsId");

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
                name: "IX_CorporationCountry_PresentInCountriesId",
                table: "CorporationCountry",
                column: "PresentInCountriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationFaction_FinancialBackersId",
                table: "CorporationFaction",
                column: "FinancialBackersId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationProfession_MemberProfessionsId",
                table: "CorporationProfession",
                column: "MemberProfessionsId");

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
                name: "IX_Corporations_TaxationSystemId",
                table: "Corporations",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCulture_PredominantInCountriesId",
                table: "CountryCulture",
                column: "PredominantInCountriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryFaction_OperatesInCountriesId",
                table: "CountryFaction",
                column: "OperatesInCountriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryGuild_PresentInCountriesId",
                table: "CountryGuild",
                column: "PresentInCountriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryMilitaryOrganization_MilitaryOrganizationsId",
                table: "CountryMilitaryOrganization",
                column: "MilitaryOrganizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryNation_NationsId",
                table: "CountryNation",
                column: "NationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryPoliticalParty_PoliticalPartiesId",
                table: "CountryPoliticalParty",
                column: "PoliticalPartiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryReligion_ReligionsId",
                table: "CountryReligion",
                column: "ReligionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureEcosystem_NaturalHabitatsId",
                table: "CreatureEcosystem",
                column: "NaturalHabitatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_AnimalId",
                table: "Creatures",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_Fungus_AnimalId",
                table: "Creatures",
                column: "Fungus_AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_FungusId",
                table: "Creatures",
                column: "FungusId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_FungusId1",
                table: "Creatures",
                column: "FungusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_HistoryId",
                table: "Creatures",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_Plant_AnimalId",
                table: "Creatures",
                column: "Plant_AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_PlantId",
                table: "Creatures",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_PlantId1",
                table: "Creatures",
                column: "PlantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ReligionId",
                table: "Creatures",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ReligionId1",
                table: "Creatures",
                column: "ReligionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_CultureId",
                table: "Cuisines",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_HistoryId",
                table: "Cuisines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalClothings_CultureId",
                table: "CulturalClothings",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalClothings_HistoryId",
                table: "CulturalClothings",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalFestivals_CultureId",
                table: "CulturalFestivals",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalFestivals_HistoryId",
                table: "CulturalFestivals",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalFestivals_LocationId",
                table: "CulturalFestivals",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalInstitutions_CityId",
                table: "CulturalInstitutions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalInstitutions_CultureId",
                table: "CulturalInstitutions",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalInstitutions_HistoryId",
                table: "CulturalInstitutions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureNation_NationId",
                table: "CultureNation",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_HistoryId",
                table: "Cultures",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_LanguageId",
                table: "Cultures",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_ReligionId",
                table: "Cultures",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureSapientSpecies_PracticedBySpeciesId",
                table: "CultureSapientSpecies",
                column: "PracticedBySpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureSocialClass_InfluencedSocialClassesId",
                table: "CultureSocialClass",
                column: "InfluencedSocialClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_HistoryId",
                table: "Currencies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customs_CultureId",
                table: "Customs",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Customs_HistoryId",
                table: "Customs",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeityDeity_RivalDeitiesId",
                table: "DeityDeity",
                column: "RivalDeitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_DeityReligiousOrder_OrdersDedicatedToDeityId",
                table: "DeityReligiousOrder",
                column: "OrdersDedicatedToDeityId");

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
                name: "IX_EducationRecords_CharacterId",
                table: "EducationRecords",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_GuildId",
                table: "EducationRecords",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_HistoryId",
                table: "EducationRecords",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_SchoolId",
                table: "EducationRecords",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_UniversityId",
                table: "EducationRecords",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationSystems_HistoryId",
                table: "EducationSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeriesId",
                table: "Episode",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractionMethods_HistoryId",
                table: "ExtractionMethods",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionGuild_FactionsId",
                table: "FactionGuild",
                column: "FactionsId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionMilitaryOrganization_MilitaryConnectionsId",
                table: "FactionMilitaryOrganization",
                column: "MilitaryConnectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionPoliticalParty_PoliticalConnectionsId",
                table: "FactionPoliticalParty",
                column: "PoliticalConnectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionReligiousOrder_ReligiousConnectionsId",
                table: "FactionReligiousOrder",
                column: "ReligiousConnectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_HistoryId",
                table: "Factions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Folklores_CultureId",
                table: "Folklores",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Folklores_HistoryId",
                table: "Folklores",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FolkloreSapientSpecies_OriginatedFromSpeciesId",
                table: "FolkloreSapientSpecies",
                column: "OriginatedFromSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentSystems_HistoryId",
                table: "GovernmentSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentSystems_PoliticalIdeologyId",
                table: "GovernmentSystems",
                column: "PoliticalIdeologyId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildProfession_MemberProfessionsId",
                table: "GuildProfession",
                column: "MemberProfessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildRanks_GuildId",
                table: "GuildRanks",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildRanks_HistoryId",
                table: "GuildRanks",
                column: "HistoryId");

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
                name: "IX_GuildSocialClass_SocialClassesId",
                table: "GuildSocialClass",
                column: "SocialClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_HistoryId",
                table: "Hobbies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_DeityId",
                table: "HolySites",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_HistoryId",
                table: "HolySites",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_LocationId",
                table: "HolySites",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_ReligionId",
                table: "HolySites",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_CityId",
                table: "Industries",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_CountryId",
                table: "Industries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Industries_HistoryId",
                table: "Industries",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CurrentOwnerId",
                table: "Items",
                column: "CurrentOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_FactionId",
                table: "Items",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_HistoryId",
                table: "Items",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_StoredAtId",
                table: "Items",
                column: "StoredAtId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRanks_HistoryId",
                table: "JobRanks",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRanks_ProfessionId",
                table: "JobRanks",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageNation_NationsId",
                table: "LanguageNation",
                column: "NationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_HistoryId",
                table: "Languages",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalSystems_HistoryId",
                table: "LegalSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_HistoryId",
                table: "Libraries",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LocationId",
                table: "Libraries",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_UniversityId",
                table: "Libraries",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationNaturalResource_NaturalResourcesId",
                table: "LocationNaturalResource",
                column: "NaturalResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ArchitectureStyleId",
                table: "Locations",
                column: "ArchitectureStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_EconomicSystemId",
                table: "Locations",
                column: "City_EconomicSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_EducationSystemId",
                table: "Locations",
                column: "City_EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_GovernmentSystemId",
                table: "Locations",
                column: "City_GovernmentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_LegalSystemId",
                table: "Locations",
                column: "City_LegalSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId",
                table: "Locations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EconomicSystemId",
                table: "Locations",
                column: "EconomicSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EducationSystemId",
                table: "Locations",
                column: "EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_FactionId",
                table: "Locations",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_GovernmentSystemId",
                table: "Locations",
                column: "GovernmentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_HistoryId",
                table: "Locations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LegalSystemId",
                table: "Locations",
                column: "LegalSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationId",
                table: "Locations",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTradeRoute_TradeRoutesId",
                table: "LocationTradeRoute",
                column: "TradeRoutesId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryDoctrines_HistoryId",
                table: "MilitaryDoctrines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryEquipment_HistoryId",
                table: "MilitaryEquipment",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryEquipmentMilitaryUnit_MilitaryUnitsId",
                table: "MilitaryEquipmentMilitaryUnit",
                column: "MilitaryUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizations_HistoryId",
                table: "MilitaryOrganizations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizations_MilitaryDoctrineId",
                table: "MilitaryOrganizations",
                column: "MilitaryDoctrineId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRanks_HistoryId",
                table: "MilitaryRanks",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRanks_MilitaryUnitId",
                table: "MilitaryRanks",
                column: "MilitaryUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnits_BelongsToArmyId",
                table: "MilitaryUnits",
                column: "BelongsToArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnits_HistoryId",
                table: "MilitaryUnits",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_HistoryId",
                table: "Mutations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_MutantCreatureId",
                table: "Mutations",
                column: "MutantCreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_CultureId",
                table: "Myths",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_DeityId",
                table: "Myths",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_HistoryId",
                table: "Myths",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_ReligionId",
                table: "Myths",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_NationPoliticalParty_PoliticalPartiesId",
                table: "NationPoliticalParty",
                column: "PoliticalPartiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Nations_HistoryId",
                table: "Nations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Nations_SocialHierarchyId",
                table: "Nations",
                column: "SocialHierarchyId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResources_ExtractionMethodId",
                table: "NaturalResources",
                column: "ExtractionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResources_HistoryId",
                table: "NaturalResources",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalResourceTradeRoute_ResourcesTradedId",
                table: "NaturalResourceTradeRoute",
                column: "ResourcesTradedId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistories_HistoryId",
                table: "OwnershipHistories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalIdeologies_HistoryId",
                table: "PoliticalIdeologies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_GovernmentSystemId",
                table: "PoliticalParties",
                column: "GovernmentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_HistoryId",
                table: "PoliticalParties",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_PoliticalIdeologyId",
                table: "PoliticalParties",
                column: "PoliticalIdeologyId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeLaws_HistoryId",
                table: "PrivilegeLaws",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeLaws_SocialClassId",
                table: "PrivilegeLaws",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_HistoryId",
                table: "Professions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionSapientSpecies_PracticedBySpeciesId",
                table: "ProfessionSapientSpecies",
                column: "PracticedBySpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionSocialClass_TypicalProfessionsId",
                table: "ProfessionSocialClass",
                column: "TypicalProfessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionTradeSchool_TrainedProfessionsId",
                table: "ProfessionTradeSchool",
                column: "TrainedProfessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_HistoryId",
                table: "Races",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_SapientSpeciesId",
                table: "Races",
                column: "SapientSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reference_ChapterId",
                table: "Reference",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reference_ContentId",
                table: "Reference",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reference_EpisodeId",
                table: "Reference",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionSapientSpecies_OriginOfSapientSpeciesId",
                table: "RegionSapientSpecies",
                column: "OriginOfSapientSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Religions_HistoryId",
                table: "Religions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Religions_NationId",
                table: "Religions",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousEducations_HistoryId",
                table: "ReligiousEducations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousEducations_ReligionId",
                table: "ReligiousEducations",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousEducations_ReligiousOrderId",
                table: "ReligiousEducations",
                column: "ReligiousOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousFestivals_HistoryId",
                table: "ReligiousFestivals",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrders_HistoryId",
                table: "ReligiousOrders",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrders_ReligionId",
                table: "ReligiousOrders",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_DeityId",
                table: "ReligiousTexts",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_HistoryId",
                table: "ReligiousTexts",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_ReligionId",
                table: "ReligiousTexts",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_EducationSystemId",
                table: "Schools",
                column: "EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_HistoryId",
                table: "Schools",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_HistoryId",
                table: "SchoolSubjects",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_SchoolId",
                table: "SchoolSubjects",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_HistoryId",
                table: "Seasons",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialClasses_HistoryId",
                table: "SocialClasses",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialClasses_SocialHierarchyId",
                table: "SocialClasses",
                column: "SocialHierarchyId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialHierarchies_HistoryId",
                table: "SocialHierarchies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialisations_HistoryId",
                table: "Specialisations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialisations_ProfessionId",
                table: "Specialisations",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxationSystems_HistoryId",
                table: "TaxationSystems",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_BattleId",
                table: "TimelineEvents",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_FolkloreId",
                table: "TimelineEvents",
                column: "FolkloreId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_LocationId",
                table: "TimelineEvents",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_TimelineId",
                table: "TimelineEvents",
                column: "TimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Timelines_HistoryId",
                table: "Timelines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRoutes_HistoryId",
                table: "TradeRoutes",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Traditions_CultureId",
                table: "Traditions",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Traditions_HistoryId",
                table: "Traditions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Traditions_ReligionId",
                table: "Traditions",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_EducationSystemId",
                table: "Universities",
                column: "EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_HistoryId",
                table: "Universities",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityMajors_HistoryId",
                table: "UniversityMajors",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityMajors_UniversityId",
                table: "UniversityMajors",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherPatterns_ClimateZoneId",
                table: "WeatherPatterns",
                column: "ClimateZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherPatterns_HistoryId",
                table: "WeatherPatterns",
                column: "HistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityCharacter");

            migrationBuilder.DropTable(
                name: "AbilityLevels");

            migrationBuilder.DropTable(
                name: "Apprenticeships");

            migrationBuilder.DropTable(
                name: "ArmyBattle");

            migrationBuilder.DropTable(
                name: "ArtForms");

            migrationBuilder.DropTable(
                name: "CharacterClothing");

            migrationBuilder.DropTable(
                name: "CharacterFaction");

            migrationBuilder.DropTable(
                name: "CharacterHobby");

            migrationBuilder.DropTable(
                name: "CharacterRelationships");

            migrationBuilder.DropTable(
                name: "CharacterSpecialisation");

            migrationBuilder.DropTable(
                name: "CityCorporation");

            migrationBuilder.DropTable(
                name: "CityCreature");

            migrationBuilder.DropTable(
                name: "CityCulture");

            migrationBuilder.DropTable(
                name: "CityGuild");

            migrationBuilder.DropTable(
                name: "CityNation");

            migrationBuilder.DropTable(
                name: "CityPoliticalParty");

            migrationBuilder.DropTable(
                name: "CityReligion");

            migrationBuilder.DropTable(
                name: "ClimateDetailClimateZone");

            migrationBuilder.DropTable(
                name: "ClimateZoneLocation");

            migrationBuilder.DropTable(
                name: "ClimateZoneSeason");

            migrationBuilder.DropTable(
                name: "CorporateLeaderships");

            migrationBuilder.DropTable(
                name: "CorporationCountry");

            migrationBuilder.DropTable(
                name: "CorporationFaction");

            migrationBuilder.DropTable(
                name: "CorporationProfession");

            migrationBuilder.DropTable(
                name: "CountryCulture");

            migrationBuilder.DropTable(
                name: "CountryFaction");

            migrationBuilder.DropTable(
                name: "CountryGuild");

            migrationBuilder.DropTable(
                name: "CountryMilitaryOrganization");

            migrationBuilder.DropTable(
                name: "CountryNation");

            migrationBuilder.DropTable(
                name: "CountryPoliticalParty");

            migrationBuilder.DropTable(
                name: "CountryReligion");

            migrationBuilder.DropTable(
                name: "CreatureEcosystem");

            migrationBuilder.DropTable(
                name: "Cuisines");

            migrationBuilder.DropTable(
                name: "CulturalFestivals");

            migrationBuilder.DropTable(
                name: "CultureNation");

            migrationBuilder.DropTable(
                name: "CultureSapientSpecies");

            migrationBuilder.DropTable(
                name: "CultureSocialClass");

            migrationBuilder.DropTable(
                name: "Customs");

            migrationBuilder.DropTable(
                name: "DeityDeity");

            migrationBuilder.DropTable(
                name: "DeityReligiousOrder");

            migrationBuilder.DropTable(
                name: "EducationRecords");

            migrationBuilder.DropTable(
                name: "FactionGuild");

            migrationBuilder.DropTable(
                name: "FactionMilitaryOrganization");

            migrationBuilder.DropTable(
                name: "FactionPoliticalParty");

            migrationBuilder.DropTable(
                name: "FactionReligiousOrder");

            migrationBuilder.DropTable(
                name: "FolkloreSapientSpecies");

            migrationBuilder.DropTable(
                name: "GuildProfession");

            migrationBuilder.DropTable(
                name: "GuildRanks");

            migrationBuilder.DropTable(
                name: "GuildSocialClass");

            migrationBuilder.DropTable(
                name: "HolySites");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "JobRanks");

            migrationBuilder.DropTable(
                name: "LanguageNation");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "LocationNaturalResource");

            migrationBuilder.DropTable(
                name: "LocationTradeRoute");

            migrationBuilder.DropTable(
                name: "MilitaryEquipmentMilitaryUnit");

            migrationBuilder.DropTable(
                name: "MilitaryRanks");

            migrationBuilder.DropTable(
                name: "Mutations");

            migrationBuilder.DropTable(
                name: "Myths");

            migrationBuilder.DropTable(
                name: "NationPoliticalParty");

            migrationBuilder.DropTable(
                name: "NaturalResourceTradeRoute");

            migrationBuilder.DropTable(
                name: "OwnershipHistories");

            migrationBuilder.DropTable(
                name: "PrivilegeLaws");

            migrationBuilder.DropTable(
                name: "ProfessionSapientSpecies");

            migrationBuilder.DropTable(
                name: "ProfessionSocialClass");

            migrationBuilder.DropTable(
                name: "ProfessionTradeSchool");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "Reference");

            migrationBuilder.DropTable(
                name: "RegionSapientSpecies");

            migrationBuilder.DropTable(
                name: "ReligiousEducations");

            migrationBuilder.DropTable(
                name: "ReligiousFestivals");

            migrationBuilder.DropTable(
                name: "ReligiousTexts");

            migrationBuilder.DropTable(
                name: "SchoolSubjects");

            migrationBuilder.DropTable(
                name: "Traditions");

            migrationBuilder.DropTable(
                name: "WeatherPatterns");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "CulturalClothings");

            migrationBuilder.DropTable(
                name: "Hobbies");

            migrationBuilder.DropTable(
                name: "Specialisations");

            migrationBuilder.DropTable(
                name: "ClimateDetails");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Corporations");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "MilitaryEquipment");

            migrationBuilder.DropTable(
                name: "MilitaryUnits");

            migrationBuilder.DropTable(
                name: "PoliticalParties");

            migrationBuilder.DropTable(
                name: "NaturalResources");

            migrationBuilder.DropTable(
                name: "TradeRoutes");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "ReligiousOrders");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "ClimateZones");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "Creatures");

            migrationBuilder.DropTable(
                name: "CulturalInstitutions");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "SocialClasses");

            migrationBuilder.DropTable(
                name: "TimelineEvents");

            migrationBuilder.DropTable(
                name: "UniversityMajors");

            migrationBuilder.DropTable(
                name: "Armies");

            migrationBuilder.DropTable(
                name: "ExtractionMethods");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Folklores");

            migrationBuilder.DropTable(
                name: "Timelines");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MilitaryOrganizations");

            migrationBuilder.DropTable(
                name: "ArchitectureStyles");

            migrationBuilder.DropTable(
                name: "EconomicSystems");

            migrationBuilder.DropTable(
                name: "EducationSystems");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "GovernmentSystems");

            migrationBuilder.DropTable(
                name: "LegalSystems");

            migrationBuilder.DropTable(
                name: "MilitaryDoctrines");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "BankingSystems");

            migrationBuilder.DropTable(
                name: "TaxationSystems");

            migrationBuilder.DropTable(
                name: "PoliticalIdeologies");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Nations");

            migrationBuilder.DropTable(
                name: "SocialHierarchies");

            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
