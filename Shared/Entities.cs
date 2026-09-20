using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models;

public enum StudentStatus
{
    Active = 1,
    Suspended = 2,
    Warning,
    Probation,
    Graduated = 3
}

// ==========================================
// DATA TRANSFER OBJECTS (DTOs)
// ==========================================

public class FacultyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MajorDto> Majors { get; set; } = new();
}

public class MajorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int FacultyId { get; set; }
}

public class StudentHeaderDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FacultyName { get; set; } = string.Empty;
    public int FacultyId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StudentStatus Status { get; set; }
    public double Gpa { get; set; }
    public string MajorName { get; set; } = string.Empty;
    public int MajorId { get; set; }
    public string CurrentSemester { get; set; } = string.Empty;
    public int CurrentlyRegisteredCredits { get; set; }
}

public class RegistrationValidationResult
{
    public bool CanRegister { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CourseScheduleSlotDto
{
    public int Id { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string LecturerName { get; set; } = string.Empty;
    public string TaName { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

// ==========================================
// DATABASE ENTITIES
// ==========================================

public class Faculty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Major> Majors { get; set; } = new List<Major>();
}

public class Major
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsRegistrationOpen { get; set; }
    public int FacultyId { get; set; }

    [JsonIgnore]
    public Faculty? Faculty { get; set; }

    [JsonIgnore]
    public ICollection<Course> TranscriptCourses { get; set; } = new List<Course>();
}

public class Student
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public int? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public double Gpa { get; set; }

    public int? MajorId { get; set; }
    public Major? Major { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public int MajorId { get; set; }
    public int FacultyId { get; set; }

    [JsonIgnore]
    public ICollection<CourseSchedule> Schedules { get; set; } = new List<CourseSchedule>();

    [JsonIgnore]
    public ICollection<CoursePrerequisite> Prerequisites { get; set; } = new List<CoursePrerequisite>();
}

public class CourseSchedule
{
   public int Id { get; set; }
    public int CourseId { get; set; }
    public string SectionName { get; set; } = "Section 1";
    public string LecturerName { get; set; } = string.Empty;
    public string TaName { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course? Course { get; set; }
}

public class CoursePrerequisite
{
    public int CourseId { get; set; }

    [JsonIgnore]
    public Course? Course { get; set; }

    public int PrerequisiteCourseId { get; set; }

    [JsonIgnore]
    public Course? PrerequisiteCourse { get; set; }
}

public class CompletedCourse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int CreditHours { get; set; }

    [JsonIgnore]
    public Student? Student { get; set; }
}

public class StudentRegistration
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    [JsonIgnore]
    public Student? Student { get; set; }

    [Required]
    [StringLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    public int CreditHours { get; set; }
    public int? ScheduleSlotId { get; set; }
}