using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

public class TodoGroupQueryParameter : IOrderQueryParameter
{
    public string? OrderBy { get; set; }
    public string? KeyWords { get; set; }
}