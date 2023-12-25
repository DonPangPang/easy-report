namespace EasyReport.WebApi.Configurations;

public class Consts
{
    public const string Title = "EasyReport";
    public const string DefaultCorsPolicyName = "EasyReport";
    public const string DefaultAuthenticationScheme = "EasyReport";
    public const string DefaultPermission = "EasyReport:Permission";

    public class Jwt
    {
        public const string Issuer = "EasyReport";
        public const string Audience = "EasyReport";
        public const string SecurityKey = "EasyReport:WebApi:SecurityKey:HmacSha256Signature";
        public static TimeSpan ExpiresIn = TimeSpan.FromSeconds(7200);
    }
}