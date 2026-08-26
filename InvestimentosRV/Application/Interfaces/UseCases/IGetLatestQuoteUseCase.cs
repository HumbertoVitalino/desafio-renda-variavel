using Application.Commons;
using Application.UseCases.GetLatestQuoteUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetLatestQuoteUseCase
{
    Task<Output> Handle(GetLatestQuoteInput input, CancellationToken cancellationToken);
}
