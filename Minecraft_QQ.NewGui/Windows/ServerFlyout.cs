using Avalonia.Controls;
using Minecraft_QQ_NewGui.ViewModels.Items;
using Minecraft_QQ_NewGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_QQ_NewGui.Windows;

public class ServerFlyout
{
    public ServerFlyout(Control con, ServerModel model)
    {
        _ = new FlyoutsControl(
        [
            ("断开链接", true, model.Disconnect),
            ("设置配置文件", true, model.EditConfig),
        ], con);
    }
}
