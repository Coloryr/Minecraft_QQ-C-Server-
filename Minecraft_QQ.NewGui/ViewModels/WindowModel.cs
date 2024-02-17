using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_NewGui.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class WindowModel : ObservableObject
{
    public ObservableCollection<GroupModel> Groups { get; init; } = [];
    public ObservableCollection<ServerModel> Servers { get; init; } = [];
    public ObservableCollection<PlayerModel> Players { get; init; } = [];
    public ObservableCollection<string> Mutes { get; set; } = [];
    public ObservableCollection<string> NotBinds { get; set; } = [];
    public ObservableCollection<AskModel> Asks { get; init; } = [];
    public ObservableCollection<CommandModel> Commands { get; init; } = [];

    public ConfigModel Config { get; init; }
    public DatabaseModel Database { get; init; }

    [ObservableProperty]
    private GroupModel? _groupItem;
    [ObservableProperty]
    private ServerModel? _serverItem;
    [ObservableProperty]
    private PlayerModel? _playerItem;
    [ObservableProperty]
    private string? _muteItem;
    [ObservableProperty]
    private string? _notBindItem;
    [ObservableProperty]
    private AskModel? _askItem;
    [ObservableProperty]
    private CommandModel? _commandItem;

    [ObservableProperty]
    private string _state;
    [ObservableProperty]
    private string _socketST;
    [ObservableProperty]
    private ushort? _socketPort;
    [ObservableProperty]
    private bool _socketEdit;
    [ObservableProperty]
    private bool _socketCheck;

    public string Notify = "";

    private bool _isLoad = false;

    public WindowModel()
    {
        Config = new(this);
        Database = new(this);
    }

    partial void OnSocketPortChanged(ushort? value)
    {
        if (_isLoad)
        {
            return;
        }
        var con = Minecraft_QQ.MainConfig.Socket;
        if (value == null || value == 0)
        {
            SocketPort = con.Port;
        }
        else
        {
            con.Port = (ushort)value;
            ConfigWrite.Config();
        }
    }

    partial void OnSocketCheckChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }
        var con = Minecraft_QQ.MainConfig.Socket;
        con.Check = value;
        ConfigWrite.Config();
    }

    [RelayCommand]
    public void SocketOpen()
    {
        if (PluginServer.Start)
        {
            PluginServer.ServerStop();
            SocketEdit = true;
            SocketST = "启动端口";

            ShowNotify("已关闭端口");
        }
        else
        {
            PluginServer.StartServer();
            SocketEdit = false;
            SocketST = "关闭端口";

            ShowNotify("正在启动端口");
        }
    }

    public void Load()
    {
        _isLoad = true;
        LoadGroup();
        LoadServer();
        Config.Load();
        Database.Load();
        LoadPlayer();
        LoadAsk();
        LoadCommand();
        _isLoad = false;
    }

    public void LoadServer()
    {
        var con = Minecraft_QQ.MainConfig.Socket;
        SocketPort = con.Port;
        SocketCheck = con.Check;
    }

    public void UpdateServer()
    {
        Servers.Clear();
        if (!PluginServer.Start)
        {
            State = "未就绪";
            SocketST = "启动端口";
            SocketEdit = true;

            ShowNotify("端口未就绪");
        }
        else if (PluginServer.Start && !PluginServer.IsReady())
        {
            State = "等待连接";
            SocketST = "关闭端口";
            SocketEdit = false;

            ShowNotify("端口已启动");
        }
        else if (PluginServer.IsReady())
        {
            State = "运行中";
            SocketST = "关闭端口";
            SocketEdit = false;
        }
        foreach (var item in PluginServer.MCServers)
        {
            Servers.Add(new(this)
            {
                Name = item.Key,
                Addr = item.Value.Channel.RemoteAddress.ToString() ?? ""
            });
        }
    }

    public void LoadGroup()
    {
        Groups.Clear();

        foreach (var item in Minecraft_QQ.GroupConfig.Groups.Values)
        {
            Groups.Add(new(this, item));
        }
    }

    public void LoadAsk()
    {
        Asks.Clear();

        foreach (var item in Minecraft_QQ.AskConfig.AskList)
        {
            Asks.Add(new(this, item));
        }
    }

    public void LoadCommand()
    {
        Commands.Clear();

        foreach (var item in Minecraft_QQ.CommandConfig.CommandList)
        {
            Commands.Add(new(item.Key, item.Value));
        }
    }

    public void LoadPlayer()
    {
        Players.Clear();

        foreach (var item in Minecraft_QQ.PlayerConfig.PlayerList.Values)
        {
            Players.Add(new(this, item));
        }

        Mutes.Clear();

        foreach (var item in Minecraft_QQ.PlayerConfig.MuteList)
        {
            Mutes.Add(item);
        }

        NotBinds.Clear();

        foreach (var item in Minecraft_QQ.PlayerConfig.NotBindList)
        {
            NotBinds.Add(item);
        }
    }

    public void AddAsk()
    {
        DialogHost.Show(new AddAskModel(this), "Main");
    }

    public void AddPlayer()
    {
        DialogHost.Show(new AddPlayerModel(this), "Main");
    }

    public void AddGroup()
    {
        DialogHost.Show(new AddGroupModel(this), "Main");
    }

    public void AddCommand()
    {
        DialogHost.Show(new CommandSetModel(this), "Main");
    }

    public void AddPlayer(AddPlayerModel model)
    {
        DialogHost.Close("Main");

        var obj = model.ToObj();
        if (obj.QQ == 0 || string.IsNullOrWhiteSpace(obj.Name))
        {
            ShowNotify("请输入正确的信息");
            return;
        }

        Minecraft_QQ.SetPlayer(obj);

        LoadPlayer();

        ShowNotify("已添加玩家");
    }

    public void AddAsk(AddAskModel model)
    {
        DialogHost.Close("Main");

        if (string.IsNullOrWhiteSpace(model.Check) || string.IsNullOrWhiteSpace(model.Res))
        {
            ShowNotify("请输入正确的信息");
            return;
        }

        Minecraft_QQ.AddAsk(model.Check, model.Res);

        LoadAsk();

        ShowNotify("已添加自动回复");
    }


    public void AddGroup(AddGroupModel model)
    {
        DialogHost.Close("Main");

        var obj = model.ToObj();
        if (obj.Group == 0)
        {
            ShowNotify("请输入正确的群号");
            return;
        }

        Minecraft_QQ.AddGroup(obj);

        LoadGroup();

        ShowNotify("已添加群");
    }

    public async void AddNotBind()
    {
        var model = new AddNameModel()
        { 
            Title = "输入禁止绑定的名字"
        };
        var name = await DialogHost.Show(model, "Main");
        if (name is true)
        {
            if (string.IsNullOrWhiteSpace(model.Text))
            {
                ShowNotify("请输入名字");
            }
            else
            {
                Minecraft_QQ.AddNotBind(model.Text);
                LoadPlayer();
            }
        }
    }

    public void DeleteNotBind(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }
        DialogHost.Show(new YesNoModel("是否要删除改名字", () =>
        {
            Cancel();
            Minecraft_QQ.RemoveNotBind(id);
            ShowNotify("已删除禁止绑定的名字");
            LoadPlayer();
        }, Cancel), "Main");
    }

    public void DeleteMute(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }
        DialogHost.Show(new YesNoModel("是否要删除改名字", () =>
        {
            Cancel();
            Minecraft_QQ.UnmutePlayer(id);
            ShowNotify("已删除禁言的名字");
            LoadPlayer();
        }, Cancel), "Main");
    }

    public async void AddMute()
    {
        var model = new AddNameModel()
        {
            Title = "输入禁言的名字"
        };
        var name = await DialogHost.Show(model, "Main");
        if (name is true)
        {
            if (string.IsNullOrWhiteSpace(model.Text))
            {
                ShowNotify("请输入名字");
            }
            else
            {
                Minecraft_QQ.MutePlayer(model.Text);
                LoadPlayer();
            }
        }
    }

    public void Delete(GroupModel model)
    {
        DialogHost.Show(new YesNoModel("是否要删除群设置", () => 
        {
            Cancel();
            if (Minecraft_QQ.GroupConfig.Groups.Remove(model.Obj.Group))
            {
                ConfigWrite.Group();
                ShowNotify("已删除群设置");
                LoadGroup();
            }
            else
            {
                ShowNotify("群设置删除失败");
            }
        }, Cancel), "Main");
    }

    public void Delete(PlayerModel model)
    {
        if (model.UserQQ == null || model.UserQQ == 0)
        {
            return;
        }
        DialogHost.Show(new YesNoModel("是否要删除玩家", () =>
        {
            Cancel();
            if (Minecraft_QQ.PlayerConfig.PlayerList.Remove((long)model.UserQQ))
            {
                ConfigWrite.Player();
                ShowNotify("已删除玩家");
                LoadPlayer();
            }
            else
            {
                ShowNotify("玩家删除失败");
            }
        }, Cancel), "Main");
    }

    public void Delete(ServerModel model)
    {
        DialogHost.Show(new YesNoModel("是否要断开链接", () =>
        {
            PluginServer.Close(model.Name);
            ShowNotify("已断开链接");
            UpdateServer();
        }, Cancel), "Main");
    }

    public void Delete(AskModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Check))
        {
            return;
        }
        DialogHost.Show(new YesNoModel("是否要删除自动应答", () =>
        {
            Cancel();
            if (Minecraft_QQ.AskConfig.AskList.Remove(model.Check))
            {
                ConfigWrite.Ask();
                ShowNotify("已删除自动应答");
                LoadAsk();
            }
            else
            {
                ShowNotify("自动应答删除失败");
            }
        }, Cancel), "Main");
    }

    public void Delete(CommandModel model)
    {
        DialogHost.Show(new YesNoModel("是否要删除服务器指令", () =>
        {
            Cancel();
            if (Minecraft_QQ.CommandConfig.CommandList.Remove(model.Check))
            {
                ConfigWrite.Command();
                ShowNotify("已删除服务器指令");
                LoadCommand();
            }
            else
            {
                ShowNotify("服务器指令删除失败");
            }
        }, Cancel), "Main");
    }

    public void Edit(CommandModel model)
    {
        DialogHost.Show(new CommandSetModel(this, model), "Main");
    }

    public void Cancel()
    {
        DialogHost.Close("Main");
    }

    public void ShowNotify(string data)
    {
        Notify = data;
        OnPropertyChanged("Notify");
    }
}
