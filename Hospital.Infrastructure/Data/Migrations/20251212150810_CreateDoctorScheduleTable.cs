using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDoctorScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Users_DoctorId",
                table: "DoctorSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSchedule",
                table: "DoctorSchedule");

            migrationBuilder.RenameTable(
                name: "DoctorSchedule",
                newName: "DoctorSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSchedule_DoctorId",
                table: "DoctorSchedules",
                newName: "IX_DoctorSchedules_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedules_Users_DoctorId",
                table: "DoctorSchedules",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedules_Users_DoctorId",
                table: "DoctorSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules");

            migrationBuilder.RenameTable(
                name: "DoctorSchedules",
                newName: "DoctorSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedule",
                newName: "IX_DoctorSchedule_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSchedule",
                table: "DoctorSchedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Users_DoctorId",
                table: "DoctorSchedule",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
