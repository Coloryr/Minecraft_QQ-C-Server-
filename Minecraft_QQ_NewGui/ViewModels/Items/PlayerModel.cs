using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class PlayerModel(WindowModel top, PlayerObj obj) : ObservableObject
{
    public PlayerObj Obj = obj;

    [ObservableProperty]
    private long? userQQ = obj.QQ;
    [ObservableProperty]
    private string? name = obj.Name;
    [ObservableProperty]
    private string? nick = obj.Nick;
    [ObservableProperty]
    private bool admin = obj.IsAdmin;

    partial void OnAdminChanged(bool value)
    {
        Obj.IsAdmin = value;
        ConfigWrite.Player();

        top.ShowNotify("玩家设置已保存");
    }

    partial void OnNameChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Name = Obj.Name;
            return;
        }
        Obj.Name = value;
        ConfigWrite.Player();

        top.ShowNotify("玩家设置已保存");
    }

    partial void OnNickChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Nick = Obj.Nick;
            return;
        }
        Obj.Nick = value;
        ConfigWrite.Player();

        top.ShowNotify("玩家设置已保存");
    }

    partial void OnUserQQChanged(long? value)
    {
        if (value == null || value == 0)
        {
            UserQQ = Obj.QQ;
            return;
        }
        else
        {
            Obj.QQ = (long)value;
            ConfigWrite.Player();

            top.ShowNotify("玩家设置已保存");
        }
    }
}
