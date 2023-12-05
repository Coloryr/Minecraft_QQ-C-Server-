using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ.NewGui.ViewModels;

public partial class WindowModel : BaseModel
{
    private readonly Semaphore _semaphore = new(0, 2);

    [ObservableProperty]
    public CommandObj commandObj;
    [ObservableProperty]
    public string commandCheck;

    public ObservableCollection<string> ServerList { get; init; } = new();

    public WindowModel()
    {
        CommandCheck = "";
        CommandObj = new();
    }

    public void CommandSetDisplay()
    {
        OnPropertyChanged("CommandSetDisplay");
    }

    public void CommandSetHide()
    {
        OnPropertyChanged("CommandSetHide");
    }

    public async Task NewCommand()
    {
        CommandCheck = "";
        CommandObj = new();
        ServerList.Clear();

        CommandSetDisplay();

        await Task.Run(() =>
        {
            _semaphore.WaitOne();
        });
    }

    public async Task SetCommand(string key, CommandObj value)
    {
        CommandCheck = key;
        CommandObj = value;
        ServerList.Clear();
        value.Servers.ForEach(ServerList.Add);

        CommandSetDisplay();

        await Task.Run(() =>
        {
            _semaphore.WaitOne();
        });
    }

    [RelayCommand]
    public void CommandSave()
    { 
        
    }

    [RelayCommand]
    public void CommandCancel()
    { 
        
    }
}
