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
    
    
    [HttpDelete("{studentId}/{courseId}")]
    public async Task<IActionResult> DeleteEnrollment(int studentId, int courseId)
    {
        var success = await _service.DeleteEnrollmentAsync(studentId, courseId);
        if (!success)
            return NotFound(new { message = "Nie ma takiego i koniec" });

        return Ok(new { message = "Student wypisany (Rudy sie nie dostal)" });
    }

    
}
