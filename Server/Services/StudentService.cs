using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;
using Shared;

namespace Server.Services;

public class StudentService 
{
    private readonly AppDbContext _context;
    public StudentService(AppDbContext context) => _context = context;

    public async Task<EligibilityResultDto> CheckEligibilityAsync(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Major)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            return new EligibilityResultDto 
            { 
                IsEligible = false, 
                Reason = "Student record not found." 
            };
        }

        if (student.Status == StudentStatus.Suspended)
        {
            return new EligibilityResultDto 
            { 
                IsEligible = false, 
                Reason = "Registration blocked: Student account is suspended." 
            };
        }

        if (student.Major != null && !student.Major.IsRegistrationOpen)
        {
            return new EligibilityResultDto 
            { 
                IsEligible = false, 
                Reason = $"Registration blocked: Registration for major '{student.Major.Name}' is closed." 
            };
        }

        return new EligibilityResultDto { IsEligible = true };
    }

    public async Task<Student?> LoginAsync(string registrationNumber, string password)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber) || string.IsNullOrWhiteSpace(password))
            return null;

        var regNo = registrationNumber.Trim();
        var pass = password.Trim();

        return await _context.Students
            .FirstOrDefaultAsync(s => s.RegistrationNumber.Trim() == regNo && s.Password.Trim() == pass);
    }

    public async Task<List<global::Shared.Models.Course>> GetTranscriptCoursesAsync(int majorId)
    {
        return await _context.Courses
            .Where(c => c.MajorId == majorId)
            .Select(c => new global::Shared.Models.Course
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                CreditHours = c.CreditHours
            })
            .ToListAsync();
    }

    public async Task<List<CourseDto>> GetAvailableCoursesWithPrerequisitesAsync(int majorId)
    {
        return await _context.Courses
            .Where(c => c.MajorId == majorId)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                CreditHours = c.CreditHours,
                Prerequisites = c.Prerequisites
                    .Select(p => p.PrerequisiteCourse!.Code)
                    .ToList()
            })
            .ToListAsync();
    }

   public async Task<(bool IsSuccess, string ErrorMessage)> RegisterCoursesAsync(int studentId, List<SelectedCourseSlotDto> selectedCourseSlots)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return (false, "Student record not found.");
        }

        var eligibility = await CheckEligibilityAsync(studentId);
        if (!eligibility.IsEligible)
        {
            return (false, eligibility.Reason);
        }

        if (selectedCourseSlots == null || !selectedCourseSlots.Any())
        {
            return (false, "No course sections selected for registration.");
        }

        // 1. Prevent duplicate registration of the same course
        var courseIds = selectedCourseSlots.Select(s => s.CourseId).ToList();
        if (courseIds.Count != courseIds.Distinct().Count())
        {
            return (false, "You cannot select multiple sections for the same course.");
        }

        // 2. Fetch target courses with prerequisites in a single query
        var targetCourses = await _context.Courses
            .Include(c => c.Prerequisites)
            .ThenInclude(p => p.PrerequisiteCourse)
            .Where(c => courseIds.Contains(c.Id))
            .ToListAsync();

        // 3. Dynamic Credit Limit Check based on Student Status
        int maxAllowedCredits = student.Status switch
        {
            StudentStatus.Probation => 12,
            StudentStatus.Warning => 12,
            _ => 18
        };

        int requestedCredits = targetCourses.Sum(c => c.CreditHours);
        if (requestedCredits > maxAllowedCredits)
        {
            return (false, $"Credit limit exceeded! Maximum allowed: {maxAllowedCredits} credit hours. Requested: {requestedCredits} hours.");
        }

        // 4. Validate Prerequisites
        var completedCourseIds = await _context.StudentCourses
            .Where(sc => sc.StudentId == studentId)
            .Select(sc => sc.CourseId)
            .ToListAsync();

        foreach (var course in targetCourses)
        {
            var missingPrereqs = course.Prerequisites
                .Where(p => !completedCourseIds.Contains(p.PrerequisiteCourseId))
                .Select(p => p.PrerequisiteCourse!.Code)
                .ToList();

            if (missingPrereqs.Any())
            {
                return (false, $"Cannot register for '{course.Title}' ({course.Code}). Missing prerequisites: {string.Join(", ", missingPrereqs)}.");
            }
        }

        // 5. Schedule Conflict Validation
        var slotIds = selectedCourseSlots.Select(s => s.ScheduleSlotId).ToList();
        
        var incomingCourseCodes = targetCourses.Select(c => c.Code).ToList();

        var existingSlotIds = await _context.StudentRegistrations
            .Where(r => r.StudentId == studentId 
                    && r.ScheduleSlotId.HasValue 
                    && !incomingCourseCodes.Contains(r.CourseCode))
            .Select(r => r.ScheduleSlotId!.Value)
            .ToListAsync();

        var existingSlots = GetScheduleSlotsByIds(existingSlotIds);
        var incomingSlots = GetScheduleSlotsByIds(slotIds);

        foreach (var slot in incomingSlots)
        {
            Console.WriteLine($"Slot ID: {slot.Id}, Day: {slot.Day}, Start: {slot.StartTime}, End: {slot.EndTime}");
        }

        foreach (var newSlot in incomingSlots)
        {
            var existingClash = existingSlots.FirstOrDefault(existing =>
                existing.Day == newSlot.Day &&
                existing.StartTime < newSlot.EndTime &&
                existing.EndTime > newSlot.StartTime);

            if (existingClash != null)
            {
                return (false, $"Schedule conflict detected! Section on {newSlot.Day} ({newSlot.StartTime:hh\\:mm} - {newSlot.EndTime:hh\\:mm}) clashes with your existing schedule.");
            }
        }

        for (int i = 0; i < incomingSlots.Count; i++)
        {
            for (int j = i + 1; j < incomingSlots.Count; j++)
            {
                var slotA = incomingSlots[i];
                var slotB = incomingSlots[j];

                if (slotA.Day == slotB.Day && slotA.StartTime < slotB.EndTime && slotA.EndTime > slotB.StartTime)
                {
                    return (false, $"Schedule conflict detected between selected sections! Section on {slotA.Day} ({slotA.StartTime:hh\\:mm} - {slotA.EndTime:hh\\:mm}) clashes with another selected course.");
                }
            }
        }

        // 6. Overwrite Previous Term Registrations & Save New Entries
        var previousRegistrations = await _context.StudentRegistrations
            .Where(r => r.StudentId == studentId)
            .ToListAsync();

        if (previousRegistrations.Any())
        {
            _context.StudentRegistrations.RemoveRange(previousRegistrations);
        }

        foreach (var slot in selectedCourseSlots)
        {
            var course = targetCourses.First(c => c.Id == slot.CourseId);

            _context.StudentRegistrations.Add(new StudentRegistration
            {
                StudentId = studentId,
                CourseCode = course.Code,
                CreditHours = course.CreditHours,
                ScheduleSlotId = slot.ScheduleSlotId
            });
        }

        await _context.SaveChangesAsync();
        return (true, string.Empty);
    }

    private List<ScheduleSlotDto> GetScheduleSlotsByIds(List<int> slotIds)
    {
        var allSlots = new List<ScheduleSlotDto>
        {
            // SE101
            new ScheduleSlotDto { Id = 1, SectionName = "Section 1", Day = DayOfWeek.Monday, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0) },
            new ScheduleSlotDto { Id = 2, SectionName = "Section 2", Day = DayOfWeek.Wednesday, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(14, 0, 0) },
            
            // CS202
            new ScheduleSlotDto { Id = 3, SectionName = "Section 1", Day = DayOfWeek.Tuesday, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(14, 0, 0) },
            new ScheduleSlotDto { Id = 4, SectionName = "Section 2", Day = DayOfWeek.Thursday, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(16, 0, 0) },
            
            // DB301
            new ScheduleSlotDto { Id = 5, SectionName = "Section 1", Day = DayOfWeek.Wednesday, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(12, 0, 0) },
            new ScheduleSlotDto { Id = 6, SectionName = "Section 2", Day = DayOfWeek.Friday, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0) },
            
            // MATH101
            new ScheduleSlotDto { Id = 7, SectionName = "Section 1", Day = DayOfWeek.Wednesday, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0) },
            new ScheduleSlotDto { Id = 8, SectionName = "Section 2", Day = DayOfWeek.Friday, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(14, 0, 0) },
            
            // MATH102
            new ScheduleSlotDto { Id = 9, SectionName = "Section 1", Day = DayOfWeek.Wednesday, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0) },
            new ScheduleSlotDto { Id = 10, SectionName = "Section 2", Day = DayOfWeek.Monday, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(16, 0, 0) }
        };

        return allSlots.Where(s => slotIds.Contains(s.Id)).ToList();
    }
}