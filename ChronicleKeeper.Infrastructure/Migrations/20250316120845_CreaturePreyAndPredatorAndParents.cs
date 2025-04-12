using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreaturePreyAndPredatorAndParents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_AnimalId",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_FungusId",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_FungusId1",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_Fungus_AnimalId",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_PlantId",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_PlantId1",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_Plant_AnimalId",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Religions_ReligionId1",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_AnimalId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_Fungus_AnimalId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_FungusId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_FungusId1",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_Plant_AnimalId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_PlantId",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_PlantId1",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "FungusId",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "FungusId1",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Fungus_AnimalId",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "PlantId1",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Plant_AnimalId",
                table: "Creatures");

            migrationBuilder.RenameColumn(
                name: "ReligionId1",
                table: "Creatures",
                newName: "ParentCreatureId");

            migrationBuilder.RenameIndex(
                name: "IX_Creatures_ReligionId1",
                table: "Creatures",
                newName: "IX_Creatures_ParentCreatureId");

            migrationBuilder.CreateTable(
                name: "CreaturePredation",
                columns: table => new
                {
                    PredatorsId = table.Column<int>(type: "int", nullable: false),
                    PreyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreaturePredation", x => new { x.PredatorsId, x.PreyId });
                    table.ForeignKey(
                        name: "FK_CreaturePredation_Creatures_PredatorsId",
                        column: x => x.PredatorsId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreaturePredation_Creatures_PreyId",
                        column: x => x.PreyId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreatureSymbiosis",
                columns: table => new
                {
                    CreatureId = table.Column<int>(type: "int", nullable: false),
                    SymbioticPartnersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureSymbiosis", x => new { x.CreatureId, x.SymbioticPartnersId });
                    table.ForeignKey(
                        name: "FK_CreatureSymbiosis_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureSymbiosis_Creatures_SymbioticPartnersId",
                        column: x => x.SymbioticPartnersId,
                        principalTable: "Creatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreaturePredation_PreyId",
                table: "CreaturePredation",
                column: "PreyId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureSymbiosis_SymbioticPartnersId",
                table: "CreatureSymbiosis",
                column: "SymbioticPartnersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_ParentCreatureId",
                table: "Creatures",
                column: "ParentCreatureId",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_Creatures_ParentCreatureId",
                table: "Creatures");

            migrationBuilder.DropTable(
                name: "CreaturePredation");

            migrationBuilder.DropTable(
                name: "CreatureSymbiosis");

            migrationBuilder.RenameColumn(
                name: "ParentCreatureId",
                table: "Creatures",
                newName: "ReligionId1");

            migrationBuilder.RenameIndex(
                name: "IX_Creatures_ParentCreatureId",
                table: "Creatures",
                newName: "IX_Creatures_ReligionId1");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FungusId",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FungusId1",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Fungus_AnimalId",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlantId",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlantId1",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Plant_AnimalId",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_AnimalId",
                table: "Creatures",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_Fungus_AnimalId",
                table: "Creatures",
                column: "Fungus_AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_FungusId",
                table: "Creatures",
                column: "FungusId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_FungusId1",
                table: "Creatures",
                column: "FungusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_Plant_AnimalId",
                table: "Creatures",
                column: "Plant_AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_PlantId",
                table: "Creatures",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_PlantId1",
                table: "Creatures",
                column: "PlantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_AnimalId",
                table: "Creatures",
                column: "AnimalId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_FungusId",
                table: "Creatures",
                column: "FungusId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_FungusId1",
                table: "Creatures",
                column: "FungusId1",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_Fungus_AnimalId",
                table: "Creatures",
                column: "Fungus_AnimalId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_PlantId",
                table: "Creatures",
                column: "PlantId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_PlantId1",
                table: "Creatures",
                column: "PlantId1",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Creatures_Plant_AnimalId",
                table: "Creatures",
                column: "Plant_AnimalId",
                principalTable: "Creatures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_Religions_ReligionId1",
                table: "Creatures",
                column: "ReligionId1",
                principalTable: "Religions",
                principalColumn: "Id");
        }
    }
}
