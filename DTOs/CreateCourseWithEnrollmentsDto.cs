namespace CourseStudent.DTOs;

public class CreateCourseWithEnrollmentsDto
{
    public string Title { get; set; } = null!;
    public string Credits { get; set; } = null!;
    public string Teacher { get; set; } = null!;
    public List<StudentDto> Students { get; set; } = new();
    
}
