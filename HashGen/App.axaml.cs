using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using HashGen.Services;
using HashGen.ViewModels;
using HashGen.Views;

using Microsoft.Extensions.DependencyInjection;

namespace HashGen;

public partial class App : Application
{

    public static IServiceProvider ServiceProvider { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        var locator = new ViewModelLocator();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = locator.MainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>(sp =>
        {
            var mainWindow = (Window)sp.GetRequiredService<IClassicDesktopStyleApplicationLifetime>().MainWindow;
            return new FileService(mainWindow);
        }
        );
        services.AddSingleton<MainViewModel>(sp =>
            new MainViewModel(sp.GetRequiredService<IFileService>()));
        services.AddTransient<MainView>();

        // Для отладки
        var sp = services.BuildServiceProvider();
        var vm = sp.GetService<MainViewModel>();
        System.Diagnostics.Debug.WriteLine($"MainViewModel resolved: {vm != null}");
        System.Diagnostics.Debug.WriteLine($"SaveDataCommand exists: {vm?.SaveDataCommand != null}");
    }

    public new static App? Current => Application.Current as App;

}
