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
        Console.WriteLine(request.UserId);
        User user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            throw new ArgumentException("Not a user");
        }
        if (user.RoleId != 1)
        {
            throw new Exception("Admins only can create courses");
        }
        var course = new Course
        {
            Id = request.Id,
            Name = request.Name,
            MaxStudentsNumber = request.MaxStudentsNumber,
            EnrolmentDateRange = new NpgsqlRange<DateTime>(DateTime.Parse(request.EnrolmentDateRangeLowerBound).ToUniversalTime(), DateTime.Parse(request.EnrolmentDateRangeUpperBound).ToUniversalTime())
        };
        Console.WriteLine("Course EnrolmentDateRange: " + course.Name);
        
        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);
        return course.Id;
    }
}