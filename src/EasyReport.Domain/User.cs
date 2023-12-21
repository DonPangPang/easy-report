using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class User : EntityBase, IDeleted
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarId { get; set; }
    public Avatar? Avatar { get; set; }
    public bool IsDeleted { get; set; } = false;

    public Guid UserAuthorizationId { get; set; }
    public UserAuthorization? UserAuthorization { get; set; }
}