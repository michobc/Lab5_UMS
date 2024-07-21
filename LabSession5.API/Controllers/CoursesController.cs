using Asp.Versioning;
using LabSession5.Application.Commands;
using LabSession5.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabSession5.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/courses")]
[ApiVersion("1.0")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
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
        var courseview = await _mediator.Send(new GetCourseById { Id = id });
        if (courseview == null)
        {
            return NotFound();
        }
        return Ok(courseview);
    }
}
