using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_QQ_Core.Robot;

internal static class RobotCore
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
        Robot = new OneBotSDK(Minecraft_QQ.MainConfig.RobotSetting.Url,
            Minecraft_QQ.MainConfig.RobotSetting.Authorization);
        Robot.Start();
    }

    public static void SendGroupTempMessage(long qq, long group, long to, List<string> list)
    { 
        
    }

    public static void SendGroupMessage(long qq, long group, List<string> list)
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
                { "message", msg.ToString() },
                { "auto_escape", true }
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