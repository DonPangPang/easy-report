using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EasyReport.PwaClient.Common;
using Microsoft.AspNetCore.Components.Authorization;

namespace EasyReport.PwaClient.Service;

public interface IJwtAuthProvider
{
    Task Login(string token);
    Task Logout();
}

public class JwtAuthProvider : AuthenticationStateProvider, IJwtAuthProvider
{
    private readonly ILocalStorageService _localStorage;

    private AuthenticationState anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public JwtAuthProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>(Consts.TokenKey);

        if (string.IsNullOrEmpty(savedToken))
        {
            return anonimo!;
        }

        return BuildAuthenticationState(savedToken);
    }

    private AuthenticationState BuildAuthenticationState(string token)
    {
        //_httpClientFactory.CreateClient(ApiVars.ApiBase).DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
    }

    public async Task Login(string token)
    {
        await _localStorage.SetItemAsync(Consts.TokenKey, token);

        var authState = BuildAuthenticationState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(Consts.TokenKey);

        NotifyAuthenticationStateChanged(Task.FromResult(anonimo!));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);

        if (roles != null)
        {
            if (roles.ToString()!.Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                foreach (var parsedRole in parsedRoles!)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}