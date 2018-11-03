using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class config_read
    {
        public static string config = "config.xml";
        public static string player = "player.xml";
        public static string admin = "admin.xml";
        public static string message = "message.xml";
        public static string Event = "event.xml";
        public static string mute = "mute.xml";
        public static string notid = "notid.xml";     

        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";//AppDomain.CurrentDomain.SetupInformation.ApplicationBase

        public static Thread config_thread = new Thread(start);

        public static void start()
        {
            FormSettings frm = new FormSettings();
            read_config();
            reload();

            if (use.group1 == null)
            {
                MessageBox.Show("未设置群号1，请设置");
                frm.ShowDialog();
            }
            else
                Minecraft_QQ.GroupSet1 = long.Parse(use.group1); 
            Minecraft_QQ.ipaddress = use.IP;
            if (Minecraft_QQ.ipaddress == null)
            {
                MessageBox.Show("未设置IP，请设置");
                frm.ShowDialog();
            }
            else
                Minecraft_QQ.ipaddress = use.IP; 
            if (use.Port == null)
            {
                MessageBox.Show("未设置端口，请设置");
                frm.ShowDialog();
            }
            else
                Minecraft_QQ.Port = int.Parse(use.Port); 

            CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]正在启动");

            if (use.group2 != null)
                Minecraft_QQ.GroupSet2 = long.Parse(use.group2);
            else
                Minecraft_QQ.GroupSet2 = 0;
            if (use.group3 != null)
                Minecraft_QQ.GroupSet3 = long.Parse(use.group3);
            else
                Minecraft_QQ.GroupSet3 = 0;

            if (!File.Exists(path + logs.log))
            {
                try
                {
                    File.WriteAllText(path + logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception)
                {
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]日志文件创建失败");
                }
            }

            if (use.fix_mode == "关") Minecraft_QQ.server = true;
            else Minecraft_QQ.server = false;

            if (use.group2_mode == "开") Minecraft_QQ.Group2_on = true;
            else Minecraft_QQ.Group2_on = false;
            if (use.group3_mode == "开") Minecraft_QQ.Group3_on = true;
            else Minecraft_QQ.Group3_on = false;

            if (use.Mysql_mode == "开")
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]正在链接Mysql");
                if (Mysql.mysql_start() == true)
                {
                    Minecraft_QQ.Mysql_mode = true;
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql已连接");
                }
                else
                {
                    Minecraft_QQ.Mysql_mode = false;
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql错误，请检查");
                }             
            }
            else
            {
                Minecraft_QQ.Mysql_mode = false;
            }

            socket.Start_socket();
        }

        public static void read_config()
        {
            if (Directory.Exists(path) == false)
            { Directory.CreateDirectory(path); }
            if (File.Exists(path + config) == false)
            {
                XML.write(config, "更新？", "false");
                XML.write(config, "IP", "127.0.0.1");
                XML.write(config, "Port", "25555");
                XML.write(config, "编码", "ANSI（GBK）");
                XML.write(config, "发送消息", "不！");
                XML.write(config, "发送文本", "%player%:%message%");
                XML.write(config, "维护模式", "关");
                XML.write(config, "Mysql地址", "127.0.0.1");
                XML.write(config, "Mysql端口", "3306");
                XML.write(config, "Mysql账户", "root");
                XML.write(config, "Mysql密码", "123456");
                XML.write(config, "Mysql启用", "关");
                XML.write(config, "检测头", "#");
            }
            if (XML.read(config, "更新？") != "false")
            {
                XML.write(config, "更新？", "false");
                if (XML.read(config, "IP") == null) XML.write(config, "IP", "127.0.0.1");
                if (XML.read(config, "Port") == null) XML.write(config, "Port", "25555");
                if (XML.read(config, "编码") == null) XML.write(config, "编码", "ANSI（GBK）");
                if (XML.read(config, "发送消息") == null) XML.write(config, "发送消息", "不！");
                if (XML.read(config, "发送文本") == null) XML.write(config, "发送文本", "%player%:%message%");
                if (XML.read(config, "维护模式") == null) XML.write(config, "维护模式", "关");
                if (XML.read(config, "Mysql地址") == null) XML.write(config, "Mysql地址", "127.0.0.1");
                if (XML.read(config, "Mysql端口") == null) XML.write(config, "Mysql端口", "3306");
                if (XML.read(config, "Mysql账户") == null) XML.write(config, "Mysql账户", "root");
                if (XML.read(config, "Mysql密码") == null) XML.write(config, "Mysql密码", "123456");
                if (XML.read(config, "Mysql启用") == null) XML.write(config, "Mysql启用", "关");
                if (XML.read(config, "检测头") == null) XML.write(config, "检测头", "#");
            }

            if (File.Exists(path + mute) == false)
            {
                XML.CreateFile(mute, 0);
            }

            if (File.Exists(path + admin) == false)
            {               
                XML.write(admin, "发送给的人", "");
            }

            if (File.Exists(path + player) == false)
            {
                XML.CreateFile(player, 0);
            }

            if (File.Exists(path + Event) == false)
            {
                XML.write(Event, "更新？", "false");
                XML.write(Event, "事件-群员加入", "欢迎新人%player%，输入【" 
                    + XML.read(config, "检测头") + "服务器菜单】获取更多帮助。");
                XML.write(Event, "事件-群员退出", "%player%退出了群");
                XML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                XML.write(Event, "在线人数", "在线人数");
                XML.write(Event, "服务器状态", "服务器状态");
                XML.write(Event, "绑定文本", "绑定：");
                XML.write(Event, "发送文本", "服务器：");
                XML.write(Event, "禁言文本", "禁言：");
                XML.write(Event, "解禁文本", "解禁：");
                XML.write(Event, "查询玩家ID", "查询：");
                XML.write(Event, "修改玩家ID", "修改：");
                XML.write(Event, "维护文本", "服务器维护");
                XML.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                XML.write(Event, "机器人功能-重读配置文件", "重读文件");
                XML.write(Event, "机器人功能-内存回收", "内存回收");
            }
            if (XML.read(Event, "更新？") != "false")
            {
                XML.write(Event, "更新？", "false");
                if (XML.read(Event, "事件-群员加入") == null) XML.write(Event, "事件-群员加入",
                    "欢迎新人%player%，输入【" + XML.read(config, "检测头") + "服务器菜单】获取更多帮助。");
                if (XML.read(Event, "事件-群员退出") == null) XML.write(Event, "事件-群员退出", "%player%退出了群");
                if (XML.read(Event, "事件-踢出群员") == null) XML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                if (XML.read(Event, "在线人数") == null) XML.write(Event, "在线人数", "在线人数");
                if (XML.read(Event, "服务器状态") == null) XML.write(Event, "服务器状态", "服务器状态");
                if (XML.read(Event, "绑定文本") == null) XML.write(Event, "绑定文本", "绑定：");
                if (XML.read(Event, "发送文本") == null) XML.write(Event, "发送文本", "服务器：");
                if (XML.read(Event, "禁言文本") == null) XML.write(Event, "禁言文本", "禁言：");
                if (XML.read(Event, "解禁文本") == null) XML.write(Event, "解禁文本", "解禁：");
                if (XML.read(Event, "查询玩家ID") == null) XML.write(Event, "查询玩家ID", "查询：");
                if (XML.read(Event, "修改玩家ID") == null) XML.write(Event, "修改玩家ID", "修改：");
                if (XML.read(Event, "维护文本") == null) XML.write(Event, "维护文本", "服务器维护");
                if (XML.read(Event, "服务器维护文本") == null) XML.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (XML.read(Event, "机器人功能-重读配置文件") == null) XML.write(Event, "机器人功能-重读配置文件", "重读文件");
                if (XML.read(Event, "机器人功能-内存回收") == null) XML.write(Event, "机器人功能-内存回收", "内存回收");
            }

            if (File.Exists(path + message) == false)
            {
                XML.write(message, "启用", "true");
                XML.write(message, "服务器菜单", "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本")
                    + "】可以绑定你的游戏ID。\r\n【" + XML.read(config, "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(config, "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
            if (XML.read(message, "更新？") == null)
            {
                XML.write(message, "更新？", "false");
                if (XML.read(message, "服务器菜单") == null) XML.write(message, "服务器菜单",
                    "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【"
                    + XML.read(config, "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(config, "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(config, "检测头") + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
        }
        public static void reload()
        {
            use.IP = XML.read(config, "IP");
            use.Port = XML.read(config, "Port");
            use.group1 = XML.read(config, "群号1");
            use.group2 = XML.read(config, "群号2");
            use.group3 = XML.read(config, "群号3");
            use.ANSI = XML.read(config, "编码");
            use.send = XML.read(config, "发送消息");
            use.send_text = XML.read(config, "发送文本");
            use.fix_mode = XML.read(config, "维护模式");
            use.group2_mode = XML.read(config, "群2发送消息");
            use.group3_mode = XML.read(config, "群3发送消息");
            use.Mysql_mode = XML.read(config, "Mysql启用");
            use.Mysql_IP = XML.read(config, "Mysql地址");
            use.Mysql_Port = XML.read(config, "Mysql端口");
            use.Mysql_User = XML.read(config, "Mysql账户");
            use.Mysql_Password = XML.read(config, "Mysql密码");
            use.head = XML.read(config, "检测头");
            use.event_join_message = XML.read(Event, "事件-群员加入");
            use.event_quit_message = XML.read(Event, "事件-群员退出");
            use.event_kick_message = XML.read(Event, "事件-踢出群员");
            use.online_players_message = XML.read(Event, "在线人数");
            use.online_servers_message = XML.read(Event, "服务器状态");
            use.player_setid_message = XML.read(Event, "绑定文本");
            use.send_message = XML.read(Event, "发送文本");
            use.mute_message = XML.read(Event, "禁言文本");
            use.unmute_message = XML.read(Event, "解禁文本");
            use.check_id_message = XML.read(Event, "查询玩家ID");
            use.rename_id_message = XML.read(Event, "修改玩家ID");
            use.fix_message = XML.read(Event, "维护文本");
            use.fix_send_message = XML.read(Event, "服务器维护文本");
            use.reload_message = XML.read(Event, "机器人功能-重读配置文件");
            use.gc_message = XML.read(Event, "机器人功能-内存回收");
        }
    }
}
