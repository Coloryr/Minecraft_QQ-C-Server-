using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Core.Utils;
using System;
using System.Collections.Generic;

namespace Minecraft_QQ_Core.Robot;

public static class MessageHelper
{
    private static string SetNick(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 3)
            return "错误的参数";
        if (msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "QQ号获取失败";
            }
            string nick = msg[2].data.text.Trim();
            Minecraft_QQ.SetNick(qq, nick);
            return $"已修改玩家[{qq}]的昵称为：{nick}";
        }
        else
            return "找不到玩家";
    }
    private static string? SetPlayerName(long group, long fromQQ, List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 1)
        {
            return "错误的参数";
        }
        string data = msg[0].data.text;
        if (data.StartsWith(Minecraft_QQ.MainConfig.Check.Head))
        {
            data = data.Replace(Minecraft_QQ.MainConfig.Check.Head, null);
        }
        if (Minecraft_QQ.MainConfig.Setting.CanBind == false)
        {
            return Minecraft_QQ.MainConfig.Message.CantBindText;
        }
        var player = Minecraft_QQ.GetPlayer(fromQQ);
        if (player == null || string.IsNullOrWhiteSpace(player.Name) == true)
        {
            string name = data.Replace(Minecraft_QQ.MainConfig.Check.Bind, "");
            string check = name.Trim();
            if (string.IsNullOrWhiteSpace(name) || check.StartsWith("id:")
                || check.StartsWith("id：") || check.StartsWith("id "))
            {
                return "名字无效，请检查";
            }
            else
            {
                name = name.Trim();

                if (Minecraft_QQ.PlayerConfig.NotBindList.Contains(name.ToLower()) == true)
                {
                    return $"禁止绑定名字：[{name}]";
                }
                Minecraft_QQ.SetPlayerName(fromQQ, name);
                if (Minecraft_QQ.MainConfig.Setting.SendQQ != 0)
                    RobotCore.SendPrivateMessage(Minecraft_QQ.MainConfig.Setting.SendQQ,
                    [
                        $"玩家[{fromQQ}]绑定了名字：[{name}]"
                    ]);
                IMinecraft_QQ.GuiCall?.Invoke(GuiCallType.PlayerList);
                return $"绑定名字：[{name}]成功！";
            }
        }
        else
            return Minecraft_QQ.MainConfig.Message.AlreadyBindID;
    }

    private static string MutePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
            return "错误的参数";
        string name;
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            var player = Minecraft_QQ.GetPlayer(qq);
            if (player == null)
            {
                return $"玩家[{qq}]未绑定名字";
            }
            name = player.Name;
        }
        else
        {
            name = msg[1].data.text.Trim();
            name = Funtion.ReplaceFirst(name, Minecraft_QQ.MainConfig.Check.Head, "");
        }
        Minecraft_QQ.MutePlayer(name);
        return $"已禁言：[{name}]";
    }

    private static string UnmutePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
        {
            return "错误的参数";
        }
        string name;
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            var player = Minecraft_QQ.GetPlayer(qq);
            if (player == null)
                return $"玩家[{qq}]未绑定名字";
            name = player.Name;
        }
        else
        {
            name = msg[1].data.text.Trim();
            name = Funtion.ReplaceFirst(name, Minecraft_QQ.MainConfig.Check.Head, "");
        }
        Minecraft_QQ.UnmutePlayer(name);
        return $"已解禁：[{name}]";
    }

    private static string GetPlayerID(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count > 3)
            return "错误的参数";
        if (msg.Count == 3 && msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的格式";
            }
            var player = Minecraft_QQ.GetPlayer(qq);
            if (player == null)
            {
                return $"玩家[{qq}]未绑定名字";
            }
            else
                return $"玩家[{qq}]绑定的名字为：" + player.Name;
        }
        else if (msg.Count == 2)
        {
            string data = msg[1].data.text.Replace(Minecraft_QQ.MainConfig.Admin.CheckBind, "");
            if (long.TryParse(data.Remove(0, 1), out long qq) == false)
            {
                return "无效的QQ号";
            }
            var player = Minecraft_QQ.GetPlayer(qq);
            if (player == null)
            {
                return $"玩家[{qq}]未绑定名字";
            }
            else
            {
                return $"玩家[{qq}]绑定的名字为：" + player.Name;
            }
        }
        else
        {
            return "你需要@一个人或者输入它的QQ号来查询";
        }
    }
    private static string RenamePlayer(List<GroupMessagePack.Message> msg)
    {
        if (msg.Count != 3)
        {
            return "错误的参数";
        }
        if (msg[1].type == "at")
        {
            if (!long.TryParse(msg[1].data.qq, out long qq))
            {
                return "错误的文本";
            }
            string name = msg[2].data.text.Trim();
            Minecraft_QQ.SetPlayerName(qq, name);
            return $"已修改玩家[{qq}]ID为：{name}";
        }
        else
        {
            return "玩家错误，请检查";
        }
    }

    private static string GetMuteList()
    {
        if (Minecraft_QQ.PlayerConfig.MuteList.Count == 0)
            return "没有禁言的玩家";
        else
        {
            string a = "禁言的玩家：";
            foreach (string name in Minecraft_QQ.PlayerConfig.MuteList)
            {
                a += Environment.NewLine + name;
            }
            return a;
        }
    }
    private static string GetCantBind()
    {
        if (Minecraft_QQ.PlayerConfig.NotBindList.Count == 0)
        {
            return "没有禁止绑定的名字";
        }
        else
        {
            string a = "禁止绑定的名字列表：";
            foreach (string name in Minecraft_QQ.PlayerConfig.NotBindList)
            {
                a += Environment.NewLine + name;
            }
            return a;
        }
    }

    private static string FixModeChange()
    {
        string text;
        if (Minecraft_QQ.MainConfig.Setting.FixMode == false)
        {
            Minecraft_QQ.MainConfig.Setting.FixMode = true;
            text = "服务器维护模式已开启";
        }
        else
        {
            Minecraft_QQ.MainConfig.Setting.FixMode = false;
            text = "服务器维护模式已关闭";
        }
        ConfigWrite.Config();
        Logs.LogOut($"[Minecraft_QQ]{text}");
        return text;
    }
    private static string? GetOnlinePlayer(long fromGroup)
    {
        if (Minecraft_QQ.MainConfig.Setting.FixMode)
        {
            return Minecraft_QQ.MainConfig.Message.FixText;
        }
        if (PluginServer.IsReady() == true)
        {
            PluginServer.Send(new()
            {
                group = fromGroup.ToString(),
                command = CommderList.ONLINE
            });
            return null;
        }
        else
        {
            return "发送失败，服务器未准备好";
        }
    }
    private static string? GetOnlineServer(long fromGroup)
    {
        if (Minecraft_QQ.MainConfig.Setting.FixMode)
        {
            return Minecraft_QQ.MainConfig.Message.FixText;
        }
        if (PluginServer.IsReady() == true)
        {
            PluginServer.Send(new()
            {
                group = fromGroup.ToString(),
                command = CommderList.SERVER,
            });
            return null;
        }
        else
        {
            return "发送失败，服务器未准备好";
        }
    }
    private static bool SendCommand(long fromGroup, List<GroupMessagePack.Message> msg, long fromQQ)
    {
        foreach (var item in Minecraft_QQ.CommandConfig.CommandList)
        {
            string head = msg[0].data.text;
            head = Funtion.ReplaceFirst(head, Minecraft_QQ.MainConfig.Check.Head, "");
            if (!head.StartsWith(item.Key))
            {
                continue;
            }
            if (!PluginServer.IsReady())
            {
                RobotCore.SendGroupMessage(fromGroup,
                [
                    $"[CQ:at,qq={fromQQ}]",
                    "发送失败，服务器未准备好"
                ]);
                return true;
            }
            bool haveserver = false;
            List<string>? servers = null;
            if (item.Value.Servers != null && item.Value.Servers.Count != 0)
            {
                servers = [];
                foreach (var item1 in item.Value.Servers)
                {
                    if (PluginServer.MCServers.ContainsKey(item1))
                    {
                        servers.Add(item1);
                        haveserver = true;
                    }
                }
            }
            else
            {
                haveserver = true;
            }
            if (!haveserver)
            {
                RobotCore.SendGroupMessage(fromGroup,
                [
                    $"[CQ:at,qq={fromQQ}]",
                    "发送失败，对应的服务器未连接"
                ]);
                return true;
            }
            var player = Minecraft_QQ.GetPlayer(fromQQ);
            if (player == null)
            {
                RobotCore.SendGroupMessage(fromGroup,
                [
                        $"[CQ:at,qq={fromQQ}]",
                    "你未绑定ID"
                ]);
                return true;
            }
            if (!item.Value.PlayerUse && !player.IsAdmin)
            {
                return true;
            }
            string cmd = item.Value.Command;
            bool haveAt = false;
            if (cmd.Contains("{arg:at}") || cmd.Contains("{arg:atqq}"))
            {
                if (msg[1].type == "at")
                {
                    long qq = long.Parse(msg[1].data.qq);
                    var player1 = Minecraft_QQ.GetPlayer(qq);
                    if (player1 == null)
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[mirai:at:{fromQQ}]",
                            $"错误，玩家：{qq}没有绑定ID"
                        ]);
                        return true;
                    }
                    while (cmd.Contains("{arg:at}", StringComparison.CurrentCulture))
                        cmd = cmd.Replace("{arg:at}", player1.Name);
                    while (cmd.Contains("{arg:atqq}", StringComparison.CurrentCulture))
                        cmd = cmd.Replace("{arg:atqq}", $"{qq}");
                    haveAt = true;
                }
                else
                {
                    RobotCore.SendGroupMessage(fromGroup,
                    [
                        $"[mirai:at:{fromQQ}]",
                        $"错误，参数错误"
                    ]);
                    return true;
                }
            }
            while (cmd.Contains("{arg:name}", StringComparison.CurrentCulture))
                cmd = cmd.Replace("{arg:name}", player.Name);
            while (cmd.Contains("{arg:qq}", StringComparison.CurrentCulture))
                cmd = cmd.Replace("{arg:qq}", $"{player.QQ}");
            string argStr = "";
            for (int a = haveAt ? 3 : 2; a < msg.Count; a++)
            {
                if (!string.IsNullOrEmpty(msg[a].data.text))
                {
                    argStr += msg[a];
                }
            }
            var arg = argStr.Split(" ");
            int pos = 1;
            for (int index = 1; ; index++)
            {
                string args = "{arg" + index + "}";
                if (index > arg.Length)
                {
                    break;
                }
                if (cmd.Contains(args))
                {
                    for (; pos < arg.Length; pos++)
                    {
                        if (!string.IsNullOrWhiteSpace(arg[pos]))
                        {
                            cmd = cmd.Replace(args, arg[pos]);
                            break;
                        }
                    }
                }
                else
                    break;
            }
            if (cmd.Contains("{argx}"))
            {
                string temp = "";
                if (pos <= arg.Length)
                {
                    for (; pos < arg.Length; pos++)
                    {
                        if (!string.IsNullOrWhiteSpace(arg[pos]))
                        {
                            temp += $"{arg[pos]} ";
                        }
                    }
                    if (temp.Length > 1)
                    {
                        temp = temp[0..^1];
                    }
                }
                cmd = cmd.Replace("{argx}", temp);
            }
            PluginServer.Send(new TranObj
            {
                group = fromGroup.ToString(),
                command = cmd,
                isCommand = true,
                player = item.Value.PlayerSend ? player.Name : CommderList.COMM
            }, servers);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Type=2 群消息。
    /// </summary>
    /// <param name="fromGroup">来源群号。</param>
    /// <param name="fromQQ">来源QQ。</param>
    /// <param name="msg">消息内容。</param>
    public static void GroupMessage(long fromGroup, long fromQQ, string raw, List<GroupMessagePack.Message> msglist)
    {
        if (IMinecraft_QQ.IsStart == false)
            return;
        Logs.LogOut($"[{fromGroup}][QQ:{fromQQ}]:{raw}");
        if (Minecraft_QQ.GroupConfig.Groups.ContainsKey(fromGroup) == true)
        {
            GroupObj list = Minecraft_QQ.GroupConfig.Groups[fromGroup];
            //始终发送
            if (Minecraft_QQ.MainConfig.Setting.AutoSend == true && Minecraft_QQ.MainConfig.Setting.FixMode == false
                && PluginServer.IsReady() == true && list.EnableSay == true)
            {
                string msg_copy = raw;
                if (Minecraft_QQ.MainConfig.Setting.SendCommand || !msg_copy.StartsWith(Minecraft_QQ.MainConfig.Check.Head))
                {
                    var player = Minecraft_QQ.GetPlayer(fromQQ);
                    if (player != null && !Minecraft_QQ.PlayerConfig.MuteList.Contains(player.Name.ToLower())
                        && !string.IsNullOrWhiteSpace(player.Name))
                    {
                        msg_copy = Funtion.GetRich(msg_copy) ?? msg_copy;
                        if (Minecraft_QQ.MainConfig.Setting.ColorEnable == false)
                            msg_copy = Funtion.RemoveColorCodes(msg_copy);
                        if (string.IsNullOrWhiteSpace(msg_copy) == false)
                        {
                            var messagelist = new TranObj()
                            {
                                group = fromGroup.ToString(),
                                message = msg_copy,
                                player = !Minecraft_QQ.MainConfig.Setting.SendNickServer ?
                                player.Name : string.IsNullOrWhiteSpace(player.Nick) ?
                                player.Name : player.Nick,
                                command = CommderList.SPEAK
                            };
                            PluginServer.Send(messagelist);
                        }
                    }
                }
            }
            if (raw.StartsWith(Minecraft_QQ.MainConfig.Check.Head) && list.EnableCommand == true)
            {
                //去掉检测头
                raw = Funtion.ReplaceFirst(raw, Minecraft_QQ.MainConfig.Check.Head, "");
                string msg_low = raw.ToLower();
                var player = Minecraft_QQ.GetPlayer(fromQQ);
                if (Minecraft_QQ.MainConfig.Setting.AutoSend == false && msg_low.StartsWith(Minecraft_QQ.MainConfig.Check.Send))
                {
                    if (list.EnableSay == false)
                    {
                        RobotCore.SendGroupMessage(fromGroup, ["该群没有开启聊天功能"]);
                    }
                    else if (Minecraft_QQ.MainConfig.Setting.FixMode)
                    {
                        if (!string.IsNullOrWhiteSpace(Minecraft_QQ.MainConfig.Message.FixText))
                        {
                            RobotCore.SendGroupMessage(fromGroup, [$"[CQ:at,qq={fromQQ}]", Minecraft_QQ.MainConfig.Message.FixText]);
                        }
                    }
                    else if (PluginServer.IsReady() == false)
                    {
                        RobotCore.SendGroupMessage(fromGroup, [$"[CQ:at,qq={fromQQ}]", "发送失败，没有服务器链接"]);
                    }
                    else if (player == null || string.IsNullOrWhiteSpace(player.Name))
                    {
                        if (!string.IsNullOrWhiteSpace(Minecraft_QQ.MainConfig.Message.NoneBindID))
                        {
                            RobotCore.SendGroupMessage(fromGroup, [$"[CQ:at,qq={fromQQ}]", Minecraft_QQ.MainConfig.Message.NoneBindID]);
                        }
                        return;
                    }
                    else if (Minecraft_QQ.PlayerConfig.MuteList.Contains(player.Name.ToLower()))
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            "你已被禁言"
                        ]);
                    }
                    else
                    {
                        try
                        {
                            string msg_copy = raw;
                            msg_copy = msg_copy.Replace(Minecraft_QQ.MainConfig.Check.Send, "");
                            if (Minecraft_QQ.MainConfig.Setting.ColorEnable == false)
                                msg_copy = Funtion.RemoveColorCodes(msg_copy);
                            if (string.IsNullOrWhiteSpace(msg_copy) == false)
                            {
                                var messagelist = new TranObj()
                                {
                                    group = DataType.group,
                                    message = msg_copy,
                                    player = !Minecraft_QQ.MainConfig.Setting.SendNickServer ?
                                    player.Name : string.IsNullOrWhiteSpace(player.Nick) ?
                                    player.Name : player.Nick,
                                    command = CommderList.SPEAK
                                };
                                PluginServer.Send(messagelist);
                            }
                        }
                        catch (Exception e)
                        {
                            Logs.LogError(e);
                        }
                    }
                }
                else if (player != null && player.IsAdmin == true)
                {
                    if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Admin.Mute))
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            MutePlayer(msglist)
                        ]);
                    }
                    else if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Admin.UnMute))
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            UnmutePlayer(msglist)
                        ]);
                    }
                    else if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Admin.CheckBind))
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            GetPlayerID(msglist)
                        ]);
                    }
                    else if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Admin.Rename))
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            RenamePlayer(msglist)
                        ]);
                    }
                    else if (msg_low == Minecraft_QQ.MainConfig.Admin.Fix)
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            FixModeChange()
                        ]);
                    }
                    else if (msg_low == Minecraft_QQ.MainConfig.Admin.GetMuteList)
                    {
                        RobotCore.SendGroupMessage(fromGroup, [GetMuteList()]);
                    }
                    else if (msg_low == Minecraft_QQ.MainConfig.Admin.GetCantBindList)
                    {
                        RobotCore.SendGroupMessage(fromGroup, [GetCantBind()]);
                    }
                    else if (msg_low == Minecraft_QQ.MainConfig.Admin.Reload)
                    {
                        RobotCore.SendGroupMessage(fromGroup, ["开始重读配置文件"]);
                        Minecraft_QQ.Reload();
                        RobotCore.SendGroupMessage(fromGroup, ["重读完成"]);
                    }
                    else if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Admin.Nick))
                    {
                        List<string> lists = ["at:" + fromQQ, SetNick(msglist)];
                        RobotCore.SendGroupMessage(fromGroup, lists);
                    }
                }
                if (msg_low == Minecraft_QQ.MainConfig.Check.PlayList)
                {
                    var test = GetOnlinePlayer(fromGroup);
                    if (test != null)
                        RobotCore.SendGroupMessage(fromGroup, [test]);
                }
                else if (msg_low == Minecraft_QQ.MainConfig.Check.ServerCheck)
                {
                    var test = GetOnlineServer(fromGroup);
                    if (test != null)
                        RobotCore.SendGroupMessage(fromGroup, [test]);
                }

                else if (msg_low.StartsWith(Minecraft_QQ.MainConfig.Check.Bind))
                {
                    var str = SetPlayerName(fromGroup, fromQQ, msglist);
                    if (str != null)
                    {
                        RobotCore.SendGroupMessage(fromGroup,
                        [
                            $"[CQ:at,qq={fromQQ}]",
                            str
                        ]);
                    }
                }
                else if (SendCommand(fromGroup, msglist, fromQQ) == true)
                {

                }
                else if (Minecraft_QQ.MainConfig.Setting.AskEnable
                    && Minecraft_QQ.AskConfig.AskList.ContainsKey(msg_low) == true)
                {
                    string message = Minecraft_QQ.AskConfig.AskList[msg_low];
                    if (string.IsNullOrWhiteSpace(message) == false)
                    {
                        RobotCore.SendGroupMessage(fromGroup, [message]);
                    }
                }
                else if (string.IsNullOrWhiteSpace(Minecraft_QQ.MainConfig.Message.UnknowText) == false)
                {
                    RobotCore.SendGroupMessage(fromGroup, [Minecraft_QQ.MainConfig.Message.UnknowText]);
                }
            }
        }
    }
}
