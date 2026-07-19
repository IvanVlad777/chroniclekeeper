using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityMajorAndEconomyCityLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityCurrencies",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCurrencies", x => new { x.CityId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CityCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCurrencies_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityTaxationSystems",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityTaxationSystems", x => new { x.CityId, x.TaxationSystemId });
                    table.ForeignKey(
                        name: "FK_CityTaxationSystems_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityTaxationSystems_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryCurrencies",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryCurrencies", x => new { x.CountryId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CountryCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryCurrencies_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryTaxationSystems",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    TaxationSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryTaxationSystems", x => new { x.CountryId, x.TaxationSystemId });
                    table.ForeignKey(
                        name: "FK_CountryTaxationSystems_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryTaxationSystems_TaxationSystems_TaxationSystemId",
                        column: x => x.TaxationSystemId,
                        principalTable: "TaxationSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversityMajorProfessors",
                columns: table => new
                {
                    UniversityMajorId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityMajorProfessors", x => new { x.UniversityMajorId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_UniversityMajorProfessors_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversityMajorProfessors_UniversityMajors_UniversityMajorId",
                        column: x => x.UniversityMajorId,
                        principalTable: "UniversityMajors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversityMajorStudents",
                columns: table => new
                {
                    UniversityMajorId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityMajorStudents", x => new { x.UniversityMajorId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_UniversityMajorStudents_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversityMajorStudents_UniversityMajors_UniversityMajorId",
                        column: x => x.UniversityMajorId,
                        principalTable: "UniversityMajors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityCurrencies_CurrencyId",
                table: "CityCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CityTaxationSystems_TaxationSystemId",
                table: "CityTaxationSystems",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCurrencies_CurrencyId",
                table: "CountryCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryTaxationSystems_TaxationSystemId",
                table: "CountryTaxationSystems",
                column: "TaxationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityMajorProfessors_CharacterId",
                table: "UniversityMajorProfessors",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityMajorStudents_CharacterId",
                table: "UniversityMajorStudents",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityCurrencies");

            migrationBuilder.DropTable(
                name: "CityTaxationSystems");

            migrationBuilder.DropTable(
                name: "CountryCurrencies");

            migrationBuilder.DropTable(
                name: "CountryTaxationSystems");

            migrationBuilder.DropTable(
                name: "UniversityMajorProfessors");

            migrationBuilder.DropTable(
                name: "UniversityMajorStudents");
        }
    }
}
