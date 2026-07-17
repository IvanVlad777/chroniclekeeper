using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimelineEventLocationAndCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "TimelineEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimelineEventCharacters",
                columns: table => new
                {
                    TimelineEventId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEventCharacters", x => new { x.TimelineEventId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_TimelineEventCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelineEventCharacters_TimelineEvents_TimelineEventId",
                        column: x => x.TimelineEventId,
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_LocationId",
                table: "TimelineEvents",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEventCharacters_CharacterId",
                table: "TimelineEventCharacters",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimelineEvents_Locations_LocationId",
                table: "TimelineEvents",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimelineEvents_Locations_LocationId",
                table: "TimelineEvents");

            migrationBuilder.DropTable(
                name: "TimelineEventCharacters");

            migrationBuilder.DropIndex(
                name: "IX_TimelineEvents_LocationId",
                table: "TimelineEvents");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "TimelineEvents");
        }
    }
}
