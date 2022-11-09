using ColoryrSDK;
using Minecraft_QQ_Core.Utils;

namespace Minecraft_QQ_Core.Robot;

public static class RobotCore
{
    public static RobotSDK Robot = new();

    private static void Message(int type, object data)
    {
        switch (type)
        {
            case 49:
                var pack = data as GroupMessageEventPack;
                Minecraft_QQ.GroupMessage(pack.id, pack.fid, pack.message);
                break;
        }
    }

    private static void Log(LogType type, string data)
    {
        Logs.LogOut($"[Robot]{type} {data}");
    }

    private static void State(StateType type)
    {
        Logs.LogOut($"[Robot]{type}");
    }

    public static void Start()
    {
        RobotConfig config = new()
        {
            IP = Minecraft_QQ.MainConfig.RobotSetting.IP,
            Port = Minecraft_QQ.MainConfig.RobotSetting.Port,
            Name = "Minecraft_QQ",
            Pack = new() { 49 },
            RunQQ = Minecraft_QQ.MainConfig.RobotSetting.QQ,
            Time = Minecraft_QQ.MainConfig.RobotSetting.CheckDelay,
            Check = Minecraft_QQ.MainConfig.RobotSetting.Check,
            CallAction = Message,
            LogAction = Log,
            StateAction = State
        };

        Robot.Set(config);
        Robot.SetPipe(new ColorMiraiNetty(Robot));
        Robot.Start();
    }

    public static void Stop() 
    {
        Robot.Stop();
    }
}