using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Client.CustomProviders;
using System.ComponentModel.DataAnnotations;
using Blazored.Toast;
using BlazorApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IUserProfileService, AzureSWAUserProfileService>(sp =>
{
    var env = sp.GetRequiredService<IWebAssemblyHostEnvironment>();
    return new AzureSWAUserProfileService(env!.BaseAddress);
});
builder.Services.AddAuthorizationCore()
    .AddScoped<AuthenticationStateProvider, AzureSWAAuthenticationStateProvider>(sp =>
    {
        var userProfileService = sp.GetRequiredService<IUserProfileService>();
        return new AzureSWAAuthenticationStateProvider(userProfileService);
    });

builder.Services.AddBlazoredToast();
builder.Services.AddTransient<INavigationService, NavigationService>();
builder.Services.AddTransient<IDatabaseService, DatabaseRestService>();
await builder.Build().RunAsync();
