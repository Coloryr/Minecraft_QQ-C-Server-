using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels;
using Minecraft_QQ_NewGui.ViewModels.Items;

namespace Minecraft_QQ_NewGui.Windows;

public class GroupFlyout
{
    public GroupFlyout(Control con, WindowModel top, GroupModel? model = null)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddGroup),
            ("删除", model != null, model!.Delete),
        ], con);
    }
}
