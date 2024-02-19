using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_NewGui.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class ServerItem(string name, Action action) : ObservableObject
{
    [ObservableProperty]
    private string _name = name;

    partial void OnNameChanged(string value)
    {
        action();
    }
}

public partial class CommandSetModel : ObservableObject
{
    [ObservableProperty]
    public CommandObj _obj;
    [ObservableProperty]
    public string _check;

    public ObservableCollection<ServerItem> ServerList { get; init; } = [];

    private readonly WindowModel _top;

    public CommandSetModel(WindowModel top, CommandModel? obj = null)
    {
        _top = top;
        Check = obj?.Check ?? "";
        Obj = obj?.Obj ?? new();

        ServerList.Clear();
        foreach (var item in Obj.Servers)
        {
            ServerList.Add(new(item, ServerListEdit));
        }
        ServerList.Add(new("添加服务器", ServerListEdit));
    }

    private void ServerListEdit()
    {
        var last = ServerList.Last();
        if (ServerList.Count == 0 || last.Name != "添加服务器")
        {
            ServerList.Add(new("添加服务器", ServerListEdit));
        }
        foreach (var item in ServerList.ToArray())
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ServerList.Remove(item);
            }
        }
    }

    [RelayCommand]
    public void Save()
    {
        Obj.Servers.Clear();
        if (ServerList.Count > 1)
        {
            foreach (var item in ServerList.ToArray()[..^1])
            {
                Obj.Servers.Add(item.Name);
            }
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
