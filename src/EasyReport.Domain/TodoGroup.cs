using EasyReport.Infrastructure.Domain;

namespace EasyReport.Domain;

public class TodoGroup : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; } = 0;
    // public ICollection<Todo> Todos { get; set; } = new List<Todo>();
}