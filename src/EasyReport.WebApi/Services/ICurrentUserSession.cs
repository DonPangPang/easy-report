using EasyReport.Domain;

namespace EasyReport.WebApi.Services;

public interface ICurrentUserSession
{
    User? GetCurrentUser();

    void SetCurrentUser(User? user);
}