using Api.Requests;
using Core.UseCase.LoginUserUseCase.Boundaries;
using Core.UseCase.NewUserUseCase.Boundaries;

namespace Api.Mappers;

public static class UserMapper
{
    public static NewUserInput MapToInput(this NewUserRequest request)
    {
        return new NewUserInput(
            request.Name,
            request.Email,
            request.Password,
            request.Confirmation,
            request.Profile
        );
    }

    public static LoginUserInput MapToInput(this LoginUserRequest request)
    {
        return new LoginUserInput(
            request.Email,
            request.Password
        );
    }
}
