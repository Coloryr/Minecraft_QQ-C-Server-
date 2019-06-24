using Native.Csharp.App;
using Native.Csharp.Sdk.Cqp.Model;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class use
    {
        public static string RemoveColorCodes(string text)
        {
            if (!text.Contains("§"))
                return text;
            var sb = new StringBuilder(text);
            sb.Replace("§0", string.Empty);
            sb.Replace("§1", string.Empty);
            sb.Replace("§2", string.Empty);
            sb.Replace("§3", string.Empty);
            sb.Replace("§4", string.Empty);
            sb.Replace("§5", string.Empty);
            sb.Replace("§6", string.Empty);
            sb.Replace("§7", string.Empty);
            sb.Replace("§8", string.Empty);
            sb.Replace("§9", string.Empty);
            sb.Replace("§a", string.Empty);
            sb.Replace("§b", string.Empty);
            sb.Replace("§c", string.Empty);
            sb.Replace("§d", string.Empty);
            sb.Replace("§e", string.Empty);
            sb.Replace("§f", string.Empty);
            sb.Replace("§r", string.Empty);
            sb.Replace("§k", string.Empty);
            sb.Replace("§n", string.Empty);
            sb.Replace("§m", string.Empty);
            sb.Replace("&0", string.Empty);
            sb.Replace("&1", string.Empty);
            sb.Replace("&2", string.Empty);
            sb.Replace("&3", string.Empty);
            sb.Replace("&4", string.Empty);
            sb.Replace("&5", string.Empty);
            sb.Replace("&6", string.Empty);
            sb.Replace("&7", string.Empty);
            sb.Replace("&8", string.Empty);
            sb.Replace("&9", string.Empty);
            sb.Replace("&a", string.Empty);
            sb.Replace("&b", string.Empty);
            sb.Replace("&c", string.Empty);
            sb.Replace("&d", string.Empty);
            sb.Replace("&e", string.Empty);
            sb.Replace("&f", string.Empty);
            sb.Replace("&r", string.Empty);
            sb.Replace("&k", string.Empty);
            sb.Replace("&n", string.Empty);
            sb.Replace("&m", string.Empty);

            return sb.ToString();
        }
        public static string ReplaceFirst(string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.IndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }
        public static string get_string(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + b.Length;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c);
                if (y - x == 0)
                    return a;
                else
                    return a.Substring(x, y - x);
            }
            else
                return a.Substring(x);
        }

        public static bool IsNumber(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 127)
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
                a = a.Replace("[]", "&#91;图片&#93;");
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
                if (config_read.Mysql_mode == true)
                {
                    string e = Mysql_user.mysql_search(Mysql_user.Mysql_player, b);
                    if (e == null)
                        d = b;
                    else
                        d = e;
                }
                else
                {
                    string e = check_player_name(b);
                    if (e == null)
                        d = b;
                    else
                        d = e;
                }
                a = a.Replace(c, "@" + d + "");
            }
            if (a.IndexOf("[CQ:at,qq=all") != -1)
                a.Replace("CQ:at,qq=all", "@全体人员");
            a = a.Replace("[", "").Replace("]", "");
            return a;
        }
        public static string CQ_code(string a)
        {
            for (; a.IndexOf("&#91;") != -1;)
                a = a.Replace("&#91;", "[");
            for (; a.IndexOf("&#93;") != -1;)
                a = a.Replace("&#93;", "]");
            return a;
        }
        public static string code_CQ(string a)
        {
            for (; a.IndexOf("[") != -1;)
                a = a.Replace("[", "&#91;");
            for (; a.IndexOf("]") != -1;)
                a = a.Replace("]", "&#93;");
            for (; a.IndexOf(",") != -1;)
                a = a.Replace(",", "&#44;");
            return a;
        }

        public static bool key_ok(KeyEventArgs e)
        {
            if (e.Control == true)          //按下了ctrl
                if (e.KeyData == Keys.V || e.KeyData == Keys.C)
                    return true;
                else if (e.KeyCode == Keys.Back)//这是允许输入退格键
                    return true;
            if (e.KeyData == Keys.D0 || e.KeyData == Keys.D1 || e.KeyData == Keys.D2 || e.KeyData == Keys.D3 || e.KeyData == Keys.D4 ||
                e.KeyData == Keys.D5 || e.KeyData == Keys.D6 || e.KeyData == Keys.D7 || e.KeyData == Keys.D8 || e.KeyData == Keys.D9)
                return true;
            return false;
        }

        public static bool check_mute(string player)
        {
            if (config_read.Mysql_mode == true)
                if (Mysql_user.mysql_search(Mysql_user.Mysql_mute, player.ToLower()) == "true")
                    return true;
                else if (config_read.Mysql_mode == false)
                {
                    if (XML.read_memory(config_read.player_m, "QQ" + player.ToLower(), "禁言") == "是")
                        return true;
                }
            return false;
        }

        public static bool check_admin(string player)
        {
            if (XML.read_memory(config_read.player_m, "QQ" + player.ToLower(), "管理员") == "是")
                return true;
            return false;
        }
        public static string check_player_name(string player_qq)
        {
            if (config_read.Mysql_mode == true)
                return Mysql_user.mysql_search(Mysql_user.Mysql_player, player_qq);
            else
                return XML.read_memory(config_read.player_m, "QQ" + player_qq, "绑定");
        }

        public static string player_setid(long fromQQ, string msg)
        {
            string player;
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            player = check_player_name(fromQQ.ToString());
            if (player == null)
            {
                string player_name = msg.Replace(config_read.player_setid_message, "");
                if (player_name == " " || player_name == "")
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();
                    if (config_read.Mysql_mode == true)
                    {
                        if (Mysql_user.mysql_search(Mysql_user.Mysql_notid, player_name.ToLower()) == "notid")
                            return "禁止绑定ID：" + player_name;
                        if (Mysql_user.mysql_search_id(Mysql_user.Mysql_player, player_name.ToLower()) != null)
                            return "ID：" + player_name + "已经被绑定过了";
                        Mysql_user.mysql_add(Mysql_user.Mysql_player, fromQQ.ToString(), player_name.ToString());
                        
                    }
                    else
                    {
                        if (XML.read_memory(config_read.player_m, "ID" + player_name, "禁止绑定") == "是")
                            return "禁止绑定ID：" + player_name;
                        if(XML.read_id(config_read.player, player_name) == true)
                            return "ID：" + player_name + "已经被绑定过了";
                        XML.write(config_read.player, "QQ" + fromQQ.ToString(), "绑定", player_name);
                        StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                        config_read.player_m = sr.ReadToEnd().TrimStart();
                        sr.Close();
                    }

                    string qq_admin = XML.read_memory(config_read.player_m, "管理员", "发送给的人");
                    if (qq_admin != null)
                    {
                        QQ qqInfo;
                        Common.CqApi.GetQQInfo(fromQQ, out qqInfo);
                        Common.CqApi.SendPrivateMessage(long.Parse(qq_admin), "玩家[" + qqInfo.Nick + "]绑定了ID：[" + player_name + "]");
                    }
                    return "绑定ID：" + player_name + "成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
        }
        public static string player_mute(string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.mute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name;
            if (player.IndexOf("[CQ:at,qq=") != -1)
                player_name = check_player_name(player.ToString());
            else
                player_name = player;
            if (player_name == null)
                return "ID无效";
            else
            {
                if (config_read.Mysql_mode == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "true");
                else
                {
                    XML.write(config_read.player, "QQ" + player_name.ToLower(), "禁言", "是");
                    StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                    config_read.player_m = sr.ReadToEnd().TrimStart();
                    sr.Close();
                }
                return "已禁言：[" + player_name + "]";
            }
        }
        public static string player_unmute(string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.unmute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name;
            if (player.IndexOf("[CQ:at,qq=") != -1)
                player_name = check_player_name(player);
            else
                player_name = player;
            if (player_name == null)
                return "玩家无ID";
            else
            {
                if (config_read.Mysql_mode == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "false");
                else
                {
                    XML.write(config_read.player, player_name.ToLower(), "禁言", "否");
                    StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                    config_read.player_m = sr.ReadToEnd().TrimStart();
                    sr.Close();
                }
                return "已解禁：[" + player_name + "]";
            }
        }
        public static string player_checkid(long fromQQ, string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.check_id_message, "");
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
            string player_name = check_player_name(player);
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
        public static string player_rename(string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.rename_id_message, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player = get_string(msg, "=", "]");
                string player_name;
                player_name = get_string(msg, "]");
                player_name = player_name.Trim();
                if (config_read.Mysql_mode == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_player, player, player_name);
                else
                {
                    XML.write(config_read.player, "QQ" + player, "绑定", player_name);
                    StreamReader sr = new StreamReader(config_read.path + config_read.player, Encoding.Default);
                    config_read.player_m = sr.ReadToEnd().TrimStart();
                    sr.Close();
                }
                return "已修改玩家[" + player + "]ID为：" + player_name;
            }
            else
                return "玩家错误，请检查";
        }
        public static string fix_mode_change()
        {
            if (config_read.fix_mode == false)
            {
                XML.write(config_read.config, "核心设置", "维护模式", "开");
                config_read.fix_mode = true;
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已开启");
                return "服务器维护模式已开启";
            }
            else
            {
                XML.write(config_read.config, "核心设置", "维护模式", "关");
                config_read.fix_mode = false;
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已关闭");
                return "服务器维护模式已关闭";
            }
        }
        public static string online(long fromGroup)
        {
            if (config_read.fix_mode == false)
            {
                if (socket.ready == true)
                {
                    messagelist messagelist = new messagelist();
                    messagelist.group = fromGroup.ToString();
                    messagelist.message = "在线人数";
                    messagelist.is_commder = false;
                    messagelist.player = null;
                    socket.Send(messagelist);
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return config_read.fix_send_message;
            return null;
        }
        public static string server(long fromGroup)
        {
            if (config_read.fix_mode == false)
            {
                if (socket.ready == true)
                {
                    messagelist messagelist = new messagelist();
                    messagelist.group = fromGroup.ToString();
                    messagelist.message = "服务器状态";
                    messagelist.is_commder = false;
                    messagelist.player = null;
                    socket.Send(messagelist);
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return config_read.fix_send_message;
            return null;
        }

        public static bool GC_now()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return true;
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR]" + e.ToString());
                return false;
            }
        }

        public static bool commder_check(long fromGroup, string msg, long fromQQ)
        {
            if (XML.read_memory(config_read.commder_m, "核心配置", "启用") != "是")
            {
                return false;
            }
            string a;
            int i = 0;
            while (true)
            {
                i++;
                a = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "触发");
                if (a == null)
                    return false;
                if (msg.IndexOf(a) == 0)
                {
                    if (XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "玩家可用") == "是"
                        || check_admin(fromQQ.ToString()) == true)
                    {
                        if (socket.ready == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                            return false;
                        }
                        messagelist messagelist = new messagelist();
                        messagelist.group = fromGroup.ToString();
                        if (XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "附带参数") == "是")
                        {
                            string b = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "指令");
                            b = b.Replace("%playername%", check_player_name(fromQQ.ToString()));
                            msg = msg.Replace(a, "");
                            messagelist.message = b + msg;
                        }
                        else
                        {
                            messagelist.message = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "指令");
                            messagelist.message = messagelist.message.Replace("%playername%", check_player_name(fromQQ.ToString()));
                        }
                        messagelist.is_commder = true;
                        if (XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "玩家发送") == "是")
                        {
                            messagelist.player = check_player_name(fromQQ.ToString());
                            if (messagelist.player == null)
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "你未绑定ID");
                                return true;
                            }
                        }
                            
                        else
                            messagelist.player = "后台";
                        socket.Send(messagelist);
                        return true;
                    }
                }
            }
        }
    }
}
