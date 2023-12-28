using Blazored.LocalStorage;
using EasyReport.PwaClient;
using EasyReport.PwaClient.Service;
using Masa.Blazor;
using Masa.Blazor.Presets;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#if DEBUG
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5203") });
#else
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
#endif

builder.Services.AddScoped<Request>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<JwtAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthProvider>(
    provider => provider.GetRequiredService<JwtAuthProvider>()
);
builder.Services.AddScoped<IJwtAuthProvider, JwtAuthProvider>(provider => provider.GetRequiredService<JwtAuthProvider>());

builder.Services.AddSingleton<EventService>();
builder.Services.AddScoped<FatchDataService>();

builder.Services.AddMasaBlazor(options =>
{
    options.Defaults = new Dictionary<string, IDictionary<string, object?>?>()
    {
        {
            PopupComponents.SNACKBAR, new Dictionary<string, object?>()
            {
                { nameof(PEnqueuedSnackbars.Closeable), false },
                { nameof(PEnqueuedSnackbars.Position), SnackPosition.TopRight },
                { nameof(PEnqueuedSnackbars.Timeout), 2000 },
                { nameof(PEnqueuedSnackbars.MaxCount), 3 },
            }
        }
    };
});


await builder.Build().RunAsync();
