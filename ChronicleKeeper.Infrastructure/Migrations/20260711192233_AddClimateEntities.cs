using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClimateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClimateDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    Precipitation = table.Column<double>(type: "float", nullable: false),
                    WindSpeed = table.Column<double>(type: "float", nullable: false),
                    WindDirection = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsExtremeClimate = table.Column<bool>(type: "bit", nullable: false),
                    NotableWeatherPhenomena = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateDetails_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ClimateDetails_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClimateZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AverageTemperature = table.Column<double>(type: "float", nullable: false),
                    AverageHumidity = table.Column<double>(type: "float", nullable: false),
                    AveragePrecipitation = table.Column<double>(type: "float", nullable: false),
                    HasDistinctSeasons = table.Column<bool>(type: "bit", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateZones_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ClimateZones_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    TypicalTemperature = table.Column<double>(type: "float", nullable: false),
                    TypicalPrecipitation = table.Column<double>(type: "float", nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Seasons_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClimateZoneDetails",
                columns: table => new
                {
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    ClimateDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZoneDetails", x => new { x.ClimateZoneId, x.ClimateDetailId });
                    table.ForeignKey(
                        name: "FK_ClimateZoneDetails_ClimateDetails_ClimateDetailId",
                        column: x => x.ClimateDetailId,
                        principalTable: "ClimateDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClimateZoneDetails_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationClimateZones",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationClimateZones", x => new { x.LocationId, x.ClimateZoneId });
                    table.ForeignKey(
                        name: "FK_LocationClimateZones_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationClimateZones_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    PatternType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Effects = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HistoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherPatterns_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherPatterns_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WeatherPatterns_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClimateZoneSeasons",
                columns: table => new
                {
                    ClimateZoneId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateZoneSeasons", x => new { x.ClimateZoneId, x.SeasonId });
                    table.ForeignKey(
                        name: "FK_ClimateZoneSeasons_ClimateZones_ClimateZoneId",
                        column: x => x.ClimateZoneId,
                        principalTable: "ClimateZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClimateZoneSeasons_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDetails_HistoryId",
                table: "ClimateDetails",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDetails_WorldId_Name",
                table: "ClimateDetails",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZoneDetails_ClimateDetailId",
                table: "ClimateZoneDetails",
                column: "ClimateDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZones_HistoryId",
                table: "ClimateZones",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZones_WorldId_Name",
                table: "ClimateZones",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateZoneSeasons_SeasonId",
                table: "ClimateZoneSeasons",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationClimateZones_ClimateZoneId",
                table: "LocationClimateZones",
                column: "ClimateZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_HistoryId",
                table: "Seasons",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_WorldId_Name",
                table: "Seasons",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherPatterns_ClimateZoneId",
                table: "WeatherPatterns",
                column: "ClimateZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherPatterns_HistoryId",
                table: "WeatherPatterns",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherPatterns_WorldId_Name",
                table: "WeatherPatterns",
                columns: new[] { "WorldId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClimateZoneDetails");

            migrationBuilder.DropTable(
                name: "ClimateZoneSeasons");

            migrationBuilder.DropTable(
                name: "LocationClimateZones");

            migrationBuilder.DropTable(
                name: "WeatherPatterns");

            migrationBuilder.DropTable(
                name: "ClimateDetails");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "ClimateZones");
        }
    }
}
