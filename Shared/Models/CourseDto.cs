namespace Shared.Models;
using System.Text.Json.Serialization;

public class CourseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public string LecturerName { get; set; } = string.Empty;
    public string TaName { get; set; } = string.Empty;
    public List<ScheduleSlotDto> ScheduleSlots { get; set; } = new();
    public List<string> Prerequisites { get; set; } = new();
}

public class ScheduleSlotDto
{
    public int Id { get; set; }
    public string SectionName { get; set; } = "Section 1";
    public string LecturerName { get; set; } = string.Empty;
    public string TaName { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class RegisterCoursesRequest
{
    public List<SelectedCourseSlotDto> SelectedSlots { get; set; } = new();
}

public class SelectedCourseSlotDto
{
    public int CourseId { get; set; }
    public int ScheduleSlotId { get; set; }
}

public class RegistrationRequestDto
{
    public int StudentId { get; set; }
    public List<int> CourseIds { get; set; } = new();
}