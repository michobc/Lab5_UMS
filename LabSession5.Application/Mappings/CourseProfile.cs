using AutoMapper;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;

namespace LabSession5.Application.Mappings;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseViewModel>();
    }
}