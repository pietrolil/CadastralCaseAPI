namespace CadastralCase.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    
    protected void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}
