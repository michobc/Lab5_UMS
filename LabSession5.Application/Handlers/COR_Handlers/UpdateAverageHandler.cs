using LabSession5.Application.Commands;
using LabSession5.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LabSession5.Application.Handlers.COR_Handlers;

public class UpdateAverageHandler : GradeHandler
{
    private readonly UniversityContext _context;

    public UpdateAverageHandler(UniversityContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(GradeCommand request)
    {
        var student = await _context.Users
            .Include(s => s.Grades)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        if (student == null)
        {
            throw new Exception("Student not found");
        }

        student.GradeAverage = student.Grades.Average(g => g.Value);
        await _context.SaveChangesAsync();

        if (NextHandler != null)
        {
            await NextHandler.HandleAsync(request);
        }
    }
}
