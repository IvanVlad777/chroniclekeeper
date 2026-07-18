using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCultureDetailEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchitectureStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MaterialsUsed = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DesignFeatures = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsFortified = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArchitectureStyles_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NotableArtists = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HistoricalInfluences = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArtForms_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clothing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ClothingType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Materials = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DesignFeatures = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRitualistic = table.Column<bool>(type: "bit", nullable: false),
                    IsArmor = table.Column<bool>(type: "bit", nullable: false),
                    SpecialProperties = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clothing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clothing_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clothing_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clothing_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cuisines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MainIngredients = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CookingMethods = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    TypicalDishes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cuisines_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CulturalFestivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Activities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsNationalHoliday = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CulturalFestivals_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CulturalFestivals_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CulturalInstitutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    InstitutionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsGovernmentFunded = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalInstitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutions_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsUniversal = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customs_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Customs_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Customs_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Folktales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Story = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Moral = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsHistorical = table.Column<bool>(type: "bit", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folktales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folktales_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folktales_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Folktales_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Myths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    CreationStory = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Symbolism = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HasReligiousConnections = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Myths", x => x.Id);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Myths_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Myths_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Traditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Practice = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Traditions_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Traditions_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArchitectureStyleLocations",
                columns: table => new
                {
                    ArchitectureStyleId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchitectureStyleLocations", x => new { x.ArchitectureStyleId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_ArchitectureStyleLocations_ArchitectureStyles_ArchitectureStyleId",
                        column: x => x.ArchitectureStyleId,
                        principalTable: "ArchitectureStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArchitectureStyleLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FolkloreSapientSpecies",
                columns: table => new
                {
                    FolkloreId = table.Column<int>(type: "int", nullable: false),
                    SapientSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolkloreSapientSpecies", x => new { x.FolkloreId, x.SapientSpeciesId });
                    table.ForeignKey(
                        name: "FK_FolkloreSapientSpecies_Folktales_FolkloreId",
                        column: x => x.FolkloreId,
                        principalTable: "Folktales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolkloreSapientSpecies_SapientSpecies_SapientSpeciesId",
                        column: x => x.SapientSpeciesId,
                        principalTable: "SapientSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FolkloreTimelineEvents",
                columns: table => new
                {
                    FolkloreId = table.Column<int>(type: "int", nullable: false),
                    TimelineEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolkloreTimelineEvents", x => new { x.FolkloreId, x.TimelineEventId });
                    table.ForeignKey(
                        name: "FK_FolkloreTimelineEvents_Folktales_FolkloreId",
                        column: x => x.FolkloreId,
                        principalTable: "Folktales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolkloreTimelineEvents_TimelineEvents_TimelineEventId",
                        column: x => x.TimelineEventId,
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyleLocations_LocationId",
                table: "ArchitectureStyleLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyles_CultureId",
                table: "ArchitectureStyles",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyles_HistoryId",
                table: "ArchitectureStyles",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectureStyles_WorldId_Name",
                table: "ArchitectureStyles",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ArtForms_CultureId",
                table: "ArtForms",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtForms_HistoryId",
                table: "ArtForms",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtForms_WorldId_Name",
                table: "ArtForms",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Clothing_CultureId",
                table: "Clothing",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Clothing_HistoryId",
                table: "Clothing",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Clothing_WorldId_Name",
                table: "Clothing",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_CultureId",
                table: "Cuisines",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_HistoryId",
                table: "Cuisines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_WorldId_Name",
                table: "Cuisines",
                columns: new[] { "WorldId", "Name" });

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
                name: "IX_CulturalFestivals_WorldId_Name",
                table: "CulturalFestivals",
                columns: new[] { "WorldId", "Name" });

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
                name: "IX_CulturalInstitutions_WorldId_Name",
                table: "CulturalInstitutions",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Customs_CultureId",
                table: "Customs",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Customs_HistoryId",
                table: "Customs",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customs_WorldId_Name",
                table: "Customs",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FolkloreSapientSpecies_SapientSpeciesId",
                table: "FolkloreSapientSpecies",
                column: "SapientSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_FolkloreTimelineEvents_TimelineEventId",
                table: "FolkloreTimelineEvents",
                column: "TimelineEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Folktales_CultureId",
                table: "Folktales",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Folktales_HistoryId",
                table: "Folktales",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Folktales_WorldId_Name",
                table: "Folktales",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Myths_CultureId",
                table: "Myths",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_HistoryId",
                table: "Myths",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_ReligionId",
                table: "Myths",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_WorldId_Name",
                table: "Myths",
                columns: new[] { "WorldId", "Name" });

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
                name: "IX_Traditions_WorldId_Name",
                table: "Traditions",
                columns: new[] { "WorldId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchitectureStyleLocations");

            migrationBuilder.DropTable(
                name: "ArtForms");

            migrationBuilder.DropTable(
                name: "Clothing");

            migrationBuilder.DropTable(
                name: "Cuisines");

            migrationBuilder.DropTable(
                name: "CulturalFestivals");

            migrationBuilder.DropTable(
                name: "CulturalInstitutions");

            migrationBuilder.DropTable(
                name: "Customs");

            migrationBuilder.DropTable(
                name: "FolkloreSapientSpecies");

            migrationBuilder.DropTable(
                name: "FolkloreTimelineEvents");

            migrationBuilder.DropTable(
                name: "Myths");

            migrationBuilder.DropTable(
                name: "Traditions");

            migrationBuilder.DropTable(
                name: "ArchitectureStyles");

            migrationBuilder.DropTable(
                name: "Folktales");
        }
    }
}
