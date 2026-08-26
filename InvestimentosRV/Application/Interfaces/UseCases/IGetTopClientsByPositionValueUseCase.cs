using Application.Commons;
using Application.UseCases.GetTopClientsByPositionValueUseCase.Boundaries;

namespace Application.Interfaces.UseCases;

public interface IGetTopClientsByPositionValueUseCase
{
    Task<Output> Handle(GetTopClientsByPositionValueInput input, CancellationToken cancellationToken);
}
