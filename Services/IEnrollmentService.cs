using CourseStudent.DTOs;

namespace CourseStudent.Services;

public interface IEnrollmentService
{
    Task<List<EnrollmentResponseDto>> GetEnrollmentsAsync();
    Task<object> CreateCourseWithEnrollmentsAsync(CreateCourseWithEnrollmentsDto dto); 
    Task<object> UpdateCourseWithEnrollmentsAsync(int courseId, CreateCourseWithEnrollmentsDto dto);

}