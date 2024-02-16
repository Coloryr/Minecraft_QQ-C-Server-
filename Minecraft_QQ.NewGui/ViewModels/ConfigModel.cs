using CommunityToolkit.Mvvm.ComponentModel;
using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class ConfigModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private string? _commandHead;
    [ObservableProperty]
    private string? _commandOnline;
    [ObservableProperty]
    private string? _commandPlayer;
    [ObservableProperty]
    private string? _commandName;
    [ObservableProperty]
    private string? _commandSend;

    [ObservableProperty]
    private bool _settingAsk;
    [ObservableProperty]
    private bool _settingColor;
    [ObservableProperty]
    private bool _settingFix;
    [ObservableProperty]
    private bool _settingAuto;
    [ObservableProperty]
    private bool _settingNickServer;
    [ObservableProperty]
    private bool _settingNickGroup;
    [ObservableProperty]
    private bool _settingBind;
    [ObservableProperty]
    private bool _settingLog;
    [ObservableProperty]
    private bool _settingCommand;
    [ObservableProperty]
    private int? _settingDelay;
    [ObservableProperty]
    private long? _settingQQ;

    [ObservableProperty]
    private string? _adminMute;
    [ObservableProperty]
    private string? _adminUnmute;
    [ObservableProperty]
    private string? _adminName;
    [ObservableProperty]
    private string? _adminRename;
    [ObservableProperty]
    private string? _adminFix;
    [ObservableProperty]
    private string? _adminReload;
    [ObservableProperty]
    private string? _adminNick;
    [ObservableProperty]
    private string? _adminList;
    [ObservableProperty]
    private string? _adminMuteList;

    private bool _isLoad = false;

    partial void OnCommandHeadChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Check;
        con.Head = value;
        ConfigWrite.Config();
    }

    partial void OnCommandOnlineChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Check;
        con.ServerCheck = value;
        ConfigWrite.Config();
    }

    partial void OnCommandPlayerChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Check;
        con.PlayList = value;
        ConfigWrite.Config();
    }

    partial void OnCommandNameChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Check;
        con.Bind = value;
        ConfigWrite.Config();
    }

    partial void OnCommandSendChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Check;
        con.Send = value;
        ConfigWrite.Config();
    }

    partial void OnSettingAskChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.AskEnable = value;
        ConfigWrite.Config();
    }

    partial void OnSettingColorChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.ColorEnable = value;
        ConfigWrite.Config();
    }

    partial void OnSettingAutoChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.AutoSend = value;
        ConfigWrite.Config();
    }

    partial void OnSettingBindChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.CanBind = value;
        ConfigWrite.Config();
    }

    partial void OnSettingCommandChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendCommand = value;
        ConfigWrite.Config();
    }

    partial void OnSettingLogChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendLog = value;
        ConfigWrite.Config();
    }

    partial void OnSettingFixChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.FixMode = value;
        ConfigWrite.Config();
    }

    partial void OnSettingNickGroupChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendNickGroup = value;
        ConfigWrite.Config();
    }

    partial void OnSettingNickServerChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendNickServer = value;
        ConfigWrite.Config();
    }

    partial void OnSettingDelayChanged(int? value)
    {
        if (_isLoad || value == null || value == 0)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendDelay = (int)value;
        ConfigWrite.Config();
    }

    partial void OnSettingQQChanged(long? value)
    {
        if (_isLoad || value == null || value == 0)
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Setting;
        con.SendQQ = (long)value;
        ConfigWrite.Config();
    }

    partial void OnAdminMuteChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.Mute = value;
        ConfigWrite.Config();
    }

    partial void OnAdminUnmuteChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.UnMute = value;
        ConfigWrite.Config();
    }

    partial void OnAdminNickChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.Nick = value;
        ConfigWrite.Config();
    }

    partial void OnAdminNameChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.CheckBind = value;
        ConfigWrite.Config();
    }

    partial void OnAdminFixChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.Fix = value;
        ConfigWrite.Config();
    }

    partial void OnAdminReloadChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.Reload = value;
        ConfigWrite.Config();
    }

    partial void OnAdminListChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.GetCantBindList = value;
        ConfigWrite.Config();
    }

    partial void OnAdminMuteListChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.GetMuteList = value;
        ConfigWrite.Config();
    }

    partial void OnAdminRenameChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var con = Minecraft_QQ.MainConfig.Admin;
        con.Rename = value;
        ConfigWrite.Config();
    }

    public void Load()
    {
        _isLoad = true;
        {
            var con = Minecraft_QQ.MainConfig.Check;
            CommandHead = con.Head;
            CommandOnline = con.ServerCheck;
            CommandPlayer = con.PlayList;
            CommandName = con.Bind;
            CommandSend = con.Send;
        }
        {
            var con = Minecraft_QQ.MainConfig.Setting;
            SettingAsk = con.AskEnable;
            SettingColor = con.ColorEnable;
            SettingAuto = con.AutoSend;
            SettingBind = con.CanBind;
            SettingCommand = con.SendCommand;
            SettingLog = con.SendLog;
            SettingFix = con.FixMode;
            SettingDelay = con.SendDelay;
            SettingNickGroup = con.SendNickGroup;
            SettingNickServer = con.SendNickServer;
            SettingQQ = con.SendQQ;
        }
        {
            var con = Minecraft_QQ.MainConfig.Admin;
            AdminMute = con.Mute;
            AdminUnmute = con.UnMute;
            AdminNick = con.Nick;
            AdminName = con.CheckBind;
            AdminRename = con.Rename;
            AdminFix = con.Fix;
            AdminReload = con.Reload;
            AdminList = con.GetCantBindList;
            AdminMuteList = con.GetMuteList;
        }
        _isLoad = false;
    }
}
