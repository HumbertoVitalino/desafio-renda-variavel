using Domain.Enums;

namespace Infra.Repositories.Models;

public sealed class UserModel : Model
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public byte[] PasswordHash { get; private set; } = default!;
    public byte[] PasswordSalt { get; private set; } = default!;
    public decimal BrokerageRate { get; private set; }
    public InvestorProfile Profile { get; private set; }

    public ICollection<OperationModel> Operations { get; private set; } = [];
    public ICollection<PositionModel> Positions { get; private set; } = [];

    private UserModel() { }

    public UserModel(
        int id,
        string name,
        string email,
        byte[] passwordHash,
        byte[] passwordSalt,
        decimal brokerageRate,
        InvestorProfile profile,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id, createdAt, updatedAt)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        BrokerageRate = brokerageRate;
        Profile = profile;
    }
}
