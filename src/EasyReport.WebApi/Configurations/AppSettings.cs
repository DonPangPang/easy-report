using Microsoft.Extensions.Options;

namespace EasyReport.WebApi.Configurations;

public class AppSettings : IOptions<AppSettings>
{
    public string? ConnectionString { get; set; }
    public string? Redis { get; set; }

    public AppSettings Value => this;
}