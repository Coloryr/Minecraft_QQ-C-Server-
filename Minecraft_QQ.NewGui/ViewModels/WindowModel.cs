using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core;
using Minecraft_QQ_NewGui.ViewModels.Items;
using System.Collections.ObjectModel;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class WindowModel : ObservableObject
{
    public ObservableCollection<GroupModel> Groups { get; init; } = [];

    [ObservableProperty]
    private GroupModel? _groupItem;

    public void Load()
    {
        Groups.Clear();

        foreach (var item in Minecraft_QQ.GroupConfig.Groups.Values)
        {
            Groups.Add(new(item));
        }
    }

    public void AddGroup()
    { 
        
    }
}
