using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

public class UserQueryParameter : IQueryParameter, IPagedQueryParameter, IOrderQueryParameter
{
    public string? KeyWords { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}