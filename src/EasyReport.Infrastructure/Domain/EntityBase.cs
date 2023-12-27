namespace EasyReport.Infrastructure.Domain;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; }

    public Guid CreateId()
    {
        Id = Guid.NewGuid();
        return Id;
    }
}