using Native.Csharp.App;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public class Minecraft_QQ
    {
        public static string vision = "2.2.2";
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

        public static bool is_start = false;

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
            if (File.Exists(path + config_file.group) == false)
                XML.write(path + config_file.group, "测试", "测试", "测试");
            if (File.Exists(path + config_file.player) == false)
            {
                XML.write(path + config_file.player, "禁止绑定", "ID", "Color_yr");
                XML.write(path + config_file.player, "禁言", "ID", "Color_yr");
            }
            if (File.Exists(path + config_file.message) == false)
            {
                message_save message = new message_save();
                message.check = "服务器菜单";
                message.message = "服务器查询菜单：\r\n【" + check_config.head + check_config.player_setid
                    + "】可以绑定你的游戏ID。\r\n【" + check_config.head + check_config.online_players
                    + "】可以查询服务器在线人数。\r\n【" + check_config.head + check_config.online_servers
                    + "】可以查询服务器是否在运行。\r\n【" + check_config.head + check_config.send_message
                    + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，输入"
                    + check_config.head + check_config.player_setid + "ID，来绑定ID）";
                config_write.write_message(path + config_file.message, message);
            }
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

            config_read.read_config();
            config_read.read_cant_bind();
            config_read.read_mute();
            config_read.read_commder();
            config_read.read_group();
            config_read.read_player();
            config_read.read_message();
            if (config_file.group_list.Count == 0)
            {
                setform frm = new setform();
                MessageBox.Show("参数错误，请设置");
                frm.ShowDialog();
                config_read.read_config();
                config_read.read_cant_bind();
                config_read.read_mute();
                config_read.read_commder();
                config_read.read_group();
                config_read.read_player();
                config_read.read_message();
            }

            if (config_file.group_list.Count == 0 || GroupSet_Main == 0)
            {
                MessageBox.Show("群设置错误请修改后重载应用");
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
            socket.socket_stop();
            socket.Start_socket();
            is_start = true;
        }
        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public static void GroupMessage(long fromGroup, long fromQQ, string msg)
        {
            if (is_start == false)
                return;
            msg = use.CQ_code(msg);
            string msg_low = msg.ToLower();
            logs.Log_write('[' + fromGroup.ToString() + ']' + "[QQ:" + fromQQ.ToString() + "]:" + msg);
            // 处理群消息。
            group_save list = null;
            if (config_file.group_list.ContainsKey(fromGroup) == true)
                list = config_file.group_list[fromGroup];
            if (list != null)
            {
                if (main_config.allways_send == true && main_config.fix_mode == false && socket.ready == true && list.say == true)
                {
                    player_save player = use.check_player(fromQQ);
                    if (player != null && !config_file.mute_list.Contains(player.player.ToLower()) && !string.IsNullOrWhiteSpace(player.player))
                    {
                        string send;
                        string msg_copy = msg;
                        send = message_config.send_text;
                        if (main_config.nick_server == true)
                        {
                            if (string.IsNullOrWhiteSpace(player.nick) == true)
                                send = send.Replace("%player%", player.player);
                            else
                                send = send.Replace("%player%", player.nick);
                        }
                        else
                            send = send.Replace("%player%", player.player);
                        msg_copy = msg_copy.Replace(check_config.send_message, "");
                        if (main_config.color_code == false)
                            msg_copy = use.RemoveColorCodes(msg_copy);
                        if (msg_copy.IndexOf("CQ:rich") != -1)
                            msg_copy = use.rich(msg_copy);
                        if (msg_copy.IndexOf("CQ:sign") != -1)
                            msg_copy = use.sign(msg_copy, player.player);
                        else if (msg_copy.IndexOf("CQ:") != -1)
                        {
                            msg_copy = use.remove_pic(msg_copy);
                            msg_copy = use.get_at(msg_copy);
                            msg_copy = use.CQ_code(msg_copy);
                        }
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            send = send.Replace("%message%", use.remove_pic(msg_copy));
                            message_send messagelist = new message_send();
                            messagelist.group = fromGroup.ToString();
                            messagelist.message = "说话" + send;
                            socket.Send(messagelist);
                        }
                    }
                }
                if (msg_low.IndexOf(check_config.head) == 0 && list.commder == true)
                {
                    msg_low = use.ReplaceFirst(msg_low, check_config.head, "");
                    player_save player = use.check_player(fromQQ);
                    if (main_config.allways_send == false && msg_low.IndexOf(check_config.send_message) == 0)
                    {
                        if (list.say == false)
                            Common.CqApi.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                        else if (main_config.fix_mode == false)
                        {
                            if (socket.ready == true)
                            {
                                try
                                {
                                    if (player != null || string.IsNullOrWhiteSpace(player.player))
                                    {
                                        if (config_file.mute_list.Contains(player.player.ToLower()) == true)
                                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "你已被禁言");
                                        else
                                        {
                                            string send = message_config.send_text;
                                            string msg_copy = msg;
                                            if (main_config.nick_server == true)
                                            {
                                                if (string.IsNullOrWhiteSpace(player.nick) == true)
                                                    send = send.Replace("%player%", player.player);
                                                else
                                                    send = send.Replace("%player%", player.nick);
                                            }
                                            else
                                                send = send.Replace("%player%", player.player);
                                            msg_copy = msg_copy.Replace(check_config.send_message, "");
                                            if (main_config.color_code == false)
                                                msg_copy = use.RemoveColorCodes(msg_copy);
                                            if (msg_copy.IndexOf("CQ:") != -1)
                                            {
                                                msg_copy = use.remove_pic(msg_copy);
                                                msg_copy = use.get_at(msg_copy);
                                                msg_copy = use.CQ_code(msg_copy);
                                            }
                                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                                            {
                                                send = send.Replace("%message%", use.remove_pic(msg_copy));
                                                message_send messagelist = new message_send();
                                                messagelist.group = "group";
                                                messagelist.message = "说话" + send;
                                                socket.Send(messagelist);
                                            }
                                        }
                                    }
                                    else
                                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ)
                                            + "检测到你没有绑定服务器ID，发送：" + check_config.head + check_config.player_setid
                                            + "[ID]来绑定，如：" + "\n" + check_config.head + check_config.player_setid + " Color_yr");
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
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + message_config.fix_send);
                        return;
                    }
                    else if (msg_low == check_config.online_players)
                    {
                        string test = use.online(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                        return;
                    }
                    else if (msg_low == check_config.online_servers)
                    {
                        string test = use.server(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                        return;
                    }

                    else if (msg_low.IndexOf(check_config.player_setid) == 0)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_setid(fromQQ, msg));
                        return;
                    }
                    else if (msg_low.IndexOf(admin_config.mute) == 0 && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_mute(msg));
                        return;
                    }
                    else if (msg_low.IndexOf(admin_config.unmute) == 0 && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_unmute(msg));
                        return;
                    }
                    else if (msg_low.IndexOf(admin_config.check) == 0 && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_checkid(fromQQ, msg));
                        return;
                    }
                    else if (msg_low.IndexOf(admin_config.rename) == 0 && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_rename(msg));
                        return;
                    }
                    else if (msg_low == admin_config.fix && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.fix_mode_change());
                        return;
                    }
                    else if (msg_low == admin_config.mute_list && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.mutelist());
                        return;
                    }
                    else if (msg_low == admin_config.unbind_list && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.unbindlist());
                        return;
                    }
                    else if (msg_low == admin_config.menu && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                        OpenSettingForm();
                        return;
                    }
                    else if (msg_low == admin_config.reload && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "开始重读配置文件");
                        config_read.read_config();
                        config_read.read_cant_bind();
                        config_read.read_mute();
                        config_read.read_commder();
                        config_read.read_group();
                        config_read.read_player();
                        config_read.read_message();
                        Common.CqApi.SendGroupMessage(fromGroup, "重读完成");
                        return;
                    }
                    else if (msg_low.IndexOf(admin_config.nick) == 0 && player != null && player.admin == true)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.set_nick(msg));
                        return;
                    }

                    if (use.commder_check(fromGroup, msg, fromQQ) == true)
                        return;

                    if (main_config.message_enable && config_file.message_list.ContainsKey(msg_low) == true)
                    {
                        message_save message = config_file.message_list[msg_low];
                        if (string.IsNullOrWhiteSpace(message.message) == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, message.message);
                            return;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(message_config.unknow) == false)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, message_config.unknow);
                        return;
                    }
                }
            }
        }
    }
}
