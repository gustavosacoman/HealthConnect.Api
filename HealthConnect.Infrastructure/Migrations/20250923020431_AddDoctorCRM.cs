using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorCRM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorCRMs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CRMNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    State = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCRMs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorCRMs_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCRMs_CRMNumber_State",
                table: "DoctorCRMs",
                columns: new[] { "CRMNumber", "State" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCRMs_DoctorId",
                table: "DoctorCRMs",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorCRMs");
        }
    }
}
