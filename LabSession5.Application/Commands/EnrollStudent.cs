using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using MediatR;

namespace LabSession5.Application.Commands;

public class EnrollStudent : IRequest<ClassEnrollementViewModel>
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long ClassId { get; set; }
}