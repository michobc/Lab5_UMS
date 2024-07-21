using AutoMapper;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;

namespace LabSession5.Application.Mappings;

public class UniversityProfile : Profile
{
    public UniversityProfile()
    {
        CreateMap<Course, CourseViewModel>();
        CreateMap<SessionTime, SessionTimeViewModel>();
        CreateMap<TeacherPerCourse, TeacherPerCourseViewModel>();
        CreateMap<TeacherPerCoursePerSessionTime, TeacherPerCoursePerSessionTimeViewModel>();
    }
}