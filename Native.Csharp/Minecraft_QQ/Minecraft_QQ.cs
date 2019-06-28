using Native.Csharp.App;
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
                            string play_name = use.check_player_name(fromQQ.ToString());

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
                    msg_low = use.ReplaceFirst(msg_low, config_read.head, "");
                    if (config_read.allways_send == false && msg_low.IndexOf(config_read.send_message) == 0)
                    {
                        if ((fromGroup == config_read.GroupSet2 && config_read.group2_mode == false)
                            || (fromGroup == config_read.GroupSet3 && config_read.group3_mode == false))
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
