using EasyReport.Infrastructure.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyReport.Domain;

public class User : EntityBase, ISafeDeleted
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    [ForeignKey(nameof(Avatar))]
    public Guid? AvatarId { get; set; }
    public virtual Avatar? Avatar { get; set; }
    public bool IsDeleted { get; set; } = false;
}