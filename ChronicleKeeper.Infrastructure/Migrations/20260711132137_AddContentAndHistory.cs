using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContentAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "Timelines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "Nations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "Factions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Book_Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueNumber = table.Column<int>(type: "int", nullable: true),
                    Director = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Movie_ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    PrequelId = table.Column<int>(type: "int", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Seasons = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.CheckConstraint("CK_Movies_Prequel_NotSelf", "[PrequelId] <> [Id]");
                    table.ForeignKey(
                        name: "FK_Contents_Contents_PrequelId",
                        column: x => x.PrequelId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contents_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Contents_BookId",
                        column: x => x.BookId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chapters_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Contents_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Episodes_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: true),
                    EpisodeId = table.Column<int>(type: "int", nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    NationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                    table.ForeignKey(
                        name: "FK_References_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_References_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_References_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_References_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_References_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_References_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_References_Nations_NationId",
                        column: x => x.NationId,
                        principalTable: "Nations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timelines_HistoryId",
                table: "Timelines",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Nations_HistoryId",
                table: "Nations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_HistoryId",
                table: "Locations",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_HistoryId",
                table: "Factions",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_HistoryId",
                table: "Characters",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_BookId",
                table: "Chapters",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_WorldId_Name",
                table: "Chapters",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PrequelId",
                table: "Contents",
                column: "PrequelId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_WorldId_Name",
                table: "Contents",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeriesId",
                table: "Episodes",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_WorldId_Name",
                table: "Episodes",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_WorldId_Name",
                table: "Histories",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_References_ChapterId",
                table: "References",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_References_CharacterId",
                table: "References",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_References_ContentId",
                table: "References",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_References_EpisodeId",
                table: "References",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_References_FactionId",
                table: "References",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_References_LocationId",
                table: "References",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_References_NationId",
                table: "References",
                column: "NationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Histories_HistoryId",
                table: "Characters",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Factions_Histories_HistoryId",
                table: "Factions",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Histories_HistoryId",
                table: "Locations",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Nations_Histories_HistoryId",
                table: "Nations",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Timelines_Histories_HistoryId",
                table: "Timelines",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Histories_HistoryId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Factions_Histories_HistoryId",
                table: "Factions");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Histories_HistoryId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Nations_Histories_HistoryId",
                table: "Nations");

            migrationBuilder.DropForeignKey(
                name: "FK_Timelines_Histories_HistoryId",
                table: "Timelines");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Timelines_HistoryId",
                table: "Timelines");

            migrationBuilder.DropIndex(
                name: "IX_Nations_HistoryId",
                table: "Nations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_HistoryId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Factions_HistoryId",
                table: "Factions");

            migrationBuilder.DropIndex(
                name: "IX_Characters_HistoryId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "Timelines");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "Nations");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "Factions");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "Characters");
        }
    }
}
