using AutoMapper;
using EasyReport.Domain;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.Shared;

[AutoMap(typeof(UserAuthorization), ReverseMap = true)]
public class UserAuthorizationDto : IDto
{
    public required string Account { get; set; }
    public required string Password { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}