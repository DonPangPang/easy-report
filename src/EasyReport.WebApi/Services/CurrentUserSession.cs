using EasyReport.Domain;

namespace EasyReport.WebApi.Services;

public class CurrentUserSession : ICurrentUserSession
{
    private User? _currentUser;

    public User? GetCurrentUser()
    {
        return _currentUser;
    }

    public void SetCurrentUser(User? user)
    {
        _currentUser = user;
    }
}