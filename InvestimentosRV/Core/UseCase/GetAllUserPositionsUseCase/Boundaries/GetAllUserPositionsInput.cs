using Core.Commons;
using MediatR;

namespace Core.UseCase.GetAllUserPositionsUseCase.Boundaries;

public sealed record GetAllUserPositionsInput(
    int UserId
) : IRequest<Output>;
