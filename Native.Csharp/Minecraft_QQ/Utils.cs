using Native.Csharp.App;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Color_yr.Minecraft_QQ
{
    class Utils
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
        public static string Get_string(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + b.Length;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c, x);
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
        public static string Remove_pic(string a)
        {
            while (a.IndexOf("[CQ:image") != -1)
            {
                string b = Get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;图片&#93;");
            }
            while (a.IndexOf("[CQ:face") != -1)
            {
                string b = Get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;表情&#93;");
            }
            while (a.IndexOf("[CQ:emoji") != -1)
            {
                string b = Get_string(a, "[", "]");
                a = a.Replace(b, "");
                a = a.Replace("[]", "&#91;表情&#93;");
            }
            return a;
        }
        public static string Get_from_at(string msg)
        {
            while (msg.IndexOf("CQ:at,qq=") != -1)
            {
                string player_QQ = Get_string(msg, "=", "]");
                string msg_QQ = Get_string(msg, "[", "]");
                string player_name;
                long.TryParse(player_QQ, out long qq);
                Player_save_obj player = Get_player(qq);
                if (player == null)
                    player_name = player_QQ;
                else
                {
                    if (string.IsNullOrWhiteSpace(player.nick) == false)
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
        public static string Get_rich(string a)
        {
            string title = null;
            string json_string;
            string text = null;
            try
            {
                title = Get_string(a, "title=", ",content");
                json_string = "{" + Get_string(a, ":{", "}") + "}";
                json_string = json_string.Replace("&#44;", ",");
                JObject jsonData = JObject.Parse(json_string);
                if (jsonData.ContainsKey("text"))
                {
                    text = jsonData["text"].ToString();
                    byte[] bytes = Convert.FromBase64String(text);
                    text = Encoding.GetEncoding("utf-8").GetString(bytes);
                }
                else if (jsonData.ContainsKey("jumpUrl"))
                {
                    if (jsonData.ContainsKey("tag"))
                    {
                        text = jsonData["tag"].ToString() + "分享："
                            + jsonData["jumpUrl"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][group]" + e.Message);
            }
            return title + "：\n" + text;
        }
        public static string Get_sign(string a, string player)
        {
            string title;
            string text = null;
            try
            {
                title = Get_string(a, "title=", ",image");
                text = player + "群签到：" + title;
            }
            catch (Exception e)
            {
                logs.Log_write("[ERROR][group]" + e.Message);
            }
            return text;
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
        public static string Code_CQ(string a)
        {
            while (a.IndexOf("[") != -1)
                a = a.Replace("[", "&#91;");
            while (a.IndexOf("]") != -1)
                a = a.Replace("]", "&#93;");
            while (a.IndexOf(",") != -1)
                a = a.Replace(",", "&#44;");
            return a;
        }
        public static bool Key_is_ok(KeyEventArgs e)
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
        public static Player_save_obj Get_player(long player_qq)
        {
            if (Minecraft_QQ.Playerconfig.玩家列表.ContainsKey(player_qq) == true)
                return Minecraft_QQ.Playerconfig.玩家列表[player_qq];
            return null;
        }
        public static Player_save_obj Get_player_from_id(string id, bool low)
        {
            Dictionary<long, Player_save_obj>.ValueCollection valueCol = Minecraft_QQ.Playerconfig.玩家列表.Values;
            foreach (Player_save_obj value in valueCol)
            {
                if (low == true && value.player.ToLower() == id.ToLower())
                    return value;
                else if (value.player == id)
                    return value;
            }
            return null;
        }
        public static string Set_nick(string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            msg = msg.Replace(Minecraft_QQ.Mainconfig.管理员.设置昵称, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string nick = Get_string(msg, "]").Trim();
                long.TryParse(Get_string(msg, "=", "]"), out long qq);
                Player_save_obj player;
                if (Minecraft_QQ.Playerconfig.玩家列表.ContainsKey(qq) == true)
                {
                    player = Minecraft_QQ.Playerconfig.玩家列表[qq];
                    player.nick = nick;
                    Minecraft_QQ.Playerconfig.玩家列表.Remove(qq);
                    Minecraft_QQ.Playerconfig.玩家列表.Add(qq, player);
                    if (Minecraft_QQ.Mysql_ok == true)
                    {
                        new Mysql_Add_data().player(player);
                    }
                    else
                    {
                        new Config_write().Write_player();
                    }
                }
                else
                {
                    player = new Player_save_obj();
                    player.qq = qq;
                    player.nick = nick;
                    Minecraft_QQ.Playerconfig.玩家列表.Add(qq, player);
                    new Config_write().Write_player();
                }
                return "已修改玩家[" + qq + "]的昵称为：" + nick;
            }
            return "找不到玩家";
        }
        public static string Set_player_name(long fromQQ, string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            if (Minecraft_QQ.Mainconfig.设置.可以绑定名字 == false)
                return Minecraft_QQ.Mainconfig.消息.不能绑定文本;
            Player_save_obj player = Get_player(fromQQ);
            if (player == null || (player != null && string.IsNullOrWhiteSpace(player.player) == true))
            {
                string player_name = msg.Replace(Minecraft_QQ.Mainconfig.检测.玩家设置名字, "");
                if (string.IsNullOrWhiteSpace(player_name) == true)
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();

                    if (Minecraft_QQ.Playerconfig.禁止绑定列表.Contains(player_name.ToLower()) == true)
                        return "禁止绑定ID：[" + player_name + "]";
                    else if (Get_player_from_id(player_name, true) != null)
                        return "ID：[" + player_name + "]已经被绑定过了";
                    if (Minecraft_QQ.Playerconfig.玩家列表.ContainsKey(fromQQ) == true)
                    {
                        player = Minecraft_QQ.Playerconfig.玩家列表[fromQQ];
                        Minecraft_QQ.Playerconfig.玩家列表.Remove(fromQQ);
                    }
                    else
                        player = new Player_save_obj();
                    player.player = player_name;
                    player.qq = fromQQ;
                    Minecraft_QQ.Playerconfig.玩家列表.Add(fromQQ, player);
                    if (Minecraft_QQ.Mysql_ok == true)
                    {
                        new Mysql_Add_data().player(player);
                    }
                    else
                    {
                        new Config_write().Write_player();
                    }
                    if (Minecraft_QQ.Mainconfig.管理员.发送绑定信息QQ号 != 0)
                    {
                        Common.CqApi.SendPrivateMessage(Minecraft_QQ.Mainconfig.管理员.发送绑定信息QQ号, "玩家[" + fromQQ + "]绑定了ID：[" + player_name + "]");
                    }
                    return "绑定ID：[" + player_name + "]成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
        }
        public static string Mute_player(string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            msg = msg.Replace(Minecraft_QQ.Mainconfig.管理员.禁言, "");
            string name;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                Player_save_obj player;
                long.TryParse(Get_string(msg, "=", "]"), out long qq);
                player = Get_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                name = player.player;
            }
            else
            {
                name = msg.Replace(Minecraft_QQ.Mainconfig.管理员.禁言, "").Trim();
            }
            if (Minecraft_QQ.Playerconfig.禁言列表.Contains(name.ToLower()) == false)
                Minecraft_QQ.Playerconfig.禁言列表.Add(name.ToLower());
            if (Minecraft_QQ.Mysql_ok == true)
            {
                new Mysql_Add_data().mute(name.ToLower());
            }
            else
            {
                new Config_write().Write_player();
            }
            return "已禁言：[" + name + "]";
        }
        public static string Unmute_player(string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            msg = msg.Replace(Minecraft_QQ.Mainconfig.管理员.取消禁言, "");
            string name;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                Player_save_obj player;
                long.TryParse(Get_string(msg, "=", "]"), out long qq);
                player = Get_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                name = player.player;
            }
            else
            {
                name = msg.Replace(Minecraft_QQ.Mainconfig.管理员.取消禁言, "").Trim();
            }
            if (Minecraft_QQ.Playerconfig.禁言列表.Contains(name.ToLower()) == true)
                Minecraft_QQ.Playerconfig.禁言列表.Remove(name.ToLower());
            if (Minecraft_QQ.Mysql_ok == true)
            {
                new Mysql_remove_data().mute(name);
            }
            else
            {
                new Config_write().Write_player();
            }
            return "已解禁：[" + name + "]";
        }
        public static string Get_Player_id(long fromQQ, string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            msg = msg.Replace(Minecraft_QQ.Mainconfig.管理员.查询绑定名字, "");
            Player_save_obj player;
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                long.TryParse(Get_string(msg, "=", "]"), out long qq);
                player = Get_player(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                else
                    return "玩家[" + qq + "]绑定的ID为：" + player.player;
            }
            else
            {
                player = Get_player(fromQQ);
                if (player == null)
                    return "你没有绑定ID";
                else
                    return "你绑定的ID为：" + player.player;
            }
        }
        public static string Rename_player(string msg)
        {
            if (msg.IndexOf(Minecraft_QQ.Mainconfig.检测.检测头) == 0)
                msg = msg.Replace(Minecraft_QQ.Mainconfig.检测.检测头, null);
            msg = msg.Replace(Minecraft_QQ.Mainconfig.管理员.重命名, "");
            if (msg.IndexOf("[CQ:at,qq=") != -1)
            {
                string player_qq = Get_string(msg, "=", "]");
                string player_name;
                player_name = Get_string(msg, "]");
                player_name = player_name.Trim();
                long.TryParse(player_qq, out long qq);
                if (Minecraft_QQ.Playerconfig.玩家列表.ContainsKey(qq) == false)
                {
                    Player_save_obj player = new Player_save_obj();
                    player.qq = qq;
                    player.player = player_name;
                    Minecraft_QQ.Playerconfig.玩家列表.Add(qq, player);
                    if (Minecraft_QQ.Mysql_ok == true)
                    {
                        new Mysql_Add_data().player(player);
                    }
                    else
                    {
                        new Config_write().Write_player();
                    }
                }
                else
                {
                    Minecraft_QQ.Playerconfig.玩家列表[qq].player = player_name;
                    if (Minecraft_QQ.Mysql_ok == true)
                    {
                        new Mysql_Add_data().player(Minecraft_QQ.Playerconfig.玩家列表[qq]);
                    }
                    else
                    {
                        new Config_write().Write_player();
                    }
                }
                return "已修改玩家[" + player_qq + "]ID为：" + player_name;
            }
            else
                return "玩家错误，请检查";
        }

        public static string Get_mute_list()
        {
            if (Minecraft_QQ.Playerconfig.禁言列表.Count == 0)
                return "没有禁言的玩家";
            else
            {
                string a = "禁言的玩家：";
                foreach (string name in Minecraft_QQ.Playerconfig.禁言列表)
                {
                    a += "\n" + name;
                }
                return a;
            }
        }

        public static string Get_cant_bind()
        {
            if (Minecraft_QQ.Playerconfig.禁止绑定列表.Count == 0)
                return "没有禁止绑定的ID";
            else
            {
                string a = "禁止绑定的ID：";
                foreach (string name in Minecraft_QQ.Playerconfig.禁止绑定列表)
                {
                    a += "\n" + name;
                }
                return a;
            }
        }

        public static string Fix_mode_change()
        {
            if (Minecraft_QQ.Mainconfig.设置.维护模式 == false)
            {
                Minecraft_QQ.Mainconfig.设置.维护模式 = true;
                new Config_write().Write_config();
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已开启");
                return "服务器维护模式已开启";
            }
            else
            {
                Minecraft_QQ.Mainconfig.设置.维护模式 = false;
                new Config_write().Write_config();
                logs.Log_write("[INFO][Minecraft_QQ]服务器维护模式已关闭");
                return "服务器维护模式已关闭";
            }
        }
        public static string Get_online_player(long fromGroup)
        {
            if (Minecraft_QQ.Mainconfig.设置.维护模式 == false)
            {
                if (socket.ready == true)
                {
                    Message_send_obj message = new Message_send_obj();
                    message.group = fromGroup.ToString();
                    message.commder = Commder_list.ONLINE;
                    message.is_commder = false;
                    message.player = null;
                    socket.Send(message);
                    return null;
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return Minecraft_QQ.Mainconfig.消息.维护提示文本;
        }
        public static string Get_online_server(long fromGroup)
        {
            if (Minecraft_QQ.Mainconfig.设置.维护模式 == false)
            {
                if (socket.ready == true)
                {
                    Message_send_obj message = new Message_send_obj();
                    message.group = fromGroup.ToString();
                    message.commder = Commder_list.SERVER;
                    message.is_commder = false;
                    message.player = null;
                    socket.Send(message);
                    return null;
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return Minecraft_QQ.Mainconfig.消息.维护提示文本;
        }
        public static bool Send_command(long fromGroup, string msg, long fromQQ)
        {
            msg = ReplaceFirst(msg, Minecraft_QQ.Mainconfig.检测.检测头, "");
            foreach (KeyValuePair<string, Command_save_obj> value in Minecraft_QQ.Commandconfig.命令列表)
            {

                if (msg.ToLower().IndexOf(value.Key) == 0)
                {
                    if (socket.ready == false)
                    {
                        Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "发送失败，服务器未准备好");
                        return true;
                    }
                    Player_save_obj player = Get_player(fromQQ);
                    if (player != null)
                    {
                        if (value.Value.player_use == true || player.admin == true)
                        {
                            Message_send_obj message_send = new Message_send_obj();
                            message_send.group = fromGroup.ToString();

                            string cmd = value.Value.command;

                            if (cmd.IndexOf("%player_name%") != -1)
                                cmd = cmd.Replace("%player_name%", player.player);
                            if (msg.IndexOf("CQ:at,qq=") != -1 && cmd.IndexOf("%player_at%") != -1)
                            {
                                string a = Get_string(msg, "=", "]");
                                long.TryParse(a, out long qq);
                                Player_save_obj player1 = Get_player(qq);
                                if (player1 == null)
                                {
                                    Common.CqApi.SendGroupMessage(fromGroup, Common.CqApi.CqCode_At(fromQQ) + "错误，玩家：" + a + "没有绑定ID");
                                    return true;
                                }
                                cmd = cmd.Replace("%player_at%", player1.player);
                            }

                            if (value.Value.parameter == true)
                            {
                                if (msg.IndexOf("CQ:at,qq=") != -1 && msg.IndexOf("]") != -1)
                                    message_send.commder = cmd + Get_string(msg, "]");
                                else
                                    message_send.commder = cmd + ReplaceFirst(msg, value.Value.check, "");
                            }
                            else
                                message_send.commder = cmd;
                            message_send.is_commder = true;
                            if (value.Value.player_send)
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
