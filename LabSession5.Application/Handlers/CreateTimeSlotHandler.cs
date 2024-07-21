using AutoMapper;
using LabSession5.Application.Commands;
using LabSession5.Application.ViewModels;
using LabSession5.Domain.Models;
using LabSession5.Persistence.Data;
using MediatR;

namespace LabSession5.Application.Handlers;

public class CreateTimeSlotHandler : IRequestHandler<CreateTimeSlot, SessionTimeViewModel>
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public CreateTimeSlotHandler(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SessionTimeViewModel> Handle(CreateTimeSlot request, CancellationToken cancellationToken)
    {
        var sessionTime = new SessionTime
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Duration = (int)(request.EndTime - request.StartTime).TotalMinutes
        };

        _context.SessionTimes.Add(sessionTime);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SessionTimeViewModel>(sessionTime);
    }
}