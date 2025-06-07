public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Credits { get; set; } = null!;
    public string Teacher { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}