using EasyReport.Domain;
using EasyReport.Shared;
using EasyReport.WebApi.Extensions;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyReport.WebApi.Controllers;

public class UserController(IUnitOfWork unitOfWork)
    : ApiControllerBase<UserQueryParameter, User, User, UserDto, UserDto>(unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<IActionResult> GetAsync([FromQuery] UserQueryParameter parameter)
    {
        var result = await _unitOfWork.Query<User>()
            .WhereIf(!string.IsNullOrWhiteSpace(parameter.KeyWords), x => x.Name.Contains(parameter.KeyWords!))
            .ToListAsync(parameter);

        return Ok(result);
    }
}