using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRegistrationOpen = table.Column<bool>(type: "bit", nullable: false),
                    CollegeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MajorDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MajorDto_Colleges_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Colleges",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    MajorId = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true),
                    MajorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Colleges_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Colleges",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoursePrerequisite",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePrerequisite", x => new { x.CourseId, x.PrerequisiteCourseId });
                    table.ForeignKey(
                        name: "FK_CoursePrerequisite_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoursePrerequisite_Courses_PrerequisiteCourseId",
                        column: x => x.PrerequisiteCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSchedules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompletedCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    ScheduleSlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Colleges",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Faculty of Engineering & Technology" });

            migrationBuilder.InsertData(
                table: "Majors",
                columns: new[] { "Id", "CollegeId", "IsRegistrationOpen", "Name" },
                values: new object[] { 1, 1, false, "Computer Science" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Code", "CreditHours", "FacultyId", "MajorId", "Title" },
                values: new object[,]
                {
                    { 1, "SE101", 3, 1, 1, "Software Architecture" },
                    { 2, "CS202", 4, 1, 1, "Data Structures & Algorithms" },
                    { 3, "DB301", 3, 1, 1, "Database Systems" },
                    { 4, "MATH101", 3, 1, 1, "Calculus 1" },
                    { 5, "MATH102", 3, 1, 1, "Calculus 2" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "FacultyId", "FullName", "MajorId", "Password", "RegistrationNumber", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Alex Morgan", 1, "849201", "210001", 1 },
                    { 2, 1, "Jordan Lee", 1, "371940", "210002", 1 },
                    { 3, 1, "Taylor Smith", 1, "592018", "210003", 1 },
                    { 4, 1, "Sam Wilson", 1, "148302", "210004", 1 }
                });

            migrationBuilder.InsertData(
                table: "CompletedCourses",
                columns: new[] { "Id", "CourseCode", "CreditHours", "StudentId" },
                values: new object[,]
                {
                    { 1, "MATH101", 0, 1 },
                    { 2, "CS202", 0, 1 },
                    { 3, "MATH101", 0, 2 },
                    { 4, "CS202", 0, 2 },
                    { 5, "MATH101", 0, 4 },
                    { 6, "CS202", 0, 4 }
                });

            migrationBuilder.InsertData(
                table: "CoursePrerequisite",
                columns: new[] { "CourseId", "PrerequisiteCourseId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 5, 4 }
                });

            migrationBuilder.InsertData(
                table: "CourseSchedules",
                columns: new[] { "Id", "CourseId", "Day", "EndTime", "LecturerName", "SectionName", "StartTime", "TaName" },
                values: new object[,]
                {
                    { 1, 1, 1, new TimeSpan(0, 10, 30, 0, 0), "", "Section 1", new TimeSpan(0, 8, 30, 0, 0), "" },
                    { 2, 2, 2, new TimeSpan(0, 14, 30, 0, 0), "", "Section 1", new TimeSpan(0, 12, 30, 0, 0), "" },
                    { 3, 3, 3, new TimeSpan(0, 12, 30, 0, 0), "", "Section 1", new TimeSpan(0, 10, 30, 0, 0), "" },
                    { 5, 4, 0, new TimeSpan(0, 10, 0, 0, 0), "", "Section 1", new TimeSpan(0, 8, 30, 0, 0), "" },
                    { 6, 5, 2, new TimeSpan(0, 12, 0, 0, 0), "", "Section 1", new TimeSpan(0, 10, 30, 0, 0), "" }
                });

            migrationBuilder.InsertData(
                table: "StudentRegistrations",
                columns: new[] { "Id", "CourseCode", "CreditHours", "ScheduleSlotId", "StudentId" },
                values: new object[,]
                {
                    { 1, "CS202", 3, null, 2 },
                    { 2, "DB301", 3, null, 2 },
                    { 3, "SE101", 3, null, 2 },
                    { 4, "ENG101", 3, null, 2 },
                    { 5, "", 0, 1, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourses_StudentId",
                table: "CompletedCourses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePrerequisite_PrerequisiteCourseId",
                table: "CoursePrerequisite",
                column: "PrerequisiteCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MajorId",
                table: "Courses",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CourseId",
                table: "CourseSchedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorDto_FacultyId",
                table: "MajorDto",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_StudentId",
                table: "StudentRegistrations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyId",
                table: "Students",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedCourses");

            migrationBuilder.DropTable(
                name: "CoursePrerequisite");

            migrationBuilder.DropTable(
                name: "CourseSchedules");

            migrationBuilder.DropTable(
                name: "MajorDto");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "StudentRegistrations");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Colleges");

            migrationBuilder.DropTable(
                name: "Majors");
        }
    }
}
