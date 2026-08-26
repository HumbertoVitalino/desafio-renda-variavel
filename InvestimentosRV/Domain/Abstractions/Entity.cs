namespace Domain.Abstractions;

public abstract class Entity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
