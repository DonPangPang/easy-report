using Blazored.LocalStorage;
using EasyReport.Infrastructure.Dto;
using EasyReport.Infrastructure.Extensions;
using EasyReport.PwaClient.Common;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace EasyReport.PwaClient.Service;

public class Request(HttpClient httpClient, NavigationManager navigationManager, IToastService toastService, ISyncLocalStorageService localStorage, IJwtAuthProvider jwtAuthProvider)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly NavigationManager _navigationManager = navigationManager;

    private readonly IToastService _toastService = toastService;
    private readonly ISyncLocalStorageService _localStorge = localStorage;
    private readonly IJwtAuthProvider _jwtAuthProvider = jwtAuthProvider;

    private HttpClient GetHttpClient()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorge.GetItem<string>(Consts.TokenKey) ?? string.Empty);
        return _httpClient;
    }

    public async Task<T?> GetAsync<T>([Url] string uri)
    {
        var response = await GetHttpClient().GetAsync(uri);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> GetAsync<T>([Url] string uri, IQueryParameter @params)
    {
        var response = await GetHttpClient().GetAsync(QueryHelpers.AddQueryString(uri, @params));

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> PostAsync<T>([Url] string uri, object? value)
    {
        var response = await GetHttpClient().PostAsJsonAsync(uri, value);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> PostAsync<T>([Url] string uri, object? value, IQueryParameter @params)
    {
        var response = await GetHttpClient().PostAsJsonAsync(QueryHelpers.AddQueryString(uri, @params), value);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> PutAsync<T>([Url] string uri, object? value)
    {
        var response = await GetHttpClient().PutAsJsonAsync(uri, value);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> PutAsync<T>([Url] string uri, object? value, IQueryParameter @params)
    {
        var response = await GetHttpClient().PutAsJsonAsync(QueryHelpers.AddQueryString(uri, @params), value);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> DeleteAsync<T>([Url] string uri)
    {
        var response = await GetHttpClient().DeleteAsync(uri);

        return await HandleResponseMessageAsync<T>(response);
    }

    public async Task<T?> DeleteAsync<T>([Url] string uri, IQueryParameter @params)
    {
        var response = await GetHttpClient().DeleteAsync(QueryHelpers.AddQueryString(uri, @params));

        return await HandleResponseMessageAsync<T>(response);
    }

    private async Task<T?> HandleResponseMessageAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.RequestMessage?.Method != HttpMethod.Get)
            {
                await _toastService.SuccessAsync("操作成功");
            }
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _toastService.WarningAsync("认证失效");
            _navigationManager.NavigateTo("/login");
            await _jwtAuthProvider.Logout();
            return default;
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            await _toastService.WarningAsync("没有权限");
            _navigationManager.NavigateTo("/403");
            return default;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            await _toastService.WarningAsync("资源不存在");
            _navigationManager.NavigateTo("/404");
            return default;
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            ResponseMessage? content = null;

            try
            {
                content = await response.Content.ReadFromJsonAsync<ResponseMessage>();
            }
            catch (Exception) { }
            await _toastService.ErrorAsync(content?.Message ?? "服务器错误");
            return default;
        }

        try
        {
            var content = await response.Content.ReadFromJsonAsync<T>();
            return content;
        }
        catch
        {
            return default;
        }
    }

    class ResponseMessage
    {
        public string? Message { get; set; }
    }
}
