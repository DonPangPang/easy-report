using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class TodoTag : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    // public ICollection<Todo> Todos { get; set; } = new List<Todo>();
}