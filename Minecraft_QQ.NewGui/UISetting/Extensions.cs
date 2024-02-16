using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia;
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