using Asp.Versioning;
using LabSession5.Application.Commands;
using LabSession5.Application.Queries;
using LabSession5.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LabSession5.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/courses")]
[ApiVersion("1.0")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public CoursesController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromForm] AddCourse command)
    {
        var courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, courseId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(long id)
    {
        if (!_cache.TryGetValue($"Class_{id}", out CourseViewModel courseViewModel))
        {
            var courseview = await _mediator.Send(new GetCourseById { Id = id });
            if (courseview == null)
            {
                return NotFound();
            }
            _cache.Set($"Class_{id}", courseview, _cacheDuration);
            Console.WriteLine("added to cache");
            return Ok(courseview);
        }
        Console.WriteLine("got it from cache");
        return Ok(courseViewModel);
    }
}
