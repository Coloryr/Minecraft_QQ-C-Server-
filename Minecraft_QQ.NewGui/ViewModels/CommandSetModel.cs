using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_NewGui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using static Minecraft_QQ_Core.MyMysql;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class CommandSetModel : ObservableObject
{
    [ObservableProperty]
    public CommandObj _obj;
    [ObservableProperty]
    public string _check;

    public ObservableCollection<string> ServerList { get; init; } = [];

    private readonly WindowModel _top;

    public CommandSetModel(WindowModel top, CommandModel? obj = null)
    {
        _top = top;
        Check = obj?.Check ?? "";
        Obj = obj?.Obj ?? new();

        ServerList.Clear();
        Obj.Servers.ForEach(ServerList.Add);
        Obj.Servers.Add("修改添加服务器\x01");

        ServerList.CollectionChanged += ServerList_CollectionChanged;
    }

    private void ServerList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in Obj.Servers.ToArray())
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                ServerList.Remove(item);
            }
        }
        var last = Obj.Servers.Last();
        if (!last.EndsWith('\x01'))
        {
            Obj.Servers.Add("修改添加服务器\x01");
        }
    }

    [RelayCommand]
    public void Save()
    {
        Obj.Servers.Clear();
        if (ServerList.Count > 1)
        {
            Obj.Servers.AddRange(ServerList.ToArray()[..^1]);
        }
        if (!Minecraft_QQ.CommandConfig.CommandList.TryAdd(Check, Obj))
        {
            Minecraft_QQ.CommandConfig.CommandList[Check] = Obj;
        }

        ConfigWrite.Command();

        _top.Cancel();
        _top.LoadCommand();
        _top.ShowNotify("指令已修改");
    }

    [RelayCommand]
    public void Cancel()
    {
        _top.Cancel();
    }
}
