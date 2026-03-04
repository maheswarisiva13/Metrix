using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "Invitations",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Visitors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SecurityUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HRUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "HRUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SecurityUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HRUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "HRUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Invitations",
                newName: "SentAt");
        }
    }
}
