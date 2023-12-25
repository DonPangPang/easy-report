namespace EasyReport.PwaClient.Service;

public interface IToastService
{
    Task SuccessAsync(string message);
    Task ErrorAsync(string message);
    Task WarningAsync(string message);
    Task InfoAsync(string message);
    Task DefaultAsync(string message);
}
