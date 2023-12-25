using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class Resource : EntityBase, ICreationAudited
{
    public string? Name { get; set; }
    public long Size { get; set; }
    public string? Extension { get; set; }
    public string? Path { get; set; }
    public string? Uri { get; set; }
    public string? MimeType { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }
    public virtual User? Creator { get; set; }
}