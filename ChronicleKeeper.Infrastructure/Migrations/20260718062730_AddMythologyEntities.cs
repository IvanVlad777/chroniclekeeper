using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMythologyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReligiousOrderId",
                table: "ReligiousEducations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeityId",
                table: "Myths",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorshipMethods = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMonotheistic = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    DeityType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsImmortal = table.Column<bool>(type: "bit", nullable: false),
                    CanManifestPhysically = table.Column<bool>(type: "bit", nullable: false),
                    GrantsPowers = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deities_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Deities_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Deities_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Beliefs = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMilitant = table.Column<bool>(type: "bit", nullable: false),
                    IsSecretive = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousOrders_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReligiousOrders_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReligiousOrders_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeityAlliances",
                columns: table => new
                {
                    DeityId = table.Column<int>(type: "int", nullable: false),
                    AlliedDeityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeityAlliances", x => new { x.DeityId, x.AlliedDeityId });
                    table.ForeignKey(
                        name: "FK_DeityAlliances_Deities_AlliedDeityId",
                        column: x => x.AlliedDeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeityAlliances_Deities_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeityRivalries",
                columns: table => new
                {
                    DeityId = table.Column<int>(type: "int", nullable: false),
                    RivalDeityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeityRivalries", x => new { x.DeityId, x.RivalDeityId });
                    table.ForeignKey(
                        name: "FK_DeityRivalries_Deities_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeityRivalries_Deities_RivalDeityId",
                        column: x => x.RivalDeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HolySites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Significance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPilgrimageDestination = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    DeityId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolySites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolySites_Deities_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HolySites_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HolySites_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolySites_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolySites_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContentSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    DeityId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Deities_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReligiousTexts_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeityReligiousOrders",
                columns: table => new
                {
                    DeityId = table.Column<int>(type: "int", nullable: false),
                    ReligiousOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeityReligiousOrders", x => new { x.DeityId, x.ReligiousOrderId });
                    table.ForeignKey(
                        name: "FK_DeityReligiousOrders_Deities_DeityId",
                        column: x => x.DeityId,
                        principalTable: "Deities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeityReligiousOrders_ReligiousOrders_ReligiousOrderId",
                        column: x => x.ReligiousOrderId,
                        principalTable: "ReligiousOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousOrderFactions",
                columns: table => new
                {
                    ReligiousOrderId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousOrderFactions", x => new { x.ReligiousOrderId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_ReligiousOrderFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReligiousOrderFactions_ReligiousOrders_ReligiousOrderId",
                        column: x => x.ReligiousOrderId,
                        principalTable: "ReligiousOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousFestivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Traditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPilgrimageEvent = table.Column<bool>(type: "bit", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    HolySiteId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousFestivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReligiousFestivals_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReligiousFestivals_HolySites_HolySiteId",
                        column: x => x.HolySiteId,
                        principalTable: "HolySites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReligiousFestivals_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReligiousFestivals_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousEducations_ReligiousOrderId",
                table: "ReligiousEducations",
                column: "ReligiousOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Myths_DeityId",
                table: "Myths",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_Deities_HistoryId",
                table: "Deities",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Deities_ReligionId",
                table: "Deities",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Deities_WorldId_Name",
                table: "Deities",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_DeityAlliances_AlliedDeityId",
                table: "DeityAlliances",
                column: "AlliedDeityId");

            migrationBuilder.CreateIndex(
                name: "IX_DeityReligiousOrders_ReligiousOrderId",
                table: "DeityReligiousOrders",
                column: "ReligiousOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeityRivalries_RivalDeityId",
                table: "DeityRivalries",
                column: "RivalDeityId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_DeityId",
                table: "HolySites",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_HistoryId",
                table: "HolySites",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_LocationId",
                table: "HolySites",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_ReligionId",
                table: "HolySites",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_HolySites_WorldId_Name",
                table: "HolySites",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousFestivals_HistoryId",
                table: "ReligiousFestivals",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousFestivals_HolySiteId",
                table: "ReligiousFestivals",
                column: "HolySiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousFestivals_ReligionId",
                table: "ReligiousFestivals",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousFestivals_WorldId_Name",
                table: "ReligiousFestivals",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrderFactions_FactionId",
                table: "ReligiousOrderFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrders_HistoryId",
                table: "ReligiousOrders",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrders_ReligionId",
                table: "ReligiousOrders",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousOrders_WorldId_Name",
                table: "ReligiousOrders",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_DeityId",
                table: "ReligiousTexts",
                column: "DeityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_HistoryId",
                table: "ReligiousTexts",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_ReligionId",
                table: "ReligiousTexts",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReligiousTexts_WorldId_Name",
                table: "ReligiousTexts",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Myths_Deities_DeityId",
                table: "Myths",
                column: "DeityId",
                principalTable: "Deities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReligiousEducations_ReligiousOrders_ReligiousOrderId",
                table: "ReligiousEducations",
                column: "ReligiousOrderId",
                principalTable: "ReligiousOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Myths_Deities_DeityId",
                table: "Myths");

            migrationBuilder.DropForeignKey(
                name: "FK_ReligiousEducations_ReligiousOrders_ReligiousOrderId",
                table: "ReligiousEducations");

            migrationBuilder.DropTable(
                name: "DeityAlliances");

            migrationBuilder.DropTable(
                name: "DeityReligiousOrders");

            migrationBuilder.DropTable(
                name: "DeityRivalries");

            migrationBuilder.DropTable(
                name: "ReligiousFestivals");

            migrationBuilder.DropTable(
                name: "ReligiousOrderFactions");

            migrationBuilder.DropTable(
                name: "ReligiousTexts");

            migrationBuilder.DropTable(
                name: "HolySites");

            migrationBuilder.DropTable(
                name: "ReligiousOrders");

            migrationBuilder.DropTable(
                name: "Deities");

            migrationBuilder.DropIndex(
                name: "IX_ReligiousEducations_ReligiousOrderId",
                table: "ReligiousEducations");

            migrationBuilder.DropIndex(
                name: "IX_Myths_DeityId",
                table: "Myths");

            migrationBuilder.DropColumn(
                name: "ReligiousOrderId",
                table: "ReligiousEducations");

            migrationBuilder.DropColumn(
                name: "DeityId",
                table: "Myths");
        }
    }
}
