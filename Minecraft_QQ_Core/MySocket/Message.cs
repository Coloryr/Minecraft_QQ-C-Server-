using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;

namespace Minecraft_QQ_Core.MySocket;

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
        ReadObj message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(read));
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

                if (Main.PlayerConfig.MuteList.Contains(message.player.ToLower()) == true)
                {
                    return;
                }
                if (message.group == DataType.group)
                {
                    if (Main.MainConfig.Setting.SendNickGroup == true)
                    {
                        PlayerObj player = Main.GetPlayer(message.player);
                        if (player != null && string.IsNullOrWhiteSpace(player.Nick) == false)
                        {
                            message.message = Funtion.ReplaceFirst(message.message, message.player, player.Nick);
                        }
                    }
                    foreach (var item in Main.GroupConfig.Groups)
                    {
                        if (item.Value.EnableSay)
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
                    if (Main.GroupConfig.Groups.ContainsKey(group))
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
    }
    public static string StartCheck(string read)
    {
        ReadObj message = JsonConvert.DeserializeObject<ReadObj>(Funtion.RemoveColorCodes(read));
        if (message.data == DataType.start)
        {
            return message.message;
        }
        return null;
    }
}
