using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityAndNeighborhoodOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DoctorOffices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "DoctorOffices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "DoctorOffices");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "DoctorOffices");
        }
    }
}
