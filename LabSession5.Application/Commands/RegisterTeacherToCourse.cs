using LabSession5.Application.ViewModels;
using MediatR;

namespace LabSession5.Application.Commands;

public class RegisterTeacherToCourse : IRequest<TeacherPerCourseViewModel>
{
    public long TeacherId { get; set; }
    public long CourseId { get; set; }
}