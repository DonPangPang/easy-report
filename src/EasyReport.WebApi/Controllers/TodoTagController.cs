using EasyReport.Domain;
using EasyReport.Shared;
using EasyReport.WebApi.Extensions;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyReport.WebApi.Controllers;

public class TodoTagController(IUnitOfWork unitOfWork)
    : ApiControllerBase<TodoTagQueryParameter, TodoTag, TodoTagDto, TodoTagDto, TodoTagDto>(unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<IActionResult> GetAsync([FromQuery] TodoTagQueryParameter parameter)
    {
        var result = await _unitOfWork.Query<TodoTag>()
            .WhereIf(!string.IsNullOrWhiteSpace(parameter.KeyWords), x => x.Name.Contains(parameter.KeyWords!))
            .ToListAsync(parameter);

        return Ok(result);
    }
}