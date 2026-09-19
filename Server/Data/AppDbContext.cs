using Microsoft.EntityFrameworkCore;
using Shared; // Contains StudentStatus enum
using Shared.Models;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Faculty> Colleges => Set<Faculty>();
        public DbSet<Major> Majors => Set<Major>();
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<CompletedCourse> CompletedCourses { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EXPLICIT FIX FOR ERROR 547: Ensure EF Core ignores in-memory DTOs as database entities
            modelBuilder.Ignore<ScheduleSlotDto>();

            // Configure Prerequisites Many-to-Many / Join Table
            modelBuilder.Entity<CoursePrerequisite>()
                .HasKey(cp => new { cp.CourseId, cp.PrerequisiteCourseId });

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(cp => cp.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(cp => cp.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(cp => cp.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Course Schedule Relationship
            modelBuilder.Entity<CourseSchedule>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Schedules)
                .HasForeignKey(cs => cs.CourseId);

            // Configure Student Course Composite Key
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany()
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany()
                .HasForeignKey(sc => sc.CourseId);

            // Prevent Cascade Delete Cycles
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // 1. Seed Faculty
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name = "Faculty of Engineering & Technology" }
            );

            // 2. Seed Major
            modelBuilder.Entity<Major>().HasData(
                new Major { Id = 1, Name = "Computer Science", FacultyId = 1 }
            );

            // 3. Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Code = "SE101", Title = "Software Architecture", CreditHours = 3, MajorId = 1, FacultyId = 1 },
                new Course { Id = 2, Code = "CS202", Title = "Data Structures & Algorithms", CreditHours = 4, MajorId = 1, FacultyId = 1 },
                new Course { Id = 3, Code = "DB301", Title = "Database Systems", CreditHours = 3, MajorId = 1, FacultyId = 1 },
                new Course { Id = 4, Code = "MATH101", Title = "Calculus 1", CreditHours = 3, FacultyId = 1, MajorId = 1 },
                new Course { Id = 5, Code = "MATH102", Title = "Calculus 2", CreditHours = 3, FacultyId = 1, MajorId = 1 }
            );

            // 4. Seed Course Prerequisites
            modelBuilder.Entity<CoursePrerequisite>().HasData(
                new CoursePrerequisite { CourseId = 1, PrerequisiteCourseId = 2 },
                new CoursePrerequisite { CourseId = 5, PrerequisiteCourseId = 4 }
            );

            // 5. Seed Course Schedules
            modelBuilder.Entity<CourseSchedule>().HasData(
                new CourseSchedule 
                { 
                    Id = 1, 
                    CourseId = 1, 
                    Day = DayOfWeek.Monday, 
                    StartTime = new TimeSpan(8, 30, 0), 
                    EndTime = new TimeSpan(10, 30, 0) 
                },
                new CourseSchedule 
                { 
                    Id = 2, 
                    CourseId = 2, 
                    Day = DayOfWeek.Tuesday, 
                    StartTime = new TimeSpan(12, 30, 0), 
                    EndTime = new TimeSpan(14, 30, 0) 
                },
                new CourseSchedule 
                { 
                    Id = 3, 
                    CourseId = 3, 
                    Day = DayOfWeek.Wednesday, 
                    StartTime = new TimeSpan(10, 30, 0), 
                    EndTime = new TimeSpan(12, 30, 0) 
                },
                new CourseSchedule
                {
                    Id = 5,
                    CourseId = 4,
                    Day = DayOfWeek.Sunday,
                    StartTime = new TimeSpan(8, 30, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                },
                new CourseSchedule
                {
                    Id = 6,
                    CourseId = 5,
                    Day = DayOfWeek.Tuesday,
                    StartTime = new TimeSpan(10, 30, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                }
            );

            // 6. Seed Test Students
            modelBuilder.Entity<Student>().HasData(
                new Student 
                { 
                    Id = 1, 
                    RegistrationNumber = "210001", 
                    Password = "849201", 
                    FullName = "Alex Morgan", 
                    FacultyId = 1,
                    Gpa = 3.8,
                    Status = StudentStatus.Active, 
                    MajorId = 1 
                },
                new Student 
                { 
                    Id = 2, 
                    RegistrationNumber = "210002", 
                    Password = "371940", 
                    FullName = "Jordan Lee", 
                    FacultyId = 1,
                    Gpa = 2.6,
                    Status = StudentStatus.Active, 
                    MajorId = 1 
                },
                new Student 
                { 
                    Id = 3, 
                    RegistrationNumber = "210003", 
                    Password = "592018", 
                    FullName = "Taylor Smith", 
                    FacultyId = 1,
                    Gpa = 3.1,
                    Status = StudentStatus.Active, 
                    MajorId = 1 
                },
                new Student 
                { 
                    Id = 4, 
                    RegistrationNumber = "210004", 
                    Password = "148302", 
                    FullName = "Sam Wilson", 
                    FacultyId = 1,
                    Gpa = 3.7,
                    Status = StudentStatus.Active, 
                    MajorId = 1 
                }
            );

            // 7. Seed Completed Courses
            modelBuilder.Entity<CompletedCourse>().HasData(
                new CompletedCourse { Id = 1, StudentId = 1, CourseCode = "MATH101" },
                new CompletedCourse { Id = 2, StudentId = 1, CourseCode = "CS202" },

                new CompletedCourse { Id = 3, StudentId = 2, CourseCode = "MATH101" },
                new CompletedCourse { Id = 4, StudentId = 2, CourseCode = "CS202" },

                new CompletedCourse { Id = 5, StudentId = 4, CourseCode = "MATH101" },
                new CompletedCourse { Id = 6, StudentId = 4, CourseCode = "CS202" }
            );

            // 8. Seed Student Registrations
            modelBuilder.Entity<StudentRegistration>().HasData(
                new StudentRegistration { Id = 1, StudentId = 2, CourseCode = "CS202", CreditHours = 3 },
                new StudentRegistration { Id = 2, StudentId = 2, CourseCode = "DB301", CreditHours = 3 },
                new StudentRegistration { Id = 3, StudentId = 2, CourseCode = "SE101", CreditHours = 3 },
                new StudentRegistration { Id = 4, StudentId = 2, CourseCode = "ENG101", CreditHours = 3 },
                new StudentRegistration { Id = 5, StudentId = 4, ScheduleSlotId = 1 } 
            );
        }
    }
}