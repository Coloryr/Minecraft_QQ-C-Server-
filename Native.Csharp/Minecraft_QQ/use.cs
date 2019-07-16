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
            if (string.IsNullOrWhiteSpace(oldValue))
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
        public static string get_at(string msg)
        {
            while (msg.IndexOf("CQ:at,qq=") != -1)
            {
                string player_QQ = get_string(msg, "=", "]");
                string msg_QQ = get_string(msg, "[", "]");
                string player_name;
                long.TryParse(player_QQ, out long qq);
                player_save player = check_player(qq);
                if (player == null)
                    player_name = player_QQ;
                else
                {
                    if(string.IsNullOrWhiteSpace(player.nick) == false)
                        player_name = player.nick;
                    else
                        player_name = player.player;
                }
                msg = msg.Replace(msg_QQ, "@" + player_name + "");
            }
            if (msg.IndexOf("CQ:at,qq=all") != -1)
                msg.Replace("CQ:at,qq=all", "@全体人员");
            msg = msg.Replace("[", "").Replace("]", "");
            return msg;
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
            catch(Exception e)
            {
                logs.Log_write("[ERROR][group]" + e.Message);
            }
            return title + "：\n" + text;
        }
        public static string CQ_code(string a)
        {
            while (a.IndexOf("&#91;") != -1)
                a = a.Replace("&#91;", "[");
            while (a.IndexOf("&#93;") != -1)
                a = a.Replace("&#93;", "]");
            while (a.IndexOf("&#44;") != -1)
                a = a.Replace("&#44;", ",");
            return a;
        }
        public static string code_CQ(string a)
        {
            while (a.IndexOf("[") != -1)
                a = a.Replace("[", "&#91;");
            while (a.IndexOf("]") != -1)
                a = a.Replace("]", "&#93;");
            while (a.IndexOf(",") != -1)
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
        public static player_save check_player(long player_qq)
        {
            if (config_file.player_list.ContainsKey(player_qq) == true)
                return config_file.player_list[player_qq];
            return null;
        }
        public static player_save check_player_form_id(string id, bool low)
        {
            Dictionary<long, player_save>.ValueCollection valueCol = config_file.player_list.Values;
            foreach (player_save value in valueCol)
            {
                if (low == true && value.player.ToLower() == id.ToLower())
                    return value;
                else if (value.player == id)
                    return value;
            }
            return null;
        }
        public static string set_nick(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.nick, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string nick = get_string(msg, "]").Trim();
                long.TryParse(get_string(msg, "=", "]"), out long qq);
                if (config_file.player_list.ContainsKey(qq) == true)
                {
                    config_file.player_list[qq].nick = nick;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, config_file.player_list[qq]);
                }
                else
                {
                    player_save player = new player_save();
                    player.qq = qq;
                    player.nick = nick;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                }
                return "已修改玩家[" + qq + "]的昵称为：" + nick;
            }
            return "找不到玩家";
        }
        public static string player_setid(long fromQQ, string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            player_save player = check_player(fromQQ);
            if (player == null || (player != null && string.IsNullOrWhiteSpace(player.player) == true))
            {
                string player_name = msg.Replace(check_config.player_setid, "");
                if (string.IsNullOrWhiteSpace(player_name) == true)
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();
                    player_save player1;

                    if (config_file.cant_bind.Contains(player_name.ToLower()) == true)
                        return "禁止绑定ID：[" + player_name + "]";
                    else if (check_player_form_id(player_name, true) != null)
                        return "ID：[" + player_name + "]已经被绑定过了";
                    if (config_file.player_list.ContainsKey(fromQQ) == true)
                    {
                        player1 = config_file.player_list[fromQQ];
                        config_file.player_list.Remove(fromQQ);
                    }
                    else
                        player1 = new player_save();
                    player1.player = player_name;
                    player1.qq = fromQQ;
                    config_file.player_list.Add(fromQQ, player1);
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player1);
                    if (admin_config.Admin_Send != 0)
                    {
                        QQInfo qqInfo = Common.CqApi.GetQQInfo(fromQQ);
                        Common.CqApi.SendPrivateMessage(admin_config.Admin_Send, "玩家[" + qqInfo.Id.ToString() + "]绑定了ID：[" + player_name + "]");
                    }
                    return "绑定ID：[" + player_name + "]成功！";
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
            player_save player;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                long.TryParse(get_string(msg, "=", "]"), out long qq);
                player = check_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
            }
            else
            {
                player = check_player_form_id(msg, true);
                if (player == null)
                    return "ID无效";
            }
            config_file.player_list[player.qq].mute = true;
            player.mute = true;
            config_write.write_player(Minecraft_QQ.path + config_file.player, player);
            return "已禁言：[" + player.qq + "]";
        }
        public static string player_unmute(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.unmute, "");
            player_save player;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                long.TryParse(get_string(msg, "=", "]"), out long qq);
                player = check_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
            }
            else
            {
                player = check_player_form_id(msg, true);
                if (player == null)
                    return "ID无效";
            }
            config_file.player_list[player.qq].mute = false;
            player.mute = false;
            config_write.write_player(Minecraft_QQ.path + config_file.player, player);
            return "已解禁：[" + player.qq + "]";
        }
        public static string player_checkid(long fromQQ, string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.check, "");
            player_save player;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                long.TryParse(get_string(msg, "=", "]"), out long qq);
                player = check_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                else
                    return "玩家[" + qq + "]绑定的ID为：" + player.player;
            }
            else
            {
                player = check_player(fromQQ);
                if (player == null)
                    return "你没有绑定ID";
                else
                    return "你绑定的ID为：" + player.player;
            }
        }
        public static string player_rename(string msg)
        {
            if (msg.IndexOf(check_config.head) == 0)
                msg = msg.Replace(check_config.head, null);
            msg = msg.Replace(admin_config.rename, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player_qq = get_string(msg, "=", "]");
                string player_name;
                player_name = get_string(msg, "]");
                player_name = player_name.Trim();
                long.TryParse(player_qq, out long qq);
                if (config_file.player_list.ContainsKey(qq) == false)
                {
                    player_save player = new player_save();
                    player.qq = qq;
                    player.player = player_name;
                    config_file.player_list.Add(qq, player);
                    config_write.write_player(Minecraft_QQ.path + config_file.player, player);
                }
                else
                {
                    config_file.player_list[qq].player = player_name;
                    config_write.write_player(Minecraft_QQ.path + config_file.player, config_file.player_list[qq]);
                }
                return "已修改玩家[" + player_qq + "]ID为：" + player_name;
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
        public static bool commder_check(long fromGroup, string msg, long fromQQ)
        {
            Dictionary<string, commder_save>.KeyCollection valueCol = config_file.commder_list.Keys;
            foreach (string value in valueCol)
            {
                if (msg.IndexOf(value) == 0)
                {
                    if (socket.ready == false)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                        return true;
                    }
                    commder_save commder = config_file.commder_list[value];
                    player_save player = check_player(fromQQ);
                    if(player != null)
                    {
                        if (commder.player_use == true || player.admin == true)
                        {
                            message_send message_send = new message_send();
                            message_send.group = fromGroup.ToString();

                            string cmd = commder.commder;
                           
                            if (cmd.IndexOf("%player_name%") != -1)
                                cmd = cmd.Replace("%player_name%", player.player);
                            if (msg.IndexOf("CQ:at,qq=") != -1 && cmd.IndexOf("%player_at%") != -1)
                            {
                                string a = get_string(msg, "=", "]");
                                long.TryParse(a, out long qq);
                                player_save player1 = check_player(qq);
                                if (player1 == null)
                                {
                                    Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "错误，玩家：" + a + "没有绑定ID");
                                    return true;
                                }
                                cmd = cmd.Replace("%player_at%", player1.player);
                            }
                           
                            if (commder.parameter == true)
                            {                               
                                if (msg.IndexOf("CQ:at,qq=") != -1 && msg.IndexOf("]") != -1)
                                    message_send.message = cmd + get_string(msg, "]");
                                else
                                    message_send.message = cmd + msg.Replace(commder.check, "");
                            }
                            else
                                message_send.message = cmd;
                            message_send.is_commder = true;
                            if (commder.player_send)
                            {
                                message_send.player = player.player;
                                if (string.IsNullOrWhiteSpace(player.player) == true)
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
                    else
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "你未绑定ID");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
