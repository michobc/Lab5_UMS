using MediatR;
using NpgsqlTypes;

namespace LabSession5.Application.Commands;

public class AddCourse : IRequest<long>
{
    public long UserId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public int MaxStudentsNumber { get; set; }
    public DateTime EnrolmentDateRangeLowerBound { get; set; }
    public DateTime EnrolmentDateRangeUpperBound { get; set; }
}