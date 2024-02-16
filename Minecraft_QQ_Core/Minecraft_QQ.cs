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
    public static PlayerObj GetPlayer(long qq)
    {
        if (PlayerConfig.PlayerList.TryGetValue(qq, out PlayerObj value))
            return value;
        return null;
    }
    /// <summary>
    /// ID取玩家
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns>玩家信息</returns>
    public static PlayerObj GetPlayer(string id)
    {
        var valueCol = PlayerConfig.PlayerList.Values.Where(a=> 
            a.Name.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        if (valueCol.Any())
            return valueCol.First();
        return null;
    }
    private static string SetNick(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 3)
            return "错误的参数";
        if (msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "QQ号获取失败";
            }
            string nick = msg[2].data.text.Trim();
            SetNick(qq, nick);
            return $"已修改玩家[{qq}]的昵称为：{nick}";
        }
        else
            return "找不到玩家";
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
            Task.Run(() => MyMysql.AddPlayerAsync(PlayerConfig.PlayerList[qq]));
        else
            ConfigWrite.Player();
    }
    private static string SetPlayerName(long group, long fromQQ, List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 1)
            return "错误的参数";
        string data = msg[1].data.text;
        if (data.StartsWith(MainConfig.Check.Head))
            data = data.Replace(MainConfig.Check.Head, null);
        if (MainConfig.Setting.CanBind == false)
            return MainConfig.Message.CantBindText;
        var player = GetPlayer(fromQQ);
        if (player == null || string.IsNullOrWhiteSpace(player.Name) == true)
        {
            string name = data.Replace(MainConfig.Check.Bind, "");
            string check = name.Trim();
            if (string.IsNullOrWhiteSpace(name) ||
                check.StartsWith("id:") || check.StartsWith("id：") ||
                  check.StartsWith("id "))
                return "ID无效，请检查";
            else
            {
                name = name.Trim();

                if (PlayerConfig.NotBindList.Contains(name.ToLower()) == true)
                    return $"禁止绑定ID：[{name}]";
                if (PlayerConfig.PlayerList.ContainsKey(fromQQ) == true)
                {
                    player = PlayerConfig.PlayerList[fromQQ];
                    PlayerConfig.PlayerList.Remove(fromQQ);
                }
                else
                    player = new PlayerObj();
                player.Name = name;
                player.QQ = fromQQ;
                PlayerConfig.PlayerList.Add(fromQQ, player);
                if (MysqlOK == true)
                    Task.Run(() => MyMysql.AddPlayerAsync(player));
                else
                    ConfigWrite.Player();
                if (MainConfig.Setting.SendQQ != 0)
                    RobotCore.SendGroupTempMessage(MainConfig.RobotSetting.QQ, group, MainConfig.Setting.SendQQ,
                    [
                        $"玩家[{fromQQ}]绑定了ID：[{name}]"
                    ]);
                IMinecraft_QQ.GuiCall?.Invoke(GuiFun.PlayerList);
                return $"绑定ID：[{name}]成功！";
            }
        }
        else
            return MainConfig.Message.AlreadyBindID;
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
            PlayerConfig.PlayerList[qq] = player;
        if (MysqlOK == true)
            Task.Run(() => MyMysql.AddPlayerAsync(player));
        else
            ConfigWrite.Player();
    }
    private static string MutePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
            return "错误的参数";
        string name;
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            var player = GetPlayer(qq);
            if (player == null)
                return $"玩家[{qq}]未绑定ID";
            name = player.Name;
        }
        else
        {
            name = msg[1].data.text.Trim();
            name = Funtion.ReplaceFirst(name, MainConfig.Check.Head, "");
        }
        MutePlayer(name);
        return $"已禁言：[{name}]";
    }
    /// <summary>
    /// 禁言玩家
    /// </summary>
    /// <param name="qq">QQ号</param>
    public static void MutePlayer(long qq)
    {
        var player = GetPlayer(qq);
        if (player != null && !string.IsNullOrWhiteSpace(player.Name))
        {
            MutePlayer(player.Name);
        }
    }
    /// <summary>
    /// 禁言玩家
    /// </summary>
    /// <param name="name">名字</param>
    public static void MutePlayer(string name)
    {
        if (PlayerConfig.MuteList.Contains(name.ToLower()) == false)
            PlayerConfig.MuteList.Add(name.ToLower());
        if (MysqlOK == true)
            Task.Run(() => MyMysql.AddMuteAsync(name.ToLower()));
        else
            ConfigWrite.Player();
    }
    private static string UnmutePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
            return "错误的参数";
        string name;
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            var player = GetPlayer(qq);
            if (player == null)
                return $"玩家[{qq}]未绑定ID";
            name = player.Name;
        }
        else
        {
            name = msg[1].data.text.Trim();
            name = Funtion.ReplaceFirst(name, MainConfig.Check.Head, "");
        }
        UnmutePlayer(name);
        return $"已解禁：[{name}]";
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
    private static string GetPlayerID(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
            return "错误的参数";
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            var player = GetPlayer(qq);
            if (player == null)
                return $"玩家[{qq}]未绑定ID";
            else
                return $"玩家[{qq}]绑定的ID为：" + player.Name;
        }
        else if (msg.Count == 2)
        {
            string data = msg[1].data.text.Replace(MainConfig.Admin.CheckBind, "");
            if (long.TryParse(data.Remove(0, 1), out long qq) == false)
            {
                return "无效的QQ号";
            }
            var player = GetPlayer(qq);
            if (player == null)
                return $"玩家[{qq}]未绑定ID";
            else
                return $"玩家[{qq}]绑定的ID为：" + player.Name;
        }
        else
        {
            return "你需要@一个人或者输入它的QQ号来查询";
        }
    }
    private static string RenamePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 4)
            return "错误的参数";
        if (msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            string name = msg[2].data.text.Trim();
            SetPlayerName(qq, name);
            return $"已修改玩家[{qq}]ID为：{name}";
        }
        else
            return "玩家错误，请检查";
    }
    private static string GetMuteList()
    {
        if (PlayerConfig.MuteList.Count == 0)
            return "没有禁言的玩家";
        else
        {
            string a = "禁言的玩家：";
            foreach (string name in PlayerConfig.MuteList)
            {
                a += Environment.NewLine + name;
            }
            return a;
        }
    }
    private static string GetCantBind()
    {
        if (PlayerConfig.NotBindList.Count == 0)
            return "没有禁止绑定的ID";
        else
        {
            string a = "禁止绑定的ID：";
            foreach (string name in PlayerConfig.NotBindList)
            {
                a += Environment.NewLine + name;
            }
            return a;
        }
    }
    /// <summary>
    /// 设置维护模式状态
    /// </summary>
    /// <param name="open">状态</param>
    public static void FixModeChange(bool open)
    {
        MainConfig.Setting.FixMode = open;
    }
    private static string FixModeChange()
    {
        string text;
        if (MainConfig.Setting.FixMode == false)
        {
            MainConfig.Setting.FixMode = true;
            text = "服务器维护模式已开启";
        }
        else
        {
            MainConfig.Setting.FixMode = false;
            text = "服务器维护模式已关闭";
        }
        ConfigWrite.Config();
        Logs.LogOut($"[Minecraft_QQ]{text}");
        return text;
    }
    private static string GetOnlinePlayer(long fromGroup)
    {
        if (MainConfig.Setting.FixMode == false)
        {
            if (PluginServer.IsReady() == true)
            {
                PluginServer.Send(new()
                {
                    group = fromGroup.ToString(),
                    command = CommderList.ONLINE
                });
                return null;
            }
            else
                return "发送失败，服务器未准备好";
        }
        else
            return MainConfig.Message.FixText;
    }
    private static string GetOnlineServer(long fromGroup)
    {
        if (MainConfig.Setting.FixMode == false)
        {
            if (PluginServer.IsReady() == true)
            {
                PluginServer.Send(new()
                {
                    group = fromGroup.ToString(),
                    command = CommderList.SERVER,
                });
                return null;
            }
            else
                return "发送失败，服务器未准备好";
        }
        else
            return MainConfig.Message.FixText;
    }
    private static bool SendCommand(long fromGroup, List<GroupMessagePack.Message> msg, long fromQQ)
    {
        foreach (var item in CommandConfig.CommandList)
        {
            string head = msg[0].data.text;
            head = Funtion.ReplaceFirst(head, MainConfig.Check.Head, "");
            if (!head.StartsWith(item.Key))
            {
                continue;
            }
            if (!PluginServer.IsReady())
            {
                RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                [
                    $"[mirai:at:{fromQQ}]",
                    "发送失败，服务器未准备好"
                ]);
                return true;
            }
            bool haveserver = false;
            List<string> servers = null;
            if (item.Value.Servers != null && item.Value.Servers.Count != 0)
            {
                servers = [];
                foreach (var item1 in item.Value.Servers)
                {
                    if (PluginServer.MCServers.ContainsKey(item1))
                    {
                        servers.Add(item1);
                        haveserver = true;
                    }
                }
            }
            else
            {
                haveserver = true;
            }
            if (!haveserver)
            {
                RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                [
                    $"[mirai:at:{fromQQ}]",
                    "发送失败，对应的服务器未连接"
                ]);
                return true;
            }
            var player = GetPlayer(fromQQ);
            if (player == null)
            {
                RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                    [
                         $"[mirai:at:{fromQQ}]",
                        "你未绑定ID"
                    ]);
                return true;
            }
            if (!item.Value.PlayerUse && !player.IsAdmin)
            {
                return true;
            }
            string cmd = item.Value.Command;
            bool haveAt = false;
            if (cmd.Contains("{arg:at}") || cmd.Contains("{arg:atqq}"))
            {
                if (msg[1].type == "at")
                {
                    long qq = long.Parse(msg[1].data.qq);
                    var player1 = GetPlayer(qq);
                    if (player1 == null)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                            [
                                $"[mirai:at:{fromQQ}]",
                                $"错误，玩家：{qq}没有绑定ID"
                            ]);
                        return true;
                    }
                    while (cmd.Contains("{arg:at}", StringComparison.CurrentCulture))
                        cmd = cmd.Replace("{arg:at}", player1.Name);
                    while (cmd.Contains("{arg:atqq}", StringComparison.CurrentCulture))
                        cmd = cmd.Replace("{arg:atqq}", $"{qq}");
                    haveAt = true;
                }
                else
                {
                    RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            $"错误，参数错误"
                        ]);
                    return true;
                }
            }
            while (cmd.Contains("{arg:name}", StringComparison.CurrentCulture))
                cmd = cmd.Replace("{arg:name}", player.Name);
            while (cmd.Contains("{arg:qq}", StringComparison.CurrentCulture))
                cmd = cmd.Replace("{arg:qq}", $"{player.QQ}");
            string argStr = "";
            for (int a = haveAt ? 3 : 2; a < msg.Count; a++)
            {
                if (!string.IsNullOrEmpty(msg[a].data.text))
                {
                    argStr += msg[a];
                }
            }
            var arg = argStr.Split(" ");
            int pos = 1;
            for (int index = 1; ; index++)
            {
                string args = "{arg" + index + "}";
                if (index > arg.Length)
                {
                    break;
                }
                if (cmd.Contains(args))
                {
                    for (; pos < arg.Length; pos++)
                    {
                        if (!string.IsNullOrWhiteSpace(arg[pos]))
                        {
                            cmd = cmd.Replace(args, arg[pos]);
                            break;
                        }
                    }
                }
                else
                    break;
            }
            if (cmd.Contains("{argx}"))
            {
                string temp = "";
                if (pos <= arg.Length)
                {
                    for (; pos < arg.Length; pos++)
                    {
                        if (!string.IsNullOrWhiteSpace(arg[pos]))
                        {
                            temp += $"{arg[pos]} ";
                        }
                    }
                    if (temp.Length > 1)
                    {
                        temp = temp[0..^1];
                    }
                }
                cmd = cmd.Replace("{argx}", temp);
            }
            PluginServer.Send(new TranObj
            {
                group = fromGroup.ToString(),
                command = cmd,
                isCommand = true,
                player = item.Value.PlayerSend ? player.Name : CommderList.COMM
            }, servers);
            return true;
        }
        return false;
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

        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, GroupSetMain,
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
    /// <summary>
    /// Type=2 群消息。
    /// </summary>
    /// <param name="fromGroup">来源群号。</param>
    /// <param name="fromQQ">来源QQ。</param>
    /// <param name="msg">消息内容。</param>
    public static void GroupMessage(long fromGroup, long fromQQ, string raw, List<GroupMessagePack.Message> msglist)
    {
        if (IMinecraft_QQ.IsStart == false)
            return;
        Logs.LogOut($"[{fromGroup}][QQ:{fromQQ}]:{raw}");
        if (GroupConfig.Groups.ContainsKey(fromGroup) == true)
        {
            GroupObj list = GroupConfig.Groups[fromGroup];
            //始终发送
            if (MainConfig.Setting.AutoSend == true && MainConfig.Setting.FixMode == false
                && PluginServer.IsReady() == true && list.EnableSay == true)
            {
                string msg_copy = raw;
                if (MainConfig.Setting.SendCommand || !msg_copy.StartsWith(MainConfig.Check.Head))
                {
                    PlayerObj player = GetPlayer(fromQQ);
                    if (player != null && !PlayerConfig.MuteList.Contains(player.Name.ToLower())
                        && !string.IsNullOrWhiteSpace(player.Name))
                    {
                        msg_copy = Funtion.GetRich(msg_copy);
                        if (MainConfig.Setting.ColorEnable == false)
                            msg_copy = Funtion.RemoveColorCodes(msg_copy);
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            var messagelist = new TranObj()
                            {
                                group = fromGroup.ToString(),
                                message = msg_copy,
                                player = !MainConfig.Setting.SendNickServer ?
                                player.Name : string.IsNullOrWhiteSpace(player.Nick) ?
                                player.Name : player.Nick,
                                command = CommderList.SPEAK
                            };
                            PluginServer.Send(messagelist);
                        }
                    }
                }
            }
            if (raw.StartsWith(MainConfig.Check.Head) && list.EnableCommand == true)
            {
                //去掉检测头
                raw = Funtion.ReplaceFirst(raw, MainConfig.Check.Head, "");
                string msg_low = raw.ToLower();
                PlayerObj player = GetPlayer(fromQQ);
                if (MainConfig.Setting.AutoSend == false && msg_low.StartsWith(MainConfig.Check.Send))
                {
                    if (list.EnableSay == false)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, ["该群没有开启聊天功能"]);
                        return;
                    }
                    else if (MainConfig.Setting.FixMode)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [$"[mirai:at:{fromQQ}]", MainConfig.Message.FixText]);
                        return;
                    }
                    else if (PluginServer.IsReady() == false)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [$"[mirai:at:{fromQQ}]", "发送失败，没有服务器链接"]);
                        return;
                    }
                    else if (player == null || string.IsNullOrWhiteSpace(player.Name))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [$"[mirai:at:{fromQQ}]", MainConfig.Message.NoneBindID]);
                        return;
                    }
                    else if (PlayerConfig.MuteList.Contains(player.Name.ToLower()))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            "你已被禁言"
                        ]);
                        return;
                    }
                    try
                    {
                        string msg_copy = raw;
                        msg_copy = msg_copy.Replace(MainConfig.Check.Send, "");
                        if (MainConfig.Setting.ColorEnable == false)
                            msg_copy = Funtion.RemoveColorCodes(msg_copy);
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            var messagelist = new TranObj()
                            {
                                group = DataType.group,
                                message = msg_copy,
                                player = !MainConfig.Setting.SendNickServer ?
                                player.Name : string.IsNullOrWhiteSpace(player.Nick) ?
                                player.Name : player.Nick,
                                command = CommderList.SPEAK
                            };
                            PluginServer.Send(messagelist);
                        }
                        return;
                    }
                    catch (Exception e)
                    {
                        Logs.LogError(e);
                        return;
                    }
                }
                else if (player != null && player.IsAdmin == true)
                {
                    if (msg_low.StartsWith(MainConfig.Admin.Mute))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            MutePlayer(msglist)
                        ]);
                        return;
                    }
                    else if (msg_low.StartsWith(MainConfig.Admin.UnMute))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            UnmutePlayer(msglist)
                        ]);
                        return;
                    }
                    else if (msg_low.StartsWith(MainConfig.Admin.CheckBind))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            GetPlayerID(msglist)
                        ]);
                        return;
                    }
                    else if (msg_low.StartsWith(MainConfig.Admin.Rename))
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            RenamePlayer(msglist)
                        ]);
                        return;
                    }
                    else if (msg_low == MainConfig.Admin.Fix)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            FixModeChange()
                        ]);
                        return;
                    }
                    else if (msg_low == MainConfig.Admin.GetMuteList)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [GetMuteList()]);
                        return;
                    }
                    else if (msg_low == MainConfig.Admin.GetCantBindList)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [GetCantBind()]);
                        return;
                    }
                    else if (msg_low == MainConfig.Admin.Reload)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, ["开始重读配置文件"]);
                        Reload();
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, ["重读完成"]);
                        return;
                    }
                    else if (msg_low.StartsWith(MainConfig.Admin.Nick))
                    {
                        List<string> lists = ["at:" + fromQQ, SetNick(msglist)];
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, lists);
                        return;
                    }
                }
                if (msg_low == MainConfig.Check.PlayList)
                {
                    string test = GetOnlinePlayer(fromGroup);
                    if (test != null)
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [test]);
                    return;
                }
                else if (msg_low == MainConfig.Check.ServerCheck)
                {
                    string test = GetOnlineServer(fromGroup);
                    if (test != null)
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [test]);
                    return;
                }

                else if (msg_low.StartsWith(MainConfig.Check.Bind))
                {
                    RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup,
                    [
                        $"[mirai:at:{fromQQ}]",
                        SetPlayerName(fromGroup, fromQQ, msglist)
                    ]);
                    return;
                }
                else if (SendCommand(fromGroup, msglist, fromQQ) == true)
                    return;

                else if (MainConfig.Setting.AskEnable && AskConfig.AskList.ContainsKey(msg_low) == true)
                {
                    string message = AskConfig.AskList[msg_low];
                    if (string.IsNullOrWhiteSpace(message) == false)
                    {
                        RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [message]);
                        return;
                    }
                }
                else if (string.IsNullOrWhiteSpace(MainConfig.Message.UnknowText) == false)
                {
                    RobotCore.SendGroupMessage(MainConfig.RobotSetting.QQ, fromGroup, [MainConfig.Message.UnknowText]);
                    return;
                }
            }
        }
    }
}
