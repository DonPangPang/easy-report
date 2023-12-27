using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyReport.PwaClient.Service;

public class EventService
{
    public event Func<Task>? OnGroupsChangeAsync;

    public async Task NotifyGroupsChangeAsync()
    {
        if (OnGroupsChangeAsync != null)
        {
            foreach (var delegateFunc in OnGroupsChangeAsync.GetInvocationList())
            {
                var func = (Func<Task>)delegateFunc;
                await func.Invoke();
            }
        }
    }
}
