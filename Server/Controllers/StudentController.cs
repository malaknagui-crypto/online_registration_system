using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;
using Shared.Models;
using Shared;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly StudentService _studentService;

    public StudentController(AppDbContext context, StudentService studentService)
    {
        _context = context;
        _studentService = studentService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        Console.WriteLine($"[DEBUG] Login attempt -> RegNo: '{request?.RegistrationNumber}', Password: '{request?.Password}'");

        if (request == null)
            return BadRequest("Invalid request payload.");

        var student = await _studentService.LoginAsync(request.RegistrationNumber, request.Password);

        if (student == null)
        {
            Console.WriteLine("[DEBUG] Authentication failed: No matching student record found in database.");
            return Unauthorized("Invalid Registration Number or Password.");
        }

        Console.WriteLine($"[DEBUG] Success! Authenticated student: {student.FullName}");
        return Ok(student);
    }

    [HttpGet("{studentId}/check-eligibility")]
    public async Task<IActionResult> CheckEligibility(int studentId)
    {
        var result = await _studentService.CheckEligibilityAsync(studentId);
        return Ok(result);
    }

    [HttpGet("courses/{majorId}")]
    public async Task<IActionResult> GetCourses(int majorId)
    {
        var courses = await _studentService.GetTranscriptCoursesAsync(majorId);
        return Ok(courses);
    }

    [HttpGet("{studentId}/completed-courses")]
    public async Task<IActionResult> GetCompletedCourses(int studentId)
    {
        var completedCourseCodes = await _context.CompletedCourses
            .Where(cc => cc.StudentId == studentId)
            .Select(cc => cc.CourseCode)
            .ToListAsync();

        return Ok(completedCourseCodes);
    }

    [HttpGet("{studentId}/header")]
    public async Task<ActionResult<StudentHeaderDto>> GetStudentHeader(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Faculty)
            .Include(s => s.Major)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null) return NotFound();

        return Ok(new StudentHeaderDto
        {
            StudentId = student.Id,
            FullName = student.FullName,
            FacultyName = student.Faculty?.Name ?? "N/A",
            FacultyId = student.FacultyId ?? 1, 
            MajorName = student.Major?.Name ?? "N/A",
            CurrentSemester = "Fall 2026/2027",
            Gpa = student.Gpa
        });
    }

    public class RegisterCoursesRequest
    {
        public List<SelectedCourseSlotDto> SelectedSlots { get; set; } = new();
    }

    [HttpPost("{id}/register")]
    public async Task<IActionResult> RegisterCourses(int id, [FromBody] RegisterCoursesRequest request)
    {
        if (request == null || request.SelectedSlots == null || !request.SelectedSlots.Any())
        {
            return BadRequest("Invalid payload. Please select at least one course section.");
        }

        var (isSuccess, errorMessage) = await _studentService.RegisterCoursesAsync(id, request.SelectedSlots);

        if (!isSuccess)
        {
            return BadRequest(errorMessage);
        }

        return Ok(new { Message = "Registration saved successfully!" });
    }
}