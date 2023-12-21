namespace EasyReport.Infrastructure.Domain;

public interface IModification
{
    public DateTime? LastModificationTime { get; set; }
}