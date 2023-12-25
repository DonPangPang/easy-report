using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

public class UserAuthorizationQueryParameter : IQueryParameter, IPagedQueryParameter, IOrderQueryParameter
{
    public string? KewWords { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}