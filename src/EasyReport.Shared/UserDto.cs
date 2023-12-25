using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

[AutoMap(typeof(User), ReverseMap = true)]
public class UserDto : DtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarId { get; set; }
    public Avatar? Avatar { get; set; }

    public Guid UserAuthorizationId { get; set; }
}