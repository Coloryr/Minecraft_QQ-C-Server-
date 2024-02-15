using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class GroupModel : ObservableObject
{
    private GroupObj _obj;

    [ObservableProperty]
    private long? _group;
    [ObservableProperty]
    private bool _enableCommand;
    [ObservableProperty]
    private bool _enableSay;
    [ObservableProperty]
    private bool _isMain;

    public GroupModel(GroupObj obj)
    {
        _obj = obj;

        _group = obj.Group;
        _enableCommand = obj.EnableCommand;
        _enableSay = obj.EnableSay;
        _isMain = obj.IsMain;
    }

    partial void OnGroupChanged(long? value)
    {
        if (value == null || value == 0)
        {
            Group = _obj.Group;
        }
        else
        {
            _obj.Group = (long)value;
            ConfigWrite.Group();
        }
    }

    partial void OnEnableSayChanged(bool value)
    {
        _obj.EnableSay = value;
        ConfigWrite.Group();
    }

    partial void OnEnableCommandChanged(bool value)
    {
        _obj.EnableCommand = value;
        ConfigWrite.Group();
    }

    partial void OnIsMainChanged(bool value)
    {
        _obj.IsMain = value;
        ConfigWrite.Group();
    }

    public void Delete()
    { 
        
    }
}
