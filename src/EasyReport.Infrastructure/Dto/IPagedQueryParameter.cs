namespace EasyReport.Infrastructure.Dto;

public interface IPagedQueryParameter : IQueryParameter
{
    int PageIndex { get; set; }
    int PageSize { get; set; }
}