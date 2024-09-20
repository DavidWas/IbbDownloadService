using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "Nugets",
                table: "Nugets",
                newName: "DownloadedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DownloadedAt",
                schema: "Nugets",
                table: "Nugets",
                newName: "UpdatedAt");
        }
    }
}
