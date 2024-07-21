using NpgsqlTypes;

namespace LabSession5.Domain.Models;

public partial class Course
{
    public long Id { get; set; }

    public string Name { get; set; }

    public int MaxStudentsNumber { get; set; }

    public NpgsqlRange<DateTime> EnrolmentDateRange { get; set; }

    public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; set; } = new List<TeacherPerCourse>();
}