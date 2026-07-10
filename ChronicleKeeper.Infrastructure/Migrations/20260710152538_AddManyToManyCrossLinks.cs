using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyCrossLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CultureNations",
                columns: table => new
                {
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureNations", x => new { x.CultureId, x.NationId });
                    table.ForeignKey(
                        name: "FK_CultureNations_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureNations_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultureSapientSpecies",
                columns: table => new
                {
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    SapientSpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureSapientSpecies", x => new { x.CultureId, x.SapientSpeciesId });
                    table.ForeignKey(
                        name: "FK_CultureSapientSpecies_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureSapientSpecies_SapientSpecies_SapientSpeciesId",
                        column: x => x.SapientSpeciesId,
                        principalTable: "SapientSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CultureSocialClasses",
                columns: table => new
                {
                    CultureId = table.Column<int>(type: "int", nullable: false),
                    SocialClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultureSocialClasses", x => new { x.CultureId, x.SocialClassId });
                    table.ForeignKey(
                        name: "FK_CultureSocialClasses_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CultureSocialClasses_SocialClasses_SocialClassId",
                        column: x => x.SocialClassId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageNations",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageNations", x => new { x.LanguageId, x.NationId });
                    table.ForeignKey(
                        name: "FK_LanguageNations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageNations_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalPartyFactions",
                columns: table => new
                {
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalPartyFactions", x => new { x.PoliticalPartyId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_PoliticalPartyFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoliticalPartyFactions_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalPartyNations",
                columns: table => new
                {
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalPartyNations", x => new { x.PoliticalPartyId, x.NationId });
                    table.ForeignKey(
                        name: "FK_PoliticalPartyNations_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoliticalPartyNations_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CultureNations_NationId",
                table: "CultureNations",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureSapientSpecies_SapientSpeciesId",
                table: "CultureSapientSpecies",
                column: "SapientSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_CultureSocialClasses_SocialClassId",
                table: "CultureSocialClasses",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageNations_NationId",
                table: "LanguageNations",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalPartyFactions_FactionId",
                table: "PoliticalPartyFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalPartyNations_NationId",
                table: "PoliticalPartyNations",
                column: "NationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CultureNations");

            migrationBuilder.DropTable(
                name: "CultureSapientSpecies");

            migrationBuilder.DropTable(
                name: "CultureSocialClasses");

            migrationBuilder.DropTable(
                name: "LanguageNations");

            migrationBuilder.DropTable(
                name: "PoliticalPartyFactions");

            migrationBuilder.DropTable(
                name: "PoliticalPartyNations");
        }
    }
}
