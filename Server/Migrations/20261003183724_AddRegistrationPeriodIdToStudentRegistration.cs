using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationPeriodIdToStudentRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistrationPeriodId",
                table: "StudentRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "StudentRegistrations",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationPeriodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "StudentRegistrations",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationPeriodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "StudentRegistrations",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegistrationPeriodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "StudentRegistrations",
                keyColumn: "Id",
                keyValue: 4,
                column: "RegistrationPeriodId",
                value: null);

            migrationBuilder.UpdateData(
                table: "StudentRegistrations",
                keyColumn: "Id",
                keyValue: 5,
                column: "RegistrationPeriodId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_RegistrationPeriodId",
                table: "StudentRegistrations",
                column: "RegistrationPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegistrations_RegistrationPeriods_RegistrationPeriodId",
                table: "StudentRegistrations",
                column: "RegistrationPeriodId",
                principalTable: "RegistrationPeriods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegistrations_RegistrationPeriods_RegistrationPeriodId",
                table: "StudentRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_StudentRegistrations_RegistrationPeriodId",
                table: "StudentRegistrations");

            migrationBuilder.DropColumn(
                name: "RegistrationPeriodId",
                table: "StudentRegistrations");
        }
    }
}
