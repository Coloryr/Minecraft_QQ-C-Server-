using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_NewGui.ViewModels.Items;
using Minecraft_QQ_NewGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
