using CourseStudent.Services;

namespace CourseStudent.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEnrollments()
    {
        var result = await _service.GetEnrollmentsAsync();
        return Ok(result);
    }
}
