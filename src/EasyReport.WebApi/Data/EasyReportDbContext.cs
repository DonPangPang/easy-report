using System.Reflection;
using EasyReport.Domain;
using EasyReport.Infrastructure.Domain;
using EasyReport.WebApi.Configurations;
using EasyReport.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EasyReport.WebApi.Data;

public class EasyReportDbContext(DbContextOptions<EasyReportDbContext> options
    , IOptions<AppSettings> appSettings
    , ICurrentUserSession currentUser) : DbContext(options)
{
    private AppSettings AppSettings => appSettings.Value;
    private readonly ICurrentUserSession _currentUser = currentUser;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var db = AppSettings.ConnectionString;

        optionsBuilder.UseSqlite("Data Source=er.db");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseLazyLoadingProxies();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = Assembly.Load("EasyReport.Domain").GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IEntity)) && x is { IsAbstract: false, IsInterface: false })
            .ToArray();

        foreach (var entityType in entityTypes)
        {
            if (modelBuilder.Model.FindEntityType(entityType) != null)
            {
                continue;
            }
            modelBuilder.Model.AddEntityType(entityType);
        }

        modelBuilder.Entity<UserAuthorization>()
            .HasData(new UserAuthorization()
            {
                Id = Guid.Parse("0E8F1716-9C9A-B243-6954-4050F8BFBE98"),
                Account = "admin",
                Password = "123456",
                IsSuper = true,
                UserId = Guid.Parse("0E8F1716-9C9A-B243-6954-4050F8BFBE99"),
            });
        modelBuilder.Entity<User>()
            .HasData(new User()
            {
                Id = Guid.Parse("0E8F1716-9C9A-B243-6954-4050F8BFBE99"),
                Name = "超级管理员",
            });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {

        foreach (var entityEntry in ChangeTracker.Entries<IEntity>())
        {

            if (entityEntry is { Entity: ICreationAudited creation, State: EntityState.Added })
            {
                // TODO: 从登录信息中获取用户信息
                creation.CreatorId = _currentUser.GetCurrentUser()?.Id ?? Guid.Empty;
            }

            if (entityEntry is { Entity: ICreationTime creationTime, State: EntityState.Added })
            {
                creationTime.CreationTime = DateTime.Now;
            }

            if (entityEntry is { Entity: IModificationAudited modification, State: EntityState.Modified })
            {
                // TODO: 从登录信息中获取用户信息
                modification.LastModifierId = _currentUser.GetCurrentUser()?.Id ?? Guid.Empty;
            }

            if (entityEntry is { Entity: IModification modificationTime, State: EntityState.Modified })
            {
                modificationTime.LastModificationTime = DateTime.Now;
            }

            if (entityEntry is { Entity: ISafeDeleted safeDelete, State: EntityState.Deleted })
            {
                safeDelete.IsDeleted = true;
                entityEntry.State = EntityState.Modified;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}