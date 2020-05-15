using Minecraft_QQ.Config;
using Minecraft_QQ.MyMysql;
using Minecraft_QQ.MySocket;
using Minecraft_QQ.Utils;
using Minecraft_QQ.SetWindow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using Minecraft_QQ.Properties;

namespace Minecraft_QQ
{
    internal class Minecraft_QQ
    {
        public static NotifyIcon notifyIcon;
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
        public static bool IsStart = false;
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

        public static Window1 SetWindow { get; private set; }

        public static void CloseSetWindow()
        {
            Task.Factory.StartNew(() => SetWindow = null);
        }

        /// <summary>
        /// 接受私聊消息
        /// </summary>
        /// <param name="FromQQ">QQ号</param>
        /// <param name="message">消息</param>
        public static void PrivateMessage(long FromQQ, string message)
        {
            if (MainConfig.设置.自动应答开关 && AskConfig.自动应答列表.ContainsKey(message) == true)
            {
                string message1 = AskConfig.自动应答列表[message];
                if (string.IsNullOrWhiteSpace(message1) != false)
                    return;
                IMinecraft_QQ.SPrivateMessage(FromQQ, message1);
            }
        }

        /// <summary>
        /// 打开菜单
        /// </summary>
        public static void OpenSettingForm()
        {
            Thread th = new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    if (SetWindow == null)
                    {
                        SetWindow = new Window1();
                        SetWindow.ShowDialog();
                    }
                    else
                    {
                        SetWindow.Activate();
                    }
                }
                catch (Exception e)
                {
                    Logs.LogError(e);
                    MessageBox.Show("新版UI发生错误，使用旧版UI");
                    setform frm = new setform();
                    frm.ShowDialog();
                }
                finally
                {
                    if (SetWindow != null)
                        SetWindow.Close();
                    SetWindow = null;
                }
            }));
            th.TrySetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
        }

        /// <summary>
        /// 重载配置
        /// </summary>
        public static async void Reload()
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
                catch
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
                Logs.LogWrite("[Info][Config]新建主配置");
                MainConfig = new MainConfig();
                File.WriteAllText(ConfigFile.主要配置文件.FullName, JsonConvert.SerializeObject(MainConfig, Formatting.Indented));
            }
            else
                MainConfig = read.ReadConfig();

            ConfigShow.Show(MainConfig);

            //读取群设置
            if (ConfigFile.群设置.Exists == false)
            {
                Logs.LogWrite("[Info][Config]新建群设置配置");
                GroupConfig = new GroupConfig();
                File.WriteAllText(ConfigFile.群设置.FullName, JsonConvert.SerializeObject(GroupConfig, Formatting.Indented));
            }
            else
                GroupConfig = read.ReadGroup();

            //读自动应答消息
            if (ConfigFile.自动应答.Exists == false)
            {
                AskConfig = new AskConfig
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
                File.WriteAllText(ConfigFile.自动应答.FullName, JsonConvert.SerializeObject(AskConfig, Formatting.Indented));
            }
            else
                AskConfig = read.ReadAsk();

            //读取玩家数据
            if (MainConfig.数据库.是否启用 == true)
            {
                await Mysql.MysqlStartAsync();
                if (MysqlOK == false)
                {
                    IMinecraft_QQ.SGroupMessage(GroupSetMain, "[Minecraft_QQ]Mysql链接失败");
                    if (ConfigFile.玩家储存.Exists == false)
                    {
                        PlayerConfig = new PlayerConfig();
                        File.WriteAllText(ConfigFile.玩家储存.FullName, JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented));
                    }
                    else
                        PlayerConfig = read.ReadPlayer();
                }
                else
                {
                    Mysql Mysql = new Mysql();
                    if (PlayerConfig == null)
                        PlayerConfig = new PlayerConfig();
                    Mysql.Load();
                    IMinecraft_QQ.SGroupMessage(GroupSetMain, "[Minecraft_QQ]Mysql已连接");
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
                CommandConfig = new CommandConfig
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
                File.WriteAllText(ConfigFile.自定义指令.FullName, JsonConvert.SerializeObject(CommandConfig, Formatting.Indented));
            }
            else
                CommandConfig = read.ReadCommand();

            while (GroupConfig.群列表.Count == 0 || GroupSetMain == 0)
            {
                MessageBox.Show("请设置QQ群，有且最多一个主群", "参数错误，请设置");
                OpenSettingForm();
                GroupConfig = read.ReadGroup();
            }
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public static void Start()
        {
            Reload();

            MySocketServer.ServerStop();
            MySocketServer.StartServer();
            IsStart = true;

            Send.SendT = new Thread(Send.SendToGroup);
            Send.SendT.Start();

            try
            {
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = Resource1.icon;
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipText = "已启动";
                notifyIcon.ShowBalloonTip(1000);
            }
            catch
            {
                MessageBox.Show("错误");
            }
            
            IMinecraft_QQ.SGroupMessage(GroupSetMain, "[Minecraft_QQ]已启动" + IMinecraft_QQ.Version);
            OpenSettingForm();

        }
        public static void Stop()
        {
            IsStart = false;
            if (SetWindow != null)
            {
                SetWindow.Close();
                CloseSetWindow();
            }
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
            if (IsStart == false)
                return;
            Logs.LogWrite('[' + fromGroup + ']' + "[QQ:" + fromQQ + "]:" + msg);
            if (GroupConfig.群列表.ContainsKey(fromGroup) == true)
            {
                msg = Funtion.CQtoCode(msg);
                GroupObj list = GroupConfig.群列表[fromGroup];
                //始终发送
                if (MainConfig.设置.始终发送消息 == true && MainConfig.设置.维护模式 == false
                    && MySocketServer.IsReady() == true && list.开启对话 == true)
                {
                    PlayerObj player = Funtion.GetPlayer(fromQQ);
                    if (player != null && !PlayerConfig.禁言列表.Contains(player.名字.ToLower())
                        && !string.IsNullOrWhiteSpace(player.名字))
                    {
                        string msg_copy = msg;
                        if (MainConfig.设置.颜色代码开关 == false)
                            msg_copy = Funtion.RemoveColorCodes(msg_copy);
                        if (msg_copy.IndexOf("CQ:rich") != -1)
                            msg_copy = Funtion.GetRich(msg_copy);
                        if (msg_copy.IndexOf("CQ:sign") != -1)
                            msg_copy = Funtion.GetSign(msg_copy, player.名字);
                        else if (msg_copy.IndexOf("CQ:") != -1)
                        {
                            msg_copy = Funtion.RemovePic(msg_copy);
                            msg_copy = Funtion.GetFromAt(msg_copy);
                            msg_copy = Funtion.CQtoCode(msg_copy);
                        }
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            MessageObj messagelist = new MessageObj();
                            messagelist.group = fromGroup.ToString();
                            messagelist.message = Funtion.RemovePic(msg_copy);
                            messagelist.player = !MainConfig.设置.使用昵称发送至服务器 ?
                                player.名字 : string.IsNullOrWhiteSpace(player.昵称) ?
                                player.名字 : player.昵称;
                            messagelist.commder = CommderList.SPEAK;
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
                            IMinecraft_QQ.SGroupMessage(fromGroup, "该群没有开启聊天功能");
                            return;
                        }
                        else if (MainConfig.设置.维护模式)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + MainConfig.消息.维护提示文本);
                            return;
                        }
                        else if (MySocketServer.IsReady() == false)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + "发送失败，没有服务器链接");
                            return;
                        }
                        else if (player == null || string.IsNullOrWhiteSpace(player.名字))
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ)
                                                + "检测到你没有绑定服务器ID，发送：" + MainConfig.检测.检测头
                                                + MainConfig.检测.玩家设置名字
                                                + "[ID]来绑定，如：" + "\n" + MainConfig.检测.检测头
                                                + MainConfig.检测.玩家设置名字 + " Color_yr");
                            return;
                        }
                        else if (PlayerConfig.禁言列表.Contains(player.名字.ToLower()))
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + "你已被禁言");
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
                                msg_copy = Funtion.RemovePic(msg_copy);
                                msg_copy = Funtion.GetFromAt(msg_copy);
                                msg_copy = Funtion.CQtoCode(msg_copy);
                            }
                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                            {
                                MessageObj messagelist = new MessageObj();
                                messagelist.group = "group";
                                messagelist.message = Funtion.RemovePic(msg_copy);
                                messagelist.player = !MainConfig.设置.使用昵称发送至服务器 ?
                                    player.名字 : string.IsNullOrWhiteSpace(player.昵称) ?
                                    player.名字 : player.昵称;
                                messagelist.commder = CommderList.SPEAK;
                                MySocketServer.Send(messagelist);
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
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.MutePlayer(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.取消禁言) == 0)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.UnmutePlayer(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.查询绑定名字) == 0)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.GetPlayerID(fromQQ, msg));
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.重命名) == 0)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.RenamePlayer(msg));
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.维护模式切换)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.FixModeChange());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁言列表)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.GetMuteList());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.获取禁止绑定列表)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.GetCantBind());
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.打开菜单)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, "已打开，请前往后台查看");
                            OpenSettingForm();
                            return;
                        }
                        else if (msg_low == MainConfig.管理员.重读配置)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, "开始重读配置文件");
                            Reload();
                            IMinecraft_QQ.SGroupMessage(fromGroup, "重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(MainConfig.管理员.设置昵称) == 0)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.SetNick(msg));
                            return;
                        }
                    }
                    if (msg_low == MainConfig.检测.在线玩家获取)
                    {
                        string test = Funtion.GetOnlinePlayer(fromGroup);
                        if (test != null)
                            IMinecraft_QQ.SGroupMessage(fromGroup, test);
                        return;
                    }
                    else if (msg_low == MainConfig.检测.服务器在线检测)
                    {
                        string test = Funtion.GetOnlineServer(fromGroup);
                        if (test != null)
                            IMinecraft_QQ.SGroupMessage(fromGroup, test);
                        return;
                    }

                    else if (msg_low.IndexOf(MainConfig.检测.玩家设置名字) == 0)
                    {
                        IMinecraft_QQ.SGroupMessage(fromGroup, IMinecraft_QQ.CodeAt(fromQQ) + Funtion.SetPlayerName(fromQQ, msg));
                        return;
                    }
                    else if (Funtion.SendCommand(fromGroup, msg, fromQQ) == true)
                        return;

                    else if (MainConfig.设置.自动应答开关 && AskConfig.自动应答列表.ContainsKey(msg_low) == true)
                    {
                        string message = AskConfig.自动应答列表[msg_low];
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            IMinecraft_QQ.SGroupMessage(fromGroup, message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(MainConfig.消息.未知指令文本) == false)
                    {
                        IMinecraft_QQ.SGroupMessage(fromGroup, MainConfig.消息.未知指令文本);
                        return;
                    }
                }
            }
        }
    }
}
