using CourseStudent.Data;
using CourseStudent.DTOs;
using CourseStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseStudent.Services 
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnrollmentResponseDto>> GetEnrollmentsAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentResponseDto
                {
                    Student = new StudentDto
                    {
                        Id = e.Student.Id,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Email = e.Student.Email
                    },
                    Course = new CourseDto
                    {
                        Id = e.Course.Id,
                        Title = e.Course.Title,
                        Teacher = e.Course.Teacher
                    },
                    EnrollmentDate = e.EnrollmentDate
                })
                .ToListAsync();
        }

        public async Task<object> CreateCourseWithEnrollmentsAsync(CreateCourseWithEnrollmentsDto dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Credits = dto.Credits,
                Teacher = dto.Teacher
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var enrollments = new List<object>();

            foreach (var studentDto in dto.Students)
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s =>
                        s.FirstName == studentDto.FirstName &&
                        s.LastName == studentDto.LastName &&
                        s.Email == studentDto.Email);

                if (student == null)
                {
                    student = new Student
                    {
                        FirstName = studentDto.FirstName,
                        LastName = studentDto.LastName,
                        Email = studentDto.Email
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                }

                var enrollment = new Enrollment
                {
                    StudentId = student.Id,
                    CourseId = course.Id,
                    EnrollmentDate = DateTime.UtcNow
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                enrollments.Add(new
                {
                    studentId = student.Id,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    enrollmentDate = enrollment.EnrollmentDate
                });
            }

            return new
            {
                message = "Kurs został utworzony i studenci zostali zapisani.",
                course = new
                {
                    course.Id,
                    course.Title,
                    course.Credits,
                    course.Teacher
                },
                enrollments
            };
        }
    }
}
