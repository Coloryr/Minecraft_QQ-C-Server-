using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System;

namespace Minecraft_QQ_NewGui.UISetting;

public static class Indexer
{
    public const string IndexerName = "Item";
    public const string IndexerArrayName = "Item[]";
}

public class ColorsExtension : MarkupExtension
{
    public ColorsExtension(string key)
    {
        Key = key;
    }

    public string Key { get; set; }


    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var keyToUse = Key;

        var binding = new ReflectionBindingExtension($"[{keyToUse}]")
        {
            Mode = BindingMode.OneWay,
            Source = ColorSel.Instance,
        };

        return binding.ProvideValue(serviceProvider);
    }
}