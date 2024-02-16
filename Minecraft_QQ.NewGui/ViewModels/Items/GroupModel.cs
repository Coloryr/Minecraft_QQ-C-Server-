using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class GroupModel(WindowModel top, GroupObj obj) : ObservableObject
{
    public GroupObj Obj { get; init; } = obj;

    [ObservableProperty]
    private long? _group = obj.Group;
    [ObservableProperty]
    private bool _enableCommand = obj.EnableCommand;
    [ObservableProperty]
    private bool _enableSay = obj.EnableSay;
    [ObservableProperty]
    private bool _isMain = obj.IsMain;

    partial void OnGroupChanged(long? value)
    {
        if (value == null || value == 0)
        {
            Group = Obj.Group;
        }
        else
        {
            Obj.Group = (long)value;
            ConfigWrite.Group();
        }
    }

    partial void OnEnableSayChanged(bool value)
    {
        Obj.EnableSay = value;
        ConfigWrite.Group();

        top.ShowNotify("群设置已保存");
    }

    partial void OnEnableCommandChanged(bool value)
    {
        Obj.EnableCommand = value;
        ConfigWrite.Group();

        top.ShowNotify("群设置已保存");
    }

    partial void OnIsMainChanged(bool value)
    {
        Obj.IsMain = value;
        ConfigWrite.Group();

        top.ShowNotify("群设置已保存");
    }

    public void Delete()
    {
        top.Delete(this);
    }
}
