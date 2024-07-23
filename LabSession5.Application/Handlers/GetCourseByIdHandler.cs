using AutoMapper;
using LabSession5.Application.Queries;
using LabSession5.Application.ViewModels;
using LabSession5.Persistence.Data;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LabSession5.Application.Handlers;

public class GetCourseByIdHandler: IRequestHandler<GetCourseById, CourseViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public GetCourseByIdHandler(UniversityContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<CourseViewModel> Handle(GetCourseById request, CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue($"Class_{request.Id}", out CourseViewModel courseViewModel))
        {
            var course = await _context.Courses.FindAsync(request.Id);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            var courseview = _mapper.Map<CourseViewModel>(course);
            _cache.Set($"Class_{request.Id}", courseview, _cacheDuration);
            Console.WriteLine("added to cache");
            return courseview;
        }
        Console.WriteLine("From cache");
        return courseViewModel;
    }
}