using Native.Csharp.App;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Color_yr.Minecraft_QQ
{
    class message
    {
        public static messagelist Message_re(string read)
        {
            messagelist messagelist = new messagelist();
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
            catch { messagelist.player = "没有玩家"; }
            return messagelist;
        }

        public static void Message(string read)
        {
            while (read.IndexOf(config_read.data_Head) == 0 && read.IndexOf(config_read.data_End) != -1)
            {
                string buff = use.get_string(read, config_read.data_Head, config_read.data_End);
                buff = use.RemoveColorCodes(buff);
                messagelist messagelist = Message_re(buff);
                if (messagelist.message == null)
                    return;
                if (use.check_mute(messagelist.player) == true)
                    return;
                if (messagelist.is_commder == false && messagelist.group == "group")
                {
                    if (config_read.nick_mode_group == true)
                        messagelist.message = messagelist.message.Replace(messagelist.player, use.get_nick(messagelist.player));
                    Dictionary<long, grouplist>.ValueCollection valueCol = config_read.group_list.Values;
                    foreach (grouplist value in valueCol)
                    {
                        if (value.say == true)
                            Common.CqApi.SendGroupMessage(value.group_l, messagelist.message);
                    }
                }
                else
                {
                    long.TryParse(messagelist.group, out long group);
                    if (config_read.group_list.ContainsKey(group) == true)
                    {
                        grouplist list = config_read.group_list[group];
                        Common.CqApi.SendGroupMessage(list.group_l, messagelist.message);
                    }
                }
                int i = read.IndexOf(config_read.data_End);
                read = read.Substring(i + config_read.data_End.Length);
            }
        }
    }
}
