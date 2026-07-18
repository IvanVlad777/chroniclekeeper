using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSapientSpeciesToCreatureHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_SapientSpecies_SapientSpeciesId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_CultureSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "CultureSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_FolkloreSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "FolkloreSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "ProfessionSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_SapientSpecies_SapientSpeciesId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "RegionSapientSpecies");

            migrationBuilder.DropTable(
                name: "SapientSpecies");

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Creatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHumanoid",
                table: "Creatures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SapientType",
                table: "Creatures",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sapient_Lifespan",
                table: "Creatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sapient_ScientificName",
                table: "Creatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CultureSapientSpecies_Creatures_SapientSpeciesId",
                table: "CultureSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FolkloreSapientSpecies_Creatures_SapientSpeciesId",
                table: "FolkloreSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionSapientSpecies_Creatures_SapientSpeciesId",
                table: "ProfessionSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Creatures_SapientSpeciesId",
                table: "Races",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionSapientSpecies_Creatures_SapientSpeciesId",
                table: "RegionSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_CultureSapientSpecies_Creatures_SapientSpeciesId",
                table: "CultureSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_FolkloreSapientSpecies_Creatures_SapientSpeciesId",
                table: "FolkloreSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionSapientSpecies_Creatures_SapientSpeciesId",
                table: "ProfessionSapientSpecies");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Creatures_SapientSpeciesId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionSapientSpecies_Creatures_SapientSpeciesId",
                table: "RegionSapientSpecies");

            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "IsHumanoid",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "SapientType",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Sapient_Lifespan",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Sapient_ScientificName",
                table: "Creatures");

            migrationBuilder.CreateTable(
                name: "SapientSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CommonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsHumanoid = table.Column<bool>(type: "bit", nullable: false),
                    Lifespan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapientSpecies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SapientSpecies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SapientSpecies_WorldId_Name",
                table: "SapientSpecies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_SapientSpecies_SapientSpeciesId",
                table: "Characters",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CultureSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "CultureSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FolkloreSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "FolkloreSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "ProfessionSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_SapientSpecies_SapientSpeciesId",
                table: "Races",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionSapientSpecies_SapientSpecies_SapientSpeciesId",
                table: "RegionSapientSpecies",
                column: "SapientSpeciesId",
                principalTable: "SapientSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
