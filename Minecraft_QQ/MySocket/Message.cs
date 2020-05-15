using Minecraft_QQ.Config;
using Minecraft_QQ.SetWindow;
using Minecraft_QQ.Utils;
using Newtonsoft.Json;

namespace Minecraft_QQ.MySocket
{
    public class DataType
    {
        public const string data = "data";
        public const string group = "group";
        public const string start = "start";
        public const string pause = "pause";
        public const string config = "config";
        public const string message = "message";
        public const string player = "player";
        public const string set = "set";
    }
    internal class CommderList
    {
        public static readonly string SPEAK = "speak";
        public static readonly string ONLINE = "online";
        public static readonly string SERVER = "server";
    }
    internal class Message
    {
        public static void MessageDo(string server, string read)
        {
            int local;
            while (read.IndexOf(Minecraft_QQ.MainConfig.链接.数据头) == 0 && read.IndexOf(Minecraft_QQ.MainConfig.链接.数据尾) != -1)
            {
                string buff = Funtion.GetString(read, Minecraft_QQ.MainConfig.链接.数据头, Minecraft_QQ.MainConfig.链接.数据尾);
                var message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(buff));
                if (string.IsNullOrWhiteSpace(message.data))
                    return;
                switch (message.data)
                {
                    case DataType.data:
                        if (string.IsNullOrWhiteSpace(message.message) == true ||
                            string.IsNullOrWhiteSpace(message.player) == true)
                            return;
                        if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(message.player.ToLower()) == true)
                            return;
                        if (message.group == DataType.group)
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
                        break;
                    case DataType.config:
                        Minecraft_QQ.SetWindow?.ServerConfig(server, message.message);
                        break;
                }

                local = read.IndexOf(Minecraft_QQ.MainConfig.链接.数据尾);
                read = read.Substring(local + Minecraft_QQ.MainConfig.链接.数据尾.Length);
            }
        }
        public static string StartCheck(string read)
        {
            string buff = Funtion.GetString(read, Minecraft_QQ.MainConfig.链接.数据头, Minecraft_QQ.MainConfig.链接.数据尾);
            var message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(buff));
            if (message.data == DataType.start)
                return message.message;
            return null;
        }
    }
}
