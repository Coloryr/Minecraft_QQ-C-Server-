using Native.Csharp.App;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public class Minecraft_QQ
    {
        /// <summary>
        /// 插件启动线程
        /// </summary>
        public static Thread start = new Thread(Start);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";
        /// <summary>
        /// 主群群号
        /// </summary>
        public static long GroupSet_Main = 0;
        /// <summary>
        /// 发送绑定信息群号
        /// </summary>
        public static long Admin_Send = 0;
        /// <summary>
        /// 打开菜单
        /// </summary>
        public static void OpenSettingForm()
        {
            setform frm = new setform();
            frm.ShowDialog();
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        public static void Start()
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            if (File.Exists(path + config_file.config) == false)
                config_write.write_config(path + config_file.config);
            else
                config_read.read_config();
            if (File.Exists(path + config_file.group) == false)
                XML.write(path + config_file.group, "测试", "测试", "测试");
            else
                config_read.read_group();
            if (File.Exists(path + config_file.player) == false)
                XML.write(path + config_file.player, "测试", "测试", "测试");
            else
                config_read.read_player();
            if (File.Exists(path + config_file.message) == false)
            {
                XML.write(path + config_file.message, "自动回复", "服务器菜单", "服务器查询菜单：\r\n【" + XML.read(config_file.config, "检测", "绑定文本")
                    + "】可以绑定你的游戏ID。\r\n【" + XML.read(path + config_file.config, "检测", "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(path + config_file.config, "检测", "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(path + config_file.config, "检测", "发送文本") + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，输入"
                    + XML.read(path + config_file.config, "检测", "绑定文本") + "ID，来绑定ID）");
            }
            else

            if (File.Exists(path + config_file.commder) == false)
            {
                commder_save commder = new commder_save();

                commder.check = "插件帮助";
                commder.commder = "qq help";
                commder.player_use = false;
                commder.player_send = false;
                commder.parameter = true;
                config_write.write_commder(path + config_file.commder, commder);

                commder.check = "查钱";
                commder.commder = "money %player_name%";
                commder.player_use = true;
                commder.player_send = false;
                commder.parameter = false;
                config_write.write_commder(path + config_file.commder, commder);

                commder.check = "禁言";
                commder.commder = "mute ";
                commder.player_use = false;
                commder.player_send = false;
                commder.parameter = true;
                config_write.write_commder(path + config_file.commder, commder);

                commder.check = "传送";
                commder.commder = "tpa %player_at%";
                commder.player_use = true;
                commder.player_send = false;
                commder.parameter = false;
                config_write.write_commder(path + config_file.commder, commder);
            }
            if (config_file.group_list.Count == 0)
            {
                setform frm = new setform();
                MessageBox.Show("参数错误，请设置");
                frm.ShowDialog();
                config_read.read_config();
            }

            if (config_file.group_list.Count == 0 || GroupSet_Main == 0)
            {
                MessageBox.Show("[Minecraft_QQ]群设置错误请修改后重载应用");
                return;
            }

            if (!File.Exists(path + logs.log))
            {
                try
                {
                    File.WriteAllText(path + logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception)
                {
                    Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]日志文件创建失败");
                }
            }

            if (Mysql_mode == true)
            {

                logs.Log_write("[INFO][Mysql]正在链接Mysql");
                if (Mysql_user.mysql_start() == true)
                {
                    Mysql_mode = true;
                    Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql已连接");
                    logs.Log_write("[INFO][Mysql]Mysql已连接");
                }
                else
                {
                    Mysql_mode = false;
                    Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql错误，请检查");
                    logs.Log_write("[ERROR][Mysql]Mysql错误，请检查");
                }
            }
            else
                Mysql_mode = false;
            socket.Start_socket();
        }
        public static void PrivateMessage(long fromQQ, string msg)
        {
            // 处理私聊消息。
            string msg_copy = use.CQ_code(msg);
            string msg_low = msg.ToLower();
            logs.Log_write("私聊消息" + "[QQ:" + fromQQ.ToString() + "]" + msg_copy);
            if (msg_low.IndexOf(config_read.head) == 0)
            {
                msg_low = msg_low.Replace(config_read.head, "");
                if (msg_low.IndexOf(config_read.player_setid_message) == 0)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_setid(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.mute_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_mute(msg));
                else if (msg_low.IndexOf(config_read.unmute_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_unmute(msg));
                else if (msg_low.IndexOf(config_read.check_id_message) == 0)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_checkid(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.rename_id_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_rename(msg));
                else if (msg_low == config_read.fix_message && use.check_admin(fromQQ.ToString()) == true)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.fix_mode_change());
                else if (msg_low == config_read.menu_message && use.check_admin(fromQQ.ToString()) == true)
                {
                    Common.CqApi.SendPrivateMessage(fromQQ, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                else if (msg_low == config_read.reload_message && use.check_admin(fromQQ.ToString()) == true)
                {
                    Common.CqApi.SendPrivateMessage(fromQQ, "开始重读配置文件");
                    config_read.read_config();
                    Common.CqApi.SendPrivateMessage(fromQQ, "重读完成");
                }
                else if (msg_low == config_read.gc_message && use.check_admin(fromQQ.ToString()) == true)
                {
                    if (use.GC_now() == true)
                        Common.CqApi.SendPrivateMessage(fromQQ, "已清理内存");
                    else
                        Common.CqApi.SendPrivateMessage(fromQQ, "内存清理失败-请看日志");
                }
                else if (config_read.unknow_message != "" && config_read.unknow_message != null)
                    Common.CqApi.SendGroupMessage(fromQQ, config_read.unknow_message);
            }
        }

        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public static bool GroupMessage(long fromGroup, long fromQQ, string msg)
        {
            msg = use.CQ_code(msg);
            string msg_low = msg.ToLower();
            logs.Log_write('[' + fromGroup.ToString() + ']' + "[QQ:" + fromQQ.ToString() + "]:" + msg);
            // 处理群消息。
            grouplist list = null;
            if (config_read.group_list.ContainsKey(fromGroup) == true)
            {
                list = config_read.group_list[fromGroup];
            }
            if (list != null)
            {
                if (config_read.allways_send == true && config_read.fix_mode == false && socket.ready == true && list.say == true)
                {
                    string play_name = use.check_player_name(fromQQ.ToString());
                    if (play_name != null && use.check_mute(play_name) == false)
                    {
                        string send;
                        string msg_copy = msg;
                        send = config_read.send_text;
                        if (config_read.nick_mode_server == true)
                            send = send.Replace("%player%", play_name);
                        else
                            send = send.Replace("%player%", use.get_nick(play_name));
                        msg_copy = use.remove_pic(msg_copy);
                        if (msg_copy != "")
                        {
                            if (msg_copy.IndexOf("CQ:rich") != -1)
                                msg_copy = use.anno(msg_copy);
                            else
                            {
                                if (config_read.color_code == false)
                                    msg_copy = use.RemoveColorCodes(msg_copy);
                                msg_copy = use.get_at(msg_copy);
                                msg_copy = use.CQ_code(msg_copy);
                            }
                            send = send.Replace("%message%", use.remove_pic(msg_copy));
                            messagelist messagelist = new messagelist();
                            messagelist.group = fromGroup.ToString();
                            messagelist.message = "说话" + send;
                            socket.Send(messagelist);
                        }
                    }
                }
                if (msg_low.IndexOf(config_read.head) == 0 && list.commder == true)
                {
                    msg_low = use.ReplaceFirst(msg_low, config_read.head, "");
                    if (config_read.allways_send == false && msg_low.IndexOf(config_read.send_message) == 0)
                    {
                        if (list.say == false)
                            Common.CqApi.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                        else if (config_read.fix_mode == false)
                        {
                            if (socket.ready == true)
                            {
                                try
                                {
                                    string play_name = use.check_player_name(fromQQ.ToString());
                                    if (play_name != null && use.check_mute(play_name) == false)
                                    {
                                        string send;
                                        string msg_copy = msg;
                                        send = config_read.send_text;
                                        if (config_read.nick_mode_server == true)
                                            send = send.Replace("%player%", play_name);
                                        else
                                            send = send.Replace("%player%", use.get_nick(play_name));
                                        msg_copy = msg_copy.Replace(config_read.send_text, "");
                                        msg_copy = use.remove_pic(msg_copy);
                                        if (msg_copy != "")
                                        {
                                            if (config_read.color_code == false)
                                                msg_copy = use.RemoveColorCodes(msg_copy);
                                            msg_copy = use.get_at(msg_copy);
                                            msg_copy = use.CQ_code(msg_copy);
                                            int temp = config_read.send_message.Length + config_read.head.Length;
                                            msg_copy = msg_copy.Substring(temp - 1, msg_copy.Length - temp + 1);
                                            send = send.Replace("%message%", use.remove_pic(msg_copy));
                                            messagelist messagelist = new messagelist();
                                            messagelist.group = "group";
                                            messagelist.message = "说话" + send;
                                            socket.Send(messagelist);
                                        }
                                    }
                                    else
                                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "检测到你没有绑定服务器ID，发送：" + config_read.player_setid_message + " [ID]来绑定，如：" +
                                        "\n" + config_read.player_setid_message + " Color_yr");
                                }
                                catch (InvalidCastException e)
                                {
                                    logs.Log_write(e.Message);
                                }
                            }
                            else
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                        }
                        else
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + config_read.fix_send_message);
                        return true;
                    }
                    else if (msg_low == config_read.online_players_message)
                    {
                        string test = use.online(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                        return true;
                    }
                    else if (msg_low == config_read.online_servers_message)
                    {
                        string test = use.server(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                        return true;
                    }

                    else if (msg_low.IndexOf(config_read.player_setid_message) == 0)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_setid(fromQQ, msg));
                        return true;
                    }
                    else if (msg_low.IndexOf(config_read.mute_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_mute(msg));
                        return true;
                    }
                    else if (msg_low.IndexOf(config_read.unmute_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_unmute(msg));
                        return true;
                    }
                    else if (msg_low.IndexOf(config_read.check_id_message) == 0)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_checkid(fromQQ, msg));
                        return true;
                    }
                    else if (msg_low.IndexOf(config_read.rename_id_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_rename(msg));
                        return true;
                    }
                    else if (msg_low == config_read.fix_message && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.fix_mode_change());
                        return true;
                    }
                    else if (msg_low == config_read.menu_message && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                        OpenSettingForm();
                        return true;
                    }
                    else if (msg_low == config_read.reload_message && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "开始重读配置文件");
                        config_read.read_config();
                        Common.CqApi.SendGroupMessage(fromGroup, "重读完成");
                        return true;
                    }
                    else if (msg_low == config_read.gc_message && use.check_admin(fromQQ.ToString()) == true)
                    {
                        if (use.GC_now() == true)
                            Common.CqApi.SendGroupMessage(fromGroup, "已清理内存");
                        else
                            Common.CqApi.SendGroupMessage(fromGroup, "内存清理失败-请看日志");
                        return true;
                    }
                    else if (msg_low.IndexOf(config_read.nick_message) == 0 && use.check_admin(fromQQ.ToString()) == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.set_nick(msg));
                        return true;
                    }

                    if (use.commder_check(fromGroup, msg_low, fromQQ) == true) return true;

                    string message = XML.read_memory(config_read.message_m, "自动回复消息", msg_low);
                    if (config_read.message_enable == true && message != null)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, message);
                        return true;
                    }
                    if (config_read.unknow_message != "" && config_read.unknow_message != null)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, config_read.unknow_message);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
