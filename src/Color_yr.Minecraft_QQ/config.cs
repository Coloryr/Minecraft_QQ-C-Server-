using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Minecraft_QQ/";//AppDomain.CurrentDomain.SetupInformation.ApplicationBase

        public static void read_config()
        {
            FormSettings frm = new FormSettings();
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
            }
            if (XML.read(config, "更新？") != "false")
            {
                XML.write(config, "更新？", "false");
                if (XML.read(config, "IP") == "") XML.write(config, "IP", "127.0.0.1");
                if (XML.read(config, "Port") == "") XML.write(config, "Port", "25555");
                if (XML.read(config, "编码") == "") XML.write(config, "编码", "ANSI（GBK）");
                if (XML.read(config, "发送消息") == "") XML.write(config, "发送消息", "不！");
                if (XML.read(config, "发送文本") == "") XML.write(config, "发送文本", "%player%:%message%");
                if (XML.read(config, "维护模式") == "") XML.write(config, "维护模式", "关");
                if (XML.read(config, "Mysql地址") == "") XML.write(config, "Mysql地址", "127.0.0.1");
                if (XML.read(config, "Mysql端口") == "") XML.write(config, "Mysql端口", "3306");
                if (XML.read(config, "Mysql账户") == "") XML.write(config, "Mysql账户", "root");
                if (XML.read(config, "Mysql密码") == "") XML.write(config, "Mysql密码", "123456");
                if (XML.read(config, "Mysql启用") == "") XML.write(config, "Mysql启用", "关");
            }

            if (File.Exists(path + mute) == false)
            { XML.write(mute, "预留位", "预留位"); }

            if (File.Exists(path + admin) == false)
            { XML.write(admin, "发送给的人", ""); }

            if (File.Exists(path + player) == false)
            { XML.write(player, "预留位", "预留位"); }

            if (File.Exists(path + Event) == false)
            {
                XML.write(Event, "更新？", "false");
                XML.write(Event, "事件-群员加入", "欢迎新人%player%，输入【%服务器菜单】获取更多帮助。");
                XML.write(Event, "事件-群员退出", "%player%退出了群");
                XML.write(Event, "事件-文件上传", "%player%上传了文件%file%");
                XML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                XML.write(Event, "在线人数", "%online%");
                XML.write(Event, "服务器状态", "%server_online%");
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
                XML.write(Event, "启用群内绑定", "启用");
            }
            if (XML.read(Event, "更新？") != "false")
            {
                XML.write(Event, "更新？", "false");
                if (XML.read(Event, "事件-群员加入") == "") XML.write(Event, "事件-群员加入", "欢迎新人%player%，输入【%服务器菜单】获取更多帮助。");
                if (XML.read(Event, "事件-群员退出") == "") XML.write(Event, "事件-群员退出", "%player%退出了群");
                if (XML.read(Event, "事件-文件上传") == "") XML.write(Event, "事件-文件上传", "%player%上传了文件%file%");
                if (XML.read(Event, "事件-踢出群员") == "") XML.write(Event, "事件-踢出群员", "%player%感受制裁吧！");
                if (XML.read(Event, "在线人数") == "") XML.write(Event, "在线人数", "%online%");
                if (XML.read(Event, "服务器状态") == "") XML.write(Event, "服务器状态", "%server_online%");
                if (XML.read(Event, "绑定文本") == "") XML.write(Event, "绑定文本", "绑定：");
                if (XML.read(Event, "发送文本") == "") XML.write(Event, "发送文本", "服务器：");
                if (XML.read(Event, "禁言文本") == "") XML.write(Event, "禁言文本", "禁言：");
                if (XML.read(Event, "解禁文本") == "") XML.write(Event, "解禁文本", "解禁：");
                if (XML.read(Event, "查询玩家ID") == "") XML.write(Event, "查询玩家ID", "查询：");
                if (XML.read(Event, "修改玩家ID") == "") XML.write(Event, "修改玩家ID", "修改：");
                if (XML.read(Event, "修改玩家ID") == "") XML.write(Event, "修改玩家ID", "修改：");
                if (XML.read(Event, "维护文本") == "") XML.write(Event, "维护文本", "服务器维护");
                if (XML.read(Event, "服务器维护文本") == "") XML.write(Event, "服务器维护文本", "服务器正在维护，请等待维护结束！");
                if (XML.read(Event, "机器人功能-重读配置文件") == "") XML.write(Event, "机器人功能-重读配置文件", "重读文件");
                if (XML.read(Event, "机器人功能-内存回收") == "") XML.write(Event, "机器人功能-内存回收", "内存回收");
                if (XML.read(Event, "启用群内绑定") == "") XML.write(Event, "启用群内绑定", "启用");
            }

            if (File.Exists(path + message) == false)
            {
                XML.write(message, "启用", "true");
                XML.write(message, "%服务器菜单", "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【在线人数】可以查询服务器在线人数。\r\n【服务器状态】可以查询服务器是否在运行。\r\n【" + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }
            if (XML.read(message, "启用") == "")
            {
                if (XML.read(message, "%服务器菜单") == "") XML.write(message, "%服务器菜单", "服务器查询菜单：\r\n【" + XML.read(Event, "绑定文本") + "】可以绑定你的游戏ID。\r\n【在线人数】可以查询服务器在线人数。\r\n【服务器状态】可以查询服务器是否在运行。\r\n【" + XML.read(Event, "发送文本") + "内容】可以向服务器里发送消息。");
            }

            string check = XML.read(config, "群号1");
            if (check == "") { MessageBox.Show("未设置群号，请设置"); frm.ShowDialog(); }
            else { Minecraft_QQ.GroupSet1 = long.Parse(check); }
            Minecraft_QQ.ipaddress = XML.read(config, "IP");
            if (Minecraft_QQ.ipaddress == "")
            { MessageBox.Show("未设置IP，请设置"); frm.ShowDialog(); }
            else { Minecraft_QQ.ipaddress = XML.read(config, "IP"); }
            check = XML.read(config, "Port");
            if (check == "")
            { MessageBox.Show("未设置端口，请设置"); frm.ShowDialog(); }
            else { Minecraft_QQ.Port = int.Parse(check); }

            CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]正在启动");

            check = XML.read(config, "群号2");
            if (check != "")
                Minecraft_QQ.GroupSet2 = long.Parse(check);
            else
                Minecraft_QQ.GroupSet2 = 0;
            check = XML.read(config, "群号3");
            if (check != "")
                Minecraft_QQ.GroupSet3 = long.Parse(check);
            else
                Minecraft_QQ.GroupSet3 = 0;

            if (!File.Exists(path + logs.log))
            {
                File.WriteAllText(path + logs.log, "正在尝试创建文件" + Environment.NewLine);
            }

            if (XML.read(config, "维护模式") == "关") Minecraft_QQ.server = true;
            else Minecraft_QQ.server = false;

            if (XML.read(config, "群2发送消息") == "开") Minecraft_QQ.Group2_on = true;
            else Minecraft_QQ.Group2_on = false;
            if (XML.read(config, "群3发送消息") == "开") Minecraft_QQ.Group3_on = true;
            else Minecraft_QQ.Group3_on = false;

            if (XML.read(config, "Mysql启用") == "开")
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]正在链接Mysql");
                if (Mysql.mysql_start() == true)
                {
                    Minecraft_QQ.Mysql_mode = true;
                }
                else
                {
                    Minecraft_QQ.Mysql_mode = false;
                    MessageBox.Show("Mysql错误，请检查");
                }
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Mysql已连接");
            }


        }
    }
}
