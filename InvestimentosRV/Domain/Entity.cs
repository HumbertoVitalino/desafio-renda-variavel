namespace Domain;

public abstract class Entity
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
