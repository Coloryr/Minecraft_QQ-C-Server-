using Native.Csharp.App;
using Native.Csharp.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class use
    {
        public static string RemoveColorCodes(string text)
        {
            if (text.Contains("§") || text.Contains("&"))
            {
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
            else
                return text;
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
                if (y - x <= 0)
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
            while (a.IndexOf("[CQ:image") != -1)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;图片&#93;");
            }
            while (a.IndexOf("[CQ:face") != -1)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;表情&#93;");
            }
            while (a.IndexOf("[CQ:emoji") != -1)
            {
                string b = get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;表情&#93;");
            }
            return a;
        }
        public static string get_at(string a)
        {
            while (a.IndexOf("CQ:at,qq=") != -1)
            {
                string b = get_string(a, "=", "]");
                string c = get_string(a, "[", "]");
                string d;
                if (mysql_config.enable == true)
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
            if (a.IndexOf("CQ:at,qq=all") != -1)
                a.Replace("CQ:at,qq=all", "@全体人员");
            a = a.Replace("[", "").Replace("]", "");
            return a;
        }
        public static string anno(string a)
        {
            string title = null;
            string json_string;
            string text = null;
            try
            {
                title = get_string(a, "title=", ",content");
                json_string = "{" + get_string(a, ":{", "}") + "}";
                json_string = json_string.Replace("&#44;", ",");
                JObject jsonData = JObject.Parse(json_string);
                text = jsonData["text"].ToString();
                byte[] bytes = Convert.FromBase64String(text);
                text = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch { }
            return title + "：\n" + text;
        }
        public static string CQ_code(string a)
        {
            for (; a.IndexOf("&#91;") != -1;)
                a = a.Replace("&#91;", "[");
            for (; a.IndexOf("&#93;") != -1;)
                a = a.Replace("&#93;", "]");
            for (; a.IndexOf("&#44;") != -1;)
                a = a.Replace("&#44;", ",");
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
        public static bool check_mute(string player_name)
        {
            if (mysql_config.enable == true)
                if (Mysql_user.mysql_search(Mysql_user.Mysql_mute, player_name.ToLower()) == "true")
                    return true;
                else
                {
                    player_save player = config_read.read_player_form_id(player_name);
                    if (player != null)
                        return player.mute;
                }
            return false;
        }
        public static bool check_admin(string player_name)
        {
            player_save player = config_read.read_player_form_id(player_name);
            if (player != null)
                return player.admin;
            return false;
        }
        public static string check_player_name(string player_qq)
        {
            if (mysql_config.enable == true)
                return Mysql_user.mysql_search(Mysql_user.Mysql_player, player_qq);
            else
            {
                int.TryParse(player_qq, out int qq);
                if (config_file.player_list.ContainsKey(qq) == true)
                    return config_file.player_list[qq].player;
            }
            return null;
        }
        public static string get_nick(string player_name)
        {
            player_save player = config_read.read_player_form_id(player_name);
            if (player != null)
                return player.nick;
            return null;
        }
        public static string set_nick(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.nick, "");
            string player_name;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
                player_name = check_player_name(get_string(msg, "=", "]").ToString());
            else
                player_name = msg;
            if (player_name == null)
                return "ID无效";
            else
            {
                string nick = get_string(msg, "]").Trim();
                player_save player = config_read.read_player_form_id(player_name);
                config_file.player_list[player.qq].nick = nick;
                player.nick = nick;
                config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                return "已修改玩家" + player_name + "的昵称为：" + nick;
            }
        }
        public static string player_setid(long fromQQ, string msg)
        {
            string player;
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            player = check_player_name(fromQQ.ToString());
            if (player == null)
            {
                string player_name = msg.Replace(check_config.player_setid, "");
                if (player_name == " " || player_name == "")
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();
                    player_save player1 = new player_save();
                    if (mysql_config.enable == true)
                    {
                        if (Mysql_user.mysql_search(Mysql_user.Mysql_notid, player_name.ToLower()) == "notid")
                            return "禁止绑定ID：" + player_name;
                        if (Mysql_user.mysql_search_id(Mysql_user.Mysql_player, player_name.ToLower()) != null)
                            return "ID：" + player_name + "已经被绑定过了";
                        Mysql_user.mysql_add(Mysql_user.Mysql_player, fromQQ.ToString(), player_name.ToString());

                    }
                    else
                    {
                        if (config_file.cant_bind.Contains(player_name.ToLower()) == true)
                            return "禁止绑定ID：" + player_name;
                        else if (config_read.read_player_form_id(player_name) != null)
                            return "ID：" + player_name + "已经被绑定过了";
                        player1.player = player_name;
                        player1.qq = fromQQ;
                        config_file.player_list.Add(fromQQ, player1);
                        config_write.write_player(Minecraft_QQ.path + config_file.player, player1);
                    }

                    if (admin_config.Admin_Send != 0)
                    {
                        QQInfo qqInfo = Common.CqApi.GetQQInfo(fromQQ);
                        Common.CqApi.SendPrivateMessage(admin_config.Admin_Send, "玩家[" + qqInfo.Id.ToString() + "]绑定了ID：[" + player_name + "]");
                    }
                    return "绑定ID：" + player_name + "成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
        }
        public static string player_mute(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.mute, "");
            string player_name;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
                player_name = check_player_name(get_string(msg, "=", "]").ToString());
            else
                player_name = msg;
            if (player_name == null)
                return "ID无效";
            else
            {
                if (mysql_config.enable == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "true");
                else
                {
                    player_save player = config_read.read_player_form_id(player_name);
                    config_file.player_list[player.qq].mute = true;
                    player.mute = true;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                }
                return "已禁言：[" + player_name + "]";
            }
        }
        public static string player_unmute(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.unmute, "");
            string player_name;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
                player_name = check_player_name(get_string(msg, "=", "]").ToString());
            else
                player_name = msg;
            if (player_name == null)
                return "ID无效";
            else
            {
                if (mysql_config.enable == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_mute, player_name.ToLower(), "false");
                else
                {
                    player_save player = config_read.read_player_form_id(player_name);
                    config_file.player_list[player.qq].mute = false;
                    player.mute = false;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                }
                return "已解禁：[" + player_name + "]";
            }
        }
        public static string player_checkid(long fromQQ, string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.check, "");
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
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.rename, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player = get_string(msg, "=", "]");
                string player_name;
                player_name = get_string(msg, "]");
                player_name = player_name.Trim();
                if (mysql_config.enable == true)
                    Mysql_user.mysql_add(Mysql_user.Mysql_player, player, player_name);
                else
                {
                    long.TryParse(player, out long qq);
                    config_file.player_list[qq].player = player_name;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, config_file.player_list[qq]);
                }
                return "已修改玩家[" + player + "]ID为：" + player_name;
            }
            else
                return "玩家错误，请检查";
        }
        public static string fix_mode_change()
        {
            if (main_config.fix_mode == false)
            {
                main_config.fix_mode = true;
                config_write.write_config(Minecraft_QQ.path + config_file.config);
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已开启");
                return "服务器维护模式已开启";
            }
            else
            {
                main_config.fix_mode = false;
                config_write.write_config(Minecraft_QQ.path + config_file.config);
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已关闭");
                return "服务器维护模式已关闭";
            }
        }
        public static string online(long fromGroup)
        {
            if (main_config.fix_mode == false)
            {
                if (socket.ready == true)
                {
                    message_send message = new message_send();
                    message.group = fromGroup.ToString();
                    message.message = "在线人数";
                    message.is_commder = false;
                    message.player = null;
                    socket.Send(message);
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return message_config.fix_send;
            return null;
        }
        public static string server(long fromGroup)
        {
            if (main_config.fix_mode == false)
            {
                if (socket.ready == true)
                {
                    message_send message = new message_send();
                    message.group = fromGroup.ToString();
                    message.message = "服务器状态";
                    message.is_commder = false;
                    message.player = null;
                    socket.Send(message);
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return message_config.fix_send;
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
            Dictionary<string, commder_save>.KeyCollection valueCol = config_file.commder_list.Keys;
            foreach (string value in valueCol)
            {
                if (msg.IndexOf(value) == 0)
                {
                    commder_save commder = config_file.commder_list[value];
                    if (commder.player_use == true || check_admin(fromQQ.ToString()) == true)
                    {
                        if (socket.ready == false)
                        {
                            Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                            return true;
                        }
                        message_send message_send = new message_send();
                        message_send.group = fromGroup.ToString();
                        if (commder.parameter == true)
                        {
                            string b = commder.commder;
                            if (b.IndexOf("%player_name%") != -1)
                                b = b.Replace("%player_name%", check_player_name(fromQQ.ToString()));
                            if (msg.IndexOf("CQ:at,qq=") != -1 && b.IndexOf("%player_at%") != -1)
                            {
                                string a = get_string(msg, "=", "]");
                                if (a == null)
                                {
                                    Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "错误，玩家：" + a + "没有绑定ID");
                                    return true;
                                }
                                b = b.Replace("%player_at%", check_player_name(a));
                            }
                            msg = msg.Replace(commder.check, "");
                            message_send.message = b + msg;
                        }
                        else
                        {
                            message_send.message = commder.commder;
                            message_send.message = message_send.message.Replace("%player_name%", check_player_name(fromQQ.ToString()));
                        }
                        message_send.is_commder = true;
                        if (commder.player_send)
                        {
                            message_send.player = check_player_name(fromQQ.ToString());
                            if (message_send.player == null)
                            {
                                Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "你未绑定ID");
                                return true;
                            }
                        }
                        else
                            message_send.player = "后台";
                        socket.Send(message_send);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
