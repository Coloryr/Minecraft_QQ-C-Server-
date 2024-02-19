using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace Minecraft_QQ_NewGui.UISetting;

public static class ColorSel
{
    public static readonly IBrush AppLightBackColor = Brush.Parse("#FFF3F3F3");
    public static readonly IBrush AppLightBackColor1 = Brush.Parse("#AA989898");
    public static readonly IBrush AppLightBackColor2 = Brush.Parse("#11FFFFFF");
    public static readonly IBrush AppLightBackColor3 = Brush.Parse("#EEEEEE");
    public static readonly IBrush AppLightBackColor4 = Brush.Parse("#CCCCCC");
    public static readonly IBrush AppLightBackColor5 = Brush.Parse("#22FFFFFF");
    public static readonly IBrush AppLightBackColor6 = Brush.Parse("#FFDDDDDD");
    public static readonly IBrush AppLightBackColor7 = Brush.Parse("#DDFFFFFF");
    public static readonly IBrush AppLightBackColor8 = Brush.Parse("#FFb7bac1"); //button boder
    public static readonly IBrush AppLightBackColor9 = Brush.Parse("#AAFFFFFF");

    public static readonly IBrush AppDarkBackColor = Brush.Parse("#FF202020");
    public static readonly IBrush AppDarkBackColor1 = Brush.Parse("#CC3A3A3A");
    public static readonly IBrush AppDarkBackColor2 = Brush.Parse("#11202020");
    public static readonly IBrush AppDarkBackColor3 = Brush.Parse("#222222");
    public static readonly IBrush AppDarkBackColor4 = Brush.Parse("#888888");
    public static readonly IBrush AppDarkBackColor5 = Brush.Parse("#AA000000");
    public static readonly IBrush AppDarkBackColor6 = Brush.Parse("#FF333333");
    public static readonly IBrush AppDarkBackColor7 = Brush.Parse("#EE000000");
    public static readonly IBrush AppDarkBackColor8 = Brush.Parse("#FFEEEEEE"); //button boder
    public static readonly IBrush AppDarkBackColor9 = Brush.Parse("#EE000000");

    public const string MainColorStr = "#FF5ABED6";

    public const string BackLigthColorStr = "#FFF4F4F5";
    public const string Back1LigthColorStr = "#66FFFFFF";
    public const string ButtonLightFontStr = "#FFFFFFFF";
    public const string FontLigthColorStr = "#FF000000";

    public const string BackDarkColorStr = "#FF202020";
    public const string Back1DarkColorStr = "#46202020";
    public const string ButtonDarkFontStr = "#FF000000";
    public const string FontDarkColorStr = "#FFE9E9E9";

    public const string GroupLightColorStr = "#CCfbfbfb";
    public const string GroupDarkColorStr = "#CC000000";

    public const string GroupLightColor1Str = "#FFe5e5e5";
    public const string GroupDarkColor1Str = "#FF1d1d1d";

    public static IBrush MainColor { get; private set; } = Brush.Parse(MainColorStr);
    public static IBrush BackColor { get; private set; } = Brush.Parse(BackLigthColorStr);
    public static IBrush Back1Color { get; private set; } = Brush.Parse(Back1LigthColorStr);
    public static IBrush Back2Color { get; private set; } = AppLightBackColor6;
    public static IBrush ButtonFont { get; private set; } = Brush.Parse(ButtonLightFontStr);
    public static IBrush FontColor { get; private set; } = Brush.Parse(FontLigthColorStr);
    public static IBrush MotdColor { get; private set; } = Brush.Parse("#FFFFFFFF");
    public static IBrush MotdBackColor { get; private set; } = Brush.Parse("#FF000000");
    public static IBrush BottomColor { get; private set; } = AppLightBackColor;
    public static IBrush TopBottomColor { get; private set; } = AppLightBackColor1;
    public static IBrush BottomTranColor { get; private set; } = AppLightBackColor2;
    public static IBrush BottomColor1 { get; private set; } = AppLightBackColor3;
    public static IBrush BottomColor2 { get; private set; } = AppLightBackColor4;
    public static IBrush BGColor { get; private set; } = AppLightBackColor5;
    public static IBrush BGColor1 { get; private set; } = AppLightBackColor7;
    public static IBrush GroupBackColor { get; private set; } = Brush.Parse(GroupLightColorStr);
    public static IBrush GroupBackColor1 { get; private set; } = Brush.Parse(GroupLightColor1Str);
    public static IBrush ButtonBorder { get; private set; } = AppLightBackColor8;
    public static IBrush MainButtonBG { get; private set; } = AppLightBackColor9;

    private static readonly Dictionary<string, List<WeakReference<IObserver<IBrush>>>> s_colorList = [];

    public static IDisposable Add(string key, IObserver<IBrush> observer)
    {
        if (s_colorList.TryGetValue(key, out var list))
        {
            list.Add(new(observer));
        }
        else
        {
            list = [new(observer)];
            s_colorList.Add(key, list);
        }
        var value = GetColor(key);
        observer.OnNext(value);
        return new Unsubscribe(list, observer);
    }

    private class Unsubscribe(List<WeakReference<IObserver<IBrush>>> observers, IObserver<IBrush> observer) : IDisposable
    {
        public void Dispose()
        {
            foreach (var item in observers.ToArray())
            {
                if (item.TryGetTarget(out var target)
                    && target == observer)
                {
                    observers.Remove(item);
                }
            }
        }
    }

    private static IBrush GetColor(string key)
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
        else if (key == "Motd")
            return MotdColor;
        else if (key == "MotdBack")
            return MotdBackColor;
        else if (key == "Bottom")
            return BottomColor;
        else if (key == "Bottom1")
            return BottomColor1;
        else if (key == "Bottom2")
            return BottomColor2;
        else if (key == "TopBottom")
            return TopBottomColor;
        else if (key == "BottomTran")
            return BottomTranColor;
        else if (key == "PointIn")
            return MainColor;
        else if (key == "GroupBack")
            return GroupBackColor;
        else if (key == "GroupColor")
            return GroupBackColor1;
        else if (key == "BG")
            return BGColor;
        else if (key == "BG1")
            return BGColor1;
        else if (key == "Back1")
            return Back2Color;
        else if (key == "ButtonBorder")
            return ButtonBorder;
        else if (key == "MainButtonBG")
            return MainButtonBG;

        return Brushes.White;
    }
}
