using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingLinksAndNavs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "TimelineEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolkloreId",
                table: "TimelineEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreaturePredations",
                columns: table => new
                {
                    PredatorCreatureId = table.Column<int>(type: "int", nullable: false),
                    PreyCreatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreaturePredations", x => new { x.PredatorCreatureId, x.PreyCreatureId });
                    table.ForeignKey(
                        name: "FK_CreaturePredations_Creatures_PredatorCreatureId",
                        column: x => x.PredatorCreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreaturePredations_Creatures_PreyCreatureId",
                        column: x => x.PreyCreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreatureSymbioses",
                columns: table => new
                {
                    CreatureId = table.Column<int>(type: "int", nullable: false),
                    SymbioticPartnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureSymbioses", x => new { x.CreatureId, x.SymbioticPartnerId });
                    table.ForeignKey(
                        name: "FK_CreatureSymbioses_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureSymbioses_Creatures_SymbioticPartnerId",
                        column: x => x.SymbioticPartnerId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSubjectTeachers",
                columns: table => new
                {
                    SchoolSubjectId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubjectTeachers", x => new { x.SchoolSubjectId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_SchoolSubjectTeachers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubjectTeachers_SchoolSubjects_SchoolSubjectId",
                        column: x => x.SchoolSubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_BattleId",
                table: "TimelineEvents",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_FolkloreId",
                table: "TimelineEvents",
                column: "FolkloreId");

            migrationBuilder.CreateIndex(
                name: "IX_CreaturePredations_PreyCreatureId",
                table: "CreaturePredations",
                column: "PreyCreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureSymbioses_SymbioticPartnerId",
                table: "CreatureSymbioses",
                column: "SymbioticPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjectTeachers_CharacterId",
                table: "SchoolSubjectTeachers",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineEvents_Battles_BattleId",
                table: "TimelineEvents",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineEvents_Folktales_FolkloreId",
                table: "TimelineEvents",
                column: "FolkloreId",
                principalTable: "Folktales",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineEvents_Battles_BattleId",
                table: "TimelineEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_TimelineEvents_Folktales_FolkloreId",
                table: "TimelineEvents");

            migrationBuilder.DropTable(
                name: "CreaturePredations");

            migrationBuilder.DropTable(
                name: "CreatureSymbioses");

            migrationBuilder.DropTable(
                name: "SchoolSubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_TimelineEvents_BattleId",
                table: "TimelineEvents");

            migrationBuilder.DropIndex(
                name: "IX_TimelineEvents_FolkloreId",
                table: "TimelineEvents");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "TimelineEvents");

            migrationBuilder.DropColumn(
                name: "FolkloreId",
                table: "TimelineEvents");
        }
    }
}
