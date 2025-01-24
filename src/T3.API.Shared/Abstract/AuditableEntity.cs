namespace T3.API.Shared.Abstract;

public record AuditableEntity
{
    public virtual DateTime CreatedDt { get; init; }
}