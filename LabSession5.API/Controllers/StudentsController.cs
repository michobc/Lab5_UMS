using Asp.Versioning;
using LabSession5.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabSession5.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/students")]
[ApiVersion("1.0")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("enrollments")]
    public async Task<IActionResult> EnrollStudent([FromForm] EnrollStudent command)
    {
        await _mediator.Send(command);
        return Ok(command);
    }
}