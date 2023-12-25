using EasyReport.Infrastructure.Domain;
using EasyReport.WebApi.Data;

namespace EasyReport.WebApi.Services;

public class UnitOfWork(EasyReportDbContext dbContext) : IUnitOfWork
{
    private readonly EasyReportDbContext _dbContext = dbContext;

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity => _dbContext.Set<TEntity>();

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        _dbContext.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }
}