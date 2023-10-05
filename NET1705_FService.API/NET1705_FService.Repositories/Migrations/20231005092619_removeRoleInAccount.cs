using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1705_FService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class removeRoleInAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
