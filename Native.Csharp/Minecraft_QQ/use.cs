using Native.Csharp.App;
using Native.Csharp.Sdk.Cqp.Model;
using System;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class use
    {
        public string RemoveColorCodes(string text)
        {
            if (!text.Contains("§"))

            {
                return text;
            }

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

        public string get_string(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + b.Length;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c);
                return a.Substring(x, y - x);
            }
            else
                return a.Substring(x);
        }

        public bool IsNumber(string str)
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

        public string remove_pic(string a)
        {
            for (; a.IndexOf("[CQ:image") != -1;)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;图片&#93;");
            }
            return a;
        }

        public string get_at(string a)
        {
            for (; a.IndexOf("[CQ:at,qq=") != -1;)
            {
                string b = get_string(a, "=", "]");
                string c = get_string(a, "[", "]");
                string d;
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    string e = Mysql_user.mysql_search(Mysql_user.Mysql_player, b);
                    if (e == null)
                        d = b;
                    else
                        d = e;
                }
                else
                {
                    XML XML = new XML();
                    string e = XML.read(config_read.player, "玩家", b);
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
        public string CQ_code(string a)
        {
            for (; a.IndexOf("&#91;") != -1;)
            {
                a = a.Replace("&#91;", "[");
            }
            for (; a.IndexOf("&#93;") != -1;)
            {
                a = a.Replace("&#93;", "]");
            }
            return a;
        }
        public string code_CQ(string a)
        {
            for (; a.IndexOf("[") != -1;)
            {
                a = a.Replace("[", "&#91;");
            }
            for (; a.IndexOf("]") != -1;)
            {
                a = a.Replace("]", "&#93;");
            }
            for (; a.IndexOf(",") != -1;)
            {
                a = a.Replace(",", "&#44;");
            }
            return a;
        }

        public bool key_ok(KeyEventArgs e)
        {
            if (e.Control == true)          //按下了ctrl
            {
                if (e.KeyData == Keys.V || e.KeyData == Keys.C)
                    return true;
            }
            else if (e.KeyCode == Keys.Back)//这是允许输入退格键
                return true;
            if (e.KeyData == Keys.D0 || e.KeyData == Keys.D1 || e.KeyData == Keys.D2 || e.KeyData == Keys.D3 || e.KeyData == Keys.D4 ||
                e.KeyData == Keys.D5 || e.KeyData == Keys.D6 || e.KeyData == Keys.D7 || e.KeyData == Keys.D8 || e.KeyData == Keys.D9)
                return true;
            return false;
        }

        public static bool check_mute(string player)
        {
            if (Minecraft_QQ.Mysql_mode == true)
                if (Mysql_user.mysql_search(Mysql_user.Mysql_mute, player.ToLower()) == "true")
                    return true;
                else if (Minecraft_QQ.Mysql_mode == false)
                {
                    XML XML = new XML();
                    if (XML.read(config_read.player, "禁言", player.ToLower()) == "true")
                        return true;
                }
            return false;
        }

        public string player_setid(long fromQQ, string msg)
        {
            string player = null;
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            if (Minecraft_QQ.Mysql_mode == true)
                player = Mysql_user.mysql_search(Mysql_user.Mysql_player, fromQQ.ToString());
            else
            {
                XML XML = new XML();
                player = XML.read(config_read.player, "玩家", fromQQ.ToString());
            }
            if (player == null)
            {
                string player_name = msg.Replace(config_read.player_setid_message, "");
                if (player_name == " " || player_name == "")
                    return "ID无效，请检查";
                else
                {
                    XML XML = new XML();
                    player_name = player_name.Trim();
                    if (Minecraft_QQ.Mysql_mode == true)
                    {
                        if (Mysql_user.mysql_search(Mysql_user.Mysql_notid, player_name.ToLower()) == "notid")
                            return "禁止绑定ID：" + player_name;
                        Mysql_user sql = new Mysql_user();
                        sql.mysql_add(Mysql_user.Mysql_player, fromQQ.ToString(), player_name.ToString());
                    }
                    else
                    {
                        if (XML.read(config_read.player, "禁止绑定", player_name.ToLower()) == "notid")
                            return "禁止绑定ID：" + player_name;
                        XML.write(config_read.player, "禁止绑定", fromQQ.ToString(), player_name);
                    }

                    string qq_admin = XML.read(config_read.player, "管理员", "发送给的人");
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
        public string player_mute(long fromQQ, string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.mute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name = null;
            if (player.IndexOf("[CQ:at,qq=") != -1)
                if (Minecraft_QQ.Mysql_mode == true)
                    player_name = Mysql_user.mysql_search(Mysql_user.Mysql_player, player);
                else
                {
                    XML XML = new XML();
                    player_name = XML.read(config_read.player, "玩家", player);
                }
            else
                player_name = player;
            if (player_name == null)
                return "ID无效";
            else
            {
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    Mysql_user sql = new Mysql_user();
                    sql.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "true");
                }
                else
                {
                    XML xml = new XML();
                    xml.write(config_read.player, "禁言", player_name.ToLower(), "true");
                }
                return "已禁言：[" + player_name + "]";
            }
        }
        public string player_unmute(long fromQQ, string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.unmute_message, "");
            string player = get_string(msg, "=", "]");
            string player_name = null;
            if (player.IndexOf("[CQ:at,qq=") != -1)
            {
                if (Minecraft_QQ.Mysql_mode == true)
                    player_name = Mysql_user.mysql_search(Mysql_user.Mysql_player, player);
                else
                {
                    XML XML = new XML();
                    player_name = XML.read(config_read.player, "玩家", player);
                }
            }
            else
                player_name = player;
            if (player_name == null)
                return "玩家无ID";
            else
            {
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    Mysql_user sql = new Mysql_user();
                    sql.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "false");
                }
                else
                {
                    XML xml = new XML();
                    xml.write(config_read.player, "禁言", player_name.ToLower(), "false");
                }
                return "已解禁：[" + player_name + "]";
            }
        }
        public string player_checkid(long fromQQ, string msg)
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
            string player_name = null;
            if (Minecraft_QQ.Mysql_mode == true)
                player_name = Mysql_user.mysql_search(Mysql_user.Mysql_player, player);
            else
            {
                XML XML = new XML();
                player_name = XML.read(config_read.player, "玩家", player);
            }
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
        public string player_rename(long fromQQ, string msg)
        {
            if (msg.IndexOf(config_read.head) == 0)
                msg = msg.Replace(config_read.head, null);
            msg = msg.Replace(config_read.rename_id_message, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player = get_string(msg, "=", "]");
                string player_name = null;
                player_name = get_string(msg, "]");
                player_name = player_name.Trim();
                if (Minecraft_QQ.Mysql_mode == true)
                {
                    Mysql_user sql = new Mysql_user();
                    sql.mysql_add(Mysql_user.Mysql_player, player, player_name);
                }
                else
                {
                    XML xml = new XML();
                    xml.write(config_read.player, "玩家", player, player_name);
                }
                return "已修改玩家[" + player + "]ID为：" + player_name;
            }
            else
                return "玩家错误，请检查";
        }
        public string fix_mode_change()
        {
            XML XML = new XML();
            logs logs = new logs();
            if (XML.read(config_read.config, "核心设置", "维护模式") == "关")
            {
                XML.write(config_read.config, "核心设置", "维护模式", "开");
                config_read.fix_mode = true;
                Minecraft_QQ.server = false;
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已开启");
                return "服务器维护模式已开启";
            }
            else
            {
                XML.write(config_read.config, "核心设置", "维护模式", "关");
                config_read.fix_mode = false;
                Minecraft_QQ.server = true;
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已关闭");
                return "服务器维护模式已关闭";
            }
        }
        public string online(long fromGroup)
        {
            if (Minecraft_QQ.server == true)
            {
                if (socket.ready == true)
                {
                    messagelist messagelist = new messagelist();
                    messagelist.group = fromGroup.ToString();
                    messagelist.message = "在线人数";
                    messagelist.is_commder = false;
                    messagelist.player = null;
                    socket.Send(messagelist, socket.MCserver);
                }
                else
                {
                    return "发送失败，服务器未准备好";
                }
            }
            else
            {
                return config_read.fix_send_message;
            }
            return null;
        }
        public string server(long fromGroup)
        {
            if (Minecraft_QQ.server == true)
            {
                if (socket.ready == true)
                {
                    messagelist messagelist = new messagelist();
                    messagelist.group = fromGroup.ToString();
                    messagelist.message = "服务器状态";
                    messagelist.is_commder = false;
                    messagelist.player = null;
                    socket.Send(messagelist, socket.MCserver);
                }
                else
                {
                    return "发送失败，服务器未准备好";
                }
            }
            else
            {
                return config_read.fix_send_message;
            }
            return null;
        }

        public bool GC_now()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return true;
            }
            catch (Exception e)
            {
                logs logs = new logs();
                logs.Log_write("[ERROR]" + e.ToString());
                return false;
            }
        }

        public bool commder_check(long fromGroup, string msg, long fromQQ)
        {
            XML XML = new XML();
            if (XML.read_memory(config_read.commder_m, "核心配置", "启用") != "是")
                return false;
            string a;
            int i = 0;
            while (true)
            {
                i++;
                a = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "触发");
                if (a == msg)
                {
                    if (XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "玩家可用") == "是" && socket.ready == true)
                    {
                        if (socket.ready == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                            return false;
                        }
                        messagelist messagelist = new messagelist();
                        messagelist.group = fromGroup.ToString();
                        messagelist.message = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "指令");
                        messagelist.is_commder = true;
                        messagelist.player = fromQQ.ToString();
                        socket.Send(messagelist, socket.MCserver);
                        return true;
                    }
                    else if (XML.read_memory(config_read.player_m, "管理员", "admin" + fromQQ.ToString()) == "true" && socket.ready == true)
                    {
                        if (socket.ready == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                            return false;
                        }
                        messagelist messagelist = new messagelist();
                        messagelist.group = fromGroup.ToString();
                        messagelist.message = XML.read_memory(config_read.commder_m, "指令" + i.ToString(), "指令");
                        messagelist.is_commder = true;
                        messagelist.player = fromQQ.ToString();
                        socket.Send(messagelist, socket.MCserver);
                        return true;
                    }
                }
                else
                    return false;
            }
        }
    }
}
