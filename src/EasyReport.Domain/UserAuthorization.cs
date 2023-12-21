using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class UserAuthorization : EntityBase, IEnabled, IDeleted
{
    public required string Account { get; set; }
    public required string Password { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}