using Asp.Versioning;
using LabSession5.Application.Commands;
using LabSession5.Application.Handlers;
using LabSession5.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LabSession5.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teachers")]
[ApiVersion("1.0")]
public class TeachersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GradeService _gradeService;

    public TeachersController(IMediator mediator, GradeService gradeService)
    {
        _mediator = mediator;
        _gradeService = gradeService;
    }

    [HttpPost("{teacherId}/courses/{courseId}")]
    public async Task<IActionResult> RegisterTeacherToCourse( long teacherId, long courseId)
    {
        var command = new RegisterTeacherToCourse { TeacherId = teacherId, CourseId = courseId };
        await _mediator.Send(command);
        return Ok(command);
    }
    
    [HttpPost("timeslots")]
    public async Task<IActionResult> CreateTimeSlot([FromForm] CreateTimeSlot command)
    {
        await _mediator.Send(command);
        return Ok(command);
    }
    
    [HttpPost("teacherCourses/{teacherPerCourseId}/sessionTime/{sessionTimeId}")]
    public async Task<IActionResult> RegisterCourseToSessionTime(long teacherPerCourseId, long sessionTimeId)
    {
        var command = new RegisterTeacherCourseToSessionTime { TeacherPerCourseId = teacherPerCourseId, SessionTimeId = sessionTimeId };
        await _mediator.Send(command);
        return Ok(command);
    }
    
    [HttpPost("setGrade")]
    public async Task<IActionResult> SetGrade([FromForm] GradeCommand request)
    {
        try
        {
            await _gradeService.SetGradeAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}