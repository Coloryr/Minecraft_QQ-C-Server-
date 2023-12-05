using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Minecraft_QQ.NewGui.ViewModels;
using Minecraft_QQ.NewGui.Windows;
using System;

namespace Minecraft_QQ.NewGui;

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