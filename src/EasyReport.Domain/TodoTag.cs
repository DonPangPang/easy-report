using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class TodoTag : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public string Color { get; set; } = "#000000";
    public string TextColor { get; set; } = "#ffffff";

    // public ICollection<Todo> Todos { get; set; } = new List<Todo>();
}