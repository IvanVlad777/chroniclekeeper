using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEcosystemHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CaveDepth",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaveKind",
                table: "Locations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesertKind",
                table: "Locations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForestKind",
                table: "Locations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrasslandKind",
                table: "Locations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreshwater",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaltwater",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxDepth",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxElevation",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MountainRangeLength",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MouthLocationId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Prominence",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RiverLength",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceLocationId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueFeatures",
                table: "Locations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WaterDepth",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreatureEcosystems",
                columns: table => new
                {
                    CreatureId = table.Column<int>(type: "int", nullable: false),
                    EcosystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureEcosystems", x => new { x.CreatureId, x.EcosystemId });
                    table.ForeignKey(
                        name: "FK_CreatureEcosystems_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureEcosystems_Locations_EcosystemId",
                        column: x => x.EcosystemId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_MouthLocationId",
                table: "Locations",
                column: "MouthLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SourceLocationId",
                table: "Locations",
                column: "SourceLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureEcosystems_EcosystemId",
                table: "CreatureEcosystems",
                column: "EcosystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_MouthLocationId",
                table: "Locations",
                column: "MouthLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_SourceLocationId",
                table: "Locations",
                column: "SourceLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_MouthLocationId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_SourceLocationId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "CreatureEcosystems");

            migrationBuilder.DropIndex(
                name: "IX_Locations_MouthLocationId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_SourceLocationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CaveDepth",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CaveKind",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DesertKind",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ForestKind",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GrasslandKind",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsFreshwater",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsSaltwater",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MaxDepth",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MaxElevation",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MountainRangeLength",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MouthLocationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Prominence",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RiverLength",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SourceLocationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UniqueFeatures",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WaterDepth",
                table: "Locations");
        }
    }
}
