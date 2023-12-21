namespace EasyReport.Infrastructure.Domain;

public interface IModificationAudited: IModification
{
    public Guid? LastModifierId { get; set; }
}