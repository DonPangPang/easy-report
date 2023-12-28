using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

[AutoMap(typeof(TodoTag), ReverseMap = true)]
public class TodoTagDto : DtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public string TextColor { get; set; } = "#ffffff";
}