using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels.Items;
using Minecraft_QQ_NewGui.ViewModels;

namespace Minecraft_QQ_NewGui.Windows;

public class CommandFlyout
{
    public CommandFlyout(Control con, WindowModel top, CommandModel? model = null)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddCommand),
            ("修改", model != null, ()=>{ top.Edit(model!); }),
            ("删除", model != null, ()=>{ top.Delete(model!); }),
        ], con);
    }
}
