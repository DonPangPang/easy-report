using EasyReport.Infrastructure.Domain;
using EasyReport.Infrastructure.Dto;
using EasyReport.WebApi.Configurations;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soda.AutoMapper;
using System.Linq.Dynamic.Core;

namespace EasyReport.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Consts.DefaultPermission)]
public class ApiControllerBase : ControllerBase
{

}


public abstract class ApiControllerBase<TQueryParameter, TEntity, TViewDto, TAddDto, TUpdateDto>(
    IUnitOfWork unitOfWork) : ApiControllerBase
    where TQueryParameter : IQueryParameter
    where TEntity : EntityBase
    where TViewDto : class
    where TAddDto : IDto
    where TUpdateDto : DtoBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public abstract Task<IActionResult> GetAsync([FromQuery] TQueryParameter parameter);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var result = await _unitOfWork.Query<TEntity>()
            .Where(x => x.Id == id)
            .Map<TEntity, TViewDto>()
            .FirstOrDefaultAsync();
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] TAddDto dto)
    {
        var entity = dto.MapTo<TEntity>();
        await _unitOfWork.AddAsync(entity);
        if (await _unitOfWork.CommitAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] TUpdateDto dto)
    {
        var entity = await _unitOfWork.Query<TEntity>().FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (entity == null)
        {
            return NotFound();
        }

        entity.Map(dto);
        await _unitOfWork.UpdateAsync(entity);
        if (await _unitOfWork.CommitAsync())
        {
            return NoContent();
        }

        return BadRequest();
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] IEnumerable<Guid> ids)
    {
        await _unitOfWork.Query<TEntity>()
            .Where(x => ids.Contains(x.Id))
            .ExecuteDeleteAsync();

        return Ok();
    }
}