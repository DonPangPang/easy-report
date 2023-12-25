using Blazored.LocalStorage;
using EasyReport.PwaClient;
using EasyReport.PwaClient.Service;
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

builder.Services.AddMasaBlazor();

await builder.Build().RunAsync();
