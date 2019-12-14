using Native.Csharp.App;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public class Minecraft_QQ
    {
        public readonly static string vision = "2.4.3";
        /// <summary>
        /// 插件启动线程
        /// </summary>
        public static Thread start = new Thread(Start);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";

        public static void PrivateMessage(long fromQQ, string message_re)
        {
            if (main_config.message_enable && config_file.message_list.ContainsKey(message_re) == true)
            {
                message_save message = config_file.message_list[message_re];
                if (string.IsNullOrWhiteSpace(message.message) == false)
                {
                    Common.CqApi.SendPrivateMessage(fromQQ, message.message);
                    return;
                }
            }
        }

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
        public static void reload()
        {
            config_read.read_group();

            while (config_file.group_list.Count == 0 && GroupSet_Main == 0)
            {
                setform frm = new setform();
                MessageBox.Show("请设置QQ群，有且最多一个主群", "参数错误，请设置");
                frm.ShowDialog();
                config_read.read_group();
            }

            config_read.read_config();
            config_read.read_commder();
            config_read.read_message();

            if (mysql_config.use == true)
            {
                if (Mysql.mysql_start() == false)
                {
                    Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql链接失败");
                    config_file.Mysql_ok = false;
                }
                else
                {
                    Mysql Mysql = new Mysql();
                    Mysql.load();
                    Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]Mysql已连接");
                    config_file.Mysql_ok = true;
                }
            }
            else
            {
                config_read.read_cant_bind();
                config_read.read_mute();
                config_read.read_player();
            }
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

            reload();

            socket.socket_stop();
            socket.Start_socket();
            is_start = true;

            Send.Send_T = new Thread(Send.Send_);
            Send.Send_T.Start();

            Common.CqApi.SendGroupMessage(GroupSet_Main, "[Minecraft_QQ]已启动" + vision);
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
            logs.Log_write('[' + fromGroup.ToString() + ']' + "[QQ:" + fromQQ.ToString() + "]:" + msg);
            if (config_file.group_list.ContainsKey(fromGroup) == true)
            {
                msg = use.CQ_code(msg);
                group_save list = config_file.group_list[fromGroup];
                //始终发送
                if (main_config.allways_send == true && main_config.fix_mode == false && socket.ready == true && list.say == true)
                {
                    player_save player = use.check_player(fromQQ);
                    if (player != null && !config_file.mute_list.Contains(player.player.ToLower()) && !string.IsNullOrWhiteSpace(player.player))
                    {
                        string send = message_config.send_text;
                        string msg_copy = msg;
                        send = send.Replace("%player%", main_config.nick_server ? player.player : (string.IsNullOrWhiteSpace(player.nick) ? player.player : player.nick));
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
                            messagelist.message = send;
                            messagelist.commder = commder_list.SPEAK;
                            socket.Send(messagelist);
                        }
                    }
                }
                if (msg.IndexOf(check_config.head) == 0 && list.commder == true)
                {
                    string msg_low = use.ReplaceFirst(msg.ToLower(), check_config.head, "");
                    //去掉检测头
                    msg = use.ReplaceFirst(msg, check_config.head, "");
                    player_save player = use.check_player(fromQQ);
                    if (player != null)
                    {
                        if (main_config.allways_send == false && msg_low.IndexOf(check_config.send_message) == 0)
                        {
                            if (list.say == false)
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                                return;
                            }
                            else if (main_config.fix_mode)
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + message_config.fix_send);
                                return;
                            }
                            else if (socket.ready == false)
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                                return;
                            }
                            else if (string.IsNullOrWhiteSpace(player.player))
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ)
                                                    + "检测到你没有绑定服务器ID，发送：" + check_config.head + check_config.player_setid
                                                    + "[ID]来绑定，如：" + "\n" + check_config.head + check_config.player_setid + " Color_yr");
                                return;
                            }
                            else if (config_file.mute_list.Contains(player.player.ToLower()))
                            {
                                Common.CqApi.SendPrivateMessage(fromQQ, Common.CqApi.CqCode_At(fromQQ) + "你已被禁言");
                            }
                            try
                            {
                                string send = message_config.send_text;
                                string msg_copy = msg;
                                send = send.Replace("%player%", main_config.nick_server ? player.player : (string.IsNullOrWhiteSpace(player.nick) ? player.player : player.nick));
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
                                    messagelist.message = send;
                                    messagelist.commder = commder_list.SPEAK;
                                    socket.Send(messagelist);
                                }
                            }
                            catch (InvalidCastException e)
                            {
                                logs.Log_write(e.Message);
                                return;
                            }
                        }
                        else if (msg_low.IndexOf(admin_config.mute) == 0 && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_mute(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(admin_config.unmute) == 0 && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_unmute(msg));
                            return;
                        }
                        else if (msg_low.IndexOf(admin_config.check) == 0 && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_checkid(fromQQ, msg));
                            return;
                        }
                        else if (msg_low.IndexOf(admin_config.rename) == 0 && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_rename(msg));
                            return;
                        }
                        else if (msg_low == admin_config.fix && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.fix_mode_change());
                            return;
                        }
                        else if (msg_low == admin_config.mute_list && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.mutelist());
                            return;
                        }
                        else if (msg_low == admin_config.unbind_list && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.unbindlist());
                            return;
                        }
                        else if (msg_low == admin_config.menu && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                            OpenSettingForm();
                            return;
                        }
                        else if (msg_low == admin_config.reload && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, "开始重读配置文件");
                            reload();
                            Common.CqApi.SendGroupMessage(fromGroup, "重读完成");
                            return;
                        }
                        else if (msg_low.IndexOf(admin_config.nick) == 0 && player.admin == true)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.set_nick(msg));
                            return;
                        }
                    }
                    if (msg_low == check_config.online_players)
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
                    else if(use.commder_check(fromGroup, msg, fromQQ) == true)
                        return;

                    else if (main_config.message_enable && config_file.message_list.ContainsKey(msg_low) == true)
                    {
                        message_save message = config_file.message_list[msg_low];
                        if (string.IsNullOrWhiteSpace(message.message) == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, message.message);
                            return;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(message_config.unknow) == false)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, message_config.unknow);
                        return;
                    }
                }
            }
        }
    }
}
