using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeUserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CPF",
                table: "Users",
                column: "CPF",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CPF",
                table: "Users");
        }
    }
}
