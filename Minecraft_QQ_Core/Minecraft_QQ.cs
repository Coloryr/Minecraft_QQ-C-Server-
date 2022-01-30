using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core
{
    public class Minecraft_QQ
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string Path { get; init; } = AppContext.BaseDirectory + "Minecraft_QQ/";
        /// <summary>
        /// Mysql启用
        /// </summary>
        public bool MysqlOK = false;
        /// <summary>
        /// 主群群号
        /// </summary>
        public long GroupSetMain { get; set; } = 0;
        /// <summary>
        /// 主配置文件
        /// </summary>
        public MainConfig MainConfig { get; set; }
        /// <summary>
        /// 玩家储存配置
        /// </summary>
        public PlayerConfig PlayerConfig { get; set; }
        /// <summary>
        /// 群储存配置
        /// </summary>
        public GroupConfig GroupConfig { get; set; }
        /// <summary>
        /// 自动应答储存
        /// </summary>
        public AskConfig AskConfig { get; set; }
        /// <summary>
        /// 自定义指令
        /// </summary>
        public CommandConfig CommandConfig { get; set; }

        /// <summary>
        /// Socket服务器
        /// </summary>
        public readonly MySocketServer Server;
        /// <summary>
        /// Mysql
        /// </summary>
        public readonly MyMysql Mysql;
        /// <summary>
        /// 机器人
        /// </summary>
        public readonly RobotSocket Robot;
        /// <summary>
        /// 发送群消息
        /// </summary>
        public readonly SendGroup SendGroup;

        public Minecraft_QQ()
        {
            Server = new(this);
            Mysql = new(this);
            Robot = new(this);
            SendGroup = new(this);
        }

        /// <summary>
        /// QQ号取玩家
        /// </summary>
        /// <param name="qq">qq号</param>
        /// <returns>玩家信息</returns>
        public PlayerObj GetPlayer(long qq)
        {
            if (PlayerConfig.PlayerList.ContainsKey(qq) == true)
                return PlayerConfig.PlayerList[qq];
            return null;
        }
        /// <summary>
        /// ID取玩家
        /// </summary>
        /// <param name="id">玩家ID</param>
        /// <returns>玩家信息</returns>
        public PlayerObj GetPlayer(string id)
        {
            var valueCol = PlayerConfig.PlayerList.Values;
            foreach (PlayerObj value in valueCol)
            {
                if (value == null || value.Name == null)
                    return null;
                if (value.Name.ToLower() == id.ToLower())
                    return value;
            }
            return null;
        }
        private string SetNick(List<string> msg)
        {
            if (msg.Count != 5)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", "]"), out long qq))
                {
                    return "QQ号获取失败";
                }
                string nick = msg[3].Trim();
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
        public void SetNick(long qq, string nick)
        {
            if (PlayerConfig.PlayerList.ContainsKey(qq) == true)
            {
                PlayerConfig.PlayerList[qq].Nick = nick;
                if (MysqlOK == true)
                    Task.Run(() => Mysql.AddPlayerAsync(PlayerConfig.PlayerList[qq]));
                else
                    ConfigWrite.Player();
            }
            else
            {
                PlayerConfig.PlayerList.Add(qq, new()
                {
                    QQ = qq,
                    Nick = nick
                });
                ConfigWrite.Player();
            }
        }
        private string SetPlayerName(long group, long fromQQ, List<string> msg)
        {
            if (msg.Count != 3)
                return "错误的参数";
            string data = msg[1];
            if (data.IndexOf(MainConfig.Check.Head) == 0)
                data = data.Replace(MainConfig.Check.Head, null);
            if (MainConfig.Setting.CanBind == false)
                return MainConfig.Message.CantBindText;
            var player = GetPlayer(fromQQ);
            if (player == null || string.IsNullOrWhiteSpace(player.Name) == true)
            {
                string player_name = data.Replace(MainConfig.Check.Bind, "");
                string check = player_name.Trim();
                if (string.IsNullOrWhiteSpace(player_name) ||
                    check.StartsWith("id:") || check.StartsWith("id：") ||
                      check.StartsWith("id "))
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();

                    if (PlayerConfig.NotBindList.Contains(player_name.ToLower()) == true)
                        return $"禁止绑定ID：[{player_name}]";
                    else if (GetPlayer(player_name) != null)
                        return $"ID：[{player_name}]已经被绑定过了";
                    if (PlayerConfig.PlayerList.ContainsKey(fromQQ) == true)
                    {
                        player = PlayerConfig.PlayerList[fromQQ];
                        PlayerConfig.PlayerList.Remove(fromQQ);
                    }
                    else
                        player = new PlayerObj();
                    player.Name = player_name;
                    player.QQ = fromQQ;
                    PlayerConfig.PlayerList.Add(fromQQ, player);
                    if (MysqlOK == true)
                        Task.Run(() => Mysql.AddPlayerAsync(player));
                    else
                        ConfigWrite.Player();
                    if (MainConfig.Admin.SendQQ != 0)
                        Robot.SendGroupPrivateMessage(group, MainConfig.Admin.SendQQ, $"玩家[{fromQQ}]绑定了ID：[{player_name}]");
                    IMinecraft_QQ.GuiCall?.Invoke(GuiFun.PlayerList);
                    return $"绑定ID：[{player_name}]成功！";
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
        public void SetPlayerName(long qq, string name)
        {
            var player = GetPlayer(qq);
            if (player == null)
                player = new();
            player.Name = name;
            player.QQ = qq;
            if (PlayerConfig.PlayerList.ContainsKey(qq))
                PlayerConfig.PlayerList[qq] = player;
            else
                PlayerConfig.PlayerList.Add(qq, player);
            if (MysqlOK == true)
                Task.Run(() => Mysql.AddPlayerAsync(player));
            else
                ConfigWrite.Player();
        }
        private string MutePlayer(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            string name;
            if (msg.Count == 4 && msg[2].IndexOf("[mirai:at:") != -1)
            {
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", "]"), out long qq))
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
                name = msg[2].Replace(MainConfig.Admin.Mute, "").Trim();
                name = Funtion.ReplaceFirst(name, MainConfig.Check.Head, "");
            }
            MutePlayer(name);
            return $"已禁言：[{name}]";
        }
        /// <summary>
        /// 禁言玩家
        /// </summary>
        /// <param name="qq">QQ号</param>
        public void MutePlayer(long qq)
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
        public void MutePlayer(string name)
        {
            if (PlayerConfig.MuteList.Contains(name.ToLower()) == false)
                PlayerConfig.MuteList.Add(name.ToLower());
            if (MysqlOK == true)
                Task.Run(() => Mysql.AddMuteAsync(name.ToLower()));
            else
                ConfigWrite.Player();
        }
        private string UnmutePlayer(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            string name;
            if (msg.Count == 4 && msg[2].IndexOf("[mirai:at:") != -1)
            {
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", "]"), out long qq))
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
                name = msg[2].Replace(MainConfig.Admin.Mute, "").Trim();
                name = Funtion.ReplaceFirst(name, MainConfig.Check.Head, "");
            }
            UnmutePlayer(name);
            return $"已解禁：[{name}]";
        }
        /// <summary>
        /// 解除禁言
        /// </summary>
        /// <param name="qq">玩家QQ号</param>
        public void UnmutePlayer(long qq)
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
        public void UnmutePlayer(string name)
        {
            if (PlayerConfig.MuteList.Contains(name.ToLower()) == true)
                PlayerConfig.MuteList.Remove(name.ToLower());
            if (MysqlOK == true)
                Task.Run(() => Mysql.DeleteMuteAsync(name));
            else
                ConfigWrite.Player();
        }
        private string GetPlayerID(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", "]"), out long qq))
                {
                    return "错误的文本";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return $"玩家[{qq}]未绑定ID";
                else
                    return $"玩家[{qq}]绑定的ID为：" + player.Name;
            }
            else if (msg.Count == 3)
            {
                string data = msg[2].Replace(MainConfig.Admin.CheckBind, "");
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
        private string RenamePlayer(List<string> msg)
        {
            if (msg.Count != 5)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", "]"), out long qq))
                {
                    return "错误的文本";
                }
                string name = msg[3].Trim();
                SetPlayerName(qq, name);
                return $"已修改玩家[{qq}]ID为：{name}";
            }
            else
                return "玩家错误，请检查";
        }
        private string GetMuteList()
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
        private string GetCantBind()
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
        public void FixModeChange(bool open)
        {
            MainConfig.Setting.FixMode = open;
        }
        private string FixModeChange()
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
        private string GetOnlinePlayer(long fromGroup)
        {
            if (MainConfig.Setting.FixMode == false)
            {
                if (Server.IsReady() == true)
                {
                    Server.Send(new()
                    {
                        group = fromGroup.ToString(),
                        command = CommderList.ONLINE,
                        player = null
                    });
                    return null;
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return MainConfig.Message.FixText;
        }
        private string GetOnlineServer(long fromGroup)
        {
            if (MainConfig.Setting.FixMode == false)
            {
                if (Server.IsReady() == true)
                {
                    Server.Send(new()
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
        private bool SendCommand(long fromGroup, List<string> msg, long fromQQ)
        {
            foreach (var item in CommandConfig.CommandList)
            {
                string head = msg[1];
                head = Funtion.ReplaceFirst(head, MainConfig.Check.Head, "");
                if (head.StartsWith(item.Key))
                {
                    if (!Server.IsReady())
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>()
                        {
                            $"[mirai:at:{fromQQ}]",
                            "发送失败，服务器未准备好"
                        });
                        return true;
                    }
                    bool haveserver = false;
                    List<string> servers = null;
                    if (item.Value.Servers != null && item.Value.Servers.Count != 0)
                    {
                        servers = new();
                        foreach (var item1 in item.Value.Servers)
                        {
                            if (Server.MCServers.ContainsKey(item1))
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
                        Robot.SendGroupMessage(fromGroup, new List<string>
                        {
                            $"[mirai:at:{fromQQ}]",
                            "发送失败，对应的服务器未连接"
                        });
                    }
                    var player = GetPlayer(fromQQ);
                    if (player == null)
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>
                        {
                             $"[mirai:at:{fromQQ}]",
                            "你未绑定ID"
                        });
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
                        string item1 = msg[2];
                        if (item1.IndexOf("[mirai:at:") != -1)
                        {
                            long qq = long.Parse(Funtion.GetString(item1, "at:", "]"));
                            var player1 = GetPlayer(qq);
                            if (player1 == null)
                            {
                                Robot.SendGroupMessage(fromGroup, new List<string>
                                {
                                    $"[mirai:at:{fromQQ}]",
                                    $"错误，玩家：{qq}没有绑定ID"
                                });
                                return true;
                            }
                            while (cmd.IndexOf("{arg:at}") != -1)
                                cmd = cmd.Replace("{arg:at}", player1.Name);
                            while (cmd.IndexOf("{arg:atqq}") != -1)
                                cmd = cmd.Replace("{arg:atqq}", $"{qq}");
                            haveAt = true;
                        }
                        else
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                $"错误，参数错误"
                            });
                            return true;
                        }
                    }
                    while (cmd.IndexOf("{arg:name}") != -1)
                        cmd = cmd.Replace("{arg:name}", player.Name);
                    while (cmd.IndexOf("{arg:qq}") != -1)
                        cmd = cmd.Replace("{arg:qq}", $"{player.QQ}");
                    string argStr = "";
                    for (int a = haveAt ? 3 : 2; a < msg.Count; a++)
                    {
                        if (!string.IsNullOrWhiteSpace(msg[a]))
                        {
                            argStr = msg[a];
                            break;
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
                        if (pos < arg.Length)
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
                    Server.Send(new TranObj
                    {
                        group = fromGroup.ToString(),
                        command = cmd,
                        isCommand = true,
                        player = item.Value.PlayerSend ? player.Name : CommderList.COMM
                    }, servers);
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 重载配置
        /// </summary>
        public bool Reload()
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
            ConfigFile.PlayerSave = new FileInfo(Path + "Player.json");
            ConfigFile.AskConfig = new FileInfo(Path + "Ask.json");
            ConfigFile.CommandSave = new FileInfo(Path + "Command.json");
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

            ConfigShow.Show(MainConfig);

            //读取群设置
            if (ConfigFile.GroupConfig.Exists == false)
            {
                Logs.LogOut("[Config]新建群设置配置");
                
                GroupConfig = new GroupConfig()
                {
                    Groups = new Dictionary<long, GroupObj>()
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
                Mysql.MysqlStart();
                if (MysqlOK == false)
                {
                    Logs.LogOut("[Mysql]Mysql链接失败");
                    if (ConfigFile.PlayerSave.Exists == false)
                    {
                        PlayerConfig = new();
                        File.WriteAllText(ConfigFile.PlayerSave.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                    }
                    else
                        PlayerConfig = ConfigRead.ReadPlayer();
                }
                else
                {
                    if (PlayerConfig == null)
                        PlayerConfig = new();
                    Mysql.Load();
                    Logs.LogOut("[Mysql]Mysql已连接");
                }
            }
            else
            {
                if (ConfigFile.PlayerSave.Exists == false)
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
                        NotBindList = new()
                        {
                            "Color_yr",
                            "id"
                        },
                        MuteList = new()
                        {
                            "playerid"
                        }
                    };
                    File.WriteAllText(ConfigFile.PlayerSave.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                }
                else
                    PlayerConfig = ConfigRead.ReadPlayer();
            };

            //读取自定义指令
            if (ConfigFile.CommandSave.Exists == false)
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
                File.WriteAllText(ConfigFile.CommandSave.FullName, JsonConvert.SerializeObject(CommandConfig, Formatting.Indented));
            }
            else
                CommandConfig = ConfigRead.ReadCommand();

            return true;
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public async Task Start()
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

            Robot.Start();
            Server.ServerStop();
            Server.StartServer();
            SendGroup.Start();
            IMinecraft_QQ.IsStart = true;

            Robot.SendGroupMessage(GroupSetMain, $"[Minecraft_QQ]已启动[{IMinecraft_QQ.Version}]");
        }

        public void Stop()
        {
            IMinecraft_QQ.IsStart = false;
            Server.ServerStop();
            Mysql.MysqlStop();
            Robot.Stop();
            SendGroup.Stop();
        }
        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public void GroupMessage(long fromGroup, long fromQQ, List<string> msglist)
        {
            if (IMinecraft_QQ.IsStart == false)
                return;
            string msg = msglist[msglist.Count - 1];
            Logs.LogOut($"[{fromGroup}][QQ:{fromQQ}]:{msg}");
            if (GroupConfig.Groups.ContainsKey(fromGroup) == true)
            {
                GroupObj list = GroupConfig.Groups[fromGroup];
                //始终发送
                if (MainConfig.Setting.AutoSend == true && MainConfig.Setting.FixMode == false
                    && Server.IsReady() == true && list.EnableSay == true)
                {
                    string msg_copy = msg;
                    if (MainConfig.Setting.SendCommand || msg_copy.IndexOf(MainConfig.Check.Head) != 0)
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
                                Server.Send(messagelist);
                            }
                        }
                    }
                }
                if (msg.IndexOf(MainConfig.Check.Head) == 0 && list.EnableCommand == true)
                {
                    //去掉检测头
                    msg = Funtion.ReplaceFirst(msg, MainConfig.Check.Head, "");
                    string msg_low = msg.ToLower();
                    PlayerObj player = GetPlayer(fromQQ);
                    if (MainConfig.Setting.AutoSend == false && msg_low.IndexOf(MainConfig.Check.Send) == 0)
                    {
                        if (list.EnableSay == false)
                        {
                            Robot.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                            return;
                        }
                        else if (MainConfig.Setting.FixMode)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string> { $"[mirai:at:{fromQQ}]", MainConfig.Message.FixText });
                            return;
                        }
                        else if (Server.IsReady() == false)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string> { $"[mirai:at:{fromQQ}]", "发送失败，没有服务器链接" });
                            return;
                        }
                        else if (player == null || string.IsNullOrWhiteSpace(player.Name))
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string> { $"[mirai:at:{fromQQ}]", MainConfig.Message.NoneBindID });
                            return;
                        }
                        else if (PlayerConfig.MuteList.Contains(player.Name.ToLower()))
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                "你已被禁言"
                            });
                            return;
                        }
                        try
                        {
                            string msg_copy = msg;
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
                                Server.Send(messagelist);
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
                        if (msg_low.IndexOf(MainConfig.Admin.Mute) == 0)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                MutePlayer(msglist)
                            });
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.Admin.UnMute) == 0)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                UnmutePlayer(msglist)
                            });
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.Admin.CheckBind) == 0)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                GetPlayerID(msglist)
                            });
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.Admin.Rename) == 0)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                RenamePlayer(msglist)
                            });
                            return;
                        }
                        else if (msg_low == MainConfig.Admin.Fix)
                        {
                            Robot.SendGroupMessage(fromGroup, new List<string>
                            {
                                $"[mirai:at:{fromQQ}]",
                                FixModeChange()
                            });
                            return;
                        }
                        else if (msg_low == MainConfig.Admin.GetMuteList)
                        {
                            Robot.SendGroupMessage(fromGroup, GetMuteList());
                            return;
                        }
                        else if (msg_low == MainConfig.Admin.GetCantBindList)
                        {
                            Robot.SendGroupMessage(fromGroup, GetCantBind());
                            return;
                        }
                        else if (msg_low == MainConfig.Admin.Reload)
                        {
                            Robot.SendGroupMessage(fromGroup, "开始重读配置文件");
                            Reload();
                            Robot.SendGroupMessage(fromGroup, "重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.Admin.Nick) == 0)
                        {
                            List<string> lists = new();
                            lists.Add("at:" + fromQQ);
                            lists.Add(SetNick(msglist));
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                    }
                    if (msg_low == MainConfig.Check.PlayList)
                    {
                        string test = GetOnlinePlayer(fromGroup);
                        if (test != null)
                            Robot.SendGroupMessage(fromGroup, test);
                        return;
                    }
                    else if (msg_low == MainConfig.Check.ServerCheck)
                    {
                        string test = GetOnlineServer(fromGroup);
                        if (test != null)
                            Robot.SendGroupMessage(fromGroup, test);
                        return;
                    }

                    else if (msg_low.IndexOf(MainConfig.Check.Bind) == 0)
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>
                        {
                            $"[mirai:at:{fromQQ}]",
                            SetPlayerName(fromGroup, fromQQ, msglist)
                        });
                        return;
                    }
                    else if (SendCommand(fromGroup, msglist, fromQQ) == true)
                        return;

                    else if (MainConfig.Setting.AskEnable && AskConfig.AskList.ContainsKey(msg_low) == true)
                    {
                        string message = AskConfig.AskList[msg_low];
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            Robot.SendGroupMessage(fromGroup, message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(MainConfig.Message.UnknowText) == false)
                    {
                        Robot.SendGroupMessage(fromGroup, MainConfig.Message.UnknowText);
                        return;
                    }
                }
            }
        }
    }
}
