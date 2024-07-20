using LabSession5.Application.ViewModels;
using MediatR;

namespace LabSession5.Application.Queries;

public class GetCourseById : IRequest<CourseViewModel>
{ 
    public long Id { get; set; }
}