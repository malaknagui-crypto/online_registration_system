using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MajorDto");

            migrationBuilder.RenameColumn(
                name: "CollegeId",
                table: "Majors",
                newName: "FacultyId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CompletedCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CourseId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CourseId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CourseId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CourseId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CourseId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CompletedCourses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CourseId",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Majors_FacultyId",
                table: "Majors",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Majors_Colleges_FacultyId",
                table: "Majors",
                column: "FacultyId",
                principalTable: "Colleges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Majors_Colleges_FacultyId",
                table: "Majors");

            migrationBuilder.DropIndex(
                name: "IX_Majors_FacultyId",
                table: "Majors");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CompletedCourses");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Majors",
                newName: "CollegeId");

            migrationBuilder.CreateTable(
                name: "MajorDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MajorDto_FacultyId",
                table: "MajorDto",
                column: "FacultyId");
        }
    }
}
