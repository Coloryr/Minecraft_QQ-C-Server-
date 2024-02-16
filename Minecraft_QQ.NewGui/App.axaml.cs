using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Minecraft_QQ_Core;
using Minecraft_QQ_NewGui.ViewModels;
using Minecraft_QQ_NewGui.Windows;
using System;
using System.Threading;

namespace Minecraft_QQ_NewGui;

public partial class App : Application
{
    public static readonly CrossFade CrossFade300 = new(TimeSpan.FromMilliseconds(300));

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new WindowModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}