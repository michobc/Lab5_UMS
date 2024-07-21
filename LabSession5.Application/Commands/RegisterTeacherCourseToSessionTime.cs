using LabSession5.Application.ViewModels;
using MediatR;

namespace LabSession5.Application.Commands;

public class RegisterTeacherCourseToSessionTime : IRequest<TeacherPerCoursePerSessionTimeViewModel>
{
    public long TeacherPerCourseId { get; set; }
    public long SessionTimeId { get; set; }
}