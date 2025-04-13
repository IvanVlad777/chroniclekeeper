using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeSomeEntitiesNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "SapientSpeciesId",
                table: "Characters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations",
                column: "City_LegalSystemId",
                principalTable: "LegalSystems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "SapientSpeciesId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Creatures_SapientSpeciesId",
                table: "Characters",
                column: "SapientSpeciesId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations",
                column: "City_LegalSystemId",
                principalTable: "LegalSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
