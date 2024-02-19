using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels;

namespace Minecraft_QQ_NewGui.Windows;

public class NotBindFlyout
{
    public NotBindFlyout(Control con, WindowModel top, string? data = null)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddNotBind),
            ("删除", data != null, ()=>{ top.DeleteNotBind(data); }),
        ], con);
    }
}
