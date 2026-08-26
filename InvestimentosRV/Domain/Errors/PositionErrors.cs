namespace Domain.Errors;

public static class PositionErrors
{
    public const string InsufficientQuantity = "Insufficient assets to perform the sale.";
    public const string NegativeQuantity = "The position quantity cannot be negative.";
}
