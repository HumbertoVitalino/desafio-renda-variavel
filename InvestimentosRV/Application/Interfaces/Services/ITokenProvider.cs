namespace Application.Interfaces.Services;

public interface ITokenProvider
{
    string GenerateToken(int userId, string email);
}
