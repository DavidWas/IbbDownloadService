using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIsInsertedByUpdater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInsertedByUpdater",
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
                name: "IsInsertedByUpdater",
                schema: "Nugets",
                table: "Nugets");
        }
    }
}
