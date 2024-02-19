using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using System.Text;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class CommandModel(string head, CommandObj obj) : ObservableObject
{
    public CommandObj Obj = obj;

    [ObservableProperty]
    private string _check = head;
    [ObservableProperty]
    private string _command = obj.Command;
    [ObservableProperty]
    private bool _player = obj.PlayerUse;
    [ObservableProperty]
    private bool _send = obj.PlayerSend;
    [ObservableProperty]
    private string _server = obj.Servers.GetString();
}
