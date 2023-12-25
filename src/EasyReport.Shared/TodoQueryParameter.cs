using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

public class TodoQueryParameter : IQueryParameter, IPagedQueryParameter, IOrderQueryParameter
{
    public string? Title { get; set; }
    public Guid? GroupId { get; set; }
    public bool? IsCompleted { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public string? KeyWords { get; set; }


    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}