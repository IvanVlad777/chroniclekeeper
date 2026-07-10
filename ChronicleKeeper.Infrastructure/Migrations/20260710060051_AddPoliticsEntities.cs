using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPoliticsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiplomaticAgreements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgreementType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SignedDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TerminationDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationYears = table.Column<int>(type: "int", nullable: true),
                    Terms = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsUnequal = table.Column<bool>(type: "bit", nullable: false),
                    FirstNationId = table.Column<int>(type: "int", nullable: false),
                    SecondNationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiplomaticAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiplomaticAgreements_Nations_FirstNationId",
                        column: x => x.FirstNationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiplomaticAgreements_Nations_SecondNationId",
                        column: x => x.SecondNationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiplomaticAgreements_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LegalSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Laws = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    JudicialIndependence = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PunishmentMethods = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalSystems_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalIdeologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAuthoritarian = table.Column<bool>(type: "bit", nullable: false),
                    IsSocialist = table.Column<bool>(type: "bit", nullable: false),
                    IsLiberal = table.Column<bool>(type: "bit", nullable: false),
                    IsRadical = table.Column<bool>(type: "bit", nullable: false),
                    IsMilitaristic = table.Column<bool>(type: "bit", nullable: false),
                    SupportsFreeMarket = table.Column<bool>(type: "bit", nullable: false),
                    SupportsPlannedEconomy = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalIdeologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliticalIdeologies_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GovernmentSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDemocratic = table.Column<bool>(type: "bit", nullable: false),
                    IsMonarchic = table.Column<bool>(type: "bit", nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    IsFederal = table.Column<bool>(type: "bit", nullable: false),
                    IsCentralized = table.Column<bool>(type: "bit", nullable: false),
                    PoliticalIdeologyId = table.Column<int>(type: "int", nullable: true),
                    ElectionSystem = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StabilityLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HasTermLimits = table.Column<bool>(type: "bit", nullable: false),
                    MaxTermLength = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernmentSystems_PoliticalIdeologies_PoliticalIdeologyId",
                        column: x => x.PoliticalIdeologyId,
                        principalTable: "PoliticalIdeologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GovernmentSystems_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeologyDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PoliticalIdeologyId = table.Column<int>(type: "int", nullable: false),
                    GovernmentSystemId = table.Column<int>(type: "int", nullable: true),
                    InfluenceLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_GovernmentSystems_GovernmentSystemId",
                        column: x => x.GovernmentSystemId,
                        principalTable: "GovernmentSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_PoliticalIdeologies_PoliticalIdeologyId",
                        column: x => x.PoliticalIdeologyId,
                        principalTable: "PoliticalIdeologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiplomaticAgreements_FirstNationId",
                table: "DiplomaticAgreements",
                column: "FirstNationId");

            migrationBuilder.CreateIndex(
                name: "IX_DiplomaticAgreements_SecondNationId",
                table: "DiplomaticAgreements",
                column: "SecondNationId");

            migrationBuilder.CreateIndex(
                name: "IX_DiplomaticAgreements_WorldId_Name",
                table: "DiplomaticAgreements",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentSystems_PoliticalIdeologyId",
                table: "GovernmentSystems",
                column: "PoliticalIdeologyId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentSystems_WorldId_Name",
                table: "GovernmentSystems",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_LegalSystems_WorldId_Name",
                table: "LegalSystems",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalIdeologies_WorldId_Name",
                table: "PoliticalIdeologies",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_GovernmentSystemId",
                table: "PoliticalParties",
                column: "GovernmentSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_PoliticalIdeologyId",
                table: "PoliticalParties",
                column: "PoliticalIdeologyId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_WorldId_Name",
                table: "PoliticalParties",
                columns: new[] { "WorldId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiplomaticAgreements");

            migrationBuilder.DropTable(
                name: "LegalSystems");

            migrationBuilder.DropTable(
                name: "PoliticalParties");

            migrationBuilder.DropTable(
                name: "GovernmentSystems");

            migrationBuilder.DropTable(
                name: "PoliticalIdeologies");
        }
    }
}
