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
        public static int g;
        public static String ipaddress; //地址
        public static long GroupSet1;    //QQ群号1
        public static long GroupSet2;    //QQ群号2
        public static long GroupSet3;    //QQ群号3
        public static string config = "config.xml";
        public static string player = "player.xml";
        public static string admin = "admin.xml";
        public static string message = "message.xml";
        public static string log = path + "logs.log";
        public static string Event = "event.xml";
        static Boolean message_on=false;
        public static string command = " ";
        public static string text = "";
        public static string read_text = "";
        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"Minecraft_QQ/";//AppDomain.CurrentDomain.SetupInformation.ApplicationBase
        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "Minecraft_QQ";
            this.Version = new Version("1.2.0.0");
            this.Author = "yan_color";
            this.Description = "Minecraft服务器与QQ群互联";
                 
        }
        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。
            FormSettings frm = new FormSettings();
            if (Directory.Exists(path) == false)
            { Directory.CreateDirectory(path); }
            if (File.Exists(path + config) == false)
            {LinqXML.write(config, "启用", "true");}
            if (File.Exists(path + admin) == false)
            { LinqXML.write(admin, "启用", "true"); }
            if (File.Exists(path + player) == false)
            { LinqXML.write(player, "启用", "true"); }
            if (File.Exists(path + Event) == false)
            {
                LinqXML.write(Event, "启用", "true");
                LinqXML.write(Event, "事件-群员加入", "欢迎新人" + "%player%" + "，请把名字改成ID，客户端在群文件");
                LinqXML.write(Event, "事件-群员退出", "%player%" + "退出了群");
                LinqXML.write(Event, "事件-文件上传", "%player%" + "上传了文件" + "%file%");
                LinqXML.write(Event, "事件-踢出群员", "%player%" + "感受制裁吧！");
                LinqXML.write(Event, "在线人数", "%online%");
                LinqXML.write(Event, "服务器状态", "%server_online%");
            }
            if (File.Exists(path + message) == false)
            {
                LinqXML.write(message, "启用", "true");
                LinqXML.write(message, "菜单", "输入“绑定：ID”可以绑定你的游戏ID。\r\n输入“在线人数”可以查询服务器在线人数。\r\n输入“服务器状态”可以查询服务器是否在运行。\r\n输入“服务器：【内容】”可以向服务器里发送消息。");
            }
            if (LinqXML.read(config, "启用") == "true") message_on = true;
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
            CQ.SendGroupMessage(GroupSet1, "[Minecraft_QQ]已启动");
            check = LinqXML.read(config, "群号2");
            if (check != "")
            {
                GroupSet2 = long.Parse(check);
                if (GroupSet2 != 0)
                {
                    CQ.SendGroupMessage(GroupSet2, "[Minecraft_QQ]已启动");
                }
            }
            else { GroupSet2 = 0; }
            check = LinqXML.read(config, "群号3");
            if (check != "")
            {
                GroupSet3 = long.Parse(check);
                if (GroupSet3 != 0)
                {
                    CQ.SendGroupMessage(GroupSet3, "[Minecraft_QQ]已启动");
                }
            }
            else { GroupSet3 = 0; }
            if (!File.Exists(path + log))
            {
                File.WriteAllText(path+log, "创建文件"+ Environment.NewLine);
            }
            socket.Start_socket();           
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
            string x = msg.Substring(0, 4);
            if (x == "服务器：" || x == "服务器:")
            {
                string reply = LinqXML.read(player, fromQQ.ToString());
                if (reply != "")
                {
                    text = reply + ':' + RemoveLeft(msg, 4);
                    text = "群消息" + text;
                }
                else
                {
                    CQ.SendGroupMessage(fromQQ, "检测到你没有绑定服务器ID，发送“绑定：ID”来绑定，如：绑定：yan_color");
                }
            }
            if (msg.IndexOf("绑定：") == 0)
            {
                if (LinqXML.read(player, fromQQ.ToString()) == "")
                {
                    string a = msg.Replace("绑定：", "");
                    msg = a;
                    if (a == " " || a == "" || IsNatural_Number(a) == false)
                    {
                        CQ.SendGroupMessage(fromQQ, CQ.CQCode_At(fromQQ) + "绑定失败，请检查你的ID");
                    }
                    else
                    {
                        var sb = new StringBuilder(a);
                        sb.Replace(" ", string.Empty);
                        LinqXML.write(player, fromQQ.ToString(), sb.ToString());
                        CQ.SendGroupMessage(fromQQ, CQ.CQCode_At(fromQQ) + "绑定ID:" + msg.Replace("绑定", "") + "成功！");
                    }
                }
                else
                {
                    CQ.SendGroupMessage(fromQQ, CQ.CQCode_At(fromQQ) + "你已经绑定过了，想换ID私聊服主去吧");
                }
            }
            if (msg.IndexOf("打开菜单") == 0 && LinqXML.read(config, fromQQ.ToString()) != "")
            {
                CQ.SendGroupMessage(GroupSet1, "已打开，请前往后台查看");
                OpenSettingForm();
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
                string x = msg.Substring(0, 4);
                if (x == "服务器：" || x == "服务器:")
                {
                    string reply = LinqXML.read(player, fromQQ.ToString());
                    if (reply != "")
                    {
                        text = reply + ':' + RemoveLeft(msg, 4);
                        text = "群消息" + text;
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, "检测到你没有绑定服务器ID，发送“绑定：ID”来绑定，如：绑定：yan_color");
                    }
                }
                if (msg.IndexOf("绑定：") == 0)
                {
                    if (LinqXML.read(player, fromQQ.ToString()) == "")
                    {
                        string a = msg.Replace("绑定：", "");
                        msg = a;
                        if (a == " "|| a == "" || IsNatural_Number(a) == false)
                        {
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定失败，请检查你的ID");
                        }
                        else
                        {
                            var sb = new StringBuilder(a);
                            sb.Replace(" ", string.Empty);
                            LinqXML.write(player, fromQQ.ToString(), sb.ToString());
                            CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定ID:" + msg.Replace("绑定", "") + "成功！");
                        }
                    }
                    else
                    {
                        CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你已经绑定过了，想换ID私聊服主去吧");
                    }
                }
                if (LinqXML.read(Event,msg)=="%online%")
                {
                    CQ.SendGroupMessage(fromGroup, "查询中");  
                    if (fromGroup == GroupSet1) g = 1;
                    else if (fromGroup == GroupSet2) g = 2;
                    else if (fromGroup == GroupSet3) g = 3;
                    text = "在线人数:";
                }
                if (LinqXML.read(Event,msg) == "%server_online%")
                {
                    CQ.SendGroupMessage(fromGroup, "查询中，如果没有回复，则证明服务器未开启");                
                    if (fromGroup == GroupSet1) g = 1;
                    else if (fromGroup == GroupSet2) g = 2;
                    else if (fromGroup == GroupSet3) g = 3;
                    text = "服务器状态";
                }
                if (msg.IndexOf("打开菜单") == 0 && LinqXML.read(admin, fromQQ.ToString())!="")
                {
                    CQ.SendGroupMessage(fromGroup, "已打开，请前往后台查看");
                    OpenSettingForm();
                }
                if (LinqXML.read(message, msg) != "" && message_on==true)
                {
                    CQ.SendGroupMessage(fromGroup, LinqXML.read(message, msg));
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