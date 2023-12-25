using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

[AutoMap(typeof(TodoTag), ReverseMap = true)]
public class TodoTagDto : DtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}