using Color_yr.Minecraft_QQ.Utils;
using Newtonsoft.Json.Linq;

namespace Color_yr.Minecraft_QQ.MySocket
{
    class Message
    {
        public static MessageObj MessagetoObj(string read)
        {
            MessageObj temp = new MessageObj();
            try
            {
                JObject jsonData = JObject.Parse(read);
                if (jsonData["data"].ToString() == "data")
                {
                    temp.group = jsonData["group"].ToString();
                    temp.message = jsonData["message"].ToString();
                    temp.player = jsonData["player"].ToString();
                    temp.is_commder = false;
                }
                else if (jsonData["data"].ToString() == "start")
                {
                    temp.group = "start";
                    temp.message = jsonData["message"].ToString();
                    temp.is_commder = false;
                }
            }
            catch
            {
                temp.player = "没有玩家";
            }
            return temp;
        }

        public static void MessageDo(string read)
        {
            int local;
            while (read.IndexOf(Minecraft_QQ.MainConfig.链接.数据头) == 0 && read.IndexOf(Minecraft_QQ.MainConfig.链接.数据尾) != -1)
            {
                string buff = Funtion.GetString(read, Minecraft_QQ.MainConfig.链接.数据头, Minecraft_QQ.MainConfig.链接.数据尾);
                buff = Funtion.RemoveColorCodes(buff);
                MessageObj message = MessagetoObj(buff);
                if (string.IsNullOrWhiteSpace(message.message) == true ||
                    string.IsNullOrWhiteSpace(message.player) == true)
                    return;
                if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(message.player.ToLower()) == true)
                    return;
                if (message.is_commder == false && message.group == "group")
                {
                    if (Minecraft_QQ.MainConfig.设置.使用昵称发送至群 == true)
                    {
                        PlayerObj player = Funtion.GetPlayer(message.player);
                        if (player != null && string.IsNullOrWhiteSpace(player.昵称) == false)
                            message.message = message.message.Replace(message.player, player.昵称);
                    }
                    foreach (var item in Minecraft_QQ.GroupConfig.群列表)
                    {
                        if (item.Value.开启对话 == true)
                            Send.SendList.Add(new SendObj
                            {
                                Group = item.Key,
                                Message = message.message
                            });
                    }
                }
                else
                {
                    long.TryParse(message.group, out long group);
                    if (Minecraft_QQ.GroupConfig.群列表.ContainsKey(group) == true)
                    {
                        Send.SendList.Add(new SendObj
                        {
                            Group = group,
                            Message = message.message
                        });
                    }
                }
                local = read.IndexOf(Minecraft_QQ.MainConfig.链接.数据尾);
                read = read.Substring(local + Minecraft_QQ.MainConfig.链接.数据尾.Length);
            }
        }
        public static string MessageCheck(string read)
        {
            string buff = Funtion.GetString(read, Minecraft_QQ.MainConfig.链接.数据头, Minecraft_QQ.MainConfig.链接.数据尾);
            buff = Funtion.RemoveColorCodes(buff);
            MessageObj message = MessagetoObj(buff);
            if (message.group == "start")
                return message.message;
            return null;
        }
    }
}
