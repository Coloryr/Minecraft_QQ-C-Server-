using Newtonsoft.Json.Linq;

namespace Color_yr.Minecraft_QQ
{
    class message
    {
        public static Message_send_obj Message_re(string read)
        {
            Message_send_obj messagelist = new Message_send_obj();
            try
            {
                JObject jsonData = JObject.Parse(read);
                if (jsonData["data"].ToString() == "data")
                {
                    messagelist.group = jsonData["group"].ToString();
                    messagelist.message = jsonData["message"].ToString();
                    messagelist.player = jsonData["player"].ToString();
                    messagelist.is_commder = false;
                }
            }
            catch
            {
                messagelist.player = "没有玩家";
            }
            return messagelist;
        }

        public static void Message(string read)
        {
            int a;
            while (read.IndexOf(Minecraft_QQ.Mainconfig.链接.数据头) == 0 && read.IndexOf(Minecraft_QQ.Mainconfig.链接.数据尾) != -1)
            {
                string buff = Utils.Get_string(read, Minecraft_QQ.Mainconfig.链接.数据头, Minecraft_QQ.Mainconfig.链接.数据尾);
                buff = Utils.RemoveColorCodes(buff);
                Message_send_obj message = Message_re(buff);
                if (string.IsNullOrWhiteSpace(message.message) == true ||
                    string.IsNullOrWhiteSpace(message.player) == true)
                    return;
                if (Minecraft_QQ.Playerconfig.禁言列表.Contains(message.player.ToLower()) == true)
                    return;
                if (message.is_commder == false && message.group == "group")
                {
                    if (Minecraft_QQ.Mainconfig.设置.使用昵称发送至群 == true)
                    {
                        Player_save_obj player = Utils.Get_player_from_id(message.player);
                        if (player != null && string.IsNullOrWhiteSpace(player.nick) == false)
                            message.message = message.message.Replace(message.player, player.nick);
                    }
                    foreach (Group_save_obj value in Minecraft_QQ.Groupconfig.群列表.Values)
                    {
                        if (value.say == true)
                            Send.Send_List.Add(new Send_Obj { group = value.group_l, message = message.message });
                    }
                }
                else
                {
                    long.TryParse(message.group, out long group);
                    if (Minecraft_QQ.Groupconfig.群列表.ContainsKey(group) == true)
                    {
                        Group_save_obj list = Minecraft_QQ.Groupconfig.群列表[group];
                        Send.Send_List.Add(new Send_Obj { group = list.group_l, message = message.message });
                    }
                }
                a = read.IndexOf(Minecraft_QQ.Mainconfig.链接.数据尾);
                read = read.Substring(a + Minecraft_QQ.Mainconfig.链接.数据尾.Length);
            }
        }
    }
}
