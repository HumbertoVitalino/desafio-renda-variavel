﻿namespace Core.Domain;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
