using AutoMapper;
using LabSession5.Application.Queries;
using LabSession5.Application.ViewModels;
using LabSession5.Persistence.Data;
using MediatR;

namespace LabSession5.Application.Handlers;

public class GetCourseByIdHandler: IRequestHandler<GetCourseById, CourseViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public GetCourseByIdHandler(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CourseViewModel> Handle(GetCourseById request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync(request.Id);
        return _mapper.Map<CourseViewModel>(course);
    }
}