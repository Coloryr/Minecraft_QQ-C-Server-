using Flexlive.CQP.Framework;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace yan_color.Minecraft_QQ
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

        public static string config = "config.xml";
        public static string player = "player.xml";
        public static string admin = "admin.xml";
        public static string message = "message.xml";
        public static string log = path + "logs.log";
        public static string Event = "event.xml";
        public static string mute = "mute.xml";

        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"Minecraft_QQ/";//AppDomain.CurrentDomain.SetupInformation.ApplicationBase

        public static Boolean server = true;
        public static Boolean Group2_on = false;
        public static Boolean Group3_on = false;

        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "Minecraft_QQ";
            this.Version = new Version("1.6.0.0");
            this.Author = "yan_color";
            this.Description = "Minecraft服务器与QQ群互联";
                 
        }
        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。
            read_config();

            socket.Start_socket();           
        }

        public static void read_config()
        {
            FormSettings frm = new FormSettings();
            if (Directory.Exists(path) == false)
            { Directory.CreateDirectory(path); }

            if (File.Exists(path + config) == false)
            {
                LinqXML.write(config, "更新？", "false");
                LinqXML.write(config, "IP", "127.0.0.1");
                LinqXML.write(config, "Port", "25555");
                LinqXML.write(config, "编码", "ANSI（GBK）");
                LinqXML.write(config, "发送消息", "不！");
                LinqXML.write(config, "发送文本", "%player%:%message%");
                LinqXML.write(config, "维护模式", "关");
            }
            if (LinqXML.read(config, "更新？") != "false")
            {
                LinqXML.write(config, "更新？", "false");
                if (LinqXML.read(config, "IP") == "") LinqXML.write(config, "IP", "127.0.0.1");
                if (LinqXML.read(config, "Port") == "") LinqXML.write(config, "Port", "25555");
                if (LinqXML.read(config, "编码") == "") LinqXML.write(config, "编码", "ANSI（GBK）");
                if (LinqXML.read(config, "发送消息") == "") LinqXML.write(config, "发送消息", "不！");
                if (LinqXML.read(config, "发送文本") == "") LinqXML.write(config, "发送文本", "%player%:%message%");
                if (LinqXML.read(config, "维护模式") == "") LinqXML.write(config, "维护模式", "关");
            }

            if (File.Exists(path + mute) == false)
            { LinqXML.write(mute, "启用", "true"); }

            if (File.Exists(path + admin) == false)
            { LinqXML.write(admin, "启用", "true"); }

            if (File.Exists(path + player) == false)
            { LinqXML.write(player, "启用", "true"); }

            if (File.Exists(path + Event) == false)
            {
                LinqXML.write(Event, "更新？", "false");
                LinqXML.write(Event, "事件-群员加入", "欢迎新人%player%，输入【%服务器菜单】获取更多帮助。");
                LinqXML.write(Event, "事件-群员退出", "%player%退出了群");
                LinqXML.write(Event, "事件-文件上传", "%player%上传了文件%file%");
                LinqXML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                LinqXML.write(Event, "在线人数", "%online%");
                LinqXML.write(Event, "服务器状态", "%server_online%");
                LinqXML.write(Event, "绑定文本", "绑定：");
                LinqXML.write(Event, "发送文本", "服务器：");
                LinqXML.write(Event, "禁言文本", "禁言：");
                LinqXML.write(Event, "解禁文本", "解禁：");
                LinqXML.write(Event, "查询玩家ID", "查询：");
                LinqXML.write(Event, "修改玩家ID", "修改：");
                LinqXML.write(Event, "维护文本", "服务器维护");
                LinqXML.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                LinqXML.write(Event, "机器人功能-重读配置文件", "重读文件");
            }
            if (LinqXML.read(Event, "更新？") != "false")
            {
                LinqXML.write(Event, "更新？", "false");
                if (LinqXML.read(Event, "事件-群员加入") == "") LinqXML.write(Event, "事件-群员加入", "欢迎新人%player%，输入【%服务器菜单】获取更多帮助。");
                if (LinqXML.read(Event, "事件-群员退出") == "") LinqXML.write(Event, "事件-群员退出", "%player%退出了群");
                if (LinqXML.read(Event, "事件-文件上传") == "") LinqXML.write(Event, "事件-文件上传", "%player%上传了文件%file%");
                if (LinqXML.read(Event, "事件-踢出群员") == "") LinqXML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                if (LinqXML.read(Event, "在线人数") == "") LinqXML.write(Event, "在线人数", "%online%");
                if (LinqXML.read(Event, "服务器状态") == "") LinqXML.write(Event, "服务器状态", "%server_online%");
                if (LinqXML.read(Event, "绑定文本") == "") LinqXML.write(Event, "绑定文本", "绑定：");
                if (LinqXML.read(Event, "发送文本") == "") LinqXML.write(Event, "发送文本", "服务器：");
                if (LinqXML.read(Event, "禁言文本") == "") LinqXML.write(Event, "禁言文本", "禁言：");
                if (LinqXML.read(Event, "解禁文本") == "") LinqXML.write(Event, "解禁文本", "解禁：");
                if (LinqXML.read(Event, "查询玩家ID") == "") LinqXML.write(Event, "查询玩家ID", "查询：");
                if (LinqXML.read(Event, "修改玩家ID") == "") LinqXML.write(Event, "修改玩家ID", "修改：");
                if (LinqXML.read(Event, "修改玩家ID") == "") LinqXML.write(Event, "修改玩家ID", "修改：");
                if (LinqXML.read(Event, "维护文本") == "") LinqXML.write(Event, "维护文本", "服务器维护");
                if (LinqXML.read(Event, "服务器维护文本") == "") LinqXML.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (LinqXML.read(Event, "机器人功能-重读配置文件") == "") LinqXML.write(Event, "机器人功能-重读配置文件", "重读文件");
            }

            if (File.Exists(path + message) == false)
            {
                LinqXML.write(message, "启用", "true");
                LinqXML.write(message, "%服务器菜单", "服务器查询菜单：\r\n【" + LinqXML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【在线人数】可以查询服务器在线人数。\r\n【服务器状态】可以查询服务器是否在运行。\r\n【" + LinqXML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
            if (LinqXML.read(message, "启用") == "")
            {
                if (LinqXML.read(message, "%服务器菜单") == "") LinqXML.write(message, "%服务器菜单", "服务器查询菜单：\r\n【" + LinqXML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【在线人数】可以查询服务器在线人数。\r\n【服务器状态】可以查询服务器是否在运行。\r\n【" + LinqXML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }

            string check = LinqXML.read(config, "群号1");
            if (check == "") { MessageBox.Show("未设置群号，请设置"); frm.ShowDialog(); }
            else { GroupSet1 = long.Parse(check); }
            ipaddress = LinqXML.read(config, "IP");
            if (ipaddress == "")
            { MessageBox.Show("未设置IP，请设置"); frm.ShowDialog(); }
            else { ipaddress = LinqXML.read(config, "IP"); }
            check = LinqXML.read(config, "Port");
            if (check == "")
            { MessageBox.Show("未设置端口，请设置"); frm.ShowDialog(); }
            else { Port = int.Parse(LinqXML.read(config, "Port")); }

            CQ.SendGroupMessage(GroupSet1, "[Minecraft_QQ]正在启动");

            check = LinqXML.read(config, "群号2");
            if (check != "")
                GroupSet2 = long.Parse(check);
            else
                GroupSet2 = 0;
            check = LinqXML.read(config, "群号3");
            if (check != "")
                GroupSet3 = long.Parse(check);
            else
                GroupSet3 = 0;

            if (!File.Exists(path + log))
            {
                File.WriteAllText(path + log, "正在尝试创建文件" + Environment.NewLine);
            }

            if (LinqXML.read(config, "维护模式") == "关") server = true;
            else server = false;

            if (LinqXML.read(config, "群2发送消息") == "开") Group2_on = true;
            else Group2_on = false;
            if (LinqXML.read(config, "群3发送消息") == "开") Group3_on = true;
            else Group3_on = false;
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
            logs.Log_write('[' + fromGroup.ToString() + ']' + '[' + fromQQ.ToString() + ']' + ':' + msg);
            // 处理群消息。
            if (fromGroup == GroupSet1 || fromGroup == GroupSet2 || fromGroup == GroupSet3)
            {
                if (LinqXML.read(message, msg) != "" && LinqXML.read(message, "启用") == "true")
                {
                    CQ.SendGroupMessage(fromGroup, LinqXML.read(message, msg));
                }                
                else if (LinqXML.read(config, "发送消息") == "当然！")
                {
                    if ((fromGroup == GroupSet2 && Group2_on == false) || (fromGroup == GroupSet3 && Group3_on == false))
                            CQ.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                    else if (server == true)
                    {
                        if(socket.ready == true)
                        { 
                            string play = LinqXML.read(player, fromQQ.ToString());
                            if (play != "")
                            {
                                if (LinqXML.read(mute, play) != "true")
                                {
                                    string a;
                                    a = LinqXML.read(config, "发送文本");
                                    a = a.Replace("%player%", play);
                                    a = a.Replace("%message%", msg);
                                    socket.Send("群消息" + a, socket.MCserver);
                                }
                            }
                        }
                    }
                }
                else if (msg.IndexOf(LinqXML.read(Event, "发送文本")) == 0 && LinqXML.read(config, "发送消息") == "不！")
                {
                    if ((fromGroup == GroupSet2 && Group2_on == false) || (fromGroup == GroupSet3 && Group3_on == false))
                            CQ.SendGroupMessage(fromGroup, "该群没有开启聊天功能");
                    else if (server == true)
                    {                       
                        if (socket.ready == true)
                        {
                            string play = LinqXML.read(player, fromQQ.ToString());
                            if (play != "")
                            {
                                if (LinqXML.read(mute, play) != "true")
                                {
                                    string a;
                                    a = LinqXML.read(config, "发送文本");
                                    a = a.Replace("%player%", play);
                                    a = a.Replace("%message%", msg.Replace(LinqXML.read(Event, "发送文本"), ""));
                                    socket.Send("群消息" + a, socket.MCserver);
                                }
                            }
                            else
                            {
                                CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "检测到你没有绑定服务器ID，发送【" + LinqXML.read(Event, "绑定文本") + "ID】来绑定，如：【" + LinqXML.read(Event, "绑定文本") + "yan_color】");
                            }
                        }
                        else
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "发送失败，请稍后尝试");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + LinqXML.read(Event, "服务器维护文本"));
                    }
                }
                if (LinqXML.read(Event, msg) == "%online%")
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
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + LinqXML.read(Event, "服务器维护文本"));
                    }
                }
                if (LinqXML.read(Event, msg) == "%server_online%")
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
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + LinqXML.read(Event, "服务器维护文本"));
                    }
                }               
                if (msg.IndexOf(LinqXML.read(Event, "绑定文本")) == 0)
                {
                    if (LinqXML.read(player, fromQQ.ToString()) == "")
                    {
                        string a = msg.Replace(LinqXML.read(Event, "绑定文本"), "");
                        if (a == " " || a == "" || IsNatural_Number(a) == false)
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定失败，请检查你的ID");
                        }
                        else
                        {
                            var sb = new StringBuilder(a);
                            sb.Replace(" ", string.Empty);
                            LinqXML.write(player, fromQQ.ToString(), sb.ToString());
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定ID：" + a + "成功！");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你已经绑定ID了，请找腐竹更改");
                    }
                }
                if (msg.IndexOf(LinqXML.read(Event, "禁言文本")) == 0 && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    string a = msg.Replace(LinqXML.read(Event, "禁言文本"), "");
                    LinqXML.write(mute, a,"true");
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已禁言：[" + a + "]");
                }
                if (msg.IndexOf(LinqXML.read(Event, "解禁文本")) == 0 && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    string a = msg.Replace(LinqXML.read(Event, "解禁文本"), "");
                    LinqXML.write(mute, a, "false");
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已解禁：[" + a + "]");
                }
                if (msg.IndexOf(LinqXML.read(Event, "查询玩家ID")) == 0 && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    string a = msg.Replace(LinqXML.read(Event, "查询玩家ID"), "");
                    a = get_string(a, "=", "]");
                    string b=LinqXML.read(player, a);
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "玩家ID：" + b);
                }
                if (msg.IndexOf(LinqXML.read(Event, "修改玩家ID")) == 0 && LinqXML.read(admin, fromQQ.ToString()) != "")
                {                    
                    string a = msg.Replace(LinqXML.read(Event, "修改玩家ID"), "");
                    string b = a;
                    a = get_string(a, "=", "]");
                    b = get_string(b, "]");
                    b = b.Trim();
                    LinqXML.write(player, a, b);
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已修改玩家[" + a + "]ID为：" + b);
                }
                if (msg == LinqXML.read(Event, "维护文本") && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    if (LinqXML.read(config, "维护模式") == "关")
                    {
                        LinqXML.write(config, "维护模式", "开");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "服务器维护模式已开启");
                        server = false;
                        return;
                    }
                    else
                    {
                        LinqXML.write(config, "维护模式", "关");
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "服务器维护模式已关闭");
                        server = true;
                        return;
                    }
                }
                if (msg.IndexOf("打开菜单") == 0 && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    CQ.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                if (msg == LinqXML.read(Event, "机器人功能-重读配置文件") && LinqXML.read(admin, fromQQ.ToString()) != "")
                {
                    CQ.SendGroupMessage(fromGroup, "开始重读配置文件");
                    read_config();
                    CQ.SendGroupMessage(fromGroup, "重读完成");
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
                string a = LinqXML.read(Event,"事件-文件上传");
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
                    String a = LinqXML.read(Event, "事件-群员退出");
                    if (a != "")
                    {
                        a = a.Replace("%player%", CQE.GetQQName(beingOperateQQ));
                        CQ.SendGroupMessage(fromGroup, a);
                    }
                }
                if (subType == 2)
                {
                    String a = LinqXML.read(Event, "事件-踢出群员");
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
                String a =LinqXML.read(Event,"事件-群员加入");
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