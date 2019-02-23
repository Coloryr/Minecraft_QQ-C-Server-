using Flexlive.CQP.Framework;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class config_read
    {
        public static string group1;
        public static string group2;
        public static string group3;
        public static string Port;
        public static string ANSI;
        public static string head;       
        public static string send_text;      
        public static string event_join_message;
        public static string event_quit_message;
        public static string event_kick_message;
        public static string online_players_message;
        public static string online_servers_message;
        public static string player_setid_message;
        public static string send_message;
        public static string mute_message;
        public static string unmute_message;
        public static string check_id_message;
        public static string rename_id_message;
        public static string fix_message;
        public static string fix_send_message;
        public static string reload_message;
        public static string gc_message;
        public static string menu_message;
        public static string unknow_message;

        public static bool message_enable;
        public static bool color_code;
        public static bool fix_mode;
        public static bool group2_mode;
        public static bool group3_mode;
        public static bool Mysql_mode;
        public static bool allways_send;

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
            config_read read = new config_read();
            setform frm = new setform();

            CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]启动中");

            read.read_config();
            read.reload();
            if (group1 == null || Port == null || (socket.setip == null && socket.useip == true))
            {
                MessageBox.Show("参数错误，请设置");
                frm.ShowDialog();
                read.read_config();
                read.reload();
            }
            else
            {
                Minecraft_QQ.GroupSet1 = long.Parse(group1);
                socket.Port = int.Parse(Port);
            }

            if (group2 != null)
                Minecraft_QQ.GroupSet2 = long.Parse(group2);
            else
                Minecraft_QQ.GroupSet2 = 0;
            if (group3 != null)
                Minecraft_QQ.GroupSet3 = long.Parse(group3);
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

            if (fix_mode == false) Minecraft_QQ.server = true;
            else Minecraft_QQ.server = false;

            if (group2_mode == true) Minecraft_QQ.Group2_on = true;
            else Minecraft_QQ.Group2_on = false;
            if (group3_mode == true) Minecraft_QQ.Group3_on = true;
            else Minecraft_QQ.Group3_on = false;

            if (Mysql_mode == true)
            {
                logs.Log_write("[INFO][Mysql]正在链接Mysql");
                Mysql_user sql = new Mysql_user();
                if (sql.mysql_start() == true)
                {
                    Minecraft_QQ.Mysql_mode = true;
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql已连接");
                    logs.Log_write("[INFO][Mysql]Mysql已连接");
                }
                else
                {
                    Minecraft_QQ.Mysql_mode = false;
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql错误，请检查");
                    logs.Log_write("[ERROR][Mysql]Mysql错误，请检查");
                }
            }
            else
            {
                Minecraft_QQ.Mysql_mode = false;
            }
            socket socket_start = new socket();
            socket_start.Start_socket();
        }

        public void read_config()
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            XML xml = new XML();

            if (File.Exists(path + config) == false)
            {
                xml.write(config, "更新？", "false");
                xml.write(config, "IP", "127.0.0.1");
                xml.write(config, "Port", "25555");
                xml.write(config, "绑定IP", "关");
                xml.write(config, "编码", "ANSI（GBK）");
                xml.write(config, "发送消息", "不！");
                xml.write(config, "发送文本", "%player%:%message%");
                xml.write(config, "维护模式", "关");
                xml.write(config, "Mysql地址", "127.0.0.1");
                xml.write(config, "Mysql端口", "3306");
                xml.write(config, "Mysql账户", "root");
                xml.write(config, "Mysql密码", "123456");
                xml.write(config, "Mysql启用", "关");
                xml.write(config, "检测头", "#");
                xml.write(config, "发送颜色代码", "关");
                xml.write(config, "数据包头", "[Head]");
                xml.write(config, "数据包尾", "[End]");
            }
            else if (XML.read(config, "更新？") != "false")
            {
                xml.write(config, "更新？", "false");
                if (XML.read(config, "IP") == null) xml.write(config, "IP", "127.0.0.1");
                if (XML.read(config, "Port") == null) xml.write(config, "Port", "25555");
                if (XML.read(config, "绑定IP") == null) xml.write(config, "绑定IP", "关");
                if (XML.read(config, "编码") == null) xml.write(config, "编码", "ANSI（GBK）");
                if (XML.read(config, "发送消息") == null) xml.write(config, "发送消息", "不！");
                if (XML.read(config, "发送文本") == null) xml.write(config, "发送文本", "%player%:%message%");
                if (XML.read(config, "维护模式") == null) xml.write(config, "维护模式", "关");
                if (XML.read(config, "Mysql地址") == null) xml.write(config, "Mysql地址", "127.0.0.1");
                if (XML.read(config, "Mysql端口") == null) xml.write(config, "Mysql端口", "3306");
                if (XML.read(config, "Mysql账户") == null) xml.write(config, "Mysql账户", "root");
                if (XML.read(config, "Mysql密码") == null) xml.write(config, "Mysql密码", "123456");
                if (XML.read(config, "Mysql启用") == null) xml.write(config, "Mysql启用", "关");
                if (XML.read(config, "检测头") == null) xml.write(config, "检测头", "#");
                if (XML.read(config, "发送颜色代码") == null) xml.write(config, "发送颜色代码", "关");
                if (XML.read(config, "数据包头") == null) xml.write(config, "数据包头", "[Head]");
                if (XML.read(config, "数据包尾") == null) xml.write(config, "数据包尾", "[End]");
            }

            if (File.Exists(path + mute) == false)
            {
                xml.CreateFile(mute, 0);
            }

            if (File.Exists(path + admin) == false)
            {
                xml.write(admin, "发送给的人", "");
            }

            if (File.Exists(path + player) == false)
            {
                xml.CreateFile(player, 0);
            }

            if (File.Exists(path + Event) == false)
            {
                xml.write(Event, "更新？", "false");
                xml.write(Event, "事件-群员加入", "欢迎新人%player%，输入【"
                    + XML.read(config, "检测头") + "服务器菜单】获取更多帮助。");
                xml.write(Event, "事件-群员退出", "%player%退出了群");
                xml.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                xml.write(Event, "在线人数", "在线人数");
                xml.write(Event, "服务器状态", "服务器状态");
                xml.write(Event, "绑定文本", "绑定：");
                xml.write(Event, "发送文本", "服务器：");
                xml.write(Event, "禁言文本", "禁言：");
                xml.write(Event, "解禁文本", "解禁：");
                xml.write(Event, "查询玩家ID", "查询：");
                xml.write(Event, "修改玩家ID", "修改：");
                xml.write(Event, "维护文本", "服务器维护");
                xml.write(Event, "打开菜单", "打开菜单");
                xml.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                xml.write(Event, "机器人功能-重读配置文件", "重读文件");
                xml.write(Event, "机器人功能-内存回收", "内存回收");
                xml.write(Event, "未知指令", "未知指令");
            }
            else if (XML.read(Event, "更新？") != "false")
            {
                xml.write(Event, "更新？", "false");
                if (XML.read(Event, "事件-群员加入") == null) xml.write(Event, "事件-群员加入",
                    "欢迎新人%player%，输入【" + XML.read(config, "检测头") + "服务器菜单】获取更多帮助。");
                if (XML.read(Event, "事件-群员退出") == null) xml.write(Event, "事件-群员退出", "%player%退出了群");
                if (XML.read(Event, "事件-踢出群员") == null) xml.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                if (XML.read(Event, "在线人数") == null) xml.write(Event, "在线人数", "在线人数");
                if (XML.read(Event, "服务器状态") == null) xml.write(Event, "服务器状态", "服务器状态");
                if (XML.read(Event, "绑定文本") == null) xml.write(Event, "绑定文本", "绑定：");
                if (XML.read(Event, "发送文本") == null) xml.write(Event, "发送文本", "服务器：");
                if (XML.read(Event, "禁言文本") == null) xml.write(Event, "禁言文本", "禁言：");
                if (XML.read(Event, "解禁文本") == null) xml.write(Event, "解禁文本", "解禁：");
                if (XML.read(Event, "查询玩家ID") == null) xml.write(Event, "查询玩家ID", "查询：");
                if (XML.read(Event, "修改玩家ID") == null) xml.write(Event, "修改玩家ID", "修改：");
                if (XML.read(Event, "维护文本") == null) xml.write(Event, "维护文本", "服务器维护");
                if (XML.read(Event, "打开菜单") == null) xml.write(Event, "打开菜单", "打开菜单");
                if (XML.read(Event, "服务器维护文本") == null) xml.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (XML.read(Event, "机器人功能-重读配置文件") == null) xml.write(Event, "机器人功能-重读配置文件", "重读文件");
                if (XML.read(Event, "机器人功能-内存回收") == null) xml.write(Event, "机器人功能-内存回收", "内存回收");
                if (XML.read(Event, "未知指令") == null) xml.write(Event, "未知指令", "未知指令");
            }

            if (File.Exists(path + message) == false)
            {
                xml.write(message, "启用", "true");
                xml.write(message, "服务器菜单", "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本")
                    + "】可以绑定你的游戏ID。\r\n【" + XML.read(config, "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(config, "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
            else if (XML.read(message, "更新？") == null)
            {
                xml.write(message, "更新？", "false");
                if (XML.read(message, "服务器菜单") == null) xml.write(message, "服务器菜单",
                    "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【"
                    + XML.read(config, "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(config, "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(config, "检测头") + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
        }
        public void reload()
        {
            logs.Log_write("[INFO][Config]读取配置文件中");
            if (XML.read(message, "启用") == "true")
                message_enable = true;
            else
                message_enable = false;
            Port = XML.read(config, "Port");
            socket.setip = XML.read(config, "IP");
            if (XML.read(config, "绑定IP") == "开")
                socket.useip = true;
            else
                socket.useip = false;
            group1 = XML.read(config, "群号1");
            group2 = XML.read(config, "群号2");
            group3 = XML.read(config, "群号3");
            ANSI = XML.read(config, "编码");
            if (XML.read(config, "发送消息") == "不！")
                allways_send = false;
            else
                allways_send = true;
            send_text = XML.read(config, "发送文本");
            if (XML.read(config, "维护模式") == "开")
                fix_mode = true;
            else
                fix_mode = false;
            if (XML.read(config, "群2发送消息") == "开")
                group2_mode = true;
            else
                group2_mode = false;
            if (XML.read(config, "群3发送消息") == "开")
                group3_mode = true;
            else
                group3_mode = false;
            if (XML.read(config, "Mysql启用") == "开")
                Mysql_mode = true;
            else
                Mysql_mode = false;
            Mysql_user.Mysql_IP = XML.read(config, "Mysql地址");
            Mysql_user.Mysql_Port = XML.read(config, "Mysql端口");
            Mysql_user.Mysql_User = XML.read(config, "Mysql账户");
            Mysql_user.Mysql_Password = XML.read(config, "Mysql密码");
            head = XML.read(config, "检测头");
            Color_yr.Minecraft_QQ.message.Head = XML.read(config, "数据包头");
            Color_yr.Minecraft_QQ.message.End = XML.read(config, "数据包尾");
            event_join_message = XML.read(Event, "事件-群员加入");
            event_quit_message = XML.read(Event, "事件-群员退出");
            event_kick_message = XML.read(Event, "事件-踢出群员");
            online_players_message = XML.read(Event, "在线人数").ToLower();
            online_servers_message = XML.read(Event, "服务器状态").ToLower();
            player_setid_message = XML.read(Event, "绑定文本").ToLower();
            send_message = XML.read(Event, "发送文本").ToLower();
            mute_message = XML.read(Event, "禁言文本").ToLower();
            unmute_message = XML.read(Event, "解禁文本").ToLower();
            check_id_message = XML.read(Event, "查询玩家ID").ToLower();
            rename_id_message = XML.read(Event, "修改玩家ID").ToLower();
            fix_message = XML.read(Event, "维护文本").ToLower();
            fix_send_message = XML.read(Event, "服务器维护文本");
            reload_message = XML.read(Event, "机器人功能-重读配置文件").ToLower();
            gc_message = XML.read(Event, "机器人功能-内存回收").ToLower();
            menu_message = XML.read(Event, "打开菜单").ToLower();
            unknow_message = XML.read(Event, "未知指令");
            if (XML.read(config, "发送颜色代码") == "开")
                color_code = true;
            else
                color_code = false;
        }
    }
}
