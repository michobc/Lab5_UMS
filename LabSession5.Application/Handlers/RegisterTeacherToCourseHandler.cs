using AutoMapper;
using LabSession5.Application.Commands;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using MediatR;

namespace LabSession5.Application.Handlers;

public class RegisterTeacherToCourseHandler : IRequestHandler<RegisterTeacherToCourse, TeacherPerCourseViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public RegisterTeacherToCourseHandler(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeacherPerCourseViewModel> Handle(RegisterTeacherToCourse request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.FindAsync(request.TeacherId);
        if (user == null)
        {
            throw new ArgumentException("No user found");
        }
        if (user.RoleId != 2)
        {
            throw new Exception("You're not a teacher");
        }
        var teacherCourse = new TeacherPerCourse
        {
            TeacherId = request.TeacherId,
            CourseId = request.CourseId
        };

        _context.TeacherPerCourses.Add(teacherCourse);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TeacherPerCourseViewModel>(teacherCourse);;
    }
}