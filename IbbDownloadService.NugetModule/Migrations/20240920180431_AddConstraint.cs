using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Nugets_Name_Version",
                schema: "Nugets",
                table: "Nugets",
                columns: new[] { "Name", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nugets_Name_Version",
                schema: "Nugets",
                table: "Nugets");
        }
    }
}
