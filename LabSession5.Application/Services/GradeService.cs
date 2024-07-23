using LabSession5.Application.Commands;
using LabSession5.Application.Handlers.COR_Handlers;
using LabSession5.Persistence.Data;

namespace LabSession5.Application.Services;

public class GradeService
{
    private readonly GradeHandler _chain;

    public GradeService(UniversityContext context)
    {
        var setGradeHandler = new SetGradeHandler(context);
        var updateAverageHandler = new UpdateAverageHandler(context);
        var checkEligibilityHandler = new CheckEligibilityHandler(context);

        setGradeHandler.SetNextHandler(updateAverageHandler);
        updateAverageHandler.SetNextHandler(checkEligibilityHandler);

        _chain = setGradeHandler;
    }

    public async Task SetGradeAsync(GradeCommand request)
    {
        await _chain.HandleAsync(request);
    }
}
