using EasyReport.WebApi.Data;

namespace EasyReport.WebApi.Extensions;

public static class EfCoreExtensions
{
    public static void InitDatabase(this IApplicationBuilder app, bool refresh = false)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<EasyReportDbContext>();
            if (refresh)
            {
                context.Database.EnsureDeleted();
            }

            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            OutputHelper.Write(ex.Message);
        }
    }
}

public static class OutputHelper
{
    public static void Write(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}