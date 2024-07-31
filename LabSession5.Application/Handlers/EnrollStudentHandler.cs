using System.Text;
using AutoMapper;
using LabSession5.Application.Commands;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using LabSession5.Infrastructure.Services;
using LabSession5.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LabSession5.Application.Handlers;

public class EnrollStudentHandler : IRequestHandler<EnrollStudent, ClassEnrollementViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;
    private readonly RabbitMqService _rabbitMqService;

    public EnrollStudentHandler(UniversityContext context, IMapper mapper, RabbitMqService rabbitMqService)
    {
        _context = context;
        _mapper = mapper;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<ClassEnrollementViewModel> Handle(EnrollStudent request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.FindAsync(request.StudentId);
        if (user == null)
        {
            throw new ArgumentException("No user found");
        }
        if (user.RoleId != 3)
        {
            throw new Exception("You're not a student");
        }
        
        // Retrieve the class
        var myclass = await _context.TeacherPerCourses
            .FirstOrDefaultAsync(t => t.Id == request.ClassId, cancellationToken);
        
        if (myclass == null)
        {
            throw new Exception("Class not found");
        }
        
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == myclass.CourseId, cancellationToken);

        var teacher = await _context.Users
            .FirstOrDefaultAsync(t => t.Id == myclass.TeacherId, cancellationToken);

        // Check if the current date is within the allowed enrollment date range
        var now = DateTime.UtcNow;
        if (now < course.EnrolmentDateRange.Value.LowerBound || now > course.EnrolmentDateRange.Value.UpperBound)
        {
            throw new Exception("Enrollment is not allowed passed due.");
        }
        
        // Check if the student is already enrolled in the class
        var isAlreadyEnrolled = await _context.ClassEnrollments
            .AnyAsync(ce => ce.StudentId == request.StudentId && ce.ClassId == myclass.Id, cancellationToken);
        
        if (isAlreadyEnrolled)
        {
            throw new Exception("Student is already enrolled in this class.");
        }

        // Add the enrollment
        var enrollment = new ClassEnrollment
        {
            StudentId = request.StudentId,
            ClassId = myclass.Id
        };
    
        _context.ClassEnrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Publish message to RabbitMQ
        var channel = _rabbitMqService.GetChannel();
        if (channel == null)
        {
            throw new Exception("RabbitMQ channel is not initialized.");
        }

        var message = JsonConvert.SerializeObject(new
        {
            enrollment.Id,
            enrollment.ClassId,
            enrollment.StudentId,
            StudentName = user.Name,
            CourseName = course.Name,
            TeacherName = teacher.Name
        });

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "",
            routingKey: "class_updates",
            basicProperties: null,
            body: body);

        return _mapper.Map<ClassEnrollementViewModel>(enrollment);
    }
}