using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class UserAuthorization : EntityBase, IEnabled, ISafeDeleted
{
    public required string Account { get; set; }
    public required string Password { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public bool IsSuper { get; set; } = false;

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

}