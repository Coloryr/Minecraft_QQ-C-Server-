using Native.Csharp.App;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    public class config_read
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
        public static bool debug_mode;
        public static bool group2_mode;
        public static bool group3_mode;
        public static bool Mysql_mode;
        public static bool allways_send;

        public static string config = "config.xml";
        public static string player = "player.xml";
        public static string message = "message.xml";
        public static string commder = "commder.xml";

        public static string player_m;
        public static string message_m;
        public static string commder_m;

        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";

        public static void start()
        {
            config_read config_read = new config_read();
            config_read.start_read();
        }
        private void start_read()
        {
            setform frm = new setform();

            read_config();
            reload();
            if (group1 == "无" || Port == null || (socket.setip == null && socket.useip == true))
            {
                MessageBox.Show("参数错误，请设置");
                frm.ShowDialog();
                read_config();
                reload();
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
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]日志文件创建失败");
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
                logs logs = new logs();
                logs.Log_write("[INFO][Mysql]正在链接Mysql");
                Mysql_user sql = new Mysql_user();
                if (sql.mysql_start() == true)
                {
                    Minecraft_QQ.Mysql_mode = true;
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql已连接");
                    logs.Log_write("[INFO][Mysql]Mysql已连接");
                }
                else
                {
                    Minecraft_QQ.Mysql_mode = false;
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql错误，请检查");
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
                xml.write(config, "核心设置", "更新配置文件", "否");
                xml.write(config, "核心设置", "维护模式", "关");
                xml.write(config, "核心设置", "调试模式", "关");

                xml.write(config, "Socket", "地址", "127.0.0.1");
                xml.write(config, "Socket", "端口", "25555");
                xml.write(config, "Socket", "绑定IP", "关");
                xml.write(config, "Socket", "编码", "ANSI（GBK）");
                xml.write(config, "Socket", "数据包头", "[Head]");
                xml.write(config, "Socket", "数据包尾", "[End]");

                xml.write(config, "消息模式", "始终发送消息", "不！");
                xml.write(config, "消息模式", "发送文本", "%player%:%message%");
                xml.write(config, "消息模式", "发送颜色代码", "关");

                xml.write(config, "QQ群设置", "绑定群号1", "无");
                xml.write(config, "QQ群设置", "绑定群号2", "无");
                xml.write(config, "QQ群设置", "绑定群号3", "无");
                xml.write(config, "QQ群设置", "群2启用聊天", "否");
                xml.write(config, "QQ群设置", "群3启用聊天", "否");

                xml.write(config, "Mysql", "地址", "127.0.0.1");
                xml.write(config, "Mysql", "端口", "3306");
                xml.write(config, "Mysql", "账户", "root");
                xml.write(config, "Mysql", "密码", "123456");
                xml.write(config, "Mysql", "启用", "关");

                xml.write(config, "检测", "检测头", "#");
                xml.write(config, "检测", "在线人数", "在线人数");
                xml.write(config, "检测", "服务器状态", "服务器状态");
                xml.write(config, "检测", "绑定文本", "绑定：");
                xml.write(config, "检测", "发送文本", "服务器：");
                xml.write(config, "检测", "禁言文本", "禁言：");
                xml.write(config, "检测", "解禁文本", "解禁：");
                xml.write(config, "检测", "查询玩家ID", "查询：");
                xml.write(config, "检测", "修改玩家ID", "修改：");
                xml.write(config, "检测", "维护文本", "服务器维护");
                xml.write(config, "检测", "打开菜单", "打开菜单");
                xml.write(config, "检测", "服务器维护文本", "服务器正在维护，请等待维护结束！");
                xml.write(config, "检测", "重读配置文件", "重读文件");
                xml.write(config, "检测", "内存回收", "内存回收");
                xml.write(config, "检测", "未知指令", "未知指令");

                xml.write(config, "事件", "群员加入", "欢迎新人%player%，输入【"
    + xml.read(config, "检测", "检测头") + "服务器菜单】获取更多帮助。");
                xml.write(config, "事件", "群员退出", "%player%退出了群");
                xml.write(config, "事件", "踢出群员", "%player%感受制裁吧！");
            }
            else if (xml.read(config, "更新", "更新配置文件") == "是")
            {
                xml.write(config, "核心设置", "更新配置文件", "否");

                if (xml.read(config, "核心设置", "维护模式") == null)
                    xml.write(config, "核心设置", "维护模式", "关");
                if (xml.read(config, "核心设置", "调试模式") == null)
                    xml.write(config, "核心设置", "调试模式", "关");

                if (xml.read(config, "Socket", "地址") == null)
                    xml.write(config, "Socket", "地址", "127.0.0.1");
                if (xml.read(config, "Socket", "端口") == null)
                    xml.write(config, "Socket", "端口", "25555");
                if (xml.read(config, "Socket", "绑定IP") == null)
                    xml.write(config, "Socket", "绑定IP", "关");
                if (xml.read(config, "Socket", "编码") == null)
                    xml.write(config, "Socket", "编码", "ANSI（GBK）");
                if (xml.read(config, "Socket", "数据包头") == null)
                    xml.write(config, "Socket", "数据包头", "[Head]");
                if (xml.read(config, "Socket", "数据包尾") == null)
                    xml.write(config, "Socket", "数据包尾", "[End]");

                if (xml.read(config, "消息模式", "始终发送消息") == null)
                    xml.write(config, "消息模式", "始终发送消息", "不！");
                if (xml.read(config, "消息模式", "发送文本") == null)
                    xml.write(config, "消息模式", "发送文本", "%player%:%message%");
                if (xml.read(config, "消息模式", "发送颜色代码") == null)
                    xml.write(config, "消息模式", "发送颜色代码", "关");

                if (xml.read(config, "QQ群设置", "绑定群号1") == null)
                    xml.write(config, "QQ群设置", "绑定群号1", "无");
                if (xml.read(config, "QQ群设置", "绑定群号2") == null)
                    xml.write(config, "QQ群设置", "绑定群号2", "无");
                if (xml.read(config, "QQ群设置", "绑定群号3") == null)
                    xml.write(config, "QQ群设置", "绑定群号3", "无");
                if (xml.read(config, "QQ群设置", "群2启用聊天") == null)
                    xml.write(config, "QQ群设置", "群2启用聊天", "否");
                if (xml.read(config, "QQ群设置", "群3启用聊天") == null)
                    xml.write(config, "QQ群设置", "群3启用聊天", "否");

                if (xml.read(config, "Mysql", "地址") == null)
                    xml.write(config, "Mysql", "地址", "127.0.0.1");
                if (xml.read(config, "Mysql", "端口") == null)
                    xml.write(config, "Mysql", "端口", "3306");
                if (xml.read(config, "Mysql", "账户") == null)
                    xml.write(config, "Mysql", "账户", "root");
                if (xml.read(config, "Mysql", "密码") == null)
                    xml.write(config, "Mysql", "密码", "123456");
                if (xml.read(config, "Mysql", "启用") == null)
                    xml.write(config, "Mysql", "启用", "关");

                if (xml.read(config, "检测", "检测头") == null)
                    xml.write(config, "检测", "检测头", "#");
                if (xml.read(config, "检测", "在线人数") == null)
                    xml.write(config, "检测", "在线人数", "在线人数");
                if (xml.read(config, "检测", "服务器状态") == null)
                    xml.write(config, "检测", "服务器状态", "服务器状态");
                if (xml.read(config, "检测", "绑定文本") == null)
                    xml.write(config, "检测", "绑定文本", "绑定：");
                if (xml.read(config, "检测", "发送文本") == null)
                    xml.write(config, "检测", "发送文本", "服务器：");
                if (xml.read(config, "检测", "禁言文本") == null)
                    xml.write(config, "检测", "禁言文本", "禁言：");
                if (xml.read(config, "检测", "解禁文本") == null)
                    xml.write(config, "检测", "解禁文本", "解禁：");
                if (xml.read(config, "检测", "查询玩家ID") == null)
                    xml.write(config, "检测", "查询玩家ID", "查询：");
                if (xml.read(config, "检测", "修改玩家ID") == null)
                    xml.write(config, "检测", "修改玩家ID", "修改：");
                if (xml.read(config, "检测", "维护文本") == null)
                    xml.write(config, "检测", "维护文本", "服务器维护");
                if (xml.read(config, "检测", "打开菜单") == null)
                    xml.write(config, "检测", "打开菜单", "打开菜单");
                if (xml.read(config, "检测", "服务器维护文本") == null)
                    xml.write(config, "检测", "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (xml.read(config, "检测", "重读配置文件") == null)
                    xml.write(config, "检测", "重读配置文件", "重读文件");
                if (xml.read(config, "检测", "内存回收") == null)
                    xml.write(config, "检测", "内存回收", "内存回收");
                if (xml.read(config, "检测", "未知指令") == null)
                    xml.write(config, "检测", "未知指令", "未知指令");

                if (xml.read(config, "事件", "群员加入") == null)
                    xml.write(config, "事件", "群员加入", "欢迎新人%player%，输入【"
   + xml.read(config, "检测", "检测头") + "服务器菜单】获取更多帮助。");
                if (xml.read(config, "事件", "群员退出") == null)
                    xml.write(config, "事件", "群员退出", "%player%退出了群");
                if (xml.read(config, "事件", "踢出群员") == null)
                    xml.write(config, "事件", "踢出群员", "%player%感受制裁吧！");
            }

            string a;
            if (File.Exists(path + player) == false)
            {
                xml.CreateFile(player, 0);
            }
            else
            {
                StreamReader sr = new StreamReader(path + player, Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                if (!string.IsNullOrEmpty(a))
                    player_m = a;
                else
                    player_m = null;
            }

            if (File.Exists(path + message) == false)
            {
                xml.write(message, "核心配置", "启用", "是");
                xml.write(message, "自动回复消息", "服务器菜单", "服务器查询菜单：\r\n【" + xml.read(config, "检测", "绑定文本")
                    + "】可以绑定你的游戏ID。\r\n【" + xml.read(config, "检测", "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + xml.read(config, "检测", "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + xml.read(config, "检测", "发送文本") + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，输入"
                    + xml.read(config, "检测", "绑定文本") + "ID，来绑定ID）");
            }
            else
            {
                StreamReader sr = new StreamReader(path + message, System.Text.Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                if (!string.IsNullOrEmpty(a))
                    message_m = a;
                else
                    message_m = null;
            }

            if (File.Exists(path + commder) == false)
            {
                xml.write(commder, "核心配置", "启用", "是");
                xml.write(commder, "指令1", "指令", "qq help");
                xml.write(commder, "指令1", "触发", "插件帮助");
                xml.write(commder, "指令1", "玩家可用", "是");
            }
            else
            {
                StreamReader sr = new StreamReader(path + commder, System.Text.Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                if (!string.IsNullOrEmpty(a))
                    commder_m = a;
                else
                    commder_m = null;
            }
        }
        public void reload()
        {
            logs logs = new logs();
            logs.Log_write("[INFO][Config]读取配置文件中");
            XML xml = new XML();

            if (xml.read(message, "核心配置", "启用") == "是")
                message_enable = true;
            else
                message_enable = false;

            if (xml.read(config, "核心设置", "维护模式") == "开")
                fix_mode = true;
            else
                fix_mode = false;
            if (xml.read(config, "核心设置", "调试模式") == "开")
                debug_mode = true;
            else
                debug_mode = false;

            Port = xml.read(config, "Socket", "端口");
            socket.setip = xml.read(config, "Socket", "地址");
            if (xml.read(config, "Socket", "绑定IP") == "开")
                socket.useip = true;
            else
                socket.useip = false;
            ANSI = xml.read(config, "Socket", "编码");
            Color_yr.Minecraft_QQ.message.Head = xml.read(config, "Socket", "数据包头");
            Color_yr.Minecraft_QQ.message.End = xml.read(config, "Socket", "数据包尾");

            send_text = xml.read(config, "消息模式", "发送文本");
            if (xml.read(config, "消息模式", "始终发送消息") == "不！")
                allways_send = false;
            else
                allways_send = true;
            if (xml.read(config, "消息模式", "发送颜色代码") == "开")
                color_code = true;
            else
                color_code = false;

            group1 = xml.read(config, "QQ群设置", "绑定群号1");
            group2 = xml.read(config, "QQ群设置", "绑定群号1");
            group3 = xml.read(config, "QQ群设置", "绑定群号1");
            if (xml.read(config, "QQ群设置", "群2启用聊天") == "开")
                group2_mode = true;
            else
                group2_mode = false;
            if (xml.read(config, "QQ群设置", "群3启用聊天") == "开")
                group3_mode = true;
            else
                group3_mode = false;

            if (xml.read(config, "Mysql", "启用") == "开")
                Mysql_mode = true;
            else
                Mysql_mode = false;
            Mysql_user.Mysql_IP = xml.read(config, "Mysql", "地址");
            Mysql_user.Mysql_Port = xml.read(config, "Mysql", "端口");
            Mysql_user.Mysql_User = xml.read(config, "Mysql", "账户");
            Mysql_user.Mysql_Password = xml.read(config, "Mysql", "密码");

            head = xml.read(config, "检测", "检测头");
            online_players_message = xml.read(config, "检测", "在线人数").ToLower();
            online_servers_message = xml.read(config, "检测", "服务器状态").ToLower();
            player_setid_message = xml.read(config, "检测", "绑定文本").ToLower();
            send_message = xml.read(config, "检测", "发送文本").ToLower();
            mute_message = xml.read(config, "检测", "禁言文本").ToLower();
            unmute_message = xml.read(config, "检测", "解禁文本").ToLower();
            check_id_message = xml.read(config, "检测", "查询玩家ID").ToLower();
            rename_id_message = xml.read(config, "检测", "修改玩家ID").ToLower();
            fix_message = xml.read(config, "检测", "维护文本").ToLower();
            fix_send_message = xml.read(config, "检测", "服务器维护文本");
            reload_message = xml.read(config, "检测", "重读配置文件").ToLower();
            gc_message = xml.read(config, "检测", "内存回收").ToLower();
            menu_message = xml.read(config, "检测", "打开菜单").ToLower();
            unknow_message = xml.read(config, "检测", "未知指令");

            event_join_message = xml.read(config, "事件", "群员加入");
            event_quit_message = xml.read(config, "事件", "群员退出");
            event_kick_message = xml.read(config, "事件", "踢出群员");
        }
    }
}
