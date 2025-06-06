namespace Core.Interfaces;

public interface IPasswordService
{
    (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password);
    bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
}
