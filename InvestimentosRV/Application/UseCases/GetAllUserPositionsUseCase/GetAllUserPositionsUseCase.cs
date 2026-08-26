using Application.Commons;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.Mappers;
using Application.UseCases.GetAllUserPositionsUseCase.Boundaries;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.GetAllUserPositionsUseCase;

public class GetAllUserPositionsUseCase(
    IPositionRepository positionRepository,
    ILogger<GetAllUserPositionsUseCase> logger
) : IGetAllUserPositionsUseCase
{
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ILogger<GetAllUserPositionsUseCase> _logger = logger;

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
