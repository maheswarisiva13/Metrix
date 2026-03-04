using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVisitorAndInvitationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegistrationId",
                table: "Visitors",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "Visitors",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Visitors",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Visitors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VerifiedBySecurityId1",
                table: "VisitLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_ApprovedByHRId",
                table: "Visitors",
                column: "ApprovedByHRId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId",
                table: "VisitLogs",
                column: "VerifiedBySecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId1",
                table: "VisitLogs",
                column: "VerifiedBySecurityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId",
                table: "VisitLogs",
                column: "VerifiedBySecurityId",
                principalTable: "SecurityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId1",
                table: "VisitLogs",
                column: "VerifiedBySecurityId1",
                principalTable: "SecurityUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_HRUsers_ApprovedByHRId",
                table: "Visitors",
                column: "ApprovedByHRId",
                principalTable: "HRUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId",
                table: "VisitLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitLogs_SecurityUsers_VerifiedBySecurityId1",
                table: "VisitLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_HRUsers_ApprovedByHRId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_ApprovedByHRId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId",
                table: "VisitLogs");

            migrationBuilder.DropIndex(
                name: "IX_VisitLogs_VerifiedBySecurityId1",
                table: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "VerifiedBySecurityId1",
                table: "VisitLogs");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationId",
                table: "Visitors",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "Visitors",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Visitors",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);
        }
    }
}
