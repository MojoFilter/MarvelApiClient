using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MarvelApiClientSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();

        var services = new ServiceCollection()
            .AddMarvelApi(configuration.GetSection("MarvelApi"))
            .AddTransient<MainViewModel>()
            .AddTransient<MainWindow>()
            .BuildServiceProvider();

        this.MainWindow = services.GetRequiredService<MainWindow>();
        this.MainWindow.Show();
    }
}
