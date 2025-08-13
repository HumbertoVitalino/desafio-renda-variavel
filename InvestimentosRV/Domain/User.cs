using Domain.Enums;

namespace Domain;

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
}
