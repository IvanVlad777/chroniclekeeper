using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocialClassId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SocialClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNoble = table.Column<bool>(type: "bit", nullable: false),
                    IsMerchantClass = table.Column<bool>(type: "bit", nullable: false),
                    IsOutcast = table.Column<bool>(type: "bit", nullable: false),
                    CanOwnLand = table.Column<bool>(type: "bit", nullable: false),
                    CanHoldOffice = table.Column<bool>(type: "bit", nullable: false),
                    HasTaxExemptions = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialClasses_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SocialClassId",
                table: "Characters",
                column: "SocialClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialClasses_WorldId_Name",
                table: "SocialClasses",
                columns: new[] { "WorldId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_SocialClasses_SocialClassId",
                table: "Characters",
                column: "SocialClassId",
                principalTable: "SocialClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_SocialClasses_SocialClassId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "SocialClasses");

            migrationBuilder.DropIndex(
                name: "IX_Characters_SocialClassId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SocialClassId",
                table: "Characters");
        }
    }
}
