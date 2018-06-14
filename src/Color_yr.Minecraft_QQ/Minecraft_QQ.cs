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
        public static String ipaddress; //地址

        public static int Group;
        public static long GroupSet1;    //QQ群号1
        public static long GroupSet2;    //QQ群号2
        public static long GroupSet3;    //QQ群号3

        public static Boolean server = true;
        public static Boolean Group2_on = false;
        public static Boolean Group3_on = false;
        public static Boolean set_name = true;
        public static Boolean Mysql_mode = false;

        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "Minecraft_QQ";
            this.Version = new Version("1.7.0.0");
            this.Author = "Color_yr";
            this.Description = "Minecraft服务器与QQ群互联";
                 
        }
        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。
            config_read.read_config();

            socket.Start_socket();           
        }

        public static string get_string(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + 1;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c);
                return a.Substring(x, y - x);
            }
            else
            {
                return a.Substring(x);
            }
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
            logs.Log_write("私聊消息" + '[' + fromQQ.ToString() + ']' + '[' + CQE.GetQQName(fromQQ) + ']'
            + ':' + msg);
            if (msg.IndexOf(XML.read(config_read.Event, "绑定文本")) == 0)
            {
                string player_name = null;
                if (Mysql_mode == true)
                {
                    player_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                }
                else
                {
                    player_name = XML.read(config_read.player, fromQQ.ToString());
                }
                if (player_name == null)
                {
                    string a = msg.Replace(XML.read(config_read.Event, "绑定文本"), "");
                    if (a == " " || a == "" || IsNatural_Number(a) == false)
                    {
                        CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "绑定失败，请检查你的ID");
                    }
                    else
                    {
                        var sb = new StringBuilder(a);
                        sb.Replace(" ", string.Empty);
                        if (Mysql_mode == true)
                        {
                            XML.write(config_read.player, fromQQ.ToString(), sb.ToString());
                        }
                        else
                        {
                            Mysql.mysql_add(Mysql.Mysql_player, fromQQ.ToString(), sb.ToString());
                        }

                        CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "绑定ID：" + a + "成功！");

                        string qq_admin = XML.read(config_read.admin, "发送给的人");
                        if(qq_admin != null)
                        {
                            CQ.SendPrivateMessage(long.Parse(qq_admin), "玩家[" + CQE.GetQQName(fromQQ) + "]绑定了ID：[" + sb.ToString() + "]");
                        }
                    }
                }
                else
                {
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "你已经绑定ID了，请找腐竹更改");
                }
            }
            if (msg.IndexOf(XML.read(config_read.Event, "禁言文本")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                string a = msg.Replace(XML.read(config_read.Event, "禁言文本"), "");
                a = get_string(a, "=", "]");
                string b = XML.read(config_read.player, a);
                if (b == "")
                {
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "该玩家未绑定ID");
                }
                else
                {
                    XML.write(config_read.mute, b, "true");
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "已禁言：[" + b + "]");
                }
            }
            if (msg.IndexOf(XML.read(config_read.Event, "解禁文本")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                string a = msg.Replace(XML.read(config_read.Event, "解禁文本"), "");
                a = get_string(a, "=", "]");
                string b = XML.read(config_read.player, a);
                if (b == "")
                {
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "该玩家未绑定ID");
                }
                else
                {
                    XML.write(config_read.mute, b, "false");
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "已解禁：[" + b + "]");
                }
            }
            if (msg.IndexOf(XML.read(config_read.Event, "查询玩家ID")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                string player = msg.Replace(XML.read(config_read.Event, "查询玩家ID"), "");
                player = get_string(player, "=", "]");
                string player_name = null;
                if (Mysql_mode == true)
                {
                    player_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                }
                else
                {
                    player_name = XML.read(player, fromQQ.ToString());
                }
                CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "玩家ID：" + player_name);
            }
            if (msg.IndexOf(XML.read(config_read.Event, "修改玩家ID")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                string player = msg.Replace(XML.read(config_read.Event, "修改玩家ID"), "");
                string player_name = player;
                player = get_string(player, "=", "]");
                player_name = get_string(player_name, "]");
                player_name = player_name.Trim();
                if (Mysql_mode == true)
                {
                    XML.write(player, player, player_name);
                }
                else
                {
                    Mysql.mysql_add(Mysql.Mysql_player, player, player_name);
                }

                CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "已修改玩家[" + player + "]ID为：" + player_name);
            }
            if (msg == XML.read(config_read.Event, "维护文本") && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                if (XML.read(config_read.config, "维护模式") == "关")
                {
                    XML.write(config_read.config, "维护模式", "开");
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "服务器维护模式已开启");
                    server = false;
                    return;
                }
                else
                {
                    XML.write(config_read.config, "维护模式", "关");
                    CQ.SendPrivateMessage(fromQQ, CQ.CQCode_At(fromQQ) + "服务器维护模式已关闭");
                    server = true;
                    return;
                }
            }
            if (msg.IndexOf("打开菜单") == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                CQ.SendPrivateMessage(fromQQ, "已打开，请前往后台查看");
                OpenSettingForm();
            }
            if (msg == XML.read(config_read.Event, "机器人功能-重读配置文件") && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                CQ.SendPrivateMessage(fromQQ, "开始重读配置文件");
                config_read.read_config();
                CQ.SendPrivateMessage(fromQQ, "重读完成");
            }
            if (msg == XML.read(config_read.Event, "机器人功能-内存回收") && XML.read(config_read.admin, fromQQ.ToString()) != "")
            {
                try
                {
                    GC.Collect();
                    CQ.SendPrivateMessage(fromQQ, "内存回收完毕");
                }
                catch (Exception exception)
                { }
            }
        }

        public static string RemoveLeft(string s, int len)
        {
            return s.PadLeft(len).Remove(0, len);
        }
        
        public bool IsNatural_Number(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] > 127)
                    return false;
                else
                    return true;
            }
            return false;
        }

        public static string remove_pic(string a)
        {
            for (; a.IndexOf("[CQ:image") != -1;)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "");
            }
            return a;
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
            logs.Log_write('[' + fromGroup.ToString() + ']' + '[' + fromQQ.ToString() + ']' + '[' + CQE.GetQQName(fromQQ) + ']'
            + ':' + msg);
            // 处理群消息。
            if (fromGroup == GroupSet1 || fromGroup == GroupSet2 || fromGroup == GroupSet3)
            {
                if (XML.read(config_read.message, msg) != "" && XML.read(config_read.message, "启用") == "true")
                {
                    CQ.SendGroupMessage(fromGroup, XML.read(config_read.message, msg));
                }                
                else if (XML.read(config_read.config, "发送消息") == "当然！")
                {
                    if (server == true)
                    {
                        if(socket.ready == true)
                        {
                            string play=null;
                            if (Mysql_mode == true)
                            {
                                play = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                            }
                            else
                            {
                                play = XML.read(config_read.player, fromQQ.ToString());
                            }
                            if (play != "")
                            {
                                if (XML.read(config_read.mute, play) != "true")
                                {
                                    string a;
                                    a = XML.read(config_read.config, "发送文本");
                                    a = a.Replace("%player%", play);
                                    if (remove_pic(msg) == "")
                                        return;
                                    a = a.Replace("%message%", remove_pic(msg));
                                    socket.Send("群消息" + a, socket.MCserver);
                                }
                            }
                        }
                    }
                }
                else if (msg.IndexOf(XML.read(config_read.Event, "发送文本")) == 0 && XML.read(config_read.config, "发送消息") == "不！")
                {
                    if ((fromGroup == GroupSet2 && Group2_on == false) || (fromGroup == GroupSet3 && Group3_on == false))
                            CQ.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                    else if (server == true)
                    {                       
                        if (socket.ready == true)
                        {
                            string play = null;
                            if (Mysql_mode == true)
                            {
                                play = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                            }
                            else
                            {
                                play = XML.read(config_read.player, fromQQ.ToString());
                            }
                            if (play != null)
                            {
                                if (XML.read(config_read.mute, play) != "true")
                                {
                                    string a,b;
                                    a = XML.read(config_read.config, "发送文本");
                                    a = a.Replace("%player%", play);
                                    b = msg.Replace(XML.read(config_read.Event, "发送文本"), "");
                                    if (remove_pic(b) == "")
                                        return;
                                    a = a.Replace("%message%", remove_pic(b));
                                    socket.Send("群消息" + a, socket.MCserver);
                                }
                            }
                            else
                            {
                                CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "检测到你没有绑定服务器ID，发送【" + XML.read(config_read.Event, "绑定文本") + "ID】来绑定，如：【" + XML.read(config_read.Event, "绑定文本") + "Color_yr】");
                            }
                        }
                        else
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "发送失败，请稍后尝试");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + XML.read(config_read.Event, "服务器维护文本"));
                    }
                }
                if (XML.read(config_read.Event, msg) == "%online%")
                {
                    if (server == true)
                    {
                        if (socket.ready == true)
                        {
                            if (fromGroup == GroupSet1) Group = 1;
                            else if (fromGroup == GroupSet2) Group = 2;
                            else if (fromGroup == GroupSet3) Group = 3;
                            socket.Send("在线人数", socket.MCserver);
                        }
                        else
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "发送失败，请稍后尝试");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + XML.read(config_read.Event, "服务器维护文本"));
                    }
                }
                if (XML.read(config_read.Event, msg) == "%server_online%")
                {
                    if (server == true)
                    {
                        if (socket.ready == true)
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "查询中，如果没有回复，则证明服务器未开启");
                            if (fromGroup == GroupSet1) Group = 1;
                            else if (fromGroup == GroupSet2) Group = 2;
                            else if (fromGroup == GroupSet3) Group = 3;
                            socket.Send("服务器状态", socket.MCserver);
                        }
                        else
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "发送失败，请稍后尝试");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + XML.read(config_read.Event, "服务器维护文本"));
                    }
                }               
                if (msg.IndexOf(XML.read(config_read.Event, "绑定文本")) == 0)
                {
                    string player_name = null;
                    if (Mysql_mode == true)
                    {
                        player_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                    }
                    else
                    {
                        player_name = XML.read(config_read.player, fromQQ.ToString());
                    }
                    if (player_name == null)
                    {
                        string a = msg.Replace(XML.read(config_read.Event, "绑定文本"), "");
                        if (a == " " || a == "" || IsNatural_Number(a) == false)
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定失败，请检查你的ID");
                        }
                        else
                        {
                            var sb = new StringBuilder(a);
                            sb.Replace(" ", string.Empty);
                            if (Mysql_mode == false)
                            {
                                XML.write(config_read.player, fromQQ.ToString(), sb.ToString());
                            }
                            else
                            {
                                Mysql.mysql_add(Mysql.Mysql_player, fromQQ.ToString(), sb.ToString());
                            }
                            
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定ID：" + a + "成功！");

                            string qq_admin = XML.read(config_read.admin, "发送给的人");
                            if (qq_admin != null)
                            {
                                CQ.SendPrivateMessage(long.Parse(qq_admin), "玩家[" + CQE.GetQQName(fromQQ) + "]绑定了ID：[" + sb.ToString() + "]");
                            }
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你已经绑定ID了，请找腐竹更改");
                    }
                }
                if (msg.IndexOf(XML.read(config_read.Event, "禁言文本")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    string a = msg.Replace(XML.read(config_read.Event, "禁言文本"), "");
                    a = get_string(a, "=", "]");
                    string b = XML.read(config_read.player, a);
                    if (b == "")
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "该玩家未绑定ID");
                    }
                    else
                    {
                        XML.write(config_read.mute, b, "true");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已禁言：[" + b + "]");
                    }
                }
                if (msg.IndexOf(XML.read(config_read.Event, "解禁文本")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    string a = msg.Replace(XML.read(config_read.Event, "解禁文本"), "");
                    a = get_string(a, "=", "]");
                    string b = XML.read(config_read.player, a);
                    if (b == "")
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "该玩家未绑定ID");
                    }
                    else
                    {
                        XML.write(config_read.mute, b, "false");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已解禁：[" + b + "]");
                    }
                }
                if (msg.IndexOf(XML.read(config_read.Event, "查询玩家ID")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    string player = msg.Replace(XML.read(config_read.Event, "查询玩家ID"), "");
                    player = get_string(player, "=", "]");
                    string player_name = null;
                    if (Mysql_mode == true)
                    {
                        player_name = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
                    }
                    else
                    {
                        player_name = XML.read(player, fromQQ.ToString());
                    }
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "玩家ID：" + player_name);
                }
                if (msg.IndexOf(XML.read(config_read.Event, "修改玩家ID")) == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {                    
                    string player = msg.Replace(XML.read(config_read.Event, "修改玩家ID"), "");
                    string player_name = player;
                    player = get_string(player, "=", "]");
                    player_name = get_string(player_name, "]");
                    player_name = player_name.Trim();
                    if (Mysql_mode == true)
                    {
                        XML.write(player, player, player_name);
                    }
                    else
                    {
                        Mysql.mysql_add(Mysql.Mysql_player, player, player_name);
                    }
                    
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已修改玩家[" + player + "]ID为：" + player_name);
                }
                if (msg == XML.read(config_read.Event, "维护文本") && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    if (XML.read(config_read.config, "维护模式") == "关")
                    {
                        XML.write(config_read.config, "维护模式", "开");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "服务器维护模式已开启");
                        server = false;
                        return;
                    }
                    else
                    {
                        XML.write(config_read.config, "维护模式", "关");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "服务器维护模式已关闭");
                        server = true;
                        return;
                    }
                }
                if (msg.IndexOf("打开菜单") == 0 && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    CQ.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                if (msg == XML.read(config_read.Event, "机器人功能-重读配置文件") && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    CQ.SendGroupMessage(fromGroup, "开始重读配置文件");
                    config_read.read_config();
                    CQ.SendGroupMessage(fromGroup, "重读完成");
                }
                if (msg == XML.read(config_read.Event, "机器人功能-内存回收") && XML.read(config_read.admin, fromQQ.ToString()) != "")
                {
                    try
                    {
                        GC.Collect();
                        CQ.SendGroupMessage(fromGroup, "内存回收完毕");
                    }
                    catch (Exception exception)
                    { }
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
        {
            // 处理群文件上传事件。
            if (fromGroup == GroupSet1)
            {
                string a = XML.read(config_read.Event,"事件-文件上传");
                if (a != "")
                {
                    a = a.Replace("%player%",CQE.GetQQName(fromQQ));
                    a = a.Replace("%file%", file);
                    CQ.SendGroupMessage(fromGroup, a);
                }
            }
        }

        /// <summary>
        /// Type=101 群事件-管理员变动。
        /// </summary>
        /// <param name="subType">子类型，1/被取消管理员 2/被设置管理员。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-管理员变动。
            if (fromGroup == GroupSet1)
            {
                //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{2}({1})被{3}管理员权限。", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "取消了" : "设置为"));
            }
        }

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
                    String a = XML.read(config_read.Event, "事件-群员退出");
                    if (a != "")
                    {
                        a = a.Replace("%player%", CQE.GetQQName(beingOperateQQ));
                        CQ.SendGroupMessage(fromGroup, a);
                    }
                }
                if (subType == 2)
                {
                    String a = XML.read(config_read.Event, "事件-踢出群员");
                    if (a != "")
                    {
                        a = a.Replace("%player%", CQE.GetQQName(beingOperateQQ));
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
                String a =XML.read(config_read.Event,"事件-群员加入");
                if (a != "")
                {
                    a = a.Replace("%player%", CQE.GetQQName(beingOperateQQ));
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
        {
            // 处理好友事件-好友已添加。
            //CQ.SendPrivateMessage(fromQQ, String.Format("[{0}]你好，我的朋友！", CQ.ProxyType));
        }

        /// <summary>
        /// Type=301 请求-好友添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-好友添加。
            //CQ.SetFriendAddRequest(responseFlag, CQReactType.Allow, "新来的朋友");
        }

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
        {
            // 处理请求-群添加。
            //CQ.SetGroupAddRequest(responseFlag, CQRequestType.GroupAdd, CQReactType.Allow, "新群友");
        }
    }

}