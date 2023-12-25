using EasyReport.Domain;
using EasyReport.WebApi.Cache;
using EasyReport.WebApi.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EasyReport.WebApi.Services;

public class PermissionHandler(IUnitOfWork unitOfWork
    , ICurrentUserSession currentUserSession
    , ICacheService cacheService) : AuthorizationHandler<PermissionRequirement>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return;
        }

        var authId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(authId))
        {
            context.Fail();
            return;
        }
        var token = await cacheService.GetAsync<string>(authId);
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Fail();
            return;
        }
        await cacheService.SetAsync(authId, token, Consts.Jwt.ExpiresIn);
        var authIdGuid = Guid.Parse(authId);

        var user = await unitOfWork.Query<UserAuthorization>().FirstOrDefaultAsync(x => x.Id == authIdGuid);
        currentUserSession.SetCurrentUser(user?.User);
        context.Succeed(requirement);
    }
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;

    public int AccessExpiration { get; set; }
}