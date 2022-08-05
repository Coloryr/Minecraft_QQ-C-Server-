using System;
using System.Collections.Generic;
using System.IO;

namespace Minecraft_QQ_Core.Config;

public record ConfigFile
{
    /// <summary>
    /// 主要配置文件
    /// </summary>
    public static FileInfo MainConfig { get; set; }
    /// <summary>
    /// 玩家管理配置文件
    /// </summary>
    public static FileInfo PlayerSave { get; set; }
    /// <summary>
    /// 自动应答配置文件
    /// </summary>
    public static FileInfo AskConfig { get; set; }
    /// <summary>
    /// 服务器命令配置文件
    /// </summary>
    public static FileInfo CommandSave { get; set; }
    /// <summary>
    /// 群配置文件
    /// </summary>
    public static FileInfo GroupConfig { get; set; }
}
public record PlayerConfig
{
    /// <summary>
    /// 禁止绑定数据储存
    /// </summary>
    public List<string> NotBindList { get; set; }
    /// <summary>
    /// 禁言数据存储
    /// </summary>
    public List<string> MuteList { get; set; }
    /// <summary>
    /// 玩家数据储存
    /// </summary>
    public Dictionary<long, PlayerObj> PlayerList { get; set; }

    public PlayerConfig()
    {
        NotBindList = new();
        MuteList = new();
        PlayerList = new();
    }
}
public record CommandConfig
{
    /// <summary>
    /// 服务器指令数据储存
    /// </summary>
    public Dictionary<string, CommandObj> CommandList { get; set; }

    public CommandConfig()
    {
        CommandList = new();
    }
}
public record GroupConfig
{
    /// <summary>
    /// 设置的群数据储存
    /// </summary>
    public Dictionary<long, GroupObj> Groups { get; set; }

    public GroupConfig()
    {
        Groups = new();
    }
}
public record AskConfig
{
    /// <summary>
    /// 自动应答存储
    /// </summary>
    public Dictionary<string, string> AskList { get; set; }

    public AskConfig()
    {
        AskList = new();
    }
}
public record MainConfig
{
    /// <summary>
    /// 设置
    /// </summary>
    public SettingConfig Setting { get; set; }
    /// <summary>
    /// 消息
    /// </summary>
    public MessageConfig Message { get; set; }
    /// <summary>
    /// 检测消息
    /// </summary>
    public CheckConfig Check { get; set; }
    /// <summary>
    /// 管理员指令
    /// </summary>
    public AdminConfig Admin { get; set; }
    /// <summary>
    /// Socket配置
    /// </summary>
    public SocketConfig Socket { get; set; }
    /// <summary>
    /// Mysql配置文件
    /// </summary>
    public MysqlConfig Database { get; set; }
    /// <summary>
    /// 机器人设定
    /// </summary>
    public SettingRobot RobotSetting { get; set; }

    public MainConfig()
    {
        Setting = new();
        Message = new();
        Check = new();
        Admin = new();
        Socket = new();
        Database = new();
        RobotSetting = new();
    }

    public override string ToString()
    {
        return $"设置:{Setting}{Environment.NewLine}"
            + $"消息:{Message}{Environment.NewLine}"
            + $"检测:{Check}{Environment.NewLine}"
            + $"管理员:{Admin}{Environment.NewLine}"
            + $"链接:{Socket}{Environment.NewLine}"
            + $"数据库:{Database}{Environment.NewLine}"
            + $"机器人设置:{RobotSetting}";
    }
}

public record SettingRobot
{
    public long QQ { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
    public int CheckDelay { get; set; }
    public bool Check { get; set; }

    public SettingRobot()
    {
        QQ = 0;
        IP = "127.0.0.1";
        Port = 23335;
        CheckDelay = 1000;
        Check = true;
    }

    public override string ToString()
    {
        return $"地址:{IP}{Environment.NewLine}"
            + $"端口:{Port}{Environment.NewLine}"
            + $"自动重连延迟:{CheckDelay}{Environment.NewLine}"
            + $"检查是否断开:{Check}{Environment.NewLine}"
            + $"QQ机器人账户:{QQ}";
    }
}

public record SettingConfig
{
    /// <summary>
    /// 自动应答-开关
    /// </summary>
    public bool AskEnable { get; set; }
    /// <summary>
    /// 彩色代码-开关
    /// </summary>
    public bool ColorEnable { get; set; }
    /// <summary>
    /// 维护模式-开关
    /// </summary>
    public bool FixMode { get; set; }
    /// <summary>
    /// 同步对话-开关
    /// </summary>
    public bool AutoSend { get; set; }
    /// <summary>
    /// 服务器昵称-开关
    /// </summary>
    public bool SendNickServer { get; set; }
    /// <summary>
    /// 群昵称-开关
    /// </summary>
    public bool SendNickGroup { get; set; }
    /// <summary>
    /// 允许玩家绑定ID
    /// </summary>
    public bool CanBind { get; set; }
    /// <summary>
    /// 发送日志消息到群
    /// </summary>
    public bool SendLog { get; set; }
    /// <summary>
    /// 发送指令到服务器
    /// </summary>
    public bool SendCommand { get; set; }
    /// <summary>
    /// 发送群消息间隔
    /// </summary>
    public int SendDelay { get; set; }

    public SettingConfig()
    {
        AskEnable = true;
        ColorEnable = true;
        FixMode = false;
        AutoSend = false;
        SendNickServer = true;
        SendNickGroup = true;
        CanBind = true;
        SendLog = true;
        SendCommand = false;
        SendDelay = 100;
    }

    public override string ToString()
    {
        return $"自动应答开关:{AskEnable}{Environment.NewLine}"
            + $"颜色代码开关:{ColorEnable}{Environment.NewLine}"
            + $"维护模式:{FixMode}{Environment.NewLine}"
            + $"始终发送消息:{AutoSend}{Environment.NewLine}"
            + $"使用昵称发送至服务器:{SendNickServer}{Environment.NewLine}"
            + $"使用昵称发送至群:{SendNickGroup}{Environment.NewLine}"
            + $"可以绑定名字:{CanBind}{Environment.NewLine}"
            + $"发送日志到群:{SendLog}{Environment.NewLine}"
            + $"不发送指令到服务器:{SendCommand}{Environment.NewLine}"
            + $"发送群消息间隔:{SendDelay}";
    }
}
public record MessageConfig
{
    /// <summary>
    /// 维护时发送的文本
    /// </summary>
    public string FixText { get; set; }
    /// <summary>
    /// 未知的指令
    /// </summary>
    public string UnknowText { get; set; }
    /// <summary>
    /// 禁止绑定ID
    /// </summary>
    public string CantBindText { get; set; }
    /// <summary>
    /// 没有绑定ID
    /// </summary>
    public string NoneBindID { get; set; }
    /// <summary>
    /// 重复绑定ID
    /// </summary>
    public string AlreadyBindID { get; set; }

    public MessageConfig()
    {
        FixText = "服务器正在维护，请等待维护结束！";
        UnknowText = "未知指令";
        CantBindText = "绑定ID已关闭";
        NoneBindID = $"你没有绑定服务器ID，发送：#绑定：[ID]来绑定，如：{Environment.NewLine}绑定：Color_yr";
        AlreadyBindID = "你已经绑定ID了，请找腐竹更改";
    }

    public override string ToString()
    {
        return $"维护提示文本:{FixText}{Environment.NewLine}"
            + $"未知指令文本:{UnknowText}{Environment.NewLine}"
            + $"不能绑定文本:{CantBindText}{Environment.NewLine}"
            + $"没有绑定ID文本：{NoneBindID}{Environment.NewLine}"
            + $"重复绑定ID文本：{AlreadyBindID}";
    }
}
public record CheckConfig
{
    /// <summary>
    /// 检测头
    /// </summary>
    public string Head { get; set; }
    /// <summary>
    /// 在线人数
    /// </summary>
    public string PlayList { get; set; }
    /// <summary>
    /// 状态检测
    /// </summary>
    public string ServerCheck { get; set; }
    /// <summary>
    /// 玩家绑定ID
    /// </summary>
    public string Bind { get; set; }
    /// <summary>
    /// 玩家发送消息
    /// </summary>
    public string Send { get; set; }

    public CheckConfig()
    {
        Head = "#";
        PlayList = "在线人数";
        ServerCheck = "服务器状态";
        Bind = "绑定：";
        Send = "服务器：";
    }

    public override string ToString()
    {
        return $"检测头:{Head}{Environment.NewLine}"
           + $"在线玩家获取:{PlayList}{Environment.NewLine}"
           + $"服务器在线检测:{ServerCheck}{Environment.NewLine}"
           + $"玩家设置名字:{Bind}{Environment.NewLine}"
           + $"发送消息至服务器:{Send}";
    }
}
public record AdminConfig
{
    /// <summary>
    /// 禁言玩家
    /// </summary>
    public string Mute { get; set; }
    /// <summary>
    /// 取消禁言
    /// </summary>
    public string UnMute { get; set; }
    /// <summary>
    /// 查询ID
    /// </summary>
    public string CheckBind { get; set; }
    /// <summary>
    /// 重命名玩家
    /// </summary>
    public string Rename { get; set; }
    /// <summary>
    /// 切换服务器维护模式
    /// </summary>
    public string Fix { get; set; }
    /// <summary>
    /// 配置文件重读
    /// </summary>
    public string Reload { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nick { get; set; }
    /// <summary>
    /// 禁止绑定列表
    /// </summary>
    public string GetCantBindList { get; set; }
    /// <summary>
    /// 禁言列表
    /// </summary>
    public string GetMuteList { get; set; }
    /// <summary>
    /// 发送绑定信息QQ号
    /// </summary>
    public long SendQQ { get; set; }

    public AdminConfig()
    {
        Mute = "禁言：";
        UnMute = "解禁：";
        CheckBind = "查询：";
        Rename = "修改：";
        Fix = "切换维护";
        Reload = "重读文件";
        Nick = "昵称：";
        GetCantBindList = "禁止绑定列表";
        GetMuteList = "禁言列表";
        SendQQ = 0;
    }

    public override string ToString()
    {
        return $"禁言:{Mute}{Environment.NewLine}"
            + $"取消禁言:{UnMute}{Environment.NewLine}"
            + $"查询绑定名字:{CheckBind}{Environment.NewLine}"
            + $"重命名:{Rename}{Environment.NewLine}"
            + $"维护模式切换:{Fix}{Environment.NewLine}"
            + $"重读配置:{Reload}{Environment.NewLine}"
            + $"设置昵称:{Nick}{Environment.NewLine}"
            + $"获取禁止绑定列表:{GetCantBindList}{Environment.NewLine}"
            + $"获取禁言列表:{GetMuteList}{Environment.NewLine}"
            + $"发送绑定信息QQ号:{SendQQ}";
    }
}
public record SocketConfig
{ 
    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 检测断开
    /// </summary>
    public bool Check { get; set; }

    public SocketConfig()
    {
        Port = 25555;
        Check = true;
    }

    public override string ToString()
    {
        return $"端口:{Port}{Environment.NewLine}"
            + $"检测断开:{Check}";
    }
}

public record MysqlConfig
{
    /// <summary>
    /// 地址
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 账户
    /// </summary>
    public string User { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// Mysql启用
    /// </summary>
    public bool Enable { get; set; }
    /// <summary>
    /// 数据库名
    /// </summary>
    public string Database { get; set; }

    public MysqlConfig()
    {
        IP = "127.0.0.1";
        Port = 3306;
        User = "root";
        Password = "root";
        Enable = false;
        Database = "minecraft_qq";
    }

    public override string ToString()
    {
        return $"地址:{IP}{Environment.NewLine}"
            + $"端口:{Port}{Environment.NewLine}"
            + $"用户名:{User}{Environment.NewLine}"
            + $"密码:{Password}{Environment.NewLine}"
            + $"是否启用:{Enable}{Environment.NewLine}"
            + $"数据库:{Database}";
    }
}
/// <summary>
/// 玩家数据储存格式
/// </summary>
public record PlayerObj
{
    public string Name { get; set; }
    public string Nick { get; set; }
    public long QQ { get; set; }
    public bool IsAdmin { get; set; }
}
/// <summary>
/// 服务器命令储存格式
/// </summary>
public record CommandObj
{
    public string Command { get; set; }
    public bool PlayerUse { get; set; }
    public bool PlayerSend { get; set; }
    public List<string> Servers { get; set; } = new();
}
/// <summary>
/// 群储存格式
/// </summary>
public record GroupObj
{
    public string Group { get; set; }
    public bool EnableCommand { get; set; }
    public bool EnableSay { get; set; }
    public bool IsMain { get; set; }

    public GroupObj Copy()
    {
        return new()
        {
            Group = Group,
            EnableCommand = EnableCommand,
            EnableSay = EnableSay,
            IsMain = IsMain
        };
    }
}
