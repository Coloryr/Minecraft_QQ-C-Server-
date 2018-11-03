using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class use
    {
        public static string group1;
        public static string group2;
        public static string group3;
        public static string IP;
        public static string Port;
        public static string ANSI;
        public static string head;
        public static string send;
        public static string send_text;
        public static string fix_mode;
        public static string group2_mode;
        public static string group3_mode;
        public static string Mysql_mode;
        public static string Mysql_IP = "127.0.0.1";
        public static string Mysql_Port = "3306";
        public static string Mysql_User = "root";
        public static string Mysql_Password = "123456";
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
                return a.Substring(x);
        }
        public static string RemoveLeft(string s, int len)
        {
            return s.PadLeft(len).Remove(0, len);
        }

        public static bool IsNatural_Number(string str)
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

        public static string get_at(string a)
        {
            for (; a.IndexOf("[CQ:at,qq=") != -1;)
            {
                string b = get_string(a, "=", "]");
                string c = get_string(a, "[", "]");
                string d;
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    string e = Mysql.mysql_search(Mysql.Mysql_player, b);
                    if (e == null)
                        d = b;
                    else
                        d = e;
                }
                else
                {    
                    string e = XML.read(config_read.player, b);
                    if (e == null)
                        d = b;
                    else
                        d = e;
                }
                a = a.Replace(c, "@" + d + "");
            }
            if (a.IndexOf("CQ:at,qq=all") != -1)
                a.Replace("CQ:at,qq=all", "@全体人员");
            a = a.Replace("[", "").Replace("]", "");
            return a;
        }

        public static bool isok(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')//这是允许输入退格键
                return true;
            else if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                return false;
            return true;
        }
        public static string player_setid(long fromQQ, string msg)
        {
            string player = null;
            if (Minecraft_QQ.Mysql_mode == true)
                player = Mysql.mysql_search(Mysql.Mysql_player, fromQQ.ToString());
            else
                player = XML.read(config_read.player, fromQQ.ToString());
            if (player == null)
            {
                string player_name = msg.Replace(use.player_setid_message, "");
                if (player_name == " " || player_name == "" || use.IsNatural_Number(player_name) == false)
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();
                    if (Minecraft_QQ.Mysql_mode == true)
                    {
                        if (Mysql.mysql_search(Mysql.Mysql_notid, player_name.ToLower()) == "notid")                        
                            return "禁止绑定ID：" + player_name;
                        Mysql.mysql_add(Mysql.Mysql_player, fromQQ.ToString(), player_name.ToString());
                    }
                    else
                    {
                        if (XML.read(config_read.notid, player_name.ToLower()) == "notid")
                            return "禁止绑定ID：" + player_name;
                        XML.write(config_read.player, fromQQ.ToString(), player_name);
                    }

                    string qq_admin = XML.read(config_read.admin, "发送给的人");
                    if (qq_admin != null)
                        CQ.SendPrivateMessage(long.Parse(qq_admin), "玩家[" + CQ.GetQQName(fromQQ) + "]绑定了ID：[" + player_name + "]");
                    return "绑定ID：" + player_name + "成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
        }
        public static string player_mute(long fromQQ, string msg)
        {
            msg = msg.Replace(mute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name = null;
            if (player.IndexOf("[CQ:at,qq=") != -1)           
                if (Minecraft_QQ.Mysql_mode == true)
                    player_name = Mysql.mysql_search(Mysql.Mysql_player, player);
                else
                    player_name = XML.read(config_read.player, player);
            else
                player_name = player;
            if (player_name == null)
                return "ID无效";
            else
            {
                if (Minecraft_QQ.Mysql_mode == true)
                    Mysql.mysql_add(Mysql.Mysql_mute, player_name.ToLower(), "true");
                else
                    XML.write(config_read.mute, player_name.ToLower(), "true");
                return "已禁言：[" + player_name + "]";
            }
        }
        public static string player_unmute(long fromQQ, string msg)
        {
            msg = msg.Replace(unmute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name = null;
            if (player.IndexOf("[CQ:at,qq=") != -1)
            {             
                if (Minecraft_QQ.Mysql_mode == true)
                    player_name = Mysql.mysql_search(Mysql.Mysql_player, player);
                else
                    player_name = XML.read(config_read.player, player);
            }
            else
                player_name = player;
            if (player_name == null)
                return "玩家无ID";
            else
            {
                if (Minecraft_QQ.Mysql_mode == true)
                    Mysql.mysql_add(Mysql.Mysql_mute, player_name.ToLower(), "false");
                else
                    XML.write(config_read.mute, player_name.ToLower(), "false");
                return "已解禁：[" + player_name + "]";
            }
        }
        public static string player_checkid(long fromQQ, string msg)
        {
            msg = msg.Replace(check_id_message, "");
            string player;
            bool is_me;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                player = get_string(msg, "=", "]");
                is_me = false;
            }
            else
            {
                player = fromQQ.ToString();
                is_me = true;
            }
            string player_name = null;
            if (Minecraft_QQ.Mysql_mode == true)
                player_name = Mysql.mysql_search(Mysql.Mysql_player, player);
            else
                player_name = XML.read(player, player);
            if (player_name == null)
            {
                if (is_me == true)
                    return "你没有绑定ID";
                else
                    return "玩家" + player + "没有绑定ID";
            }
            else
            {
                if (is_me == true)
                    return "你绑定的ID为：" + player_name;
                else
                    return "玩家" + player + "绑定的ID为：" + player_name;
            }
        }
        public static string player_rename(long fromQQ, string msg)
        {
            msg = msg.Replace(rename_id_message, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player = get_string(msg, "=", "]");
                string player_name = null;
                player_name = get_string(msg, "]");
                player_name = player_name.Trim();
                if (Minecraft_QQ.Mysql_mode == true)
                    Mysql.mysql_add(Mysql.Mysql_player, player, player_name);
                else
                    XML.write(player, player, player_name);
                return "已修改玩家[" + player + "]ID为：" + player_name;
            }
            else
                return "玩家错误，请检查";
        }
        public static string fix_mode_change()
        {
            if (XML.read(config_read.config, "维护模式") == "关")
            {
                XML.write(config_read.config, "维护模式", "开");
                fix_mode = "开";
                Minecraft_QQ.server = false;
                return "服务器维护模式已开启";
            }
            else
            {
                XML.write(config_read.config, "维护模式", "关");
                fix_mode = "关";
                Minecraft_QQ.server = true;
                return "服务器维护模式已关闭";
            }
        }
        public static string online(long fromGroup)
        {
            if (Minecraft_QQ.server == true)
            {
                if (socket.ready == true)
                {
                    if (fromGroup == Minecraft_QQ.GroupSet1) Minecraft_QQ.Group = 1;
                    else if (fromGroup == Minecraft_QQ.GroupSet2) Minecraft_QQ.Group = 2;
                    else if (fromGroup == Minecraft_QQ.GroupSet3) Minecraft_QQ.Group = 3;
                    socket.Send("在线人数", socket.MCserver);
                }
                else
                {
                    return "发送失败，服务器未准备好";
                }
            }
            else
            {
                return fix_send_message;
            }
            return null;
        }
        public static string server(long fromGroup)
        {
            if (Minecraft_QQ.server == true)
            {
                if (socket.ready == true)
                {
                    if (fromGroup == Minecraft_QQ.GroupSet1) Minecraft_QQ.Group = 1;
                    else if (fromGroup == Minecraft_QQ.GroupSet2) Minecraft_QQ.Group = 2;
                    else if (fromGroup == Minecraft_QQ.GroupSet3) Minecraft_QQ.Group = 3;
                    socket.Send("服务器状态", socket.MCserver);
                }
                else
                {
                    return "发送失败，服务器未准备好";
                }
            }
            else
            {
                return fix_send_message;
            }
            return null;
        }
    }
}
