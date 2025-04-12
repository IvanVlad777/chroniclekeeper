using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronicleKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UMajorRemoveStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UniversityMajors_UniversityMajorId1",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UniversityMajorId1",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UniversityMajorId1",
                table: "Characters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniversityMajorId1",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UniversityMajorId1",
                table: "Characters",
                column: "UniversityMajorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UniversityMajors_UniversityMajorId1",
                table: "Characters",
                column: "UniversityMajorId1",
                principalTable: "UniversityMajors",
                principalColumn: "Id");
        }
    }
}
