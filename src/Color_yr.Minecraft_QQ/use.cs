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
            {
                return a.Substring(x);
            }
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
            {
                a.Replace("CQ:at,qq=all", "@全体人员");
            }
            a = a.Replace("[", "").Replace("]", "");
            return a;
        }
        public static bool isok(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')//这是允许输入退格键
                return true;
            else if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
            {
                return false;
            }
            return true;
        }
    }
}
