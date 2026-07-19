using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryCityCrossLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityCorporations",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCorporations", x => new { x.CityId, x.CorporationId });
                    table.ForeignKey(
                        name: "FK_CityCorporations_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCorporations_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityCulturalInstitutions",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CulturalInstitutionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCulturalInstitutions", x => new { x.CityId, x.CulturalInstitutionId });
                    table.ForeignKey(
                        name: "FK_CityCulturalInstitutions_CulturalInstitutions_CulturalInstitutionId",
                        column: x => x.CulturalInstitutionId,
                        principalTable: "CulturalInstitutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCulturalInstitutions_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityCultures",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCultures", x => new { x.CityId, x.CultureId });
                    table.ForeignKey(
                        name: "FK_CityCultures_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCultures_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityGuilds",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityGuilds", x => new { x.CityId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_CityGuilds_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityGuilds_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityIndustries",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityIndustries", x => new { x.CityId, x.IndustryId });
                    table.ForeignKey(
                        name: "FK_CityIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityIndustries_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityNations",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityNations", x => new { x.CityId, x.NationId });
                    table.ForeignKey(
                        name: "FK_CityNations_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityNations_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityPoliticalParties",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityPoliticalParties", x => new { x.CityId, x.PoliticalPartyId });
                    table.ForeignKey(
                        name: "FK_CityPoliticalParties_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityPoliticalParties_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityReligions",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityReligions", x => new { x.CityId, x.ReligionId });
                    table.ForeignKey(
                        name: "FK_CityReligions_Locations_CityId",
                        column: x => x.CityId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityReligions_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryCorporations",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryCorporations", x => new { x.CountryId, x.CorporationId });
                    table.ForeignKey(
                        name: "FK_CountryCorporations_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryCorporations_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryCultures",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CultureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryCultures", x => new { x.CountryId, x.CultureId });
                    table.ForeignKey(
                        name: "FK_CountryCultures_Cultures_CultureId",
                        column: x => x.CultureId,
                        principalTable: "Cultures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryCultures_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryFactions",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryFactions", x => new { x.CountryId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_CountryFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryFactions_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryGuilds",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryGuilds", x => new { x.CountryId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_CountryGuilds_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryGuilds_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryIndustries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryIndustries", x => new { x.CountryId, x.IndustryId });
                    table.ForeignKey(
                        name: "FK_CountryIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryIndustries_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryNations",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryNations", x => new { x.CountryId, x.NationId });
                    table.ForeignKey(
                        name: "FK_CountryNations_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryNations_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryPoliticalParties",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryPoliticalParties", x => new { x.CountryId, x.PoliticalPartyId });
                    table.ForeignKey(
                        name: "FK_CountryPoliticalParties_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryPoliticalParties_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryReligions",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryReligions", x => new { x.CountryId, x.ReligionId });
                    table.ForeignKey(
                        name: "FK_CountryReligions_Locations_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryReligions_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityCorporations_CorporationId",
                table: "CityCorporations",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCulturalInstitutions_CulturalInstitutionId",
                table: "CityCulturalInstitutions",
                column: "CulturalInstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCultures_CultureId",
                table: "CityCultures",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CityGuilds_GuildId",
                table: "CityGuilds",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_CityIndustries_IndustryId",
                table: "CityIndustries",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_CityNations_NationId",
                table: "CityNations",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_CityPoliticalParties_PoliticalPartyId",
                table: "CityPoliticalParties",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CityReligions_ReligionId",
                table: "CityReligions",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCorporations_CorporationId",
                table: "CountryCorporations",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryCultures_CultureId",
                table: "CountryCultures",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryFactions_FactionId",
                table: "CountryFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryGuilds_GuildId",
                table: "CountryGuilds",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryIndustries_IndustryId",
                table: "CountryIndustries",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryNations_NationId",
                table: "CountryNations",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryPoliticalParties_PoliticalPartyId",
                table: "CountryPoliticalParties",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryReligions_ReligionId",
                table: "CountryReligions",
                column: "ReligionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityCorporations");

            migrationBuilder.DropTable(
                name: "CityCulturalInstitutions");

            migrationBuilder.DropTable(
                name: "CityCultures");

            migrationBuilder.DropTable(
                name: "CityGuilds");

            migrationBuilder.DropTable(
                name: "CityIndustries");

            migrationBuilder.DropTable(
                name: "CityNations");

            migrationBuilder.DropTable(
                name: "CityPoliticalParties");

            migrationBuilder.DropTable(
                name: "CityReligions");

            migrationBuilder.DropTable(
                name: "CountryCorporations");

            migrationBuilder.DropTable(
                name: "CountryCultures");

            migrationBuilder.DropTable(
                name: "CountryFactions");

            migrationBuilder.DropTable(
                name: "CountryGuilds");

            migrationBuilder.DropTable(
                name: "CountryIndustries");

            migrationBuilder.DropTable(
                name: "CountryNations");

            migrationBuilder.DropTable(
                name: "CountryPoliticalParties");

            migrationBuilder.DropTable(
                name: "CountryReligions");
        }
    }
}
