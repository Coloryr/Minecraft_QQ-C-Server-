using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.EventArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public class Minecraft_QQ
    {
        public readonly static string vision = "2.6.3";
        /// <summary>
        /// 插件启动线程
        /// </summary>
        private static Thread start = new Thread(Start);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string path { get; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";
        /// <summary>
        /// Mysql启用
        /// </summary>
        public static bool Mysql_ok = false;
        /// <summary>
        /// 主群群号
        /// </summary>
        public static long GroupSet_Main = 0;
        /// <summary>
        /// 已经启动
        /// </summary>
        public static bool is_start = false;

        public static CQApi Plugin { get; set; }

        /// <summary>
        /// 主配置文件
        /// </summary>
        public static Mainconfig_obj Mainconfig { get; set; }
        /// <summary>
        /// 玩家储存配置
        /// </summary>
        public static Player_obj Playerconfig { get; set; }
        /// <summary>
        /// 群储存配置
        /// </summary>
        public static Group_obj Groupconfig { get; set; }
        /// <summary>
        /// 自动应答储存
        /// </summary>
        public static Ask_obj Askconfig { get; set; }
        /// <summary>
        /// 自定义指令
        /// </summary>
        public static Command_obj Commandconfig { get; set; }

        public static void PrivateMessage(CQPrivateMessageEventArgs e)
        {
            if (Mainconfig.设置.自动应答开关 && Askconfig.自动应答列表.ContainsKey(e.Message.Text) == true)
            {
                string message = Askconfig.自动应答列表[e.Message.Text];
                if (string.IsNullOrWhiteSpace(message) == false)
                    e.FromQQ.SendPrivateMessage(message);
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
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + logs.log))
            {
                try
                {
                    File.WriteAllText(path + logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception)
                {
                    MessageBox.Show("[Minecraft_QQ]日志文件创建失败");
                    return;
                }
            }

            Config_read read = new Config_read();

            Config_file.config = new FileInfo(path + "Mainconfig.json");
            Config_file.player = new FileInfo(path + "Player.json");
            Config_file.ask = new FileInfo(path + "Ask.json");
            Config_file.command = new FileInfo(path + "Command.json");
            Config_file.group = new FileInfo(path + "Group.json");

            //读取主配置文件
            if (Config_file.config.Exists == false)
            {
                logs.Log_write("[Info][Config]新建主配置");
                Mainconfig = new Mainconfig_obj();
                File.WriteAllText(Config_file.config.FullName, JsonConvert.SerializeObject(Mainconfig, Formatting.Indented));
            }
            else
            {
                Mainconfig = read.Read_config();
            }

            //读取群设置
            if (Config_file.group.Exists == false)
            {
                logs.Log_write("[Info][Config]新建群设置配置");
                Groupconfig = new Group_obj();
                File.WriteAllText(Config_file.group.FullName, JsonConvert.SerializeObject(Groupconfig, Formatting.Indented));
            }
            else
            {
                Groupconfig = read.Read_group();
            }
            while (Groupconfig.群列表.Count == 0 || GroupSet_Main == 0)
            {
                setform frm = new setform();
                MessageBox.Show("请设置QQ群，有且最多一个主群", "参数错误，请设置");
                frm.ShowDialog();
                Groupconfig = read.Read_group();
            }

            //读自动应答消息
            if (Config_file.ask.Exists == false)
            {
                Askconfig = new Ask_obj
                {
                    自动应答列表 = new Dictionary<string, string>
                    {
                        { "服务器菜单", "服务器查询菜单：\r\n【" + Mainconfig.检测.检测头 + Mainconfig.检测.玩家设置名字
                    + "】可以绑定你的游戏ID。\r\n【" + Mainconfig.检测.检测头 + Mainconfig.检测.在线玩家获取
                    + "】可以查询服务器在线人数。\r\n【" + Mainconfig.检测.检测头 + Mainconfig.检测.服务器在线检测
                    + "】可以查询服务器是否在运行。\r\n【" + Mainconfig.检测.检测头 + Mainconfig.检测.发送消息至服务器
                    + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，）"}
                    }
                };
                File.WriteAllText(Config_file.ask.FullName, JsonConvert.SerializeObject(Askconfig, Formatting.Indented));
            }
            else
            {
                Askconfig = read.Read_ask();
            }

            //读取玩家数据
            if (Mainconfig.数据库.是否启用 == true)
            {
                if (Mysql.Mysql_start() == false)
                {
                    Plugin.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql链接失败");
                    Mysql_ok = false;
                }
                else
                {
                    Mysql Mysql = new Mysql();
                    if (Playerconfig == null)
                        Playerconfig = new Player_obj();
                    Mysql.Load();
                    Plugin.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql已连接");
                    Mysql_ok = true;
                }
            }
            else
            {
                if (Config_file.player.Exists == false)
                {
                    Playerconfig = new Player_obj();
                    File.WriteAllText(Config_file.player.FullName, JsonConvert.SerializeObject(Playerconfig, Formatting.Indented));
                }
                else
                {
                    Playerconfig = read.Read_player();
                }
            }

            //读取自定义指令
            if (Config_file.command.Exists == false)
            {
                Commandconfig = new Command_obj
                {
                    命令列表 = new Dictionary<string, Command_save_obj>
                    {
                        { "插件帮助", new Command_save_obj
                            {
                                check = "插件帮助",
                                command = "qq help",
                                player_use = false,
                                player_send = false,
                                parameter = true
                            }
                        },
                        { "查钱", new Command_save_obj
                            {
                                check = "查钱",
                                command = "money %player_name%",
                                player_use = true,
                                player_send = false,
                                parameter = false
                            }
                        },
                        { "禁言", new Command_save_obj
                            {
                                check = "禁言",
                                command = "mute ",
                                player_use = false,
                                player_send = false,
                                parameter = true
                            }
                        },
                        { "传送", new Command_save_obj
                            {
                                check = "传送",
                                command = "tpa %player_at%",
                                player_use = true,
                                player_send = false,
                                parameter = false
                            }
                        },
                    }
                };
                File.WriteAllText(Config_file.command.FullName, JsonConvert.SerializeObject(Commandconfig, Formatting.Indented));
            }
            else
            {
                Commandconfig = read.Read_commder();
            }
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public static void Start()
        {
            reload();

            socket.socket_stop();
            socket.Start_socket();
            is_start = true;

            Send.Send_T = new Thread(Send.Send_);
            Send.Send_T.Start();

            Plugin.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]已启动" + vision);
        }
        public static void stop()
        {
            is_start = false;
            socket.socket_stop();
            if (start != null && start.IsAlive)
            {
                start.Abort();
            }
        }
        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public static void GroupMessage(CQGroupMessageEventArgs e)
        {
            long fromGroup = e.FromGroup.Id;
            long fromQQ = e.FromQQ.Id;
            string msg = e.Message.Text;
            if (is_start == false)
                return;
            logs.Log_write('[' + fromGroup + ']' + "[QQ:" + fromQQ + "]:" + msg);
            if (Groupconfig.群列表.ContainsKey(fromGroup) == true)
            {
                msg = Utils.CQ_code(msg);
                Group_save_obj list = Groupconfig.群列表[fromGroup];
                //始终发送
                if (Mainconfig.设置.始终发送消息 == true && Mainconfig.设置.维护模式 == false && socket.ready == true && list.say == true)
                {
                    Player_save_obj player = Utils.Get_player(fromQQ);
                    if (player != null && !Playerconfig.禁言列表.Contains(player.player.ToLower()) && !string.IsNullOrWhiteSpace(player.player))
                    {
                        string send = Mainconfig.消息.发送至服务器文本;
                        string msg_copy = msg;
                        send = send.Replace("%player%", !Mainconfig.设置.使用昵称发送至服务器 ? player.player : (string.IsNullOrWhiteSpace(player.nick) ? player.player : player.nick));
                        if (Mainconfig.设置.颜色代码开关 == false)
                            msg_copy = Utils.RemoveColorCodes(msg_copy);
                        if (msg_copy.IndexOf("CQ:rich") != -1)
                            msg_copy = Utils.Get_rich(msg_copy);
                        if (msg_copy.IndexOf("CQ:sign") != -1)
                            msg_copy = Utils.Get_sign(msg_copy, player.player);
                        else if (msg_copy.IndexOf("CQ:") != -1)
                        {
                            msg_copy = Utils.Remove_pic(msg_copy);
                            msg_copy = Utils.Get_from_at(msg_copy);
                            msg_copy = Utils.CQ_code(msg_copy);
                        }
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            send = send.Replace("%message%", Utils.Remove_pic(msg_copy));
                            Message_send_obj messagelist = new Message_send_obj();
                            messagelist.group = fromGroup.ToString();
                            messagelist.message = send;
                            messagelist.commder = Commder_list.SPEAK;
                            socket.Send(messagelist);
                        }
                    }
                }
                if (msg.IndexOf(Mainconfig.检测.检测头) == 0 && list.commder == true)
                {
                    string msg_low = Utils.ReplaceFirst(msg.ToLower(), Mainconfig.检测.检测头, "");
                    //去掉检测头
                    msg = Utils.ReplaceFirst(msg, Mainconfig.检测.检测头, "");
                    Player_save_obj player = Utils.Get_player(fromQQ);
                    if (player != null)
                    {
                        if (Mainconfig.设置.始终发送消息 == false && msg_low.IndexOf(Mainconfig.检测.发送消息至服务器) == 0)
                        {
                            if (list.say == false)
                            {
                                e.FromGroup.SendGroupMessage("该群没有开启聊天功能");
                                return;
                            }
                            else if (Mainconfig.设置.维护模式)
                            {
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Mainconfig.消息.维护提示文本);
                                return;
                            }
                            else if (socket.ready == false)
                            {
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + "发送失败，服务器未准备好");
                                return;
                            }
                            else if (string.IsNullOrWhiteSpace(player.player))
                            {
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ)
                                                    + "检测到你没有绑定服务器ID，发送：" + Mainconfig.检测.检测头 + Mainconfig.检测.玩家设置名字
                                                    + "[ID]来绑定，如：" + "\n" + Mainconfig.检测.检测头 + Mainconfig.检测.玩家设置名字 + " Color_yr");
                                return;
                            }
                            else if (Playerconfig.禁言列表.Contains(player.player.ToLower()))
                            {
                                e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + "你已被禁言");
                            }
                            try
                            {
                                string send = Mainconfig.消息.发送至服务器文本;
                                string msg_copy = msg;
                                send = send.Replace("%player%", !Mainconfig.设置.使用昵称发送至服务器 ? player.player : (string.IsNullOrWhiteSpace(player.nick) ? player.player : player.nick));
                                msg_copy = msg_copy.Replace(Mainconfig.检测.发送消息至服务器, "");
                                if (Mainconfig.设置.颜色代码开关 == false)
                                    msg_copy = Utils.RemoveColorCodes(msg_copy);
                                if (msg_copy.IndexOf("CQ:") != -1)
                                {
                                    msg_copy = Utils.Remove_pic(msg_copy);
                                    msg_copy = Utils.Get_from_at(msg_copy);
                                    msg_copy = Utils.CQ_code(msg_copy);
                                }
                                if (string.IsNullOrWhiteSpace(msg_copy) == false)
                                {
                                    send = send.Replace("%message%", Utils.Remove_pic(msg_copy));
                                    Message_send_obj messagelist = new Message_send_obj();
                                    messagelist.group = "group";
                                    messagelist.message = send;
                                    messagelist.commder = Commder_list.SPEAK;
                                    socket.Send(messagelist);
                                }
                                return;
                            }
                            catch (InvalidCastException e1)
                            {
                                logs.Log_write(e1.Message);
                                return;
                            }
                        }
                        else if (msg_low.IndexOf(Mainconfig.管理员.禁言) == 0 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Mute_player(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(Mainconfig.管理员.取消禁言) == 0 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Unmute_player(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(Mainconfig.管理员.查询绑定名字) == 0 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Get_Player_id(fromQQ, msg));
                            return;
                        }
                        else if (msg_low.IndexOf(Mainconfig.管理员.重命名) == 0 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Rename_player(msg));
                            return;
                        }
                        else if (msg_low == Mainconfig.管理员.维护模式切换 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Fix_mode_change());
                            return;
                        }
                        else if (msg_low == Mainconfig.管理员.获取禁言列表 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Get_mute_list());
                            return;
                        }
                        else if (msg_low == Mainconfig.管理员.获取禁止绑定列表 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Get_cant_bind());
                            return;
                        }
                        else if (msg_low == Mainconfig.管理员.打开菜单 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage("已打开，请前往后台查看");
                            OpenSettingForm();
                            return;
                        }
                        else if (msg_low == Mainconfig.管理员.重读配置 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage("开始重读配置文件");
                            reload();
                            e.FromGroup.SendGroupMessage("重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(Mainconfig.管理员.设置昵称) == 0 && player.admin == true)
                        {
                            e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Set_nick(msg));
                            return;
                        }
                    }
                    if (msg_low == Mainconfig.检测.在线玩家获取)
                    {
                        string test = Utils.Get_online_player(fromGroup);
                        if (test != null)
                            e.FromGroup.SendGroupMessage(test);
                        return;
                    }
                    else if (msg_low == Mainconfig.检测.服务器在线检测)
                    {
                        string test = Utils.Get_online_server(fromGroup);
                        if (test != null)
                            e.FromGroup.SendGroupMessage(test);
                        return;
                    }

                    else if (msg_low.IndexOf(Mainconfig.检测.玩家设置名字) == 0)
                    {
                        e.FromGroup.SendGroupMessage(CQApi.CQCode_At(fromQQ) + Utils.Set_player_name(fromQQ, msg));
                        return;
                    }
                    else if (Utils.Send_command(fromGroup, msg, fromQQ) == true)
                        return;

                    else if (Mainconfig.设置.自动应答开关 && Askconfig.自动应答列表.ContainsKey(msg_low) == true)
                    {
                        string message = Askconfig.自动应答列表[msg_low];
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            e.FromGroup.SendGroupMessage(message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(Mainconfig.消息.位置指令文本) == false)
                    {
                        e.FromGroup.SendGroupMessage(Mainconfig.消息.位置指令文本);
                        return;
                    }
                }
            }
        }
    }
}
