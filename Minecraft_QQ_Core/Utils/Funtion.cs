using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MyMysql;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Core.Robot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Minecraft_QQ_Core.Utils
{
    internal class Funtion
    {
        public static string GBKtoUTF8(string msg)
        {
            try
            {
                byte[] srcBytes = Encoding.Default.GetBytes(msg);
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, srcBytes);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return msg;
            }
        }
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
                sb.Replace("§l", string.Empty);
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
                sb.Replace("&l", string.Empty);
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
        public static string GetString(string a, string b, string c = null)
        {
            int x = a.IndexOf(b) + b.Length;
            int y;
            if (c != null)
            {
                y = a.IndexOf(c, x);
                if (a[y - 1] == '"')
                {
                    y = a.IndexOf(c, y + 1);
                }
                if (y - x <= 0)
                    return a;
                else
                    return a.Substring(x, y - x);
            }
            else
                return a.Substring(x);
        }
        public static string GetRich(string a)
        {
            try
            {
                if (a.StartsWith("{"))
                {
                    int index = a.LastIndexOf("}");
                    a = a.Substring(0, index);
                    var obj = JObject.Parse(a);
                    string app = obj["app"].ToString();
                    if (app == "com.tencent.qq.checkin")
                    {
                        return obj["prompt"].ToString() + "-" + obj["meta"]["checkInData"]["desc"].ToString();
                    }
                    else if (app == "com.tencent.mannounce")
                    {
                        return obj["prompt"].ToString();
                    }
                    else if (app == "com.tencent.structmsg")
                    {
                        return obj["prompt"].ToString() + "\n" + "链接：" + obj["meta"]["news"]["jumpUrl"].ToString();
                    }

                }
                else if (a.StartsWith("<?xml"))
                {
                    int index = a.LastIndexOf(">");
                    a = a.Substring(0, index);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(a);
                    if (a.Contains("发起投票"))
                    {
                        var items = "";
                        var title = doc.GetElementsByTagName("title")[0].InnerText.Trim();
                        var list = doc.GetElementsByTagName("checklist");
                        foreach (XmlNode item in list[0].ChildNodes)
                        {
                            items += item.InnerText.Trim() + "\n";
                        }
                        items = items.Substring(0, items.Length - 2);
                        return "发起群投票：" + title + "\n" + items;
                    }
                    else if (a.Contains("聊天记录"))
                    {
                        var items = "";
                        var body = doc.GetElementsByTagName("title");
                        var title = body[0].InnerText.Trim();
                        for (int i = 1; i < body.Count; i++)
                        {
                            var item = body[i];
                            items += item.InnerText.Trim().Remove(0, 3) + "\n";
                        }
                        items = items.Substring(0, items.Length - 1);
                        return "聊天记录：" + title + "\n" + items;
                    }
                }
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            return a;
        }
        public static string GetSign(string a, string player)
        {
            try
            {
                if (a.Contains("title=") && a.Contains(",image"))
                    return player + "群签到：" + GetString(a, "title=", ",image");
            }
            catch (Exception e)
            {
                Logs.LogError(e);
            }
            return null;
        }
        public static PlayerObj GetPlayer(long qq)
        {
            if (Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(qq) == true)
                return Minecraft_QQ.PlayerConfig.玩家列表[qq];
            return null;
        }
        public static PlayerObj GetPlayer(string id)
        {
            Dictionary<long, PlayerObj>.ValueCollection valueCol = Minecraft_QQ.PlayerConfig.玩家列表.Values;
            foreach (PlayerObj value in valueCol)
            {
                if (value == null || value.名字 == null)
                    return null;
                if (value.名字.ToLower() == id.ToLower())
                    return value;
            }
            return null;
        }
        public static string SetNick(List<string> msg)
        {
            if (msg.Count != 5)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                long.TryParse(GetString(msg[2], "at:", ","), out long qq);
                PlayerObj player;
                if (Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(qq) == true)
                {
                    Minecraft_QQ.PlayerConfig.玩家列表[qq].昵称 = msg[3].Trim();
                    if (Minecraft_QQ.MysqlOK == true)
                        Task.Factory.StartNew(async () =>
                        {
                            await new MysqlAddData().PlayerAsync(Minecraft_QQ.PlayerConfig.玩家列表[qq]);
                        });
                    else
                        new ConfigWrite().Player();
                }
                else
                {
                    player = new PlayerObj()
                    {
                        QQ号 = qq,
                        昵称 = msg[3].Trim()
                    };
                    Minecraft_QQ.PlayerConfig.玩家列表.Add(qq, player);
                    new ConfigWrite().Player();
                }
                return "已修改玩家[" + qq + "]的昵称为：" + msg[3].Trim();
            }
            else
                return "找不到玩家";
        }
        public static string SetPlayerName(long group, long fromQQ, List<string> msg)
        {
            if (msg.Count != 3)
                return "错误的参数";
            string data = msg[1];
            if (data.IndexOf(Minecraft_QQ.MainConfig.检测.检测头) == 0)
                data = data.Replace(Minecraft_QQ.MainConfig.检测.检测头, null);
            if (Minecraft_QQ.MainConfig.设置.可以绑定名字 == false)
                return Minecraft_QQ.MainConfig.消息.不能绑定文本;
            var player = GetPlayer(fromQQ);
            if (player == null || string.IsNullOrWhiteSpace(player.名字) == true)
            {
                string player_name = data.Replace(Minecraft_QQ.MainConfig.检测.玩家设置名字, "");
                string check = player_name.Trim();
                if (string.IsNullOrWhiteSpace(player_name) ||
                    check.StartsWith("id:") || check.StartsWith("id：") ||
                      check.StartsWith("id "))
                    return "ID无效，请检查";
                else
                {
                    player_name = player_name.Trim();

                    if (Minecraft_QQ.PlayerConfig.禁止绑定列表.Contains(player_name.ToLower()) == true)
                        return "禁止绑定ID：[" + player_name + "]";
                    else if (GetPlayer(player_name) != null)
                        return "ID：[" + player_name + "]已经被绑定过了";
                    if (Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(fromQQ) == true)
                    {
                        player = Minecraft_QQ.PlayerConfig.玩家列表[fromQQ];
                        Minecraft_QQ.PlayerConfig.玩家列表.Remove(fromQQ);
                    }
                    else
                        player = new PlayerObj();
                    player.名字 = player_name;
                    player.QQ号 = fromQQ;
                    Minecraft_QQ.PlayerConfig.玩家列表.Add(fromQQ, player);
                    if (Minecraft_QQ.MysqlOK == true)
                        Task.Factory.StartNew(async () =>
                        {
                            await new MysqlAddData().PlayerAsync(player);
                        });
                    else
                        new ConfigWrite().Player();
                    if (Minecraft_QQ.MainConfig.管理员.发送绑定信息QQ号 != 0)
                        RobotSocket.SendGroupPrivateMessage(group, Minecraft_QQ.MainConfig.管理员.发送绑定信息QQ号, "玩家[" + fromQQ + "]绑定了ID：[" + player_name + "]");
                    IMinecraft_QQ.GuiCall?.Invoke(GuiFun.PlayerList);
                    return "绑定ID：[" + player_name + "]成功！";
                }
            }
            else
                return "你已经绑定ID了，请找腐竹更改";
        }
        public static string MutePlayer(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            string name;
            if (msg.Count == 4 && msg[2].IndexOf("[mirai:at:") != -1)
            {
                long.TryParse(GetString(msg[2], "at:", ","), out long qq);
                var player = GetPlayer(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                name = player.名字;
            }
            else
            {
                name = msg[2].Replace(Minecraft_QQ.MainConfig.管理员.禁言, "").Trim();
                name = ReplaceFirst(name, Minecraft_QQ.MainConfig.检测.检测头, "");
            }
            if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(name.ToLower()) == false)
                Minecraft_QQ.PlayerConfig.禁言列表.Add(name.ToLower());
            if (Minecraft_QQ.MysqlOK == true)
                Task.Factory.StartNew(async () =>
                {
                    await new MysqlAddData().MuteAsync(name.ToLower());
                });
            else
                new ConfigWrite().Player();
            return "已禁言：[" + name + "]";
        }
        public static string UnmutePlayer(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            string name;
            if (msg.Count == 4 && msg[2].IndexOf("[mirai:at:") != -1)
            {
                long.TryParse(GetString(msg[2], "at:", ","), out long qq);
                var player = GetPlayer(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                name = player.名字;
            }
            else
            {
                name = msg[2].Replace(Minecraft_QQ.MainConfig.管理员.禁言, "").Trim();
                name = ReplaceFirst(name, Minecraft_QQ.MainConfig.检测.检测头, "");
            }
            if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(name.ToLower()) == true)
                Minecraft_QQ.PlayerConfig.禁言列表.Remove(name.ToLower());
            if (Minecraft_QQ.MysqlOK == true)
                Task.Factory.StartNew(() => new MysqlRemoveData().MuteAsync(name));
            else
                new ConfigWrite().Player();
            return "已解禁：[" + name + "]";
        }
        public static string GetPlayerID(List<string> msg)
        {
            if (msg.Count > 4)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                long.TryParse(GetString(msg[2], "at:", ","), out long qq);
                var player = GetPlayer(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                else
                    return "玩家[" + qq + "]绑定的ID为：" + player.名字;
            }
            else if (msg.Count == 3)
            {
                string data = msg[2].Replace(Minecraft_QQ.MainConfig.管理员.查询绑定名字, "");
                if (long.TryParse(data.Remove(0, 1), out long qq) == false)
                {
                    return "无效的QQ号";
                }
                var player = GetPlayer(qq);
                if (player == null)
                    return "玩家[" + qq + "]未绑定ID";
                else
                    return "玩家[" + qq + "]绑定的ID为：" + player.名字;
            }
            else
            {
                return "你需要@一个人或者输入它的QQ号来查询";
            }
        }
        public static string RenamePlayer(List<string> msg)
        {
            if (msg.Count != 5)
                return "错误的参数";
            if (msg[2].IndexOf("[mirai:at:") != -1)
            {
                long.TryParse(GetString(msg[2], "at:", ","), out long qq);
                if (Minecraft_QQ.PlayerConfig.玩家列表.ContainsKey(qq) == false)
                {
                    var player = new PlayerObj()
                    {
                        QQ号 = qq,
                        名字 = msg[2].Trim()
                    };
                    Minecraft_QQ.PlayerConfig.玩家列表.Add(qq, player);
                    if (Minecraft_QQ.MysqlOK == true)
                        Task.Factory.StartNew(async () =>
                        {
                            await new MysqlAddData().PlayerAsync(player);
                        });
                    else
                        new ConfigWrite().Player();
                }
                else
                {
                    Minecraft_QQ.PlayerConfig.玩家列表[qq].名字 = msg[3].Trim();
                    if (Minecraft_QQ.MysqlOK == true)
                        Task.Factory.StartNew(async () =>
                        {
                            await new MysqlAddData().PlayerAsync(Minecraft_QQ.PlayerConfig.玩家列表[qq]);
                        });
                    else
                        new ConfigWrite().Player();
                }
                return "已修改玩家[" + qq + "]ID为：" + msg[3].Trim();
            }
            else
                return "玩家错误，请检查";
        }

        public static string GetMuteList()
        {
            if (Minecraft_QQ.PlayerConfig.禁言列表.Count == 0)
                return "没有禁言的玩家";
            else
            {
                string a = "禁言的玩家：";
                foreach (string name in Minecraft_QQ.PlayerConfig.禁言列表)
                {
                    a += "\n" + name;
                }
                return a;
            }
        }

        public static string GetCantBind()
        {
            if (Minecraft_QQ.PlayerConfig.禁止绑定列表.Count == 0)
                return "没有禁止绑定的ID";
            else
            {
                string a = "禁止绑定的ID：";
                foreach (string name in Minecraft_QQ.PlayerConfig.禁止绑定列表)
                {
                    a += "\n" + name;
                }
                return a;
            }
        }

        public static string FixModeChange()
        {
            if (Minecraft_QQ.MainConfig.设置.维护模式 == false)
            {
                Minecraft_QQ.MainConfig.设置.维护模式 = true;
                new ConfigWrite().Config();
                Logs.LogOut("[Minecraft_QQ]服务器维护模式已开启");
                return "服务器维护模式已开启";
            }
            else
            {
                Minecraft_QQ.MainConfig.设置.维护模式 = false;
                new ConfigWrite().Config();
                Logs.LogOut("[Minecraft_QQ]服务器维护模式已关闭");
                return "服务器维护模式已关闭";
            }
        }
        public static string GetOnlinePlayer(long fromGroup)
        {
            if (Minecraft_QQ.MainConfig.设置.维护模式 == false)
            {
                if (MySocketServer.IsReady() == true)
                {
                    var message = new TranObj()
                    {
                        group = fromGroup.ToString(),
                        command = CommderList.ONLINE,
                        player = null
                    };
                    MySocketServer.Send(message);
                    return null;
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return Minecraft_QQ.MainConfig.消息.维护提示文本;
        }
        public static string GetOnlineServer(long fromGroup)
        {
            if (Minecraft_QQ.MainConfig.设置.维护模式 == false)
            {
                if (MySocketServer.IsReady() == true)
                {
                    var message = new TranObj()
                    {
                        group = fromGroup.ToString(),
                        command = CommderList.SERVER,
                    };
                    MySocketServer.Send(message);
                    return null;
                }
                else
                    return "发送失败，服务器未准备好";
            }
            else
                return Minecraft_QQ.MainConfig.消息.维护提示文本;
        }
        public static bool SendCommand(long fromGroup, string msg, long fromQQ)
        {
            foreach (var value in Minecraft_QQ.CommandConfig.命令列表)
            {
                if (msg.ToLower().IndexOf(value.Key) == 0)
                {
                    if (MySocketServer.IsReady() == false)
                    {
                        List<string> lists = new List<string>();
                        lists.Add("at:" + fromQQ);
                        lists.Add("发送失败，服务器未准备好");
                        RobotSocket.SendGroupMessage(fromGroup, lists);
                        return true;
                    }
                    bool haveserver = false;
                    List<string> servers = new List<string>();
                    if (value.Value.服务器使用 != null)
                    {
                        foreach (var temp in value.Value.服务器使用)
                        {
                            if (MySocketServer.MCServers.ContainsKey(temp))
                            {
                                servers.Add(temp);
                                haveserver = true;
                            }
                        }
                    }
                    else
                    {
                        servers = null;
                        haveserver = true;
                    }
                    if (!haveserver)
                    {
                        List<string> lists = new List<string>();
                        lists.Add("at:" + fromQQ);
                        lists.Add("发送失败，对应的服务器未连接");
                        RobotSocket.SendGroupMessage(fromGroup, lists);
                    }
                    var player = GetPlayer(fromQQ);
                    if (player != null)
                    {
                        if (value.Value.玩家使用 == true || player.管理员 == true)
                        {
                            var messageSend = new TranObj();
                            messageSend.group = fromGroup.ToString();

                            string cmd = value.Value.命令;

                            if (cmd.IndexOf("%player_name%") != -1)
                                cmd = cmd.Replace("%player_name%", player.名字);
                            if (msg.IndexOf("CQ:at,qq=") != -1 && cmd.IndexOf("%player_at%") != -1)
                            {
                                string a = GetString(msg, "=", "]");
                                long.TryParse(a, out long qq);
                                var player1 = GetPlayer(qq);
                                if (player1 == null)
                                {
                                    List<string> lists = new List<string>();
                                    lists.Add("at:" + fromQQ);
                                    lists.Add("错误，玩家：" + a + "没有绑定ID");
                                    RobotSocket.SendGroupMessage(fromGroup, lists);
                                    return true;
                                }
                                cmd = cmd.Replace("%player_at%", player1.名字);
                            }

                            if (value.Value.附带参数 == true)
                            {
                                if (msg.IndexOf("CQ:at,qq=") != -1 && msg.IndexOf("]") != -1)
                                    messageSend.command = cmd + GetString(msg, "]");
                                else
                                    messageSend.command = cmd + ReplaceFirst(msg, value.Key, "");
                            }
                            else
                                messageSend.command = cmd;
                            messageSend.isCommand = true;
                            if (value.Value.玩家发送)
                            {
                                messageSend.player = player.名字;
                                if (string.IsNullOrWhiteSpace(player.名字) == true)
                                {
                                    List<string> lists = new List<string>();
                                    lists.Add("at:" + fromQQ);
                                    lists.Add("你未绑定ID");
                                    RobotSocket.SendGroupMessage(fromGroup, lists);
                                    return true;
                                }
                            }
                            else
                                messageSend.player = "后台";
                            MySocketServer.Send(messageSend, servers);
                            return true;
                        }
                    }
                    else
                    {
                        List<string> lists = new List<string>();
                        lists.Add("at:" + fromQQ);
                        lists.Add("你未绑定ID");
                        RobotSocket.SendGroupMessage(fromGroup, lists);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
