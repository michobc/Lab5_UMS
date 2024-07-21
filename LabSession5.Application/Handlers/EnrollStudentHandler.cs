using AutoMapper;
using LabSession5.Application.Commands;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabSession5.Application.Handlers;

public class EnrollStudentHandler : IRequestHandler<EnrollStudent, ClassEnrollementViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public EnrollStudentHandler(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassEnrollementViewModel> Handle(EnrollStudent request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.FindAsync(request.StudentId);
        if (user == null)
        {
            throw new ArgumentException("No user found");
        }
        if (user.RoleId != 3)
        {
            throw new Exception("You're not a student");
        }
        
        // Retrieve the course
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

        if (course == null)
        {
            throw new Exception("Course not found");
        }

        // Check if the current date is within the allowed enrollment date range
        var now = DateTime.UtcNow;
        if (now < course.EnrolmentDateRange.Value.LowerBound || now > course.EnrolmentDateRange.Value.UpperBound)
        {
            throw new Exception("Enrollment is not allowed passed due.");
        }

        var myclass = await _context.TeacherPerCourses
            .FirstOrDefaultAsync(t => t.CourseId == request.CourseId, cancellationToken);
        
        // Check if the student is already enrolled in the class
        var isAlreadyEnrolled = await _context.ClassEnrollments
            .AnyAsync(ce => ce.StudentId == request.StudentId && ce.ClassId == myclass.Id, cancellationToken);

        if (isAlreadyEnrolled)
        {
            throw new Exception("Student is already enrolled in this course.");
        }

        // Add the enrollment
        var enrollment = new ClassEnrollment
        {
            StudentId = request.StudentId,
            ClassId = myclass.Id
        };
    
        _context.ClassEnrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ClassEnrollementViewModel>(enrollment);
    }
}