using Minecraft_QQ_Core.Utils;
using System;

namespace Minecraft_QQ_Core.Robot;

public class RobotCore
{
    public RobotSDK Robot = new();
    private Minecraft_QQ main;

    public RobotCore(Minecraft_QQ main) 
    {
        this.main = main;
    }

    private void Message(byte type, object data)
    {
        switch (type)
        {
            case 49:
                var pack = data as GroupMessageEventPack;
                main.GroupMessage(pack.id, pack.fid, pack.message);
                break;
        }
    }

    private void Log(LogType type, string data)
    {
        Logs.LogOut($"[Robot]{type} {data}");
    }

    private void State(StateType type)
    {
        Logs.LogOut($"[Robot]{type}");
    }

    public void Start()
    {
        RobotConfig config = new()
        {
            IP = main.MainConfig.RobotSetting.IP,
            Port = main.MainConfig.RobotSetting.Port,
            Name = "Minecraft_QQ",
            Pack = new() { 49 },
            RunQQ = main.MainConfig.RobotSetting.QQ,
            Time = main.MainConfig.RobotSetting.CheckDelay,
            Check = main.MainConfig.RobotSetting.Check,
            CallAction = Message,
            LogAction = Log,
            StateAction = State
        };

        Robot.Set(config);
        Robot.SetPipe(new ColorMiraiSocket(Robot));
        Robot.Start();
    }

    public void Stop() 
    {
        Robot.Stop();
    }
}