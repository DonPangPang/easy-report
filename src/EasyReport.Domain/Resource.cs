using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class Resource : EntityBase, ICreationAudited
{
    public required string Name { get; set; }
    public long Size { get; set; }
    public required string Extension { get; set; }
    public required string Path { get; set; }
    public required string Uri { get; set; }
    public required string MimeType { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
}