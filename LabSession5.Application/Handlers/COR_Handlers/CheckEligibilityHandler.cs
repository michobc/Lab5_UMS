using LabSession5.Application.Commands;
using LabSession5.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LabSession5.Application.Handlers.COR_Handlers;

public class CheckEligibilityHandler : GradeHandler
{
    private readonly UniversityContext _context;

    public CheckEligibilityHandler(UniversityContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(GradeCommand request)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        if (student == null)
        {
            throw new Exception("Student not found");
        }

        student.CanApplyToFrance = student.GradeAverage > 15;
        await _context.SaveChangesAsync();

        if (NextHandler != null)
        {
            await NextHandler.HandleAsync(request);
        }
    }
}
