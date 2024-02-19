using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace Minecraft_QQ_NewGui.UISetting;

public class ColorsExtension(string key) : MarkupExtension, IObservable<IBrush>
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<IBrush> observer)
    {
        return ColorSel.Add(key, observer);
    }
}