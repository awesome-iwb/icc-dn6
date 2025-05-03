using System.IO;
using Microsoft.Extensions.Hosting;
using System.Windows;
using DubiousDubiUniverse.InkCanvasForClass.Services;
using DubiousDubiUniverse.InkCanvasForClass.Windows;
using Microsoft.Extensions.DependencyInjection;
using DubiousDubiUniverse.InkCanvasForClass.Controls.Toolbar;

namespace DubiousDubiUniverse.InkCanvasForClass;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public static IHost AppHost;

    public void Application_Startup(object sender, StartupEventArgs e) {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) => {
                services.AddSingleton<SettingsService>(sp =>
                    ActivatorUtilities.CreateInstance<SettingsService>(
                        sp, Path.Combine(hostContext.HostingEnvironment.ContentRootPath, "icc.config.json")
                    )
                );
                services.AddSingleton<MainToolbarWindow>();
            })
            .Build();
        AppHost.Start();

        AppHost.Services.GetRequiredService<SettingsService>().LoadAsync().Wait();
        AppHost.Services.GetRequiredService<SettingsService>().SaveAsync();
        AppHost.Services.GetRequiredService<MainToolbarWindow>().Show();
    }
}