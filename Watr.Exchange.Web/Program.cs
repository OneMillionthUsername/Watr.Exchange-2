using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using MudBlazor.Services;
using ReactiveUI;
using Watr.Exchange.Security;
using Watr.Exchange.Security.Core;
using Watr.Exchange.ViewModels;
using Watr.Exchange.Web;
using Watr.Exchange.Web.Layout;
using Watr.Exchange.Web.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    
    options.ProviderOptions.Authentication.Authority = builder.Configuration["AzureAD:Authority"];
    options.ProviderOptions.Authentication.RedirectUri = builder.Configuration["AzureAD:RedirectURI"];
    options.ProviderOptions.Authentication.ClientId = builder.Configuration["AzureAD:ClientId"];
    options.ProviderOptions.Authentication.ValidateAuthority = false;
});
builder.Services.AddSingleton<ISecurityProvider, SecurityProvider>();
builder.Services.AddSingleton(provider =>
{
    
    var client = PublicClientApplicationBuilder.Create(builder.Configuration["AzureAD:ClientId"])
                .WithB2CAuthority(builder.Configuration["AzureAD:Authority"])
                            .WithRedirectUri(builder.Configuration["AzureAD:ClientAppCallBack"]) // needed only for the system browser
                .Build();
    return client;
});
builder.Services.AddSingleton<IScreen, AppHostViewModel>();
builder.Services.AddTransient<MainViewModel>();
builder.Services.AddTransient<AlertView.AlertViewModel>();
builder.Services.AddTransient<MainPage>();
builder.Services.AddTransient<NotFoundViewModel>();
builder.Services.AddTransient<NotFound>();
await builder.Build().RunAsync();
