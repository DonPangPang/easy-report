using EasyReport.Domain;
using EasyReport.Shared;
using EasyReport.WebApi.Cache;
using EasyReport.WebApi.Configurations;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Soda.AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyReport.WebApi.Controllers;

public class AccountController(
    IUnitOfWork unitOfWork
    , ICacheService cacheService
    , ICurrentUserSession currentUser) : ApiControllerBase
{

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username)) throw new ArgumentNullException(nameof(dto.Username));
        if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentNullException(nameof(dto.Password));

        var user = await unitOfWork.Query<UserAuthorization>()
            .FirstOrDefaultAsync(x => x.Account == dto.Username && x.Password == dto.Password);
        if (user == null)
        {
            return NotFound();
        }

        var token = GenerateToken(user);
        await cacheService.SetAsync(user.Id.ToString(), token, TimeSpan.FromSeconds(7200));
        return Ok(new AccessToken()
        {
            Token = token,
            ExpiresIn = Consts.Jwt.ExpiresIn,
        });
    }

    private string GenerateToken(UserAuthorization user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Account),
            new Claim(ClaimTypes.Role, user.IsSuper ? "Super" : "Normal"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Consts.Jwt.SecurityKey));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(Consts.Jwt.Issuer, Consts.Jwt.Audience,
            claims, DateTime.Now, DateTime.Now.AddSeconds(7200), signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }


    [HttpGet("logout")]
    public async Task<IActionResult> LogOut()
    {
        var authId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrWhiteSpace(authId))
        {
            await cacheService.RemoveAsync(authId);
        }

        return Ok();
    }

    [HttpGet("current")]
    public IActionResult GetCurrentUser()
    {
        var user = currentUser.GetCurrentUser();
        if (user is null) return NotFound();

        return Ok(user);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserAuthorizationDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Account)) throw new ArgumentNullException(nameof(dto.Account));
        if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentNullException(nameof(dto.Password));

        var user = await unitOfWork.Query<UserAuthorization>()
            .FirstOrDefaultAsync(x => x.Account == dto.Account);
        if (user != null)
        {
            return BadRequest("账号已存在");
        }

        var entity = dto.MapTo<UserAuthorization>();
        await unitOfWork.AddAsync(entity);
        if (await unitOfWork.CommitAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }
}