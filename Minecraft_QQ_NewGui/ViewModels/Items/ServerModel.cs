using CommunityToolkit.Mvvm.ComponentModel;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class ServerModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _addr;

    public void Disconnect()
    {
        top.Delete(this);
    }
}
