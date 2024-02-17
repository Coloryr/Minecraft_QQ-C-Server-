using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class AddAskModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private string? _check;

    [ObservableProperty]
    private string? _res;

    [RelayCommand]
    public void Ok()
    {
        top.AddAsk(this);
    }

    [RelayCommand]
    public void Cancel()
    {
        top.Cancel();
    }
}
