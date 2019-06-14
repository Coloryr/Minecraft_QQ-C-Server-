using Native.Csharp.App;
using Native.Csharp.Sdk.Cqp.Model;
using System;

namespace Color_yr.Minecraft_QQ
{
    /// <summary>
    /// 酷Q C#版插件Demo
    /// </summary>
    public class Minecraft_QQ
    {
        public static void OpenSettingForm()
        {
            setform frm = new setform();
            frm.ShowDialog();
        }

        /// <summary>
        /// Type=21 私聊消息。
        /// </summary>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        public static void PrivateMessage(long fromQQ, string msg)
        {
            // 处理私聊消息。
            use use = new use();
            string msg_copy = use.CQ_code(msg);
            string msg_low = msg.ToLower();
            logs logs = new logs();
            QQ qqInfo;
            Common.CqApi.GetQQInfo(fromQQ, out qqInfo);
            logs.Log_write("私聊消息" + '[' + fromQQ.ToString() + "][" + qqInfo.Nick + "]:" + msg_copy);
            if (msg_low.IndexOf(config_read.head) == 0)
            {
                XML XML = new XML();
                msg_low = msg_low.Replace(config_read.head, "");
                if (msg_low.IndexOf(config_read.player_setid_message) == 0)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_setid(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.mute_message) == 0 && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_mute(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.unmute_message) == 0 && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_unmute(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.check_id_message) == 0)
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_checkid(fromQQ, msg));
                else if (msg_low.IndexOf(config_read.rename_id_message) == 0 && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    Common.CqApi.SendPrivateMessage(fromQQ, use.player_rename(fromQQ, msg));
                else if (msg_low == config_read.fix_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    Common.CqApi.SendPrivateMessage(fromQQ, use.fix_mode_change());
                else if (msg_low == config_read.menu_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                {
                    Common.CqApi.SendPrivateMessage(fromQQ, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                else if (msg_low == config_read.reload_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                {
                    config_read read = new config_read();
                    Common.CqApi.SendPrivateMessage(fromQQ, "开始重读配置文件");
                    read.read_config();
                    read.reload();
                    Common.CqApi.SendPrivateMessage(fromQQ, "重读完成");
                }
                else if (msg_low == config_read.gc_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
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
        public static void GroupMessage(long fromGroup, long fromQQ, string msg)
        {
            use use = new use();
            msg = use.CQ_code(msg);
            string msg_low = msg.ToLower();
            logs logs = new logs();
            QQ qqInfo;
            Common.CqApi.GetQQInfo(fromQQ, out qqInfo);
            logs.Log_write('[' + fromGroup.ToString() + ']' + '[' + fromQQ.ToString() + "][" + qqInfo.Nick + "]:" + msg);
            // 处理群消息。
            if (fromGroup == config_read.GroupSet1 || fromGroup == config_read.GroupSet2 || fromGroup == config_read.GroupSet3)
            {
                if (config_read.allways_send == true)
                {
                    if (config_read.fix_mode == false && socket.ready == true)
                    {
                        if ((fromGroup == config_read.GroupSet2 && config_read.group2_mode == true) 
                            || (fromGroup == config_read.GroupSet3 && config_read.group3_mode == true) 
                            || fromGroup == config_read.GroupSet1)
                        {
                            string play_name = null;
                            if (config_read.Mysql_mode == true)
                                play_name = Mysql_user.mysql_search(Mysql_user.Mysql_player, fromQQ.ToString());
                            else
                            {
                                XML XML = new XML();
                                play_name = XML.read_memory(config_read.player_m, "玩家", fromQQ.ToString());
                            }
                            if (play_name != null && use.check_mute(play_name) == false)
                            {
                                string send;
                                string msg_copy = msg;
                                send = config_read.send_text;
                                send = send.Replace("%player%", play_name);
                                msg_copy = use.remove_pic(msg_copy);
                                if (msg_copy != "")
                                {
                                    if (config_read.color_code == false)
                                        msg_copy = use.RemoveColorCodes(msg_copy);
                                    msg_copy = use.get_at(msg_copy);
                                    msg_copy = use.CQ_code(msg_copy);
                                    send = send.Replace("%message%", use.remove_pic(msg_copy));
                                    messagelist messagelist = new messagelist();
                                    messagelist.group = "group";
                                    messagelist.message = "说话" + send;
                                    socket.Send(messagelist);
                                }
                            }
                        }
                    }
                }
                if (msg_low.IndexOf(config_read.head) == 0)
                {
                    msg_low = msg_low.Replace(config_read.head, "");
                    XML XML = new XML();
                    if (config_read.allways_send == false && msg_low.IndexOf(config_read.send_message) == 0)
                    {
                        if ((fromGroup == config_read.GroupSet2 && config_read.group2_mode == false) 
                            || (fromGroup == config_read.GroupSet3 && config_read.group3_mode == false))
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                        }
                        else if (config_read.fix_mode == false)
                        {
                            if (socket.ready == true)
                            {
                                try
                                {
                                    string play_name = null;
                                    if (config_read.Mysql_mode == true)
                                        play_name = Mysql_user.mysql_search(Mysql_user.Mysql_player, fromQQ.ToString());
                                    else
                                    {
                                        play_name = XML.read(config_read.player, "玩家", fromQQ.ToString());
                                    }
                                    if (play_name != null && use.check_mute(play_name) == false)
                                    {
                                        string send;
                                        string msg_copy = msg;
                                        send = config_read.send_text;
                                        send = send.Replace("%player%", play_name);
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
                    }
                    else if (msg_low == config_read.online_players_message)
                    {
                        string test = use.online(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                    }
                    else if (msg_low == config_read.online_servers_message)
                    {
                        string test = use.server(fromGroup);
                        if (test != null)
                            Common.CqApi.SendGroupMessage(fromGroup, test);
                    }

                    else if (msg_low.IndexOf(config_read.player_setid_message) == 0)
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_setid(fromQQ, msg));
                    else if (msg_low.IndexOf(config_read.mute_message) == 0 && XML.read(config_read.player, "管理员", fromQQ.ToString()) != null)
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_mute(fromQQ, msg));
                    else if (msg_low.IndexOf(config_read.unmute_message) == 0 && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_unmute(fromQQ, msg));
                    else if (msg_low.IndexOf(config_read.check_id_message) == 0)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_checkid(fromQQ, msg));
                        return;
                    }
                    else if (msg_low.IndexOf(config_read.rename_id_message) == 0 && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.player_rename(fromQQ, msg));
                        return;
                    }
                    else if (msg_low == config_read.fix_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + use.fix_mode_change());
                        return;
                    }
                    else if (msg_low == config_read.menu_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                        OpenSettingForm();
                        return;
                    }
                    else if (msg_low == config_read.reload_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, "开始重读配置文件");
                        config_read read = new config_read();
                        read.read_config();
                        Common.CqApi.SendGroupMessage(fromGroup, "重读完成");
                        return;
                    }
                    else if (msg_low == config_read.gc_message && XML.read(config_read.player, "管理员", "admin" + fromQQ.ToString()) == "true")
                    {
                        if (use.GC_now() == true)
                            Common.CqApi.SendGroupMessage(fromGroup, "已清理内存");
                        else
                            Common.CqApi.SendGroupMessage(fromGroup, "内存清理失败-请看日志");
                        return;
                    }

                    if (use.commder_check(fromGroup, msg_low, fromQQ) == true) return;

                    string message = XML.read_memory(config_read.message_m, "自动回复消息", msg_low);
                    if (config_read.message_enable == true && message != null)
                        Common.CqApi.SendGroupMessage(fromGroup, message);
                    else if (config_read.unknow_message != "" && config_read.unknow_message != null)
                        Common.CqApi.SendGroupMessage(fromGroup, config_read.unknow_message);
                }
            }
        }

        /// <summary>
        /// Type=102 群事件-群成员减少。
        /// </summary>
        /// <param name="subType">子类型，1/群员离开 2/群员被踢 3/自己(即登录号)被踢。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public static void GroupMemberDecrease(int subType, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
            if (fromGroup == config_read.GroupSet1)
            {
                if (subType == 1)
                {
                    string a = config_read.event_quit_message;
                    if (a != "")
                    {
                        QQ qqInfo;
                        Common.CqApi.GetQQInfo(beingOperateQQ, out qqInfo);
                        a = a.Replace("%player%", qqInfo.Nick);
                        Common.CqApi.SendGroupMessage(fromGroup, a);
                    }
                }
                if (subType == 2)
                {
                    string a = config_read.event_kick_message;
                    if (a != "")
                    {
                        QQ qqInfo;
                        Common.CqApi.GetQQInfo(beingOperateQQ, out qqInfo);
                        a = a.Replace("%player%", qqInfo.Nick);
                        Common.CqApi.SendGroupMessage(fromGroup, a);
                    }
                }
            }
        }

        /// <summary>
        /// Type=103 群事件-群成员增加。
        /// </summary>
        /// <param name="subType">子类型，1/管理员已同意 2/管理员邀请。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public static void GroupMemberIncrease(long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-群成员增加。
            if (fromGroup == config_read.GroupSet1)
            {
                string a = config_read.event_join_message;
                if (a != "")
                {
                    QQ qqInfo;
                    Common.CqApi.GetQQInfo(beingOperateQQ, out qqInfo);
                    a = a.Replace("%player%", qqInfo.Nick);
                    Common.CqApi.SendGroupMessage(fromGroup, a);
                }
            }
        }
    }
}
