using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTailEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocialHierarchyId",
                table: "SocialClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocialHierarchyId",
                table: "Nations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandmarkType",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Background_Childhood",
                table: "Characters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Background_FamilyStatus",
                table: "Characters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Background_IsImmigrant",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Background_MigrationHistory",
                table: "Characters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Background_Upbringing",
                table: "Characters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_Ambitions",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_Fears",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_Flaws",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_Motivations",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_PersonalityTraits",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_PsychologicalProfile",
                table: "Characters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Personality_Virtues",
                table: "Characters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Hobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hobbies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Hobbies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mutations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    MutantCreatureId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mutations_Creatures_MutantCreatureId",
                        column: x => x.MutantCreatureId,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Mutations_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Mutations_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeLaws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrantsLegalImmunity = table.Column<bool>(type: "bit", nullable: false),
                    GrantsLandOwnershipRights = table.Column<bool>(type: "bit", nullable: false),
                    AllowsPrivateArmies = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    SocialClassId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegeLaws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivilegeLaws_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PrivilegeLaws_SocialClasses_SocialClassId",
                        column: x => x.SocialClassId,
                        principalTable: "SocialClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivilegeLaws_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCasteSystem = table.Column<bool>(type: "bit", nullable: false),
                    AllowsUpwardMobility = table.Column<bool>(type: "bit", nullable: false),
                    AllowsIntermarriage = table.Column<bool>(type: "bit", nullable: false),
                    EnforcesLegalSeparation = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialHierarchies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialHierarchies_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SocialHierarchies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialClasses_SocialHierarchyId",
                table: "SocialClasses",
                column: "SocialHierarchyId");

            migrationBuilder.CreateIndex(
                name: "IX_Nations_SocialHierarchyId",
                table: "Nations",
                column: "SocialHierarchyId");

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_HistoryId",
                table: "Hobbies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_WorldId_Name",
                table: "Hobbies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_HistoryId",
                table: "Mutations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_MutantCreatureId",
                table: "Mutations",
                column: "MutantCreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Mutations_WorldId_Name",
                table: "Mutations",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeLaws_HistoryId",
                table: "PrivilegeLaws",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeLaws_SocialClassId",
                table: "PrivilegeLaws",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeLaws_WorldId_Name",
                table: "PrivilegeLaws",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_SocialHierarchies_HistoryId",
                table: "SocialHierarchies",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialHierarchies_WorldId_Name",
                table: "SocialHierarchies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Nations_SocialHierarchies_SocialHierarchyId",
                table: "Nations",
                column: "SocialHierarchyId",
                principalTable: "SocialHierarchies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialClasses_SocialHierarchies_SocialHierarchyId",
                table: "SocialClasses",
                column: "SocialHierarchyId",
                principalTable: "SocialHierarchies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nations_SocialHierarchies_SocialHierarchyId",
                table: "Nations");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialClasses_SocialHierarchies_SocialHierarchyId",
                table: "SocialClasses");

            migrationBuilder.DropTable(
                name: "Hobbies");

            migrationBuilder.DropTable(
                name: "Mutations");

            migrationBuilder.DropTable(
                name: "PrivilegeLaws");

            migrationBuilder.DropTable(
                name: "SocialHierarchies");

            migrationBuilder.DropIndex(
                name: "IX_SocialClasses_SocialHierarchyId",
                table: "SocialClasses");

            migrationBuilder.DropIndex(
                name: "IX_Nations_SocialHierarchyId",
                table: "Nations");

            migrationBuilder.DropColumn(
                name: "SocialHierarchyId",
                table: "SocialClasses");

            migrationBuilder.DropColumn(
                name: "SocialHierarchyId",
                table: "Nations");

            migrationBuilder.DropColumn(
                name: "LandmarkType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Background_Childhood",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_FamilyStatus",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_IsImmigrant",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_MigrationHistory",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_Upbringing",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_Ambitions",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_Fears",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_Flaws",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_Motivations",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_PersonalityTraits",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_PsychologicalProfile",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Personality_Virtues",
                table: "Characters");
        }
    }
}
