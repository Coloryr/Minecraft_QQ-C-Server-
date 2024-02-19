using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class AddPlayerModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private long? userQQ;
    [ObservableProperty]
    private string? name;
    [ObservableProperty]
    private string? nick;
    [ObservableProperty]
    private bool admin;

    [RelayCommand]
    public void Save()
    {
        top.AddPlayer(this);
    }

    [RelayCommand]
    public void Cancel()
    {
        top.Cancel();
    }

    public PlayerObj ToObj()
    {
        return new()
        {
            QQ = UserQQ ?? 0,
            Name = Name ?? "",
            Nick = Nick ?? "",
            IsAdmin = Admin
        };
    }
}
