using MediatR;

namespace LabSession5.Application.Commands;

public class GradeCommand : IRequest
{
    public long StudentId { get; set; }
    public long ClassId { get; set; } // teacherPerCourseID
    public double GradeValue { get; set; }
    
    public int Coefficent { get; set; }
}