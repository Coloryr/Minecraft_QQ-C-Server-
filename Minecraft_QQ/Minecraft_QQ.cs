using Color_yr.Minecraft_QQ.Config;
using Color_yr.Minecraft_QQ.MyMysql;
using Color_yr.Minecraft_QQ.MySocket;
using Color_yr.Minecraft_QQ.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    internal class Minecraft_QQ
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string Path { get; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";
        /// <summary>
        /// Mysql启用
        /// </summary>
        public static bool MysqlOK = false;
        /// <summary>
        /// 主群群号
        /// </summary>
        public static long GroupSetMain { get; set; } = 0;
        /// <summary>
        /// 已经启动
        /// </summary>
        public static bool isStart = false;
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
        public static AskConfig Askconfig { get; set; }
        /// <summary>
        /// 自定义指令
        /// </summary>
        public static CommandConfig Commandconfig { get; set; }

        public static void PrivateMessage(long FromQQ, string message)
        {
            if (MainConfig.设置.自动应答开关 && Askconfig.自动应答列表.ContainsKey(message) == true)
            {
                string message1 = Askconfig.自动应答列表[message];
                if (string.IsNullOrWhiteSpace(message1) != false)
                    return;
                IMinecraft_QQ.SendPrivateMessage(FromQQ, message1);
            }
        }

        /// <summary>
        /// 打开菜单
        /// </summary>
        public static void OpenSettingForm()
        {
            setform frm = new setform();
            frm.ShowDialog();
        }
        public static void reload()
        {
            if (Directory.Exists(Path) == false)
            {
                Directory.CreateDirectory(Path);
            }
            if (!File.Exists(Path + logs.log))
            {
                try
                {
                    File.WriteAllText(Path + logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception)
                {
                    MessageBox.Show("[Minecraft_QQ]日志文件创建失败");
                    return;
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
                logs.LogWrite("[Info][Config]新建主配置");
                MainConfig = new MainConfig();
                File.WriteAllText(ConfigFile.主要配置文件.FullName, JsonConvert.SerializeObject(MainConfig, Formatting.Indented));
            }
            else
                MainConfig = read.ReadConfig();

            ConfigShow.Show(MainConfig);

            //读取群设置
            if (ConfigFile.群设置.Exists == false)
            {
                logs.LogWrite("[Info][Config]新建群设置配置");
                GroupConfig = new GroupConfig();
                File.WriteAllText(ConfigFile.群设置.FullName, JsonConvert.SerializeObject(GroupConfig, Formatting.Indented));
            }
            else
                GroupConfig = read.ReadGroup();
            while (GroupConfig.群列表.Count == 0 || GroupSetMain == 0)
            {
                setform frm = new setform();
                MessageBox.Show("请设置QQ群，有且最多一个主群", "参数错误，请设置");
                frm.ShowDialog();
                GroupConfig = read.ReadGroup();
            }

            //读自动应答消息
            if (ConfigFile.自动应答.Exists == false)
            {
                Askconfig = new AskConfig
                {
                    自动应答列表 = new Dictionary<string, string>
                    {
                        { "服务器菜单", "服务器查询菜单：\r\n【" + MainConfig.检测.检测头 + MainConfig.检测.玩家设置名字
                    + "】可以绑定你的游戏ID。\r\n【" + MainConfig.检测.检测头 + MainConfig.检测.在线玩家获取
                    + "】可以查询服务器在线人数。\r\n【" + MainConfig.检测.检测头 + MainConfig.检测.服务器在线检测
                    + "】可以查询服务器是否在运行。\r\n【" + MainConfig.检测.检测头 + MainConfig.检测.发送消息至服务器
                    + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，）"}
                    }
                };
                File.WriteAllText(ConfigFile.自动应答.FullName, JsonConvert.SerializeObject(Askconfig, Formatting.Indented));
            }
            else
                Askconfig = read.ReadAsk();

            //读取玩家数据
            if (MainConfig.数据库.是否启用 == true)
            {
                if (Mysql.MysqlStart() == false)
                {
                    IMinecraft_QQ.SendGroupMessage(GroupSetMain, "[Minecraft_QQ]Mysql链接失败");
                    MysqlOK = false;
                }
                else
                {
                    Mysql Mysql = new Mysql();
                    if (PlayerConfig == null)
                        PlayerConfig = new PlayerConfig();
                    Mysql.Load();
                    IMinecraft_QQ.SendGroupMessage(GroupSetMain, "[Minecraft_QQ]Mysql已连接");
                    MysqlOK = true;
                }
            }
            else
            {
                if (ConfigFile.玩家储存.Exists == false)
                {
                    PlayerConfig = new PlayerConfig();
                    File.WriteAllText(ConfigFile.玩家储存.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                }
                else
                    PlayerConfig = read.ReadPlayer();
            };

            //读取自定义指令
            if (ConfigFile.自定义指令.Exists == false)
            {
                Commandconfig = new CommandConfig
                {
                    命令列表 = new Dictionary<string, CommandObj>
                    {
                        { "插件帮助", new CommandObj
                            {
                                命令 = "qq help",
                                玩家使用 = false,
                                玩家发送 = false,
                                附带参数 = true
                            }
                        },
                        { "查钱", new CommandObj
                            {
                                命令 = "money %player_name%",
                                玩家使用 = true,
                                玩家发送 = false,
                                附带参数 = false
                            }
                        },
                        { "禁言", new CommandObj
                            {
                                命令 = "mute ",
                                玩家使用 = false,
                                玩家发送 = false,
                                附带参数 = true
                            }
                        },
                        { "传送", new CommandObj
                            {
                                命令 = "tpa %player_at%",
                                玩家使用 = true,
                                玩家发送 = false,
                                附带参数 = false
                            }
                        },
                    }
                };
                File.WriteAllText(ConfigFile.自定义指令.FullName, JsonConvert.SerializeObject(Commandconfig, Formatting.Indented));
            }
            else
                Commandconfig = read.ReadCommand();
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public static void Start()
        {
            reload();

            MySocketServer.ServerStop();
            MySocketServer.StartSocket();
            isStart = true;

            Send.Send_T = new Thread(Send.Send_);
            Send.Send_T.Start();

            IMinecraft_QQ.SendGroupMessage(GroupSetMain, "[Minecraft_QQ]已启动" + IMinecraft_QQ.Version);
        }
        public static void stop()
        {
            isStart = false;
            MySocketServer.ServerStop();
        }
        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public static void GroupMessage(long fromGroup, long fromQQ, string msg)
        {
            if (isStart == false)
                return;
            logs.LogWrite('[' + fromGroup + ']' + "[QQ:" + fromQQ + "]:" + msg);
            if (GroupConfig.群列表.ContainsKey(fromGroup) == true)
            {
                msg = Funtion.CQ_code(msg);
                GroupObj list = GroupConfig.群列表[fromGroup];
                //始终发送
                if (MainConfig.设置.始终发送消息 == true && MainConfig.设置.维护模式 == false
                    && MySocketServer.isready() == true && list.开启对话 == true)
                {
                    PlayerObj player = Funtion.GetPlayer(fromQQ);
                    if (player != null && !PlayerConfig.禁言列表.Contains(player.名字.ToLower())
                        && !string.IsNullOrWhiteSpace(player.名字))
                    {
                        string msg_copy = msg;
                        if (MainConfig.设置.颜色代码开关 == false)
                            msg_copy = Funtion.RemoveColorCodes(msg_copy);
                        if (msg_copy.IndexOf("CQ:rich") != -1)
                            msg_copy = Funtion.Get_rich(msg_copy);
                        if (msg_copy.IndexOf("CQ:sign") != -1)
                            msg_copy = Funtion.Get_sign(msg_copy, player.名字);
                        else if (msg_copy.IndexOf("CQ:") != -1)
                        {
                            msg_copy = Funtion.Remove_pic(msg_copy);
                            msg_copy = Funtion.Get_from_at(msg_copy);
                            msg_copy = Funtion.CQ_code(msg_copy);
                        }
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            MessageObj messagelist = new MessageObj();
                            messagelist.group = fromGroup.ToString();
                            messagelist.message = Funtion.Remove_pic(msg_copy);
                            messagelist.player = !MainConfig.设置.使用昵称发送至服务器 ? 
                                player.名字 : (string.IsNullOrWhiteSpace(player.昵称) ? 
                                player.名字 : player.昵称);
                            messagelist.commder = Commder_list.SPEAK;
                            MySocketServer.Send(messagelist);
                        }
                    }
                }
                if (msg.IndexOf(MainConfig.检测.检测头) == 0 && list.启用命令 == true)
                {
                    string msg_low = Funtion.ReplaceFirst(msg.ToLower(), MainConfig.检测.检测头, "");
                    //去掉检测头
                    msg = Funtion.ReplaceFirst(msg, MainConfig.检测.检测头, "");
                    PlayerObj player = Funtion.GetPlayer(fromQQ);
                    if (MainConfig.设置.始终发送消息 == false && msg_low.IndexOf(MainConfig.检测.发送消息至服务器) == 0)
                    {
                        if (list.开启对话 == false)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                            return;
                        }
                        else if (MainConfig.设置.维护模式)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + MainConfig.消息.维护提示文本);
                            return;
                        }
                        else if (MySocketServer.isready() == false)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + "发送失败，没有服务器链接");
                            return;
                        }
                        else if (player == null || string.IsNullOrWhiteSpace(player.名字))
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ)
                                                + "检测到你没有绑定服务器ID，发送：" + MainConfig.检测.检测头
                                                + MainConfig.检测.玩家设置名字
                                                + "[ID]来绑定，如：" + "\n" + MainConfig.检测.检测头
                                                + MainConfig.检测.玩家设置名字 + " Color_yr");
                            return;
                        }
                        else if (PlayerConfig.禁言列表.Contains(player.名字.ToLower()))
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + "你已被禁言");
                            return;
                        }
                        try
                        {
                            string msg_copy = msg;
                            msg_copy = msg_copy.Replace(MainConfig.检测.发送消息至服务器, "");
                            if (MainConfig.设置.颜色代码开关 == false)
                                msg_copy = Funtion.RemoveColorCodes(msg_copy);
                            if (msg_copy.IndexOf("CQ:") != -1)
                            {
                                msg_copy = Funtion.Remove_pic(msg_copy);
                                msg_copy = Funtion.Get_from_at(msg_copy);
                                msg_copy = Funtion.CQ_code(msg_copy);
                            }
                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                            {
                                MessageObj messagelist = new MessageObj();
                                messagelist.group = "group";
                                messagelist.message = Funtion.Remove_pic(msg_copy);
                                messagelist.player = !MainConfig.设置.使用昵称发送至服务器 ? 
                                    player.名字 : (string.IsNullOrWhiteSpace(player.昵称) ? 
                                    player.名字 : player.昵称);
                                messagelist.commder = Commder_list.SPEAK;
                                MySocketServer.Send(messagelist);
                            }
                            return;
                        }
                        catch (InvalidCastException e1)
                        {
                            logs.LogWrite(e1.Message);
                            return;
                        }
                    }
                    else if (player != null && player.管理员 == true)
                    {
                        if (msg_low.IndexOf(MainConfig.管理员.禁言) == 0)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Mute_player(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.取消禁言) == 0)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Unmute_player(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.查询绑定名字) == 0)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Get_Player_id(fromQQ, msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.重命名) == 0)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Rename_player(msg));
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.维护模式切换)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Fix_mode_change());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁言列表)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Get_mute_list());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁止绑定列表)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Get_cant_bind());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.打开菜单)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                            OpenSettingForm();
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.重读配置)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, "开始重读配置文件");
                            reload();
                            IMinecraft_QQ.SendGroupMessage(fromGroup, "重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.设置昵称) == 0)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.Set_nick(msg));
                            return;
                        }
                    }
                    if (msg_low == MainConfig.检测.在线玩家获取)
                    {
                        string test = Funtion.Get_online_player(fromGroup);
                        if (test != null)
                            IMinecraft_QQ.SendGroupMessage(fromGroup, test);
                        return;
                    }
                    else if (msg_low == MainConfig.检测.服务器在线检测)
                    {
                        string test = Funtion.Get_online_server(fromGroup);
                        if (test != null)
                            IMinecraft_QQ.SendGroupMessage(fromGroup, test);
                        return;
                    }

                    else if (msg_low.IndexOf(MainConfig.检测.玩家设置名字) == 0)
                    {
                        IMinecraft_QQ.SendGroupMessage(fromGroup, IMinecraft_QQ.Code_At(fromQQ) + Funtion.SetPlayerName(fromQQ, msg));
                        return;
                    }
                    else if (Funtion.SendCommand(fromGroup, msg, fromQQ) == true)
                        return;

                    else if (MainConfig.设置.自动应答开关 && Askconfig.自动应答列表.ContainsKey(msg_low) == true)
                    {
                        string message = Askconfig.自动应答列表[msg_low];
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            IMinecraft_QQ.SendGroupMessage(fromGroup, message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(MainConfig.消息.未知指令文本) == false)
                    {
                        IMinecraft_QQ.SendGroupMessage(fromGroup, MainConfig.消息.未知指令文本);
                        return;
                    }
                }
            }
        }
    }
}
