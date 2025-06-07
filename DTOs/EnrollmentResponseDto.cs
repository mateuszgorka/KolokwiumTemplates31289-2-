namespace CourseStudent.DTOs;

public class EnrollmentResponseDto
{
    public StudentDto Student { get; set; } = null!;
    public CourseDto Course { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
}
