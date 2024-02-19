using DotNetty.Buffers;
using Minecraft_QQ_Core.Utils;
using System.Text;

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
internal static class Message
{
    public static string ReadString(this IByteBuffer read)
    {
        var length = read.ReadInt();
        var bytes = new byte[length];
        read.ReadBytes(bytes);

        return Encoding.UTF8.GetString(bytes);
    }

    public static IByteBuffer WriteString(this IByteBuffer data, string tmep)
    {
        var bytes = Encoding.UTF8.GetBytes(tmep);
        data.WriteInt(bytes.Length);
        data.WriteBytes(bytes);

        return data;
    }

    public static void MessageDo(string server, IByteBuffer read)
    {
        ReadObj message = new()
        {
            group = read.ReadString(),
            message = read.ReadString(),
            player = read.ReadString(),
            data = read.ReadString()
        };
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

                if (Minecraft_QQ.PlayerConfig.MuteList.Contains(message.player.ToLower()) == true)
                {
                    return;
                }
                if (!Minecraft_QQ.MainConfig.Setting.ColorEnable)
                {
                    message.message = Funtion.RemoveColorCodes(message.message);
                }
                if (message.group == DataType.group)
                {
                    if (Minecraft_QQ.MainConfig.Setting.SendNickGroup == true)
                    {
                        var player = Minecraft_QQ.GetPlayer(message.player);
                        if (player != null && string.IsNullOrWhiteSpace(player.Nick) == false)
                        {
                            message.message = Funtion.ReplaceFirst(message.message, message.player, player.Nick);
                        }
                    }
                    foreach (var item in Minecraft_QQ.GroupConfig.Groups)
                    {
                        if (item.Value.EnableSay)
                        {
                            SendGroup.AddSend(new()
                            {
                                Group = item.Key,
                                Message = message.message
                            });
                        }
                    }
                }
                else
                {
                    _ = long.TryParse(message.group, out long group);
                    if (Minecraft_QQ.GroupConfig.Groups.ContainsKey(group))
                    {
                        SendGroup.AddSend(new()
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
}
