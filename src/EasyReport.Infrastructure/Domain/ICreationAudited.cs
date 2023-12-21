namespace EasyReport.Infrastructure.Domain;

public interface ICreationAudited: ICreationTime
{
    public Guid CreatorId { get; set; }
}