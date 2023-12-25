using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

public class TodoTagQueryParameter : IOrderQueryParameter
{
    public string? OrderBy { get; set; }
    public string? KeyWords { get; set; }
}