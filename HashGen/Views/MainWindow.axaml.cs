using Avalonia;
using Avalonia.Controls;

using HashGen.ViewModels;

using Microsoft.Extensions.DependencyInjection;

namespace HashGen.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
        var mainView = this.Find<MainView>("MainViewControl");
        if (mainView != null)
        {
            var vm = App.ServiceProvider.GetRequiredService<MainViewModel>();
            mainView.DataContext = vm;


            // Для отладки
            System.Diagnostics.Debug.WriteLine($"MainViewModel set: {vm != null}");
            System.Diagnostics.Debug.WriteLine($"SaveDataCommand exists: {vm?.SaveDataCommand != null}");
        }
    }
}
