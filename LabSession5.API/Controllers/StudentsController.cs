using Asp.Versioning;
using LabSession5.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LabSession5.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public StudentsController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }
    
    [HttpPost("enrollments")]
    public async Task<IActionResult> EnrollStudent([FromForm] EnrollStudent command)
    {
        await _mediator.Send(command);
        return Ok(command);
    }
}