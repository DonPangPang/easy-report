using EasyReport.Domain;
using EasyReport.Shared;
using EasyReport.WebApi.Extensions;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyReport.WebApi.Controllers;

public class TodoController(IUnitOfWork unitOfWork)
    : ApiControllerBase<TodoQueryParameter, Todo, TodoDto, TodoDto, TodoDto>(unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override async Task<IActionResult> GetAsync([FromQuery] TodoQueryParameter parameter)
    {
        var result = await _unitOfWork.Query<Todo>()
            .WhereIf(parameter.GroupId.HasValue, x => x.GroupId == parameter.GroupId)
            .WhereIf(parameter.StartTime.HasValue, x => x.CreationTime.Date > parameter.StartTime!.Value.Date)
            .WhereIf(parameter.EndTime.HasValue, x => x.CreationTime.Date <= parameter.EndTime!.Value.Date)
            .ToListAsync(parameter);

        return Ok(result);
    }

    [HttpGet("completed/{id:guid}")]
    public async Task<IActionResult> Completed(Guid id)
    {
        if (!await _unitOfWork.Query<Todo>().AnyAsync(x => x.Id == id))
        {
            return NotFound();
        }

        await _unitOfWork.Query<Todo>().Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.IsCompleted, true));
        return Ok();
    }

    [HttpGet("uncompleted/{id:guid}")]
    public async Task<IActionResult> UnCompleted(Guid id)
    {
        if (!await _unitOfWork.Query<Todo>().AnyAsync(x => x.Id == id))
        {
            return NotFound();
        }

        await _unitOfWork.Query<Todo>().Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(t => t.IsCompleted, false));
        return Ok();
    }
}