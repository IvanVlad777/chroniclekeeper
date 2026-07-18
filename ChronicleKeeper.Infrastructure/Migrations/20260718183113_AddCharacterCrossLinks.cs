using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterCrossLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterClothing",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ClothingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClothing", x => new { x.CharacterId, x.ClothingId });
                    table.ForeignKey(
                        name: "FK_CharacterClothing_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClothing_Clothing_ClothingId",
                        column: x => x.ClothingId,
                        principalTable: "Clothing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterHobbies",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    HobbyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterHobbies", x => new { x.CharacterId, x.HobbyId });
                    table.ForeignKey(
                        name: "FK_CharacterHobbies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterHobbies_Hobbies_HobbyId",
                        column: x => x.HobbyId,
                        principalTable: "Hobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpecialisations",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    SpecialisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpecialisations", x => new { x.CharacterId, x.SpecialisationId });
                    table.ForeignKey(
                        name: "FK_CharacterSpecialisations_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpecialisations_Specialisations_SpecialisationId",
                        column: x => x.SpecialisationId,
                        principalTable: "Specialisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CulturalInstitutionArtists",
                columns: table => new
                {
                    CulturalInstitutionId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalInstitutionArtists", x => new { x.CulturalInstitutionId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_CulturalInstitutionArtists_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CulturalInstitutionArtists_CulturalInstitutions_CulturalInstitutionId",
                        column: x => x.CulturalInstitutionId,
                        principalTable: "CulturalInstitutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryScholars",
                columns: table => new
                {
                    LibraryId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryScholars", x => new { x.LibraryId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_LibraryScholars_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryScholars_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolStudents",
                columns: table => new
                {
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolStudents", x => new { x.SchoolId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_SchoolStudents_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolStudents_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTeachers",
                columns: table => new
                {
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTeachers", x => new { x.SchoolId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_SchoolTeachers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTeachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversityProfessors",
                columns: table => new
                {
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityProfessors", x => new { x.UniversityId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_UniversityProfessors_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversityProfessors_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniversityStudents",
                columns: table => new
                {
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityStudents", x => new { x.UniversityId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_UniversityStudents_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniversityStudents_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClothing_ClothingId",
                table: "CharacterClothing",
                column: "ClothingId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterHobbies_HobbyId",
                table: "CharacterHobbies",
                column: "HobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpecialisations_SpecialisationId",
                table: "CharacterSpecialisations",
                column: "SpecialisationId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalInstitutionArtists_CharacterId",
                table: "CulturalInstitutionArtists",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryScholars_CharacterId",
                table: "LibraryScholars",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStudents_CharacterId",
                table: "SchoolStudents",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTeachers_CharacterId",
                table: "SchoolTeachers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityProfessors_CharacterId",
                table: "UniversityProfessors",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityStudents_CharacterId",
                table: "UniversityStudents",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClothing");

            migrationBuilder.DropTable(
                name: "CharacterHobbies");

            migrationBuilder.DropTable(
                name: "CharacterSpecialisations");

            migrationBuilder.DropTable(
                name: "CulturalInstitutionArtists");

            migrationBuilder.DropTable(
                name: "LibraryScholars");

            migrationBuilder.DropTable(
                name: "SchoolStudents");

            migrationBuilder.DropTable(
                name: "SchoolTeachers");

            migrationBuilder.DropTable(
                name: "UniversityProfessors");

            migrationBuilder.DropTable(
                name: "UniversityStudents");
        }
    }
}
