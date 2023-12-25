using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;
using EasyReport.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyReport.Shared;

[AutoMap(typeof(Todo), ReverseMap = true)]
public class TodoDto : DtoBase
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public Guid? GroupId { get; set; }

    public ICollection<TodoTag> Tags { get; set; } = new List<TodoTag>();

    [JsonIgnore]
    public string? Links { get; set; }

    [NotMapped]
    public List<string> LinksObject
    {
        get => (string.IsNullOrWhiteSpace(Links) ? null : Links.ToObject<List<string>>()) ?? [];
        set => Links = value.ToJson();
    }

    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
}