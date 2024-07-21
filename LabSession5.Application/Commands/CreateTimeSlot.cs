using LabSession5.Application.ViewModels;
using MediatR;

namespace LabSession5.Application.Commands;

public class CreateTimeSlot : IRequest<SessionTimeViewModel>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}