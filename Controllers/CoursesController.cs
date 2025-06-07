using CourseStudent.DTOs;
using CourseStudent.Services;

namespace CourseStudent.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public CoursesController(IEnrollmentService service)
    {
        _service = service;
    }

    [HttpPost("with-enrollments")]
    public async Task<IActionResult> CreateCourseWithEnrollments([FromBody] CreateCourseWithEnrollmentsDto dto)
    {
        var result = await _service.CreateCourseWithEnrollmentsAsync(dto);
        return Ok(result);
    }
}
