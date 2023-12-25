using EasyReport.Domain;
using EasyReport.Shared;
using EasyReport.WebApi.Extensions;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyReport.WebApi.Controllers;

public class TodoGroupController(IUnitOfWork unitOfWork)
    : ApiControllerBase<TodoGroupQueryParameter, TodoGroup, TodoGroupDto, TodoGroupDto, TodoGroupDto>(unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<IActionResult> GetAsync([FromQuery] TodoGroupQueryParameter parameter)
    {
        var result = await _unitOfWork.Query<TodoGroup>()
            .WhereIf(!string.IsNullOrWhiteSpace(parameter.KeyWords), x => x.Name.Contains(parameter.KeyWords!))
            .ToListAsync(parameter);

        return Ok(result);
    }
}