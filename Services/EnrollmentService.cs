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
        
        public async Task<object> UpdateCourseWithEnrollmentsAsync(int courseId, CreateCourseWithEnrollmentsDto dto)
{
    var course = await _context.Courses.Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == courseId);  // -----> kurs o danym id, w sensie ze id wpisane do metody
    if (course == null) return new { message = "There is no takiego kursu" };
    
    
    course.Title = dto.Title;
    course.Credits = dto.Credits;
    course.Teacher = dto.Teacher;

   
    var oldEnrollments = _context.Enrollments.Where(e => e.CourseId == courseId); // _> delete enrollments
    _context.Enrollments.RemoveRange(oldEnrollments);
    await _context.SaveChangesAsync();   // ----> zmiany w bazie
        
    var newEnrollments = new List<object>();

    foreach (var studentDto in dto.Students)
    {
        var student = await _context.Students  // ->>>>>> szukamy studenta po imimieniu, naziwsku i mailu 
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
            _context.Students.Add(student);                // ---->> zapisujemuy  nowego studenta
            await _context.SaveChangesAsync();
        }
        
        
        // =------> nowy wpis enrollment (zapisywanie)
        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow
        };
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        newEnrollments.Add(new
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
        course = new
        {
            course.Id,
            course.Title,
            course.Credits,
            course.Teacher
        },
        enrollments = newEnrollments
    };
}

        public async Task<object> DeleteCourseAsync(int courseId)
        {
            
            var course = await _context.Courses     // ---->>> pobieramy dany kurs
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            
            if (course == null)
                return new { message = "Istnieje dany kurs" };

           
            _context.Enrollments.RemoveRange(course.Enrollments);
            _context.Courses.Remove(course);

            
            await _context.SaveChangesAsync();   // -> zapisywanie zmian w bazie

            
            return new
            {
                message = "Usunieto"
            };
        }

        public async Task<bool> DeleteEnrollmentAsync(int studentId, int courseId)
        {
            
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);   // ----> znajdz id

           
            if (enrollment == null)    // jesli nie istnieje to false 
                return false;

           
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
