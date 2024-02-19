using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels.Items;

namespace Minecraft_QQ_NewGui.Windows;

public class ServerFlyout
{
    public ServerFlyout(Control con, ServerModel model)
    {
        _ = new FlyoutsControl(
        [
            ("断开链接", true, model.Disconnect)
        ], con);
    }
}
