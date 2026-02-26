using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Company",
                table: "Visitors",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Visitors",
                newName: "Company");
        }
    }
}
