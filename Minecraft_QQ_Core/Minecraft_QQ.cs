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
            if (PlayerConfig.玩家列表.ContainsKey(qq) == true)
                return PlayerConfig.玩家列表[qq];
            return null;
        }
        /// <summary>
        /// ID取玩家
        /// </summary>
        /// <param name="id">玩家ID</param>
        /// <returns>玩家信息</returns>
        public PlayerObj GetPlayer(string id)
        {
            var valueCol = PlayerConfig.玩家列表.Values;
            foreach (PlayerObj value in valueCol)
            {
                if (value == null || value.名字 == null)
                    return null;
                if (value.名字.ToLower() == id.ToLower())
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
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", ","), out long qq))
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
            if (PlayerConfig.玩家列表.ContainsKey(qq) == true)
            {
                PlayerConfig.玩家列表[qq].昵称 = nick;
                if (MysqlOK == true)
                    Task.Run(() => Mysql.AddPlayerAsync(PlayerConfig.玩家列表[qq]));
                else
                    ConfigWrite.Player();
            }
            else
            {
                PlayerConfig.玩家列表.Add(qq, new()
                {
                    QQ号 = qq,
                    昵称 = nick
                });
                ConfigWrite.Player();
            }
        }
        private string SetPlayerName(long group, long fromQQ, List<string> msg)
        {
            if (msg.Count != 3)
                return "错误的参数";
            string data = msg[1];
            if (data.IndexOf(MainConfig.检测.检测头) == 0)
                data = data.Replace(MainConfig.检测.检测头, null);
            if (MainConfig.设置.可以绑定名字 == false)
                return MainConfig.消息.不能绑定文本;
            var player = GetPlayer(fromQQ);
            if (player == null || string.IsNullOrWhiteSpace(player.名字) == true)
            {
                string player_name = data.Replace(MainConfig.检测.玩家设置名字, "");
                string check = player_name.Trim();
                if (string.IsNullOrWhiteSpace(player_name) ||
                    check.StartsWith("id:") || check.StartsWith("id：") ||
                      check.StartsWith("id "))
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();

                    if (PlayerConfig.禁止绑定列表.Contains(player_name.ToLower()) == true)
                        return $"禁止绑定ID：[{player_name}]";
                    else if (GetPlayer(player_name) != null)
                        return $"ID：[{player_name}]已经被绑定过了";
                    if (PlayerConfig.玩家列表.ContainsKey(fromQQ) == true)
                    {
                        player = PlayerConfig.玩家列表[fromQQ];
                        PlayerConfig.玩家列表.Remove(fromQQ);
                    }
                    else
                        player = new PlayerObj();
                    player.名字 = player_name;
                    player.QQ号 = fromQQ;
                    PlayerConfig.玩家列表.Add(fromQQ, player);
                    if (MysqlOK == true)
                        Task.Run(() => Mysql.AddPlayerAsync(player));
                    else
                        ConfigWrite.Player();
                    if (MainConfig.管理员.发送绑定信息QQ号 != 0)
                        Robot.SendGroupPrivateMessage(group, MainConfig.管理员.发送绑定信息QQ号, $"玩家[{fromQQ}]绑定了ID：[{player_name}]");
                    IMinecraft_QQ.GuiCall?.Invoke(GuiFun.PlayerList);
                    return $"绑定ID：[{player_name}]成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
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
            player.名字 = name;
            player.QQ号 = qq;
            PlayerConfig.玩家列表.Add(qq, player);
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
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", ","), out long qq))
                {
                    return "错误的文本";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return $"玩家[{qq}]未绑定ID";
                name = player.名字;
            }
            else
            {
                name = msg[2].Replace(MainConfig.管理员.禁言, "").Trim();
                name = Funtion.ReplaceFirst(name, MainConfig.检测.检测头, "");
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
            if (player != null && !string.IsNullOrWhiteSpace(player.名字))
            {
                MutePlayer(player.名字);
            }
        }
        /// <summary>
        /// 禁言玩家
        /// </summary>
        /// <param name="name">名字</param>
        public void MutePlayer(string name)
        {
            if (PlayerConfig.禁言列表.Contains(name.ToLower()) == false)
                PlayerConfig.禁言列表.Add(name.ToLower());
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
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", ","), out long qq))
                {
                    return "错误的文本";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return $"玩家[{qq}]未绑定ID";
                name = player.名字;
            }
            else
            {
                name = msg[2].Replace(MainConfig.管理员.禁言, "").Trim();
                name = Funtion.ReplaceFirst(name, MainConfig.检测.检测头, "");
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
            if (player != null && !string.IsNullOrWhiteSpace(player.名字))
            {
                UnmutePlayer(player.名字);
            }
        }
        /// <summary>
        /// 解除禁言
        /// </summary>
        /// <param name="name">玩家ID</param>
        public void UnmutePlayer(string name)
        {
            if (PlayerConfig.禁言列表.Contains(name.ToLower()) == true)
                PlayerConfig.禁言列表.Remove(name.ToLower());
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
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", ","), out long qq))
                {
                    return "错误的文本";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return $"玩家[{qq}]未绑定ID";
                else
                    return $"玩家[{qq}]绑定的ID为：" + player.名字;
            }
            else if (msg.Count == 3)
            {
                string data = msg[2].Replace(MainConfig.管理员.查询绑定名字, "");
                if (long.TryParse(data.Remove(0, 1), out long qq) == false)
                {
                    return "无效的QQ号";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return $"玩家[{qq}]未绑定ID";
                else
                    return $"玩家[{qq}]绑定的ID为：" + player.名字;
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
                if (!long.TryParse(Funtion.GetString(msg[2], "at:", ","), out long qq))
                {
                    return "错误的文本";
                }
                string name = msg[3].Trim();
                SetPlayerName(qq, name);
                return "已修改玩家[{qq}]ID为：" + msg[3].Trim();
            }
            else
                return "玩家错误，请检查";
        }
        private string GetMuteList()
        {
            if (PlayerConfig.禁言列表.Count == 0)
                return "没有禁言的玩家";
            else
            {
                string a = "禁言的玩家：";
                foreach (string name in PlayerConfig.禁言列表)
                {
                    a += "\n" + name;
                }
                return a;
            }
        }
        private string GetCantBind()
        {
            if (PlayerConfig.禁止绑定列表.Count == 0)
                return "没有禁止绑定的ID";
            else
            {
                string a = "禁止绑定的ID：";
                foreach (string name in PlayerConfig.禁止绑定列表)
                {
                    a += "\n" + name;
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
            MainConfig.设置.维护模式 = open;
        }
        private string FixModeChange()
        {
            string text;
            if (MainConfig.设置.维护模式 == false)
            {
                MainConfig.设置.维护模式 = true;
                text = "服务器维护模式已开启";
            }
            else
            {
                MainConfig.设置.维护模式 = false;
                text = "服务器维护模式已关闭";
            }
            ConfigWrite.Config();
            Logs.LogOut($"[Minecraft_QQ]{text}");
            return text;
        }
        private string GetOnlinePlayer(long fromGroup)
        {
            if (MainConfig.设置.维护模式 == false)
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
                return MainConfig.消息.维护提示文本;
        }
        private string GetOnlineServer(long fromGroup)
        {
            if (MainConfig.设置.维护模式 == false)
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
                return MainConfig.消息.维护提示文本;
        }
        private bool SendCommand(long fromGroup, string msg, long fromQQ)
        { 
            foreach (var value in CommandConfig.命令列表)
            {
                if (msg.ToLower().IndexOf(value.Key) == 0)
                {
                    if (Server.IsReady() == false)
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>()
                        {
                            $"at:{fromQQ}",
                            "发送失败，服务器未准备好"
                        });
                        return true;
                    }
                    bool haveserver = false;
                    List<string> servers = new();
                    if (value.Value.服务器使用 != null)
                    {
                        foreach (var temp in value.Value.服务器使用)
                        {
                            if (Server.MCServers.ContainsKey(temp))
                            {
                                servers.Add(temp);
                                haveserver = true;
                            }
                        }
                    }
                    else
                    {
                        servers = null;
                        haveserver = true;
                    }
                    if (!haveserver)
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>
                        {
                            "at:" + fromQQ,
                            "发送失败，对应的服务器未连接"
                        });
                    }
                    var player = GetPlayer(fromQQ);
                    if (player != null)
                    {
                        if (value.Value.玩家使用 == true || player.管理员 == true)
                        {
                            var messageSend = new TranObj
                            {
                                group = fromGroup.ToString()
                            };

                            string cmd = value.Value.命令;

                            if (cmd.IndexOf("%player_name%") != -1)
                                cmd = cmd.Replace("%player_name%", player.名字);
                            if (msg.IndexOf("[mirai:at:") != -1 && cmd.IndexOf("%player_at%") != -1)
                            {
                                string a = Funtion.GetString(msg, "=", "]");
                                long.TryParse(a, out long qq);
                                var player1 = GetPlayer(qq);
                                if (player1 == null)
                                {
                                    Robot.SendGroupMessage(fromGroup, new List<string>
                                    {
                                        $"at:{fromQQ}",
                                        $"错误，玩家：{a}没有绑定ID"
                                    });
                                    return true;
                                }
                                cmd = cmd.Replace("%player_at%", player1.名字);
                            }

                            if (value.Value.附带参数 == true)
                            {
                                if (msg.IndexOf("[mirai:at:") != -1 && msg.IndexOf("]") != -1)
                                    messageSend.command = cmd + Funtion.GetString(msg, "]");
                                else
                                    messageSend.command = cmd + Funtion.ReplaceFirst(msg, value.Key, "");
                            }
                            else
                                messageSend.command = cmd;
                            messageSend.isCommand = true;
                            if (value.Value.玩家发送)
                            {
                                messageSend.player = player.名字;
                                if (string.IsNullOrWhiteSpace(player.名字) == true)
                                {
                                    Robot.SendGroupMessage(fromGroup, new List<string>
                                    {
                                        "at:" + fromQQ,
                                        "你未绑定ID"
                                    });
                                    return true;
                                }
                            }
                            else
                                messageSend.player = "后台";
                            Server.Send(messageSend, servers);
                            return true;
                        }
                    }
                    else
                    {
                        Robot.SendGroupMessage(fromGroup, new List<string>
                        {
                            "at:" + fromQQ,
                            "你未绑定ID"
                        });
                        return true;
                    }
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
                    File.WriteAllText(Path + Logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception e)
                {
                    IMinecraft_QQ.ShowMessageCall?.Invoke("[Minecraft_QQ]日志文件创建失败\n" + e.ToString());
                    return false;
                }
            }

            ConfigRead read = new ConfigRead();

            ConfigFile.主要配置文件 = new FileInfo(Path + "Mainconfig.json");
            ConfigFile.玩家储存 = new FileInfo(Path + "Player.json");
            ConfigFile.自动应答 = new FileInfo(Path + "Ask.json");
            ConfigFile.自定义指令 = new FileInfo(Path + "Command.json");
            ConfigFile.群设置 = new FileInfo(Path + "Group.json");

            //读取主配置文件
            if (ConfigFile.主要配置文件.Exists == false)
            {
                Logs.LogOut("[Config]新建主配置");
                MainConfig = new MainConfig();
                File.WriteAllText(ConfigFile.主要配置文件.FullName, JsonConvert.SerializeObject(MainConfig, Formatting.Indented));
            }
            else
                MainConfig = read.ReadConfig();

            ConfigShow.Show(MainConfig);

            //读取群设置
            if (ConfigFile.群设置.Exists == false)
            {
                Logs.LogOut("[Config]新建群设置配置");
                GroupConfig = new GroupConfig()
                {
                    群列表 = new Dictionary<long, GroupObj>
                    {
                        {
                            123456789, new GroupObj
                            {
                                群号 = "123456789",
                                主群 = false,
                                启用命令 = true,
                                开启对话 = true
                            }
                        }
                    }
                };
                File.WriteAllText(ConfigFile.群设置.FullName, JsonConvert.SerializeObject(GroupConfig, Formatting.Indented));
            }
            else
                GroupConfig = ConfigRead.ReadGroup();

            //读自动应答消息
            if (ConfigFile.自动应答.Exists == false)
            {
                AskConfig = new AskConfig
                {
                    自动应答列表 = new Dictionary<string, string>
                    {
                        { 
                            "服务器菜单", 
                            "服务器查询菜单：\r\n" +
                            $"【{MainConfig.检测.检测头}{MainConfig.检测.玩家设置名字}】可以绑定你的游戏ID。\r\n" +
                            $"【{MainConfig.检测.检测头}{MainConfig.检测.在线玩家获取}】可以查询服务器在线人数。\r\n" +
                            $"【{MainConfig.检测.检测头}{MainConfig.检测.服务器在线检测}】可以查询服务器是否在运行。\r\n" +
                            $"【{MainConfig.检测.检测头}{MainConfig.检测.发送消息至服务器} 内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，）"
                        }
                    }
                };
                File.WriteAllText(ConfigFile.自动应答.FullName, JsonConvert.SerializeObject(AskConfig, Formatting.Indented));
            }
            else
                AskConfig = ConfigRead.ReadAsk();

            //读取玩家数据
            if (MainConfig.数据库.是否启用 == true)
            {
                Mysql.MysqlStart();
                if (MysqlOK == false)
                {
                    Logs.LogOut("[Mysql]Mysql链接失败");
                    if (ConfigFile.玩家储存.Exists == false)
                    {
                        PlayerConfig = new();
                        File.WriteAllText(ConfigFile.玩家储存.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
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
                if (ConfigFile.玩家储存.Exists == false)
                {
                    Logs.LogOut("[Config]新建玩家信息储存");
                    PlayerConfig = new()
                    {
                        玩家列表 = new()
                        {
                            {
                                402067010, new()
                                {
                                    QQ号 = 402067010,
                                    名字 = "Color_yr",
                                    昵称 = "Color_yr",
                                    管理员 = true
                                }
                            }
                        },
                        禁止绑定列表 = new() 
                        { 
                            "Color_yr",
                            "id"
                        },
                        禁言列表 = new()
                        {
                            "playerid"
                        }
                    };
                    File.WriteAllText(ConfigFile.玩家储存.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                }
                else
                    PlayerConfig = ConfigRead.ReadPlayer();
            };

            //读取自定义指令
            if (ConfigFile.自定义指令.Exists == false)
            {
                CommandConfig = new()
                {
                    命令列表 = new() 
                    { 
                        { 
                            "插件帮助", new() 
                            { 
                                命令 = "qq help", 
                                玩家使用 = false, 
                                玩家发送 = false, 
                                附带参数 = true 
                            } 
                        }, 
                        { 
                            "查钱", new()
                            { 
                                命令 = "money %player_name%", 
                                玩家使用 = true, 
                                玩家发送 = false, 
                                附带参数 = false 
                            } 
                        }, 
                        { 
                            "禁言", new()
                            { 
                                命令 = "mute ", 
                                玩家使用 = false, 
                                玩家发送 = false, 
                                附带参数 = true 
                            } 
                        }, 
                        { 
                            "传送", new()
                            { 
                                命令 = "tpa %player_at%", 
                                玩家使用 = true, 
                                玩家发送 = false, 
                                附带参数 = false 
                            } 
                        }, 
                    }
                };
                Logs.LogOut("[Config]新建自定义指令");
                File.WriteAllText(ConfigFile.自定义指令.FullName, JsonConvert.SerializeObject(CommandConfig, Formatting.Indented));
            }
            else
                CommandConfig = ConfigRead.ReadCommand();

            if (GroupConfig.群列表.Count == 0 || GroupSetMain == 0)
                IMinecraft_QQ.IsStop = true;

            IMinecraft_QQ.CanGo = true;

            while (GroupConfig.群列表.Count == 0 || GroupSetMain == 0)
            {
                IMinecraft_QQ.ShowMessageCall?.Invoke("请设置QQ群，有且最多一个主群");
                Thread.Sleep(500);
                if (!IMinecraft_QQ.IsStop)
                {
                    return false;
                }
                while (IMinecraft_QQ.Run)
                {
                    Thread.Sleep(500);
                }
                foreach (var item in GroupConfig.群列表)
                {
                    if (item.Value.主群 == true)
                    {
                        GroupSetMain = item.Key;
                        break;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public void Start()
        {
            if (!Reload())
                return;

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
            if (GroupConfig.群列表.ContainsKey(fromGroup) == true)
            {
                GroupObj list = GroupConfig.群列表[fromGroup];
                //始终发送
                if (MainConfig.设置.始终发送消息 == true && MainConfig.设置.维护模式 == false
                    && Server.IsReady() == true && list.开启对话 == true)
                {
                    string msg_copy = msg;
                    if (!MainConfig.设置.不发送指令到服务器 || msg_copy.IndexOf(MainConfig.检测.检测头) != 0)
                    {
                        PlayerObj player = GetPlayer(fromQQ);
                        if (player != null && !PlayerConfig.禁言列表.Contains(player.名字.ToLower())
                            && !string.IsNullOrWhiteSpace(player.名字))
                        {
                            msg_copy = Funtion.GetRich(msg_copy);
                            if (MainConfig.设置.颜色代码开关 == false)
                                msg_copy = Funtion.RemoveColorCodes(msg_copy);
                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                            {
                                var messagelist = new TranObj()
                                {
                                    group = fromGroup.ToString(),
                                    message = msg_copy,
                                    player = !MainConfig.设置.使用昵称发送至服务器 ?
                                    player.名字 : string.IsNullOrWhiteSpace(player.昵称) ?
                                    player.名字 : player.昵称,
                                    command = CommderList.SPEAK
                                };
                                Server.Send(messagelist);
                            }
                        }
                    }
                }
                if (msg.IndexOf(MainConfig.检测.检测头) == 0 && list.启用命令 == true)
                {
                    //去掉检测头
                    msg = Funtion.ReplaceFirst(msg, MainConfig.检测.检测头, "");
                    string msg_low = msg.ToLower();
                    PlayerObj player = GetPlayer(fromQQ);
                    if (MainConfig.设置.始终发送消息 == false && msg_low.IndexOf(MainConfig.检测.发送消息至服务器) == 0)
                    {
                        if (list.开启对话 == false)
                        {
                            Robot.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                            return;
                        }
                        else if (MainConfig.设置.维护模式)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                MainConfig.消息.维护提示文本
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (Server.IsReady() == false)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                "发送失败，没有服务器链接"
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (player == null || string.IsNullOrWhiteSpace(player.名字))
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                $"你没有绑定服务器ID，发送：{MainConfig.检测.检测头}{MainConfig.检测.玩家设置名字} [ID]来绑定，如：",
                                $"{MainConfig.检测.检测头}{MainConfig.检测.玩家设置名字} Color_yr"
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (PlayerConfig.禁言列表.Contains(player.名字.ToLower()))
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                "你已被禁言"
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        try
                        {
                            string msg_copy = msg;
                            msg_copy = msg_copy.Replace(MainConfig.检测.发送消息至服务器, "");
                            if (MainConfig.设置.颜色代码开关 == false)
                                msg_copy = Funtion.RemoveColorCodes(msg_copy);
                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                            {
                                var messagelist = new TranObj()
                                {
                                    group = DataType.group,
                                    message = msg_copy,
                                    player = !MainConfig.设置.使用昵称发送至服务器 ?
                                    player.名字 : string.IsNullOrWhiteSpace(player.昵称) ?
                                    player.名字 : player.昵称,
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
                    else if (player != null && player.管理员 == true)
                    {
                        if (msg_low.IndexOf(MainConfig.管理员.禁言) == 0)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                MutePlayer(msglist)
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.取消禁言) == 0)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                UnmutePlayer(msglist)
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.查询绑定名字) == 0)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                GetPlayerID(msglist)
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.重命名) == 0)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                RenamePlayer(msglist)
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.维护模式切换)
                        {
                            List<string> lists = new List<string>
                            {
                                $"at:{fromQQ}",
                                FixModeChange()
                            };
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁言列表)
                        {
                            Robot.SendGroupMessage(fromGroup, GetMuteList());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁止绑定列表)
                        {
                            Robot.SendGroupMessage(fromGroup, GetCantBind());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.重读配置)
                        {
                            Robot.SendGroupMessage(fromGroup, "开始重读配置文件");
                            Reload();
                            Robot.SendGroupMessage(fromGroup, "重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.设置昵称) == 0)
                        {
                            List<string> lists = new List<string>();
                            lists.Add("at:" + fromQQ);
                            lists.Add(SetNick(msglist));
                            Robot.SendGroupMessage(fromGroup, lists);
                            return;
                        }
                    }
                    if (msg_low == MainConfig.检测.在线玩家获取)
                    {
                        string test = GetOnlinePlayer(fromGroup);
                        if (test != null)
                            Robot.SendGroupMessage(fromGroup, test);
                        return;
                    }
                    else if (msg_low == MainConfig.检测.服务器在线检测)
                    {
                        string test = GetOnlineServer(fromGroup);
                        if (test != null)
                            Robot.SendGroupMessage(fromGroup, test);
                        return;
                    }

                    else if (msg_low.IndexOf(MainConfig.检测.玩家设置名字) == 0)
                    {
                        List<string> lists = new List<string>
                        {
                            $"at:{fromQQ}",
                            SetPlayerName(fromGroup, fromQQ, msglist)
                        };
                        Robot.SendGroupMessage(fromGroup, lists);
                        return;
                    }
                    else if (SendCommand(fromGroup, msg, fromQQ) == true)
                        return;

                    else if (MainConfig.设置.自动应答开关 && AskConfig.自动应答列表.ContainsKey(msg_low) == true)
                    {
                        string message = AskConfig.自动应答列表[msg_low];
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            Robot.SendGroupMessage(fromGroup, message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(MainConfig.消息.未知指令文本) == false)
                    {
                        Robot.SendGroupMessage(fromGroup, MainConfig.消息.未知指令文本);
                        return;
                    }
                }
            }
        }
    }
}
