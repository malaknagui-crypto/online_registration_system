using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMajorsAndAssignStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsRegistrationOpen",
                value: true);

            migrationBuilder.InsertData(
                table: "Majors",
                columns: new[] { "Id", "FacultyId", "IsRegistrationOpen", "Name" },
                values: new object[,]
                {
                    { 2, 1, true, "Software Engineering" },
                    { 3, 1, true, "Information Systems" },
                    { 4, 1, true, "Multimedia" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "MajorId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "MajorId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                column: "MajorId",
                value: 4);

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Code", "CreditHours", "FacultyId", "MajorId", "Title" },
                values: new object[,]
                {
                    { 6, "SWE201", 3, 1, 2, "Software Requirements Engineering" },
                    { 7, "SWE310", 3, 1, 2, "Software Testing & Quality Assurance" },
                    { 8, "SWE330", 3, 1, 2, "Agile Project Management" },
                    { 9, "IS210", 3, 1, 3, "Information Systems Analysis" },
                    { 10, "IS320", 3, 1, 3, "Enterprise Resource Planning" },
                    { 11, "IS340", 3, 1, 3, "Business Intelligence & Analytics" },
                    { 12, "MM150", 3, 1, 4, "Digital Media Fundamentals" },
                    { 13, "MM240", 4, 1, 4, "3D Modeling & Animation" },
                    { 14, "MM360", 3, 1, 4, "Interactive Web Design" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Majors",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsRegistrationOpen",
                value: false);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "MajorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "MajorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                column: "MajorId",
                value: 1);
        }
    }
}
