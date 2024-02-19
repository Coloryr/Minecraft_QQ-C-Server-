using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels;

namespace Minecraft_QQ_NewGui.Windows;

public class MuteFlyout
{
    public MuteFlyout(Control con, WindowModel top, string? data = null)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddMute),
            ("删除", data != null, ()=>{ top.DeleteMute(data); }),
        ], con);
    }
}
