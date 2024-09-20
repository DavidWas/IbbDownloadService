using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    /// <inheritdoc />
    public partial class AddNugetDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NugetDependencies",
                schema: "Nugets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NugetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DependencyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NugetDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NugetDependencies_Nugets_NugetId",
                        column: x => x.NugetId,
                        principalSchema: "Nugets",
                        principalTable: "Nugets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NugetDependencies_NugetId",
                schema: "Nugets",
                table: "NugetDependencies",
                column: "NugetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NugetDependencies",
                schema: "Nugets");
        }
    }
}
