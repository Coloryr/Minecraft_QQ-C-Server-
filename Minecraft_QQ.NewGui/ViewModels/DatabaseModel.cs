using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class DatabaseModel(WindowModel top) : ObservableObject
{
    [ObservableProperty]
    private string? _url;

    [ObservableProperty]
    private string _state;
    [ObservableProperty]
    private string _button;

    [ObservableProperty]
    private bool _enable = true;
    [ObservableProperty]
    private bool _enableData;

    private bool _isLoad = false;

    partial void OnUrlChanged(string? value)
    {
        if (_isLoad || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        Minecraft_QQ.MainConfig.Database.Url = value;
        ConfigWrite.Config();
    }

    partial void OnEnableDataChanged(bool value)
    {
        if (_isLoad)
        {
            return;
        }

        Minecraft_QQ.MainConfig.Database.Enable = value;
        ConfigWrite.Config();
    }

    [RelayCommand]
    public void TestConnect()
    {
        if (Minecraft_QQ.MysqlOK)
        {
            MyMysql.MysqlStop();
        }
        else
        {
            MyMysql.MysqlStart();
        }

        Update();
    }

    public void Load()
    {
        _isLoad = true;
        Url = Minecraft_QQ.MainConfig.Database.Url;
        EnableData = Minecraft_QQ.MainConfig.Database.Enable;
        Update();
        _isLoad = false;
    }

    public void Update()
    {
        if (Minecraft_QQ.MysqlOK)
        {
            State = "已连接";
            Button = "断开链接";
            Enable = false;

            top.ShowNotify("数据库已连接");
        }
        else
        {
            State = "未连接";
            Button = "测试连接";
            Enable = true;

            top.ShowNotify("数据库已断开");
        }
    }
}
