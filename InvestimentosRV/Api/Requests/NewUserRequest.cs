using Core.Domain.Enums;

namespace Api.Requests;

public sealed record NewUserRequest(
    string Name,
    string Email,
    string Password,
    string Confirmation,
    InvestorProfile Profile
);
