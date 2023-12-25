namespace EasyReport.Shared;

public class LoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class AccessToken
{
    public string? Token { get; set; }
    public TimeSpan? ExpiresIn { get; set; }
}