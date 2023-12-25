using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

[AutoMap(typeof(TodoGroup), ReverseMap = true)]
public class TodoGroupDto : DtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}