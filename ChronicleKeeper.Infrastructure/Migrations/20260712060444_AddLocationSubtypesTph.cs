using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationSubtypesTph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Schools",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "City_EducationSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "City_GovernmentSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "City_LegalSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContinentSpecifics",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictType",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EducationSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernmentSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCapital",
                table: "Locations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegalSystemId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationSubtype",
                table: "Locations",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            // Backfill existing rows: EF's discriminator map has no entry for the "" default, so every
            // pre-existing Location would fail to materialize on the next read without this. Rows already
            // tagged with one of the 5 hierarchy Types become that subtype; everything else stays plain.
            migrationBuilder.Sql(@"
                UPDATE [Locations]
                SET [LocationSubtype] = CASE
                    WHEN [Type] IN ('Continent','Region','Country','City','District') THEN [Type]
                    ELSE 'Location'
                END");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryNationId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionSpecifics",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegionSapientSpecies",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    SapientSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionSapientSpecies", x => new { x.RegionId, x.SapientSpeciesId });
                    table.ForeignKey(
                        name: "FK_RegionSapientSpecies_Locations_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionSapientSpecies_SapientSpecies_SapientSpeciesId",
                        column: x => x.SapientSpeciesId,
                        principalTable: "SapientSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_LocationId",
                table: "Schools",
                column: "LocationId");

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
                name: "IX_Locations_EducationSystemId",
                table: "Locations",
                column: "EducationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_GovernmentSystemId",
                table: "Locations",
                column: "GovernmentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LegalSystemId",
                table: "Locations",
                column: "LegalSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionSapientSpecies_SapientSpeciesId",
                table: "RegionSapientSpecies",
                column: "SapientSpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_EducationSystems_City_EducationSystemId",
                table: "Locations",
                column: "City_EducationSystemId",
                principalTable: "EducationSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_EducationSystems_EducationSystemId",
                table: "Locations",
                column: "EducationSystemId",
                principalTable: "EducationSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_GovernmentSystems_City_GovernmentSystemId",
                table: "Locations",
                column: "City_GovernmentSystemId",
                principalTable: "GovernmentSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_GovernmentSystems_GovernmentSystemId",
                table: "Locations",
                column: "GovernmentSystemId",
                principalTable: "GovernmentSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations",
                column: "City_LegalSystemId",
                principalTable: "LegalSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_LegalSystems_LegalSystemId",
                table: "Locations",
                column: "LegalSystemId",
                principalTable: "LegalSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Locations_LocationId",
                table: "Schools",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_EducationSystems_City_EducationSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_EducationSystems_EducationSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_GovernmentSystems_City_GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_GovernmentSystems_GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_LegalSystems_City_LegalSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_LegalSystems_LegalSystemId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Locations_LocationId",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "RegionSapientSpecies");

            migrationBuilder.DropIndex(
                name: "IX_Schools_LocationId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Locations_City_EducationSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_City_GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_City_LegalSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_EducationSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_LegalSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "City_EducationSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "City_GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "City_LegalSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ContinentSpecifics",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DistrictType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "EducationSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GovernmentSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsCapital",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LegalSystemId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationSubtype",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PrimaryNationId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RegionSpecifics",
                table: "Locations");
        }
    }
}
