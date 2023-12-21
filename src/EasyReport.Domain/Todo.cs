﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EasyReport.Infrastructure.Domain;
using EasyReport.Infrastructure.Extensions;

namespace EasyReport.Domain;

public class Todo : EntityBase, ICreationAudited, IModificationAudited, IDeleted
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public bool IsCompleted { get; set; } = false;
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public User? LastModifier { get; set; }

    public bool IsDeleted { get; set; } = false;

    public Guid? GroupId { get; set; }
    public TodoGroup? Group { get; set; }

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