using System;

using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;

using HashGen.ViewModels;

namespace HashGen;

public class ViewModelLocator
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelLocator()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<MainViewModel>()
            .BuildServiceProvider();
    }

    public MainViewModel MainViewModel => _serviceProvider.GetService<MainViewModel>();
}