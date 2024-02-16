using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_QQ_Core.Robot;

public static class RobotCore
{
    public static OneBotSDK Robot;

    public static void Message(object data)
    {
        if (data is GroupMessagePack pack)
        {
            Minecraft_QQ.GroupMessage(pack.group_id, pack.user_id, pack.raw_message, pack.message);
        }
    }

    public static void Start()
    {
        Robot = new OneBotSDK(Minecraft_QQ.MainConfig.Setting.BotUrl,
            Minecraft_QQ.MainConfig.Setting.BotAuthorization);
        Robot.Start();
    }

    public static void SendGroupTempMessage(long group, long to, List<string> list)
    {
        var msg = new StringBuilder();
        foreach (var item in list)
        {
            msg.Append(item);
        }
        var obj = new JObject()
        {
            { "action", "send_private_msg" },
            { "params", new JObject()
            {
                { "user_id", to },
                { "message", msg.ToString() }
            }
            }
        };

        Robot.Send(obj.ToString());
    }

    public static void SendGroupMessage(long group, List<string> list)
    {
        var msg = new StringBuilder();
        foreach (var item in list)
        {
            msg.Append(item);
        }
        var obj = new JObject()
        {
            { "action", "send_group_msg" },
            { "params", new JObject()
            {
                { "group_id", group },
                { "message", msg.ToString() }
            } 
            }
        };

        Robot.Send(obj.ToString());
    }

    public static void Stop() 
    {
        Robot.Stop();
    }
}