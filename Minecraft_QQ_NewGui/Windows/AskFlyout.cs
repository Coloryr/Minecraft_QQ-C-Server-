using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels;
using Minecraft_QQ_NewGui.ViewModels.Items;

namespace Minecraft_QQ_NewGui.Windows;

public class AskFlyout
{
    public AskFlyout(Control con, WindowModel top, AskModel? data = null)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddAsk),
            ("删除", data != null, ()=>{ top.Delete(data!); }),
        ], con);
    }
}
