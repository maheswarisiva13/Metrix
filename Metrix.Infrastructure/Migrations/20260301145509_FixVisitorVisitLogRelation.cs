using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVisitorVisitLogRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitLogs_Visitors_VisitorId",
                table: "VisitLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitLogs_Visitors_VisitorId",
                table: "VisitLogs",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitLogs_Visitors_VisitorId",
                table: "VisitLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitLogs_Visitors_VisitorId",
                table: "VisitLogs",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
