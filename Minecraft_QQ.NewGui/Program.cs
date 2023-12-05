using Avalonia;
using Avalonia.Media;
using System;

namespace Minecraft_QQ.NewGui;

internal class Program
{
    public const string Font = "resm:Minecraft_QQ.NewGui.Resource.MiSans-Normal.ttf#MiSans";

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = Font,
            })
            .UsePlatformDetect()
            .LogToTrace();
}
