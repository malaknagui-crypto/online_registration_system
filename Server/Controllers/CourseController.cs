using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

   [HttpPost("{studentId}/register")]

    [HttpGet("available/{facultyId}/{majorId}")]
    public async Task<ActionResult<List<CourseDto>>> GetAvailableCourses(int facultyId, int majorId)
    {
        try
        {
            // 1. Query courses from database
            var dbCourses = await _context.Courses
                .Where(c => c.FacultyId == facultyId && c.MajorId == majorId)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Title,
                    c.CreditHours
                })
                .ToListAsync();

            // 2. Map DTOs with schedule slots and prerequisites
            var result = dbCourses
                .DistinctBy(c => c.Code)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Title = c.Title,
                    CreditHours = c.CreditHours,
                    Capacity = 30,
                    EnrolledCount = 10,
                    Prerequisites = c.Code switch
                    {
                        "SE101" => new List<string> { "CS202" },
                        "MATH102" => new List<string> { "MATH101" },
                        _ => new List<string>()
                    },
                    ScheduleSlots = c.Code switch
                    {
                        "SE101" => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = 1, 
                                SectionName = "Section 1", 
                                LecturerName = "Dr. Margaret Hamilton", 
                                TaName = "Eng. Michael", 
                                Day = DayOfWeek.Monday, 
                                StartTime = new TimeSpan(8, 30, 0), 
                                EndTime = new TimeSpan(10, 0, 0) 
                            },
                            new ScheduleSlotDto 
                            { 
                                Id = 2, 
                                SectionName = "Section 2", 
                                LecturerName = "Dr. Grace Hopper", 
                                TaName = "Eng. Sarah", 
                                Day = DayOfWeek.Wednesday, 
                                StartTime = new TimeSpan(12, 30, 0), 
                                EndTime = new TimeSpan(14, 0, 0) 
                            }
                        },
                        "CS202" => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = 3, 
                                SectionName = "Section 1", 
                                LecturerName = "Dr. Alan Turing", 
                                TaName = "Eng. Omar", 
                                Day = DayOfWeek.Tuesday, 
                                StartTime = new TimeSpan(12, 30, 0), 
                                EndTime = new TimeSpan(14, 0, 0) 
                            },
                            new ScheduleSlotDto 
                            { 
                                Id = 4, 
                                SectionName = "Section 2", 
                                LecturerName = "Dr. John von Neumann", 
                                TaName = "Eng. Jessica", 
                                Day = DayOfWeek.Thursday, 
                                StartTime = new TimeSpan(14, 30, 0), 
                                EndTime = new TimeSpan(16, 0, 0) 
                            }
                        },
                        "DB301" => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = 5, 
                                SectionName = "Section 1", 
                                LecturerName = "Dr. Edgar Codd", 
                                TaName = "Eng. Omar", 
                                Day = DayOfWeek.Wednesday, 
                                StartTime = new TimeSpan(10, 30, 0), 
                                EndTime = new TimeSpan(12, 0, 0) 
                            },
                            new ScheduleSlotDto 
                            { 
                                Id = 6, 
                                SectionName = "Section 2", 
                                LecturerName = "Dr. Raymond Boyce", 
                                TaName = "Eng. Michael", 
                                Day = DayOfWeek.Friday, 
                                StartTime = new TimeSpan(8, 30, 0), 
                                EndTime = new TimeSpan(10, 0, 0) 
                            }
                        },
                        "MATH101" => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = 7, 
                                SectionName = "Section 1", 
                                LecturerName = "Dr. Ahmed", 
                                TaName = "Eng. Omar", 
                                Day = DayOfWeek.Wednesday, 
                                StartTime = new TimeSpan(8, 30, 0), 
                                EndTime = new TimeSpan(10, 0, 0) 
                            },
                            new ScheduleSlotDto 
                            { 
                                Id = 8, 
                                SectionName = "Section 2", 
                                LecturerName = "Dr. Mona", 
                                TaName = "Eng. Michael", 
                                Day = DayOfWeek.Friday, 
                                StartTime = new TimeSpan(12, 30, 0), 
                                EndTime = new TimeSpan(14, 0, 0) 
                            }
                        },
                        "MATH102" => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = 9, 
                                SectionName = "Section 1", 
                                LecturerName = "Dr. Ahmed", 
                                TaName = "Eng. Omar", 
                                Day = DayOfWeek.Wednesday, 
                                StartTime = new TimeSpan(8, 30, 0), 
                                EndTime = new TimeSpan(10, 0, 0) 
                            },
                            new ScheduleSlotDto 
                            { 
                                Id = 10, 
                                SectionName = "Section 2", 
                                LecturerName = "Dr. Mona", 
                                TaName = "Eng. Michael", 
                                Day = DayOfWeek.Monday, 
                                StartTime = new TimeSpan(14, 30, 0), 
                                EndTime = new TimeSpan(16, 0, 0) 
                            }
                        },
                        // Default fallback slot for any other course in database
                        _ => new List<ScheduleSlotDto>
                        {
                            new ScheduleSlotDto 
                            { 
                                Id = c.Id * 10, 
                                SectionName = "Section 1", 
                                LecturerName = "Staff", 
                                TaName = "TA", 
                                Day = DayOfWeek.Monday, 
                                StartTime = new TimeSpan(8, 30, 0), 
                                EndTime = new TimeSpan(10, 0, 0) 
                            }
                        }
                    }
                }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database Query Error: {ex.Message}");
        }
    }
}