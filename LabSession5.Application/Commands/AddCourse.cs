using MediatR;
using NpgsqlTypes;

namespace LabSession5.Application.Commands;

public class AddCourse : IRequest<long>
{
    public string Name { get; set; }
    public int MaxStudentsNumber { get; set; }
    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }
}