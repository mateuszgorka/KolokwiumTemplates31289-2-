using CourseStudent.DTOs;
using CourseStudent.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseStudent.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public StudentsController(IEnrollmentService service)
    {
        _service = service;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto dto)
    {
        var result = await _service.UpdateStudentAsync(id, dto);
        return Ok(result);
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var result = await _service.DeleteStudentAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDto dto)
    {
        var result = await _service.CreateStudentAsync(dto);
        return Ok(result);
    }
}
