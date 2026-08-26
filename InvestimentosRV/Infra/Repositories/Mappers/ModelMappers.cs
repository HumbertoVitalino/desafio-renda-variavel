using Domain.Entities;
using Infra.Repositories.Models;

namespace Infra.Repositories.Mappers;

internal static class ModelMappers
{
    internal static UserModel MapToModel(this User user)
    {
        return new UserModel(
            user.Id,
            user.Name,
            user.Email,
            user.PasswordHash,
            user.PasswordSalt,
            user.BrokerageRate,
            user.Profile,
            user.CreatedAt,
            user.UpdatedAt
        );
    }

    internal static AssetModel MapToModel(this Asset asset)
    {
        return new AssetModel(
            asset.Id,
            asset.TickerSymbol,
            asset.Name,
            asset.Risk,
            asset.CreatedAt,
            asset.UpdatedAt
        );
    }

    internal static OperationModel MapToModel(this Operation operation)
    {
        return new OperationModel(
            operation.Id,
            operation.UserId,
            operation.AssetId,
            operation.Quantity,
            operation.UnitPrice,
            operation.Type,
            operation.BrokerageFee,
            operation.DateTime,
            operation.CreatedAt,
            operation.UpdatedAt
        );
    }

    internal static QuoteModel MapToModel(this Quote quote)
    {
        return new QuoteModel(
            quote.Id,
            quote.AssetId,
            quote.UnitPrice,
            quote.DateTime,
            quote.CreatedAt,
            quote.UpdatedAt
        );
    }

    internal static PositionModel MapToModel(this Position position)
    {
        return new PositionModel(
            position.Id,
            position.UserId,
            position.AssetId,
            position.Quantity,
            position.AveragePrice,
            position.ProfitAndLoss,
            position.CreatedAt,
            position.UpdatedAt
        );
    }
}
