using EasyReport.Infrastructure.Domain;

namespace EasyReport.WebApi.Services;

public interface IUnitOfWork
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity;
    Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
    Task<bool> CommitAsync();
}