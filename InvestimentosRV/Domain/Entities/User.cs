using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public byte[] PasswordHash { get; private set; } = [];
    public byte[] PasswordSalt { get; private set; } = [];
    public decimal BrokerageRate { get; private set; }
    public InvestorProfile Profile { get; private set; }
    public ICollection<Operation> Operations { get; private set; } = [];
    public ICollection<Position> Positions { get; private set; } = [];

    protected User() { }

    public User(
        string name,
        string email,
        byte[] passwordHash,
        byte[] passwordSalt,
        decimal brokerageRate,
        InvestorProfile profile
    )
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        BrokerageRate = brokerageRate;
        Profile = profile;
    }

    public static User Reconstitute(
        int id,
        string name,
        string email,
        byte[] passwordHash,
        byte[] passwordSalt,
        decimal brokerageRate,
        InvestorProfile profile,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        var user = new User(name, email, passwordHash, passwordSalt, brokerageRate, profile);
        user.Id = id;
        user.CreatedAt = createdAt;
        user.UpdatedAt = updatedAt;

        return user;
    }

    public bool IsSuitableFor(Asset asset)
    {
        if (Profile == InvestorProfile.Conservative && asset.Risk > AssetRisk.Low) return false;
        if (Profile == InvestorProfile.Moderate && asset.Risk > AssetRisk.Medium) return false;
        return true;
    }
}
