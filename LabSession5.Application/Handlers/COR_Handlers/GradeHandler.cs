using LabSession5.Application.Commands;

namespace LabSession5.Application.Handlers.COR_Handlers;

public abstract class GradeHandler
{
    protected GradeHandler? NextHandler;

    public void SetNextHandler(GradeHandler nextHandler)
    {
        NextHandler = nextHandler;
    }

    public abstract Task HandleAsync(GradeCommand request);
}