using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId1",
                table: "VisitLogs");

            migrationBuilder.DropIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId1",
                table: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "VerifiedBySecurityId1",
                table: "VisitLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerifiedBySecurityId1",
                table: "VisitLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId1",
                table: "VisitLogs",
                column: "VerifiedBySecurityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId1",
                table: "VisitLogs",
                column: "VerifiedBySecurityId1",
                principalTable: "SecurityUsers",
                principalColumn: "Id");
        }
    }
}
