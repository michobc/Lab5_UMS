using AutoMapper;
using LabSession5.Application.Commands;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using MediatR;

namespace LabSession5.Application.Handlers;

public class RegisterTeacherCourseToSessionTimeHandler: IRequestHandler<RegisterTeacherCourseToSessionTime, TeacherPerCoursePerSessionTimeViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public RegisterTeacherCourseToSessionTimeHandler(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeacherPerCoursePerSessionTimeViewModel> Handle(RegisterTeacherCourseToSessionTime request, CancellationToken cancellationToken)
    {
        var teacherCourse = new TeacherPerCoursePerSessionTime()
        {
            TeacherPerCourseId = request.TeacherPerCourseId,
            SessionTimeId = request.SessionTimeId
        };

        _context.TeacherPerCoursePerSessionTimes.Add(teacherCourse);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherPerCoursePerSessionTimeViewModel>(teacherCourse);;
    }
}