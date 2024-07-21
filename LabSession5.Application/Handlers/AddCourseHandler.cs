using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
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
        // Console.WriteLine("HEREEEEE : " + request.EnrolmentDateRange.LowerBound.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
        var course = new Course
        {
            Id = request.Id,
            Name = request.Name,
            MaxStudentsNumber = request.MaxStudentsNumber,
            EnrolmentDateRange = new NpgsqlRange<DateTime>(request.EnrolmentDateRange.LowerBound, request.EnrolmentDateRange.UpperBound)
        };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);
        
        return course.Id;
    }
}