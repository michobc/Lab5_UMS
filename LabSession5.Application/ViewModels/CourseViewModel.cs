using NpgsqlTypes;

namespace LabSession5.Application.ViewModels;

public class CourseViewModel
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }

    public DateTime EnrolmentDateRangeLowerBound { get; set; }
    
    public DateTime EnrolmentDateRangeUpperBound { get; set; }
}