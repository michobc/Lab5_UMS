using LabSession5.Application.Commands;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LabSession5.Application.Handlers.COR_Handlers;

public class SetGradeHandler : GradeHandler
{
    private readonly UniversityContext _context;

    public SetGradeHandler(UniversityContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(GradeCommand request)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);
        
        if (student.RoleId != 3)
        {
            throw new Exception("Setting a grade for a non student");
        }
        
        var enrolledInClass = await _context.ClassEnrollments
            .AnyAsync(e => e.StudentId == request.StudentId && e.ClassId == request.ClassId);

        if (enrolledInClass == false)
        {
            throw new Exception("Student is not enrolled in this class");
        }
        
        var grade = new Grade
        {
            StudentId = request.StudentId,
            ClassId = request.ClassId,
            Value = 20*request.GradeValue/request.Coefficent, // calculate grade /20 base sur le coefficent du teacher 
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        if (NextHandler != null)
        {
            await NextHandler.HandleAsync(request);
        }
    }
}
