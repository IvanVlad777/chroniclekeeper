using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMilitaryEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    BattleDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Battles_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryDoctrines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Strategy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Philosophy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrioritizesInfantry = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesCavalry = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesArtillery = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesNavalForces = table.Column<bool>(type: "bit", nullable: false),
                    PrioritizesAirForces = table.Column<bool>(type: "bit", nullable: false),
                    RequiresHeavyIndustry = table.Column<bool>(type: "bit", nullable: false),
                    UsesMercenaries = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryDoctrines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryDoctrines_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryDoctrines_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    EquipmentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryEquipments_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryEquipments_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryOrganizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MilitaryDoctrineId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizations_MilitaryDoctrines_MilitaryDoctrineId",
                        column: x => x.MilitaryDoctrineId,
                        principalTable: "MilitaryDoctrines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizations_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Armies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    IsStandingArmy = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    MilitaryOrganizationId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armies_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Armies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Armies_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Armies_MilitaryOrganizations_MilitaryOrganizationId",
                        column: x => x.MilitaryOrganizationId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Armies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryOrganizationCountries",
                columns: table => new
                {
                    MilitaryOrganizationId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryOrganizationCountries", x => new { x.MilitaryOrganizationId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizationCountries_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizationCountries_MilitaryOrganizations_MilitaryOrganizationId",
                        column: x => x.MilitaryOrganizationId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryOrganizationFactions",
                columns: table => new
                {
                    MilitaryOrganizationId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryOrganizationFactions", x => new { x.MilitaryOrganizationId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizationFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryOrganizationFactions_MilitaryOrganizations_MilitaryOrganizationId",
                        column: x => x.MilitaryOrganizationId,
                        principalTable: "MilitaryOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmyBattles",
                columns: table => new
                {
                    ArmyId = table.Column<int>(type: "int", nullable: false),
                    BattleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmyBattles", x => new { x.ArmyId, x.BattleId });
                    table.ForeignKey(
                        name: "FK_ArmyBattles_Armies_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Armies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmyBattles_Battles_BattleId",
                        column: x => x.BattleId,
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
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    UnitType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    IsElite = table.Column<bool>(type: "bit", nullable: false),
                    BelongsToArmyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryUnits_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    RankTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RankLevel = table.Column<int>(type: "int", nullable: false),
                    MilitaryUnitId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryRanks_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MilitaryRanks_MilitaryUnits_MilitaryUnitId",
                        column: x => x.MilitaryUnitId,
                        principalTable: "MilitaryUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryRanks_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryUnitEquipments",
                columns: table => new
                {
                    MilitaryUnitId = table.Column<int>(type: "int", nullable: false),
                    MilitaryEquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryUnitEquipments", x => new { x.MilitaryUnitId, x.MilitaryEquipmentId });
                    table.ForeignKey(
                        name: "FK_MilitaryUnitEquipments_MilitaryEquipments_MilitaryEquipmentId",
                        column: x => x.MilitaryEquipmentId,
                        principalTable: "MilitaryEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MilitaryUnitEquipments_MilitaryUnits_MilitaryUnitId",
                        column: x => x.MilitaryUnitId,
                        principalTable: "MilitaryUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armies_CityId",
                table: "Armies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_FactionId",
                table: "Armies",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_HistoryId",
                table: "Armies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_MilitaryOrganizationId",
                table: "Armies",
                column: "MilitaryOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_WorldId_Name",
                table: "Armies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ArmyBattles_BattleId",
                table: "ArmyBattles",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_HistoryId",
                table: "Battles",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_WorldId_Name",
                table: "Battles",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryDoctrines_HistoryId",
                table: "MilitaryDoctrines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryDoctrines_WorldId_Name",
                table: "MilitaryDoctrines",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryEquipments_HistoryId",
                table: "MilitaryEquipments",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryEquipments_WorldId_Name",
                table: "MilitaryEquipments",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizationCountries_CountryId",
                table: "MilitaryOrganizationCountries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizationFactions_FactionId",
                table: "MilitaryOrganizationFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizations_HistoryId",
                table: "MilitaryOrganizations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizations_MilitaryDoctrineId",
                table: "MilitaryOrganizations",
                column: "MilitaryDoctrineId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryOrganizations_WorldId_Name",
                table: "MilitaryOrganizations",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRanks_HistoryId",
                table: "MilitaryRanks",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRanks_MilitaryUnitId",
                table: "MilitaryRanks",
                column: "MilitaryUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryRanks_WorldId_Name",
                table: "MilitaryRanks",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnitEquipments_MilitaryEquipmentId",
                table: "MilitaryUnitEquipments",
                column: "MilitaryEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnits_BelongsToArmyId",
                table: "MilitaryUnits",
                column: "BelongsToArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnits_HistoryId",
                table: "MilitaryUnits",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryUnits_WorldId_Name",
                table: "MilitaryUnits",
                columns: new[] { "WorldId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmyBattles");

            migrationBuilder.DropTable(
                name: "MilitaryOrganizationCountries");

            migrationBuilder.DropTable(
                name: "MilitaryOrganizationFactions");

            migrationBuilder.DropTable(
                name: "MilitaryRanks");

            migrationBuilder.DropTable(
                name: "MilitaryUnitEquipments");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "MilitaryEquipments");

            migrationBuilder.DropTable(
                name: "MilitaryUnits");

            migrationBuilder.DropTable(
                name: "Armies");

            migrationBuilder.DropTable(
                name: "MilitaryOrganizations");

            migrationBuilder.DropTable(
                name: "MilitaryDoctrines");
        }
    }
}
