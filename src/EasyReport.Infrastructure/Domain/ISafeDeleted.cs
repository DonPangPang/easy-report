namespace EasyReport.Infrastructure.Domain;

public interface ISafeDeleted
{
    public bool IsDeleted { get; set; }
}