namespace EasyReport.Infrastructure.Dto;

public interface IOrderQueryParameter : IQueryParameter
{
    string? OrderBy { get; set; }
}