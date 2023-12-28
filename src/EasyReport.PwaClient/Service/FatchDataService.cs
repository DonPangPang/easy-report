using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyReport.Domain;
using EasyReport.Infrastructure.GeneralModel;
using EasyReport.Shared;

namespace EasyReport.PwaClient.Service;

public class FatchDataService
{
    private readonly Request _request;
    public FatchDataService(Request request)
    {
        _request = request;
    }

    public async Task<List<TodoGroupDto>> FatchGroupsDataAsync(TodoGroupQueryParameter? parameter = null)
    {
        return await _request.GetAsync<List<TodoGroupDto>>("api/TodoGroup", parameter ?? new TodoGroupQueryParameter()) ?? new List<TodoGroupDto>();
    }

    public async Task<List<TodoTagDto>> FatchTagsDataAsync(TodoTagQueryParameter? parameter = null)
    {
        return await _request.GetAsync<List<TodoTagDto>>("api/TodoTag", parameter ?? new TodoTagQueryParameter()) ?? new List<TodoTagDto>();
    }
}
