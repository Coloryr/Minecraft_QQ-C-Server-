using Flexlive.CQP.Framework;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    /// <summary>
    /// 酷Q C#版插件Demo
    /// </summary>
    public class Minecraft_QQ : CQAppAbstract
    {
        public static int Port;         //端口  
        public static string ipaddress; //地址

        public static int Group;
        public static long GroupSet1;    //QQ群号1
        public static long GroupSet2;    //QQ群号2
        public static long GroupSet3;    //QQ群号3

        public static bool server = true;
        public static bool Group2_on = false;
        public static bool Group3_on = false;
        public static bool set_name = true;
        public static bool Mysql_mode = false;

        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "Minecraft_QQ";
            this.Version = new Version("1.8.0.3");
            this.Author = "Color_yr";
            this.Description = "Minecraft服务器与QQ群互联";
                 
        }
        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。
            config_read.config_thread.Start();
        }

        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        public override void OpenSettingForm()
        {
            // 打开设置窗口的相关代码。
            FormSettings frm = new FormSettings();
            frm.ShowDialog();
        }

        /// <summary>
        /// Type=21 私聊消息。
        /// </summary>
        /// <param name="subType">子类型，11/来自好友 1/来自在线状态 2/来自群 3/来自讨论组。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void PrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {
            // 处理私聊消息。
            string msg_copy = use.CQ_code(msg);
            logs.Log_write("私聊消息" + '[' + fromQQ.ToString() + "][" + CQ.GetQQName(fromQQ) + "]:" + msg_copy);
            if (msg.IndexOf(use.head) == 0)
            {
                msg = msg.Replace(use.head, "");
                if (msg.IndexOf(use.player_setid_message) == 0)
                    CQ.SendPrivateMessage(fromQQ, use.player_setid(fromQQ, msg));
                if (msg.IndexOf(use.mute_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    CQ.SendPrivateMessage(fromQQ, use.player_mute(fromQQ, msg));
                if (msg.IndexOf(use.unmute_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    CQ.SendPrivateMessage(fromQQ, use.player_unmute(fromQQ, msg));
                if (msg.IndexOf(use.check_id_message) == 0)
                    CQ.SendPrivateMessage(fromQQ, use.player_checkid(fromQQ, msg));
                if (msg.IndexOf(use.rename_id_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    CQ.SendPrivateMessage(fromQQ, use.player_rename(fromQQ, msg));
                if (msg == use.fix_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    CQ.SendPrivateMessage(fromQQ, use.fix_mode_change());
                if (msg.IndexOf("打开菜单") == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                {
                    CQ.SendPrivateMessage(fromQQ, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                if (msg == use.reload_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                {
                    CQ.SendPrivateMessage(fromQQ, "开始重读配置文件");
                    config_read.read_config();
                    config_read.reload();
                    CQ.SendPrivateMessage(fromQQ, "重读完成");
                }
                if (msg == use.gc_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                {
                    try
                    {
                        GC.Collect();
                        CQ.SendPrivateMessage(fromQQ, "内存回收完毕");
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="fromAnonymous">来源匿名者。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void GroupMessage(int subType, int sendTime, long fromGroup, long fromQQ, string fromAnonymous, string msg, int font)
        {
            string msg_copy = use.CQ_code(msg);
            logs.Log_write('[' + fromGroup.ToString() + ']' + '[' + fromQQ.ToString() + "][" + CQ.GetQQName(fromQQ) + "]:" + msg_copy);
            // 处理群消息。
            if (fromGroup == GroupSet1 || fromGroup == GroupSet2 || fromGroup == GroupSet3)
            {                
                if (use.send == "当然！")
                {
                    if (server == true && socket.ready == true)
                    {
                        if ((fromGroup == GroupSet2 && Group2_on == true) || (fromGroup == GroupSet3 && Group3_on == true) || fromGroup == GroupSet1)
                        {
                            string play_name = null;
                            if (Mysql_mode == true)
                                play_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                            else
                                play_name = XML.read(config_read.player, fromQQ.ToString());
                            if (play_name != null)
                            {
                                if (Mysql_mode == true)
                                {
                                    if (Mysql.mysql_search(Mysql.Mysql_mute, play_name.ToLower()) == "true")
                                        return;
                                }
                                else
                                {
                                    if (XML.read(config_read.mute, play_name.ToLower()) == "true")
                                        return;
                                }
                                string send;
                                send = use.send_text;
                                send = send.Replace("%player%", play_name);
                                string msg_copy1 = msg;
                                msg_copy1 = use.remove_pic(msg_copy1);
                                if (msg_copy1 != "")
                                {
                                    msg_copy1 = use.get_at(msg_copy1);
                                    msg_copy1 = use.CQ_code(msg_copy1);
                                    send = send.Replace("%message%", use.remove_pic(msg_copy1));
                                    socket.Send("群消息" + send, socket.MCserver);
                                }
                            }
                        }
                    }
                }
                else if (msg.IndexOf(use.send_message) == 0 && use.send == "不！")
                {
                    if ((fromGroup == GroupSet2 && Group2_on == false) || (fromGroup == GroupSet3 && Group3_on == false))
                    {
                        CQ.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                        return;
                    }
                    else if (server == true)
                    {
                        if (socket.ready == true)
                        {
                            string play_name = null;
                            if (Mysql_mode == true)
                                play_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                            else
                                play_name = XML.read(config_read.player, fromQQ.ToString());
                            if (play_name != null && play_name != "")
                            {
                                if (Mysql_mode == true)
                                {
                                    if (Mysql.mysql_search(Mysql.Mysql_mute, play_name.ToLower()) == "true")
                                        return;
                                }
                                else
                                {
                                    if (XML.read(config_read.mute, play_name.ToLower()) == "true")
                                        return;
                                }
                                string send;
                                send = use.send_text;
                                send = send.Replace("%player%", play_name);
                                string msg_copy1 = msg;
                                msg_copy1 = msg_copy1.Replace(use.send_text, "");
                                msg_copy1 = use.remove_pic(msg);
                                if (msg != "")
                                {
                                    msg_copy1 = use.get_at(msg_copy1);
                                    msg_copy1 = use.CQ_code(msg_copy1);
                                    send = send.Replace("%message%", use.remove_pic(msg_copy1));
                                    socket.Send("群消息" + send, socket.MCserver);
                                }
                            }
                            else
                                CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "检测到你没有绑定服务器ID，发送：" + use.player_setid_message + " [ID]来绑定，如：" +
                                "\n" + use.player_setid_message + " Color_yr");
                        }
                        else
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "发送失败，服务器未准备好");
                    }
                    else
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.fix_send_message);
                }
                if (msg.IndexOf(use.head) == 0)
                {
                    msg = msg.Replace(use.head, "");
                    if (XML.read(config_read.message, msg) != null && XML.read(config_read.message, "启用") == "true")
                    {
                        CQ.SendGroupMessage(fromGroup, XML.read(config_read.message, msg));
                        return;
                    }
                    if (msg == use.online_players_message)
                    {
                        string test = use.online(fromGroup);
                        if (test != null)
                            CQ.SendGroupMessage(fromGroup, test);
                    }
                    if (msg == use.online_servers_message)
                    {
                        string test = use.server(fromGroup);
                        if (test != null)
                            CQ.SendGroupMessage(fromGroup, test);
                    }
                    if (msg.IndexOf(use.player_setid_message) == 0)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.player_setid(fromQQ, msg));
                    if (msg.IndexOf(use.mute_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.player_mute(fromQQ, msg));
                    if (msg.IndexOf(use.unmute_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.player_unmute(fromQQ, msg));
                    if (msg.IndexOf(use.check_id_message) == 0)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.player_checkid(fromQQ, msg));
                    if (msg.IndexOf(use.rename_id_message) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != null)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.player_rename(fromQQ, msg));
                    if (msg == use.fix_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + use.fix_mode_change());
                    if (msg == use.mute_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    {
                        CQ.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                        OpenSettingForm();
                    }
                    if (msg == use.reload_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    {
                        CQ.SendGroupMessage(fromGroup, "开始重读配置文件");
                        config_read.read_config();
                        CQ.SendGroupMessage(fromGroup, "重读完成");
                    }
                    if (msg == use.gc_message && XML.read(config_read.admin, fromQQ.ToString()) != null)
                    {
                        try
                        {
                            GC.Collect();
                            CQ.SendGroupMessage(fromGroup, "内存回收完毕");
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
        }

        /// <summary>
        /// Type=4 讨论组消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromDiscuss">来源讨论组。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void DiscussMessage(int subType, int sendTime, long fromDiscuss, long fromQQ, string msg, int font)
        {
            // 处理讨论组消息。
            //CQ.SendDiscussMessage(fromDiscuss, String.Format("[{0}]{1}你发的讨论组消息是：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), msg));
        }

        /// <summary>
        /// Type=11 群文件上传事件。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="file">上传文件信息。</param>
        public override void GroupUpload(int subType, int sendTime, long fromGroup, long fromQQ, string file)
        { }

        /// <summary>
        /// Type=101 群事件-管理员变动。
        /// </summary>
        /// <param name="subType">子类型，1/被取消管理员 2/被设置管理员。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        { }

        /// <summary>
        /// Type=102 群事件-群成员减少。
        /// </summary>
        /// <param name="subType">子类型，1/群员离开 2/群员被踢 3/自己(即登录号)被踢。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
            if (fromGroup == GroupSet1)
            {
                if (subType == 1)
                {
                    string a = use.event_quit_message;
                    if (a != "")
                    {
                        a = a.Replace("%player%", CQ.GetQQName(beingOperateQQ));
                        CQ.SendGroupMessage(fromGroup, a);
                    }
                }
                if (subType == 2)
                {
                    string a = use.event_kick_message;
                    if (a != "")
                    {
                        a = a.Replace("%player%", CQ.GetQQName(beingOperateQQ));
                        CQ.SendGroupMessage(fromGroup, a);
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
        public override void GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员增加。
            if (fromGroup == GroupSet1)
            {
                string a = use.event_join_message;
                if (a != "")
                {
                    a = a.Replace("%player%", CQ.GetQQName(beingOperateQQ));
                    CQ.SendGroupMessage(fromGroup, a);
                }
            }
        }

        /// <summary>
        /// Type=201 好友事件-好友已添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        public override void FriendAdded(int subType, int sendTime, long fromQQ)
        { }

        /// <summary>
        /// Type=301 请求-好友添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        { }

        /// <summary>
        /// Type=302 请求-群添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddGroup(int subType, int sendTime, long fromGroup, long fromQQ, string msg, string responseFlag)
        { }
    }

}