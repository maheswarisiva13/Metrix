using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addschemaname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "Visitors",
                newName: "Visitors",
                newSchema: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "VisitLogs",
                newName: "VisitLogs",
                newSchema: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "SecurityUsers",
                newName: "SecurityUsers",
                newSchema: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitations",
                newSchema: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "HRUsers",
                newName: "HRUsers",
                newSchema: "Metrix_Seed");

            migrationBuilder.RenameTable(
                name: "AdminUsers",
                newName: "AdminUsers",
                newSchema: "Metrix_Seed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Visitors",
                schema: "Metrix_Seed",
                newName: "Visitors");

            migrationBuilder.RenameTable(
                name: "VisitLogs",
                schema: "Metrix_Seed",
                newName: "VisitLogs");

            migrationBuilder.RenameTable(
                name: "SecurityUsers",
                schema: "Metrix_Seed",
                newName: "SecurityUsers");

            migrationBuilder.RenameTable(
                name: "Invitations",
                schema: "Metrix_Seed",
                newName: "Invitations");

            migrationBuilder.RenameTable(
                name: "HRUsers",
                schema: "Metrix_Seed",
                newName: "HRUsers");

            migrationBuilder.RenameTable(
                name: "AdminUsers",
                schema: "Metrix_Seed",
                newName: "AdminUsers");
        }
    }
}
