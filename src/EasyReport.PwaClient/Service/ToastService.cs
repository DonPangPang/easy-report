using BlazorComponent;
using Masa.Blazor;

namespace EasyReport.PwaClient.Service;

public class ToastService : IToastService
{
    private readonly IPopupService _popupService;

    public ToastService(IPopupService popupService)
    {
        _popupService = popupService;
    }
    public async Task SuccessAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Success);
    }

    public async Task ErrorAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Error);
    }

    public async Task WarningAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Warning);
    }

    public async Task InfoAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Info);
    }

    public async Task DefaultAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.None);
    }
}