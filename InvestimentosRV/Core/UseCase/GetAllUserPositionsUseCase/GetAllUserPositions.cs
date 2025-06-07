using Core.Commons;
using Core.Interfaces;
using Core.Mappers;
using Core.UseCase.GetAllUserPositionsUseCase.Boundaries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase.GetAllUserPositionsUseCase;

public class GetAllUserPositions(
    IPositionRepository positionRepository,
    ILogger<GetAllUserPositions> logger
) : IRequestHandler<GetAllUserPositionsInput, Output>
{
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ILogger<GetAllUserPositions> _logger = logger;

    public async Task<Output> Handle(GetAllUserPositionsInput input, CancellationToken cancellationToken)
    {
        Output output = new();

        var positions = await _positionRepository.GetAllByUserIdAsync(input.UserId, cancellationToken);
        if (positions is null || !positions.Any())
        {
            _logger.LogWarning("No positions found for user {UserId}.", input.UserId);

            output.AddErrorMessage($"No positions found for user with ID {input.UserId}.");
            return output;
        }

        output.AddResult(positions.MapToDto());
        return output;
    }
}
