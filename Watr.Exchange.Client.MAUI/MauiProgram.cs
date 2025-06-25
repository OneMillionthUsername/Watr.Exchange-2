using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using ReactiveUI;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Reflection;
using Watr.Exchange.Security;
using Watr.Exchange.Security.Core;
using Watr.Exchange.ViewModels;

namespace Watr.Exchange.Client.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var a = Assembly.GetExecutingAssembly();
            using (var stream = a.GetManifestResourceStream("Watr.Exchange.Client.MAUI.appsettings.json"))
            {
                builder.Configuration.AddJsonStream(stream ?? throw new InvalidDataException()); // This line now works with the added namespace
            }
            //builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            
            builder.Services.AddSingleton(provider =>
            {
                var client = PublicClientApplicationBuilder.Create(builder.Configuration["AzureAD:ClientId"])
                            .WithB2CAuthority(builder.Configuration["AzureAD:Authority"])
#if WINDOWS
                            .WithRedirectUri(builder.Configuration["AzureAD:RedirectURI"]) // needed only for the system browser
#elif IOS
                .WithRedirectUri(builder.Configuration["AzureAD:iOSRedirectURI"])
                .WithIosKeychainSecurityGroup(builder.Configuration["AzureAD:iOSKeyChainGroup"])
#elif ANDROID
                .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                .WithRedirectUri(builder.Configuration["AzureAD:AndroidRedirectURI"])
#elif MACCATALYST
                .WithRedirectUri(builder.Configuration["AzureAD:iOSRedirectURI"])
#endif
                            .Build();
#if WINDOWS || MACCATALYST
                string fileName = Path.Join(FileSystem.CacheDirectory, "msal.token.cache2");
                client.UserTokenCache.SetBeforeAccessAsync(async args =>
                {
                    if (!(await FileSystem.Current.AppPackageFileExistsAsync(fileName)))
                        return;
                    byte[] fileBytes;
                    using (var stream = new FileStream(fileName, FileMode.Open))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                    args.TokenCache.DeserializeMsalV3(fileBytes);
                });
                client.UserTokenCache.SetAfterAccessAsync(async args =>
                {
                    if (args.HasStateChanged)
                    {
                        var data = args.TokenCache.SerializeMsalV3();
                        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            await fs.WriteAsync(data, 0, data.Length);
                        }
                    }
                });
#endif
                return client;
            });
            
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddScoped<ISecurityProvider, SecurityProvider>();
            builder.Services.AddSingleton<IViewForResolver, ViewForResolver>();
            builder.Services.AddSingleton<IScreen, AppHostViewModel>();
            builder.Services.AddScoped<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            var app = builder.Build();
            
            return app;
        }
    }
}
