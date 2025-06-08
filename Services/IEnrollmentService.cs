using CourseStudent.DTOs;

namespace CourseStudent.Services;

public interface IEnrollmentService
{
    //  GET /api/enrollments
    //  EnrollmentsController
    Task<List<EnrollmentResponseDto>> GetEnrollmentsAsync();

    //  POST /api/courses/with-enrollments
    //  CoursesController
    Task<object> CreateCourseWithEnrollmentsAsync(CreateCourseWithEnrollmentsDto dto); 

    //  PUT /api/courses/{courseId}
    //  CoursesController
    Task<object> UpdateCourseWithEnrollmentsAsync(int courseId, CreateCourseWithEnrollmentsDto dto);

    //  DELETE /api/courses/{courseId}
    //  CoursesController
    Task<object> DeleteCourseAsync(int courseId);

    //  DELETE /api/enrollments/{studentId}/{courseId}
    //  EnrollmentsController
    Task<bool> DeleteEnrollmentAsync(int studentId, int courseId);

    //  PUT /api/students/{studentId}
    //  StudentsController
    Task<object> UpdateStudentAsync(int studentId, StudentDto dto);
}