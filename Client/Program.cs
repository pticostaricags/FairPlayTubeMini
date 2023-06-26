using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Client.CustomProviders;
using System.ComponentModel.DataAnnotations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore()
    .AddScoped<AuthenticationStateProvider, AzureSWAAuthenticationStateProvider>(sp => 
    {
        var env = sp.GetService<IWebAssemblyHostEnvironment>();
        return new AzureSWAAuthenticationStateProvider(env!.BaseAddress);
    });
await builder.Build().RunAsync();
