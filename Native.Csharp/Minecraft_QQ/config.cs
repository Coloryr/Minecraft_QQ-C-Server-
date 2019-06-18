using Native.Csharp.App;
using System;
using System.IO;
using System.Text;
using System.Threading;
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

        public static string data_Head;
        public static string data_End;

        public static bool message_enable;
        public static bool color_code;
        public static bool fix_mode;
        public static bool debug_mode;
        public static bool group2_mode;
        public static bool group3_mode;
        public static bool Mysql_mode;
        public static bool allways_send;
        public static bool set_name = true;

        public static string config = "config.xml";
        public static string player = "player.xml";
        public static string message = "message.xml";
        public static string commder = "commder.xml";

        public static string player_m;
        public static string message_m;
        public static string commder_m;

        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";

        public static long GroupSet1;    //QQ群号1
        public static long GroupSet2;    //QQ群号2
        public static long GroupSet3;    //QQ群号3

        public static Thread start = new Thread(start_read);

        public static void start_read()
        {
            read_config();
            GroupSet1 = 0;
            GroupSet2 = 0;
            GroupSet3 = 0;
            if (group1 == "无" || Port == null || (socket.setip == null && socket.useip == true))
            {
                setform frm = new setform();
                MessageBox.Show("参数错误，请设置");
                frm.ShowDialog();
                read_config();
            }
            else
            {
                long.TryParse(group1, out GroupSet1);
                socket.Port = int.Parse(Port);
            }
            long.TryParse(group2, out GroupSet2);
            long.TryParse(group3, out GroupSet3);

            if (GroupSet1 == 0)
            {
                MessageBox.Show("[Minecraft_QQ]群1设置错误请修改后重载应用");
                return;
            }

            if (!File.Exists(path + logs.log))
            {
                try
                {
                    File.WriteAllText(path + logs.log, "正在尝试创建日志文件" + Environment.NewLine);
                }
                catch (Exception)
                {
                    Common.CqApi.SendGroupMessage(GroupSet1, "[Minecraft_QQ]日志文件创建失败");
                }
            }

            if (Mysql_mode == true)
            {

                logs.Log_write("[INFO][Mysql]正在链接Mysql");
                if (Mysql_user.mysql_start() == true)
                {
                    Mysql_mode = true;
                    Common.CqApi.SendGroupMessage(GroupSet1, "[Minecraft_QQ]Mysql已连接");
                    logs.Log_write("[INFO][Mysql]Mysql已连接");
                }
                else
                {
                    Mysql_mode = false;
                    Common.CqApi.SendGroupMessage(GroupSet1, "[Minecraft_QQ]Mysql错误，请检查");
                    logs.Log_write("[ERROR][Mysql]Mysql错误，请检查");
                }
            }
            else
                Mysql_mode = false;
            socket.Start_socket();
        }

        public static void read_config()
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            if (File.Exists(path + config) == false)
            {
                XML.write(config, "核心设置", "更新配置文件", "否");
                XML.write(config, "核心设置", "维护模式", "关");
                XML.write(config, "核心设置", "调试模式", "关");

                XML.write(config, "Socket", "地址", "127.0.0.1");
                XML.write(config, "Socket", "端口", "25555");
                XML.write(config, "Socket", "绑定IP", "关");
                XML.write(config, "Socket", "编码", "ANSI（GBK）");
                XML.write(config, "Socket", "数据包头", "[Head]");
                XML.write(config, "Socket", "数据包尾", "[End]");

                XML.write(config, "消息模式", "始终发送消息", "不！");
                XML.write(config, "消息模式", "发送文本", "%player%:%message%");
                XML.write(config, "消息模式", "发送颜色代码", "关");

                XML.write(config, "QQ群设置", "绑定群号1", "无");
                XML.write(config, "QQ群设置", "绑定群号2", "无");
                XML.write(config, "QQ群设置", "绑定群号3", "无");
                XML.write(config, "QQ群设置", "群2启用聊天", "否");
                XML.write(config, "QQ群设置", "群3启用聊天", "否");

                XML.write(config, "Mysql", "地址", "127.0.0.1");
                XML.write(config, "Mysql", "端口", "3306");
                XML.write(config, "Mysql", "账户", "root");
                XML.write(config, "Mysql", "密码", "123456");
                XML.write(config, "Mysql", "启用", "关");

                XML.write(config, "检测", "检测头", "#");
                XML.write(config, "检测", "在线人数", "在线人数");
                XML.write(config, "检测", "服务器状态", "服务器状态");
                XML.write(config, "检测", "绑定文本", "绑定：");
                XML.write(config, "检测", "发送文本", "服务器：");
                XML.write(config, "检测", "禁言文本", "禁言：");
                XML.write(config, "检测", "解禁文本", "解禁：");
                XML.write(config, "检测", "查询玩家ID", "查询：");
                XML.write(config, "检测", "修改玩家ID", "修改：");
                XML.write(config, "检测", "维护文本", "服务器维护");
                XML.write(config, "检测", "打开菜单", "打开菜单");
                XML.write(config, "检测", "服务器维护文本", "服务器正在维护，请等待维护结束！");
                XML.write(config, "检测", "重读配置文件", "重读文件");
                XML.write(config, "检测", "内存回收", "内存回收");
                XML.write(config, "检测", "未知指令", "未知指令");

            }
            else if (XML.read(config, "更新", "更新配置文件") == "是")
            {
                XML.write(config, "核心设置", "更新配置文件", "否");

                if (XML.read(config, "核心设置", "维护模式") == null)
                    XML.write(config, "核心设置", "维护模式", "关");
                if (XML.read(config, "核心设置", "调试模式") == null)
                    XML.write(config, "核心设置", "调试模式", "关");

                if (XML.read(config, "Socket", "地址") == null)
                    XML.write(config, "Socket", "地址", "127.0.0.1");
                if (XML.read(config, "Socket", "端口") == null)
                    XML.write(config, "Socket", "端口", "25555");
                if (XML.read(config, "Socket", "绑定IP") == null)
                    XML.write(config, "Socket", "绑定IP", "关");
                if (XML.read(config, "Socket", "编码") == null)
                    XML.write(config, "Socket", "编码", "ANSI（GBK）");
                if (XML.read(config, "Socket", "数据包头") == null)
                    XML.write(config, "Socket", "数据包头", "[Head]");
                if (XML.read(config, "Socket", "数据包尾") == null)
                    XML.write(config, "Socket", "数据包尾", "[End]");

                if (XML.read(config, "消息模式", "始终发送消息") == null)
                    XML.write(config, "消息模式", "始终发送消息", "不！");
                if (XML.read(config, "消息模式", "发送文本") == null)
                    XML.write(config, "消息模式", "发送文本", "%player%:%message%");
                if (XML.read(config, "消息模式", "发送颜色代码") == null)
                    XML.write(config, "消息模式", "发送颜色代码", "关");

                if (XML.read(config, "QQ群设置", "绑定群号1") == null)
                    XML.write(config, "QQ群设置", "绑定群号1", "无");
                if (XML.read(config, "QQ群设置", "绑定群号2") == null)
                    XML.write(config, "QQ群设置", "绑定群号2", "无");
                if (XML.read(config, "QQ群设置", "绑定群号3") == null)
                    XML.write(config, "QQ群设置", "绑定群号3", "无");
                if (XML.read(config, "QQ群设置", "群2启用聊天") == null)
                    XML.write(config, "QQ群设置", "群2启用聊天", "否");
                if (XML.read(config, "QQ群设置", "群3启用聊天") == null)
                    XML.write(config, "QQ群设置", "群3启用聊天", "否");

                if (XML.read(config, "Mysql", "地址") == null)
                    XML.write(config, "Mysql", "地址", "127.0.0.1");
                if (XML.read(config, "Mysql", "端口") == null)
                    XML.write(config, "Mysql", "端口", "3306");
                if (XML.read(config, "Mysql", "账户") == null)
                    XML.write(config, "Mysql", "账户", "root");
                if (XML.read(config, "Mysql", "密码") == null)
                    XML.write(config, "Mysql", "密码", "123456");
                if (XML.read(config, "Mysql", "启用") == null)
                    XML.write(config, "Mysql", "启用", "关");

                if (XML.read(config, "检测", "检测头") == null)
                    XML.write(config, "检测", "检测头", "#");
                if (XML.read(config, "检测", "在线人数") == null)
                    XML.write(config, "检测", "在线人数", "在线人数");
                if (XML.read(config, "检测", "服务器状态") == null)
                    XML.write(config, "检测", "服务器状态", "服务器状态");
                if (XML.read(config, "检测", "绑定文本") == null)
                    XML.write(config, "检测", "绑定文本", "绑定：");
                if (XML.read(config, "检测", "发送文本") == null)
                    XML.write(config, "检测", "发送文本", "服务器：");
                if (XML.read(config, "检测", "禁言文本") == null)
                    XML.write(config, "检测", "禁言文本", "禁言：");
                if (XML.read(config, "检测", "解禁文本") == null)
                    XML.write(config, "检测", "解禁文本", "解禁：");
                if (XML.read(config, "检测", "查询玩家ID") == null)
                    XML.write(config, "检测", "查询玩家ID", "查询：");
                if (XML.read(config, "检测", "修改玩家ID") == null)
                    XML.write(config, "检测", "修改玩家ID", "修改：");
                if (XML.read(config, "检测", "维护文本") == null)
                    XML.write(config, "检测", "维护文本", "服务器维护");
                if (XML.read(config, "检测", "打开菜单") == null)
                    XML.write(config, "检测", "打开菜单", "打开菜单");
                if (XML.read(config, "检测", "服务器维护文本") == null)
                    XML.write(config, "检测", "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (XML.read(config, "检测", "重读配置文件") == null)
                    XML.write(config, "检测", "重读配置文件", "重读文件");
                if (XML.read(config, "检测", "内存回收") == null)
                    XML.write(config, "检测", "内存回收", "内存回收");
                if (XML.read(config, "检测", "未知指令") == null)
                    XML.write(config, "检测", "未知指令", "未知指令");
            }

            string a;
            if (File.Exists(path + player) == false)
            {
                XML.write(player, "测试", "测试", "测试");
            }
            else
            {
                StreamReader sr = new StreamReader(path + player, Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                sr.Close();
                if (!string.IsNullOrEmpty(a))
                    player_m = a;
                else
                    player_m = null;
            }

            if (File.Exists(path + message) == false)
            {
                XML.write(message, "核心配置", "启用", "是");
                XML.write(message, "自动回复消息", "服务器菜单", "服务器查询菜单：\r\n【" + XML.read(config, "检测", "绑定文本")
                    + "】可以绑定你的游戏ID。\r\n【" + XML.read(config, "检测", "检测头") + "在线人数】可以查询服务器在线人数。\r\n【"
                    + XML.read(config, "检测", "检测头") + "服务器状态】可以查询服务器是否在运行。\r\n【"
                    + XML.read(config, "检测", "发送文本") + "内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，输入"
                    + XML.read(config, "检测", "绑定文本") + "ID，来绑定ID）");
            }
            else
            {
                StreamReader sr = new StreamReader(path + message, System.Text.Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                sr.Close();
                if (!string.IsNullOrEmpty(a))
                    message_m = a;
                else
                    message_m = null;
            }

            if (File.Exists(path + commder) == false)
            {
                XML.write(commder, "核心配置", "启用", "是");

                XML.write(commder, "指令1", "指令", "qq help");
                XML.write(commder, "指令1", "触发", "插件帮助");
                XML.write(commder, "指令1", "玩家可用", "是");
                XML.write(commder, "指令1", "附带参数", "否");
                XML.write(commder, "指令1", "玩家发送", "否");

                XML.write(commder, "指令2", "指令", "money %playername%");
                XML.write(commder, "指令2", "触发", "查钱");
                XML.write(commder, "指令2", "玩家可用", "是");
                XML.write(commder, "指令2", "附带参数", "否");
                XML.write(commder, "指令2", "玩家发送", "否");

                XML.write(commder, "指令3", "指令", "tp ");
                XML.write(commder, "指令3", "触发", "tp玩家");
                XML.write(commder, "指令3", "玩家可用", "是");
                XML.write(commder, "指令3", "附带参数", "是");
                XML.write(commder, "指令3", "玩家发送", "是");

                XML.write(commder, "指令4", "指令", "mute ");
                XML.write(commder, "指令4", "触发", "禁言");
                XML.write(commder, "指令4", "玩家可用", "否");
                XML.write(commder, "指令4", "附带参数", "是");
                XML.write(commder, "指令4", "玩家发送", "否");
            }
            else
            {
                StreamReader sr = new StreamReader(path + commder, Encoding.Default);
                a = sr.ReadToEnd().TrimStart();
                sr.Close();
                if (!string.IsNullOrEmpty(a))
                    commder_m = a;
                else
                    commder_m = null;
            }

            logs.Log_write("[INFO][Config]读取配置文件中");

            if (XML.read(message, "核心配置", "启用") == "是")
                message_enable = true;
            else
                message_enable = false;

            if (XML.read(config, "核心设置", "维护模式") == "开")
                fix_mode = true;
            else
                fix_mode = false;
            if (XML.read(config, "核心设置", "调试模式") == "开")
                debug_mode = true;
            else
                debug_mode = false;

            Port = XML.read(config, "Socket", "端口");
            socket.setip = XML.read(config, "Socket", "地址");
            if (XML.read(config, "Socket", "绑定IP") == "开")
                socket.useip = true;
            else
                socket.useip = false;
            ANSI = XML.read(config, "Socket", "编码");
            data_Head = XML.read(config, "Socket", "数据包头");
            data_End = XML.read(config, "Socket", "数据包尾");

            send_text = XML.read(config, "消息模式", "发送文本");
            if (XML.read(config, "消息模式", "始终发送消息") == "不！")
                allways_send = false;
            else
                allways_send = true;
            if (XML.read(config, "消息模式", "发送颜色代码") == "开")
                color_code = true;
            else
                color_code = false;

            group1 = XML.read(config, "QQ群设置", "绑定群号1");
            group2 = XML.read(config, "QQ群设置", "绑定群号2");
            group3 = XML.read(config, "QQ群设置", "绑定群号3");
            if (XML.read(config, "QQ群设置", "群2启用聊天") == "开")
                group2_mode = true;
            else
                group2_mode = false;
            if (XML.read(config, "QQ群设置", "群3启用聊天") == "开")
                group3_mode = true;
            else
                group3_mode = false;

            if (XML.read(config, "Mysql", "启用") == "开")
                Mysql_mode = true;
            else
                Mysql_mode = false;
            Mysql_user.Mysql_IP = XML.read(config, "Mysql", "地址");
            Mysql_user.Mysql_Port = XML.read(config, "Mysql", "端口");
            Mysql_user.Mysql_User = XML.read(config, "Mysql", "账户");
            Mysql_user.Mysql_Password = XML.read(config, "Mysql", "密码");

            head = XML.read(config, "检测", "检测头");
            online_players_message = XML.read(config, "检测", "在线人数").ToLower();
            online_servers_message = XML.read(config, "检测", "服务器状态").ToLower();
            player_setid_message = XML.read(config, "检测", "绑定文本").ToLower();
            send_message = XML.read(config, "检测", "发送文本").ToLower();
            mute_message = XML.read(config, "检测", "禁言文本").ToLower();
            unmute_message = XML.read(config, "检测", "解禁文本").ToLower();
            check_id_message = XML.read(config, "检测", "查询玩家ID").ToLower();

            rename_id_message = XML.read(config, "检测", "修改玩家ID").ToLower();
            fix_message = XML.read(config, "检测", "维护文本").ToLower();
            fix_send_message = XML.read(config, "检测", "服务器维护文本");
            reload_message = XML.read(config, "检测", "重读配置文件").ToLower();
            gc_message = XML.read(config, "检测", "内存回收").ToLower();
            menu_message = XML.read(config, "检测", "打开菜单").ToLower();
            unknow_message = XML.read(config, "检测", "未知指令");
        }
    }
}
