using CourseStudent.DTOs;

namespace CourseStudent.Services;

public interface IEnrollmentService
{
    Task<List<EnrollmentResponseDto>> GetEnrollmentsAsync();
    Task<object> CreateCourseWithEnrollmentsAsync(CreateCourseWithEnrollmentsDto dto); 
    Task<object> UpdateCourseWithEnrollmentsAsync(int courseId, CreateCourseWithEnrollmentsDto dto);
    
    Task<object> DeleteCourseAsync(int courseId);
    
    
    Task<bool> DeleteEnrollmentAsync(int studentId, int courseId);

}