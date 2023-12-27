using Blazored.LocalStorage;
using EasyReport.Infrastructure.Dto;
using EasyReport.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace EasyReport.PwaClient.Service;

public class Request(HttpClient httpClient, NavigationManager navigationManager, IToastService toastService, ISyncLocalStorageService localStorage)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly NavigationManager _navigationManager = navigationManager;

    private readonly IToastService _toastService = toastService;
    private readonly ISyncLocalStorageService _localStorge = localStorage;

    private HttpClient GetHttpClient()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorge.GetItem<string>("token") ?? string.Empty);
        return _httpClient;
    }

    public async Task<T?> GetAsync<T>([Url] string uri)
    {
        var response = await GetHttpClient().GetAsync(uri);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> GetAsync<T>([Url] string uri, IQueryParameter @params)
    {
        var response = await GetHttpClient().GetAsync(QueryHelpers.AddQueryString(uri, @params));

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> PostAsync<T>([Url] string uri, object? value)
    {
        var response = await GetHttpClient().PostAsJsonAsync(uri, value);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> PostAsync<T>([Url] string uri, object? value, IQueryParameter @params)
    {
        var response = await GetHttpClient().PostAsJsonAsync(QueryHelpers.AddQueryString(uri, @params), value);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> PutAsync<T>([Url] string uri, object? value)
    {
        var response = await GetHttpClient().PutAsJsonAsync(uri, value);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> PutAsync<T>([Url] string uri, object? value, IQueryParameter @params)
    {
        var response = await GetHttpClient().PutAsJsonAsync(QueryHelpers.AddQueryString(uri, @params), value);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> DeleteAsync<T>([Url] string uri)
    {
        var response = await GetHttpClient().DeleteAsync(uri);

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    public async Task<T?> DeleteAsync<T>([Url] string uri, IQueryParameter @params)
    {
        var response = await GetHttpClient().DeleteAsync(QueryHelpers.AddQueryString(uri, @params));

        await HandleResponseMessageAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return json.ToObject<T>();
    }

    private async Task HandleResponseMessageAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            await _toastService.SuccessAsync("操作成功");
            return;
        }

        var content = await response.Content.ReadFromJsonAsync<ResponseMessage>();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _toastService.WarningAsync(content?.Message ?? "认证失效");
            _navigationManager.NavigateTo("/login");
            return;
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            await _toastService.ErrorAsync(content?.Message ?? "服务器错误");
        }

        throw new Exception(response.ReasonPhrase);
    }

    class ResponseMessage
    {
        public string? Message { get; set; }
    }
}
