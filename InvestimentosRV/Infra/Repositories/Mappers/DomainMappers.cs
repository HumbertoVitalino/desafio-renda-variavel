using Domain.Entities;
using Infra.Repositories.Models;

namespace Infra.Repositories.Mappers;

internal static class DomainMappers
{
    internal static User MapToDomain(this UserModel model)
    {
        return User.Reconstitute(
            model.Id,
            model.Name,
            model.Email,
            model.PasswordHash,
            model.PasswordSalt,
            model.BrokerageRate,
            model.Profile,
            model.CreatedAt,
            model.UpdatedAt
        );
    }

    internal static IEnumerable<User> MapToDomain(this IEnumerable<UserModel> models) => models.Select(MapToDomain);

    internal static Asset MapToDomain(this AssetModel model)
    {
        return Asset.Reconstitute(
            model.Id,
            model.TickerSymbol,
            model.Name,
            model.Risk,
            model.CreatedAt,
            model.UpdatedAt
        );
    }

    internal static IEnumerable<Asset> MapToDomain(this IEnumerable<AssetModel> models) => models.Select(MapToDomain);

    internal static Operation MapToDomain(this OperationModel model)
    {
        var operation = Operation.Reconstitute(
            model.Id,
            model.UserId,
            model.AssetId,
            model.Quantity,
            model.UnitPrice,
            model.Type,
            model.BrokerageFee,
            model.DateTime,
            model.CreatedAt,
            model.UpdatedAt
        );

        if (model.Asset is not null)
            operation.Asset = model.Asset.MapToDomain();

        if (model.User is not null)
            operation.User = model.User.MapToDomain();

        return operation;
    }

    internal static Quote MapToDomain(this QuoteModel model)
    {
        var quote = Quote.Reconstitute(
            model.Id,
            model.AssetId,
            model.UnitPrice,
            model.DateTime,
            model.CreatedAt,
            model.UpdatedAt
        );

        if (model.Asset is not null)
            quote.Asset = model.Asset.MapToDomain();

        return quote;
    }

    internal static IEnumerable<Quote> MapToDomain(this IEnumerable<QuoteModel> models) => models.Select(MapToDomain);

    internal static Position MapToDomain(this PositionModel model)
    {
        var position = Position.Reconstitute(
            model.Id,
            model.UserId,
            model.AssetId,
            model.Quantity,
            model.AveragePrice,
            model.ProfitAndLoss,
            model.CreatedAt,
            model.UpdatedAt
        );

        if (model.Asset is not null)
            position.Asset = model.Asset.MapToDomain();

        if (model.User is not null)
            position.User = model.User.MapToDomain();

        return position;
    }

    internal static IEnumerable<Position> MapToDomain(this IEnumerable<PositionModel> models) => models.Select(MapToDomain);
}
