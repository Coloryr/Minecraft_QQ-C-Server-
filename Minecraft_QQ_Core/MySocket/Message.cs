using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;

namespace Minecraft_QQ_Core.MySocket
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
        public const string SPEAK = "speak";
        public const string ONLINE = "online";
        public const string SERVER = "server";

        public const string COMM = "后台";
    }
    internal class Message
    {
        private Minecraft_QQ Main;
        public Message(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
        }
        public void MessageDo(string server, string read)
        {
            int local;
            while (read.IndexOf(Main.MainConfig.链接.数据头) == 0 && read.IndexOf(Main.MainConfig.链接.数据尾) != -1)
            {
                string buff = Funtion.GetString(read, Main.MainConfig.链接.数据头, Main.MainConfig.链接.数据尾);
                ReadObj message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(buff));
                if (string.IsNullOrWhiteSpace(message.data))
                {
                    return;
                }

                switch (message.data)
                {
                    case DataType.data:
                        if (string.IsNullOrWhiteSpace(message.message) == true ||
                            string.IsNullOrWhiteSpace(message.player) == true)
                        {
                            return;
                        }

                        if (Main.PlayerConfig.禁言列表.Contains(message.player.ToLower()) == true)
                        {
                            return;
                        }
                        if (message.group == DataType.group)
                        {
                            if (Main.MainConfig.设置.使用昵称发送至群 == true)
                            {
                                PlayerObj player = Main.GetPlayer(message.player);
                                if (player != null && string.IsNullOrWhiteSpace(player.昵称) == false)
                                {
                                    message.message = Funtion.ReplaceFirst(message.message, message.player, player.昵称);
                                }
                            }
                            foreach (var item in Main.GroupConfig.群列表)
                            {
                                if (item.Value.开启对话)
                                {
                                    Main.SendGroup.AddSend(new()
                                    {
                                        Group = item.Key,
                                        Message = message.message
                                    });
                                }
                            }
                        }
                        else
                        {
                            long.TryParse(message.group, out long group);
                            if (Main.GroupConfig.群列表.ContainsKey(group))
                            {
                                Main.SendGroup.AddSend(new()
                                {
                                    Group = group,
                                    Message = message.message
                                });
                            }
                        }
                        break;
                    case DataType.config:
                        IMinecraft_QQ.ServerConfigCall?.Invoke(server, message.message);
                        break;
                    default:
                        break;
                }

                local = read.IndexOf(Main.MainConfig.链接.数据尾);
                read = read[(local + Main.MainConfig.链接.数据尾.Length)..];
            }
        }
        public string StartCheck(string read)
        {
            string buff = Funtion.GetString(read, Main.MainConfig.链接.数据头, Main.MainConfig.链接.数据尾);
            ReadObj message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(buff));
            if (message.data == DataType.start)
            {
                return message.message;
            }
            return null;
        }
    }
}
