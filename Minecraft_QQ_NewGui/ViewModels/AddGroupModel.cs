using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class AddGroupModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private long? _group;
    [ObservableProperty]
    private bool _enableCommand;
    [ObservableProperty]
    private bool _enableSay;
    [ObservableProperty]
    private bool _isMain;

    [RelayCommand]
    public void Save()
    {
        top.AddGroup(this);
    }

    [RelayCommand]
    public void Cancel()
    {
        top.Cancel();
    }

    public GroupObj ToObj()
    {
        return new()
        {
            Group = Group ?? 0,
            EnableCommand = EnableCommand,
            EnableSay = EnableSay,
            IsMain = IsMain
        };
    }
}
