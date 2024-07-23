using AutoMapper;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using NpgsqlTypes;

namespace LabSession5.Application.Mappings;

public class UniversityProfile : Profile
{
    public UniversityProfile()
    {
        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.EnrolmentDateRangeLowerBound, opt => opt.MapFrom(src => src.EnrolmentDateRange.HasValue ? src.EnrolmentDateRange.Value.LowerBound.ToUniversalTime() : DateTime.MinValue))
            .ForMember(dest => dest.EnrolmentDateRangeUpperBound, opt => opt.MapFrom(src => src.EnrolmentDateRange.HasValue ? src.EnrolmentDateRange.Value.UpperBound.ToUniversalTime() : DateTime.MinValue))
            .ReverseMap()
            .ForMember(dest => dest.EnrolmentDateRange, opt => opt.MapFrom(src => new NpgsqlRange<DateTime>(src.EnrolmentDateRangeLowerBound.ToUniversalTime(), src.EnrolmentDateRangeUpperBound.ToUniversalTime())));
        CreateMap<SessionTime, SessionTimeViewModel>();
        CreateMap<TeacherPerCourse, TeacherPerCourseViewModel>();
        CreateMap<TeacherPerCoursePerSessionTime, TeacherPerCoursePerSessionTimeViewModel>();
        CreateMap<ClassEnrollment, ClassEnrollementViewModel>();
    }
}