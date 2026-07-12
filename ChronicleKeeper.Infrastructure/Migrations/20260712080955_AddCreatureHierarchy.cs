using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatureHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    ParentCreatureId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AverageLifespan = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    IsSentient = table.Column<bool>(type: "bit", nullable: false),
                    IsArtificial = table.Column<bool>(type: "bit", nullable: false),
                    ArtificialOrigin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatureSubtype = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Diet = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDomesticated = table.Column<bool>(type: "bit", nullable: true),
                    NumberOfLegs = table.Column<int>(type: "int", nullable: true),
                    HasWings = table.Column<bool>(type: "bit", nullable: true),
                    HasMultipleHeads = table.Column<bool>(type: "bit", nullable: true),
                    HasRegeneration = table.Column<bool>(type: "bit", nullable: true),
                    IsSacred = table.Column<bool>(type: "bit", nullable: true),
                    IsMythical = table.Column<bool>(type: "bit", nullable: true),
                    IsEndangered = table.Column<bool>(type: "bit", nullable: true),
                    Intelligence = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpecialAbilities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPackAnimal = table.Column<bool>(type: "bit", nullable: true),
                    IsAggressive = table.Column<bool>(type: "bit", nullable: true),
                    IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    ScientificName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsMedicinal = table.Column<bool>(type: "bit", nullable: true),
                    IsPoisonous = table.Column<bool>(type: "bit", nullable: true),
                    IsEdible = table.Column<bool>(type: "bit", nullable: true),
                    IsHallucinogenic = table.Column<bool>(type: "bit", nullable: true),
                    IsBioluminescent = table.Column<bool>(type: "bit", nullable: true),
                    HasMutagenicProperties = table.Column<bool>(type: "bit", nullable: true),
                    Fungus_IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    CanCommunicate = table.Column<bool>(type: "bit", nullable: true),
                    SpecialProperties = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MythologicalSignificance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PlantType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Plant_ScientificName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Plant_IsMedicinal = table.Column<bool>(type: "bit", nullable: true),
                    Plant_IsPoisonous = table.Column<bool>(type: "bit", nullable: true),
                    Sunlight = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PreferredSoil = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TemperatureRange = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Plant_IsBioluminescent = table.Column<bool>(type: "bit", nullable: true),
                    IsCarnivorous = table.Column<bool>(type: "bit", nullable: true),
                    HasRegenerativeProperties = table.Column<bool>(type: "bit", nullable: true),
                    CanMove = table.Column<bool>(type: "bit", nullable: true),
                    Plant_SpecialProperties = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Plant_MythologicalSignificance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Plant_IsSymbiotic = table.Column<bool>(type: "bit", nullable: true),
                    IsParasitic = table.Column<bool>(type: "bit", nullable: true),
                    YieldPerHectare = table.Column<double>(type: "float", nullable: true),
                    CropType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Crop_IsDomesticated = table.Column<bool>(type: "bit", nullable: true),
                    MaxHeight = table.Column<double>(type: "float", nullable: true),
                    Lifespan = table.Column<int>(type: "int", nullable: true),
                    LeafType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                    table.CheckConstraint("CK_Creatures_Parent_NotSelf", "[ParentCreatureId] <> [Id]");
                    table.ForeignKey(
                        name: "FK_Creatures_Creatures_ParentCreatureId",
                        column: x => x.ParentCreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Creatures_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Creatures_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreatureCities",
                columns: table => new
                {
                    CreatureId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureCities", x => new { x.CreatureId, x.CityId });
                    table.ForeignKey(
                        name: "FK_CreatureCities_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureCities_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatureCities_CityId",
                table: "CreatureCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_HistoryId",
                table: "Creatures",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ParentCreatureId",
                table: "Creatures",
                column: "ParentCreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_WorldId_Name",
                table: "Creatures",
                columns: new[] { "WorldId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreatureCities");

            migrationBuilder.DropTable(
                name: "Creatures");
        }
    }
}
