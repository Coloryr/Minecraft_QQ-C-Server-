using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using System.ComponentModel;
using System.Threading;

namespace Minecraft_QQ.NewGui.UISetting;

public class ColorSel : INotifyPropertyChanged
{
    public static readonly IBrush AppLightBackColor = Brush.Parse("#FFF3F3F3");
    public static readonly IBrush AppLightBackColor1 = Brush.Parse("#EEEEEEEE");
    public static readonly IBrush AppLightBackColor2 = Brush.Parse("#11FFFFFF");
    public static readonly IBrush AppLightBackColor3 = Brush.Parse("#EEEEEE");

    public static readonly IBrush AppDarkBackColor = Brush.Parse("#FF202020");
    public static readonly IBrush AppDarkBackColor1 = Brush.Parse("#EE202020");
    public static readonly IBrush AppDarkBackColor2 = Brush.Parse("#11202020");
    public static readonly IBrush AppDarkBackColor3 = Brush.Parse("#222222");

    public const string MainColorStr = "#FF5ABED6";

    public const string BackLigthColorStr = "#FFF4F4F5";
    public const string Back1LigthColorStr = "#62FFFFFF";
    public const string ButtonLightFontStr = "#FFFFFFFF";
    public const string FontLigthColorStr = "#FF000000";

    public const string BackDarkColorStr = "#FF202020";
    public const string Back1DarkColorStr = "#46202020";
    public const string ButtonDarkFontStr = "#FF202020";
    public const string FontDarkColorStr = "#FFE9E9E9";

    public static IBrush MainColor { get; private set; } = Brush.Parse(MainColorStr);
    public static IBrush BackColor { get; private set; } = Brush.Parse(BackLigthColorStr);
    public static IBrush Back1Color { get; private set; } = Brush.Parse(Back1LigthColorStr);
    public static IBrush ButtonFont { get; private set; } = Brush.Parse(ButtonLightFontStr);
    public static IBrush FontColor { get; private set; } = Brush.Parse(FontLigthColorStr);
    public static IBrush BottomColor { get; private set; } = AppLightBackColor;
    public static IBrush TopBottomColor { get; private set; } = AppLightBackColor1;
    public static IBrush BottomTranColor { get; private set; } = AppLightBackColor2;
    public static IBrush BottomColor1 { get; private set; } = AppLightBackColor3;

    public readonly static ColorSel Instance = new();

    public IBrush this[string key]
    {
        get
        {
            if (key == "Main")
                return MainColor;
            else if (key == "Back")
                return BackColor;
            else if (key == "TranBack")
                return Back1Color;
            else if (key == "Font")
                return FontColor;
            else if (key == "ButtonFont")
                return ButtonFont;
            else if (key == "Bottom")
                return BottomColor;
            else if (key == "Bottom1")
                return BottomColor1;
            else if (key == "TopBottom")
                return TopBottomColor;
            else if (key == "BottomTran")
                return BottomTranColor;
            else if (key == "PointIn")
                return MainColor;

            return Brushes.White;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void Reload()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Indexer.IndexerName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Indexer.IndexerArrayName));
    }
}
