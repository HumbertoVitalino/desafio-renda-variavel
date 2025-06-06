namespace Core.Domain;

public class User : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public decimal BrokerageRate { get; set; }
    public ICollection<Operation> Operations { get; set; } = [];
    public ICollection<Position> Positions { get; set; } = [];

    protected User() { }

    public User(
        string name,
        string email,
        byte[] passwordHash,
        byte[] passwordSalt,
        decimal brokerageRate
    )
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        BrokerageRate = brokerageRate;
    }
}
