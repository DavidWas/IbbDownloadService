using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpdate",
                schema: "Nugets",
                table: "Nugets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpdate",
                schema: "Nugets",
                table: "Nugets");
        }
    }
}
