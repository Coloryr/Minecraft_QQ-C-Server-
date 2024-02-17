using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core;

public static class Minecraft_QQ
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public static string Path { get; } = AppContext.BaseDirectory + "Minecraft_QQ/";
    /// <summary>
    /// Mysql启用
    /// </summary>
    public static bool MysqlOK = false;
    /// <summary>
    /// 主群群号
    /// </summary>
    public static long GroupSetMain { get; set; } = 0;
    /// <summary>
    /// 主配置文件
    /// </summary>
    public static MainConfig MainConfig { get; set; }
    /// <summary>
    /// 玩家储存配置
    /// </summary>
    public static PlayerConfig PlayerConfig { get; set; }
    /// <summary>
    /// 群储存配置
    /// </summary>
    public static GroupConfig GroupConfig { get; set; }
    /// <summary>
    /// 自动应答储存
    /// </summary>
    public static AskConfig AskConfig { get; set; }
    /// <summary>
    /// 自定义指令
    /// </summary>
    public static CommandConfig CommandConfig { get; set; }

    /// <summary>
    /// QQ号取玩家
    /// </summary>
    /// <param name="qq">qq号</param>
    /// <returns>玩家信息</returns>
    public static PlayerObj? GetPlayer(long qq)
    {
        if (PlayerConfig.PlayerList.TryGetValue(qq, out var value))
            return value;
        return null;
    }
    /// <summary>
    /// ID取玩家
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns>玩家信息</returns>
    public static PlayerObj? GetPlayer(string id)
    {
        var valueCol = PlayerConfig.PlayerList.Values.Where(a=> 
            a.Name.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        if (valueCol.Any())
            return valueCol.First();
        return null;
    }

    /// <summary>
    /// 设置玩家昵称
    /// </summary>
    /// <param name="qq">qq号</param>
    /// <param name="nick">昵称</param>
    public static void SetNick(long qq, string nick)
    {
        if (PlayerConfig.PlayerList.ContainsKey(qq) == true)
        {
            PlayerConfig.PlayerList[qq].Nick = nick;
        }
        else
        {
            PlayerConfig.PlayerList.Add(qq, new()
            {
                QQ = qq,
                Nick = nick
            });
        }

        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.AddPlayerAsync(PlayerConfig.PlayerList[qq]));
        }
        else
        {
            ConfigWrite.Player();
        }
    }
    
    /// <summary>
    /// 设置玩家ID，如果存在直接修改，不存在创建
    /// </summary>
    /// <param name="qq">qq号</param>
    /// <param name="name">玩家ID</param>
    public static void SetPlayerName(long qq, string name)
    {
        var player = GetPlayer(qq) ?? new();
        player.Name = name;
        player.QQ = qq;
        if (!PlayerConfig.PlayerList.TryAdd(qq, player))
        {
            PlayerConfig.PlayerList[qq] = player;
        }
        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.AddPlayerAsync(player));
        }
        else
        {
            ConfigWrite.Player();
        }
    }

    public static void SetPlayer(PlayerObj player)
    {
        var player1 = GetPlayer(player.QQ) ?? player;
        player1.Name = player.Name;
        player1.QQ = player.QQ;
        player1.Nick = player.Nick;
        player1.IsAdmin = player1.IsAdmin;

        if (!PlayerConfig.PlayerList.TryAdd(player.QQ, player1))
        {
            PlayerConfig.PlayerList[player.QQ] = player1;
        }
        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.AddPlayerAsync(player1));
        }
        else
        {
            ConfigWrite.Player();
        }
    }

    /// <summary>
    /// 禁言玩家
    /// </summary>
    /// <param name="name">名字</param>
    public static void MutePlayer(string name)
    {
        name = name.ToLower();
        if (PlayerConfig.MuteList.Contains(name) == false)
        {
            PlayerConfig.MuteList.Add(name);
        }
        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.AddMuteAsync(name));
        }
        else
        {
            ConfigWrite.Player();
        }
    }

    public static void AddNotBind(string name)
    {
        name = name.ToLower();
        if (PlayerConfig.NotBindList.Contains(name) == false)
        {
            PlayerConfig.NotBindList.Add(name);
        }
        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.AddNotBindAsync(name));
        }
        else
        {
            ConfigWrite.Player();
        }
    }

    public static void RemoveNotBind(string name)
    {
        name = name.ToLower();
        PlayerConfig.NotBindList.Remove(name);
        if (MysqlOK == true)
        {
            Task.Run(() => MyMysql.DeleteNotBindAsync(name));
        }
        else
        {
            ConfigWrite.Player();
        }
    }

    public static void AddGroup(GroupObj obj)
    {
        if (!GroupConfig.Groups.TryAdd(obj.Group, obj))
        {
           GroupConfig.Groups[obj.Group] = obj;
        }

        ConfigWrite.Group();
    }
    
    /// <summary>
    /// 解除禁言
    /// </summary>
    /// <param name="qq">玩家QQ号</param>
    public static void UnmutePlayer(long qq)
    {
        var player = GetPlayer(qq);
        if (player != null && !string.IsNullOrWhiteSpace(player.Name))
        {
            UnmutePlayer(player.Name);
        }
    }
    /// <summary>
    /// 解除禁言
    /// </summary>
    /// <param name="name">玩家ID</param>
    public static void UnmutePlayer(string name)
    {
        if (PlayerConfig.MuteList.Contains(name.ToLower()) == true)
            PlayerConfig.MuteList.Remove(name.ToLower());
        if (MysqlOK == true)
            Task.Run(() => MyMysql.DeleteMuteAsync(name));
        else
            ConfigWrite.Player();
    }
   
    /// <summary>
    /// 设置维护模式状态
    /// </summary>
    /// <param name="open">状态</param>
    public static void FixModeChange(bool open)
    {
        MainConfig.Setting.FixMode = open;
    }
    
    /// <summary>
    /// 重载配置
    /// </summary>
    public static bool Reload()
    {
        if (Directory.Exists(Path) == false)
        {
            Directory.CreateDirectory(Path);
        }
        if (!File.Exists(Path + Logs.log))
        {
            try
            {
                File.WriteAllText(Path + Logs.log, $"正在尝试创建日志文件{Environment.NewLine}");
            }
            catch (Exception e)
            {
                IMinecraft_QQ.ShowMessageCall?.Invoke($"[Minecraft_QQ]日志文件创建失败{Environment.NewLine}{e}");
                return false;
            }
        }

        ConfigFile.MainConfig = new FileInfo(Path + "Mainconfig.json");
        ConfigFile.PlayerConfig = new FileInfo(Path + "Player.json");
        ConfigFile.AskConfig = new FileInfo(Path + "Ask.json");
        ConfigFile.CommandConfig = new FileInfo(Path + "Command.json");
        ConfigFile.GroupConfig = new FileInfo(Path + "Group.json");

        //读取主配置文件
        if (ConfigFile.MainConfig.Exists == false)
        {
            Logs.LogOut("[Config]新建主配置");
            MainConfig = new MainConfig();
            File.WriteAllText(ConfigFile.MainConfig.FullName, JsonConvert.SerializeObject(MainConfig, Formatting.Indented));
        }
        else
            MainConfig = ConfigRead.ReadConfig();

        //读取群设置
        if (ConfigFile.GroupConfig.Exists == false)
        {
            Logs.LogOut("[Config]新建群设置配置");
            
            GroupConfig = new GroupConfig()
            {
                Groups = []
            };

            File.WriteAllText(ConfigFile.GroupConfig.FullName, JsonConvert.SerializeObject(GroupConfig, Formatting.Indented));
        }
        else
            GroupConfig = ConfigRead.ReadGroup();

        //读自动应答消息
        if (ConfigFile.AskConfig.Exists == false)
        {
            AskConfig = new AskConfig
            {
                AskList = new Dictionary<string, string>
                {
                    {
                        "服务器菜单",
                        $"服务器查询菜单：{Environment.NewLine}" +
                        $"【{MainConfig.Check.Head}{MainConfig.Check.Bind} ID】可以绑定你的游戏ID。{Environment.NewLine}" +
                        $"【{MainConfig.Check.Head}{MainConfig.Check.PlayList}】可以查询服务器在线人数。{Environment.NewLine}" +
                        $"【{MainConfig.Check.Head}{MainConfig.Check.ServerCheck}】可以查询服务器是否在运行。{Environment.NewLine}" +
                        $"【{MainConfig.Check.Head}{MainConfig.Check.Send} 内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，）"
                    }
                }
            };
            File.WriteAllText(ConfigFile.AskConfig.FullName, JsonConvert.SerializeObject(AskConfig, Formatting.Indented));
        }
        else
            AskConfig = ConfigRead.ReadAsk();

        //读取玩家数据
        if (MainConfig.Database.Enable == true)
        {
            MyMysql.MysqlStart();
            if (MysqlOK == false)
            {
                Logs.LogOut("[Mysql]Mysql链接失败");
                if (ConfigFile.PlayerConfig.Exists == false)
                {
                    PlayerConfig = new();
                    File.WriteAllText(ConfigFile.PlayerConfig.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                }
                else
                    PlayerConfig = ConfigRead.ReadPlayer();
            }
            else
            {
                if (PlayerConfig == null)
                    PlayerConfig = new();
                MyMysql.Load();
                Logs.LogOut("[Mysql]Mysql已连接");
            }
        }
        else
        {
            if (ConfigFile.PlayerConfig.Exists == false)
            {
                Logs.LogOut("[Config]新建玩家信息储存");
                PlayerConfig = new()
                {
                    PlayerList = new()
                    {
                        {
                            402067010,
                            new()
                            {
                                QQ = 402067010,
                                Name = "Color_yr",
                                Nick = "Color_yr",
                                IsAdmin = true
                            }
                        }
                    },
                    NotBindList =
                    [
                        "Color_yr",
                        "id"
                    ],
                    MuteList =
                    [
                        "playerid"
                    ]
                };
                File.WriteAllText(ConfigFile.PlayerConfig.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
            }
            else
                PlayerConfig = ConfigRead.ReadPlayer();
        };

        //读取自定义指令
        if (ConfigFile.CommandConfig.Exists == false)
        {
            CommandConfig = new()
            {
                CommandList = new()
                {
                    {
                        "插件帮助",
                        new()
                        {
                            Command = "qq help",
                            PlayerUse = false,
                            PlayerSend = false
                        }
                    },
                    {
                        "查钱",
                        new()
                        {
                            Command = "money {arg:name}",
                            PlayerUse = true,
                            PlayerSend = false
                        }
                    },
                    {
                        "禁言",
                        new()
                        {
                            Command = "mute {arg1}",
                            PlayerUse = false,
                            PlayerSend = false
                        }
                    },
                    {
                        "传送",
                        new()
                        {
                            Command = "tpa {arg:at}",
                            PlayerUse = true,
                            PlayerSend = false
                        }
                    },
                    {
                        "给权限",
                        new()
                        {
                            Command = "lp user {arg:at} permission set {arg1} true",
                            PlayerUse = false,
                            PlayerSend = false
                        }
                    },
                    {
                        "说话",
                        new()
                        {
                            Command = "say {argx}",
                            PlayerUse = false,
                            PlayerSend = false
                        }
                    }
                }
            };
            Logs.LogOut("[Config]新建自定义指令");
            File.WriteAllText(ConfigFile.CommandConfig.FullName, JsonConvert.SerializeObject(CommandConfig, Formatting.Indented));
        }
        else
            CommandConfig = ConfigRead.ReadCommand();

        return true;
    }
    /// <summary>
    /// 插件启动
    /// </summary>
    public static async Task Start()
    {
        if (!Reload())
            return;

        await Task.Run(() =>
        {
            while (GroupConfig.Groups.Count == 0 || GroupSetMain == 0)
            {
                IMinecraft_QQ.ShowMessageCall?.Invoke("请设置QQ群，有且最多一个主群");
                IMinecraft_QQ.ConfigInitCall?.Invoke();
                foreach (var item in GroupConfig.Groups)
                {
                    if (item.Value.IsMain == true)
                    {
                        GroupSetMain = item.Key;
                        break;
                    }
                }
            }
        });

        ConfigSave.Init();
        RobotCore.Start();
        PluginServer.ServerStop();
        PluginServer.StartServer();
        SendGroup.Start();
        IMinecraft_QQ.IsStart = true;

        RobotCore.SendGroupMessage(GroupSetMain,
        [
            $"[Minecraft_QQ]已启动[{IMinecraft_QQ.Version}]"
        ]);
    }

    public static void Stop()
    {
        ConfigSave.Stop();
        IMinecraft_QQ.IsStart = false;
        PluginServer.ServerStop();
        MyMysql.MysqlStop();
        RobotCore.Stop();
        SendGroup.Stop();
    }

    public static void AddAsk(string check, string res)
    {
        if (!AskConfig.AskList.TryAdd(check, res))
        {
            AskConfig.AskList[check] = res;
        }

        ConfigWrite.Ask();
    }
}
