namespace EazyScenes.Data.Entities;

public interface IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}
