using LabSession5.Application.Commands;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using MediatR;
using NpgsqlTypes;

namespace LabSession5.Application.Handlers;

public class AddCourseHandler : IRequestHandler<AddCourse, long>
{
    private readonly UniversityContext _context;

    public AddCourseHandler(UniversityContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(AddCourse request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Name = request.Name,
            MaxStudentsNumber = request.MaxStudentsNumber,
            EnrolmentDateRange = new NpgsqlRange<DateOnly>(request.EnrolmentDateRange.Value.LowerBound, request.EnrolmentDateRange.Value.UpperBound)
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course.Id;
    }
}