using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels;
using Minecraft_QQ_NewGui.ViewModels.Items;

namespace Minecraft_QQ_NewGui.Windows;

public class PlayerFlyout
{
    public PlayerFlyout(Control con, WindowModel top, PlayerModel? model)
    {
        _ = new FlyoutsControl(
        [
            ("添加", true, top.AddPlayer),
            ("删除", model != null, ()=>
                {
                    if(model != null)
                    {
                        top.Delete(model);
                    }
                })
        ], con);
    }
}
