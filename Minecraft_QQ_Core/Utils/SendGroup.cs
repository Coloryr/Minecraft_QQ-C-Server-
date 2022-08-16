using Minecraft_QQ_Core.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Minecraft_QQ_Core.Utils;

public record SendObj
{
    public long Group { get; set; }
    public string Message { get; set; }
}
public static class SendGroup
{
    private static Thread SendT;
    private static bool Run;
    private static List<SendObj> SendList { get; set; } = new();

    public static void AddSend(SendObj obj)
    {
        SendList.Add(obj);
    }

    private static void SendToGroup()
    {
        while (Run)
        {
            if (SendList.Count != 0)
            {
                var group = SendList.First().Group;
                string temp = null;
                lock (SendList)
                {
                    var SendList_C = SendList.Where(a => a.Group == group);
                    var have = false;
                    foreach (var a in SendList_C)
                    {
                        if (string.IsNullOrWhiteSpace(a.Message) == false)
                        {
                            have = true;
                            temp += a.Message + "\n";
                        }
                    }
                    if (have)
                    {
                        temp = temp[0..^1];
                        RobotCore.Robot.SendGroupMessage(
                            Minecraft_QQ.MainConfig.RobotSetting.QQ, group, 
                            new() { temp });
                    }
                    SendList.RemoveAll(a => a.Group == group);
                }
            }
            Thread.Sleep(Minecraft_QQ.MainConfig.Setting.SendDelay);
        }
    }
    public static void Start()
    {
        Run = true;
        SendT = new(SendToGroup);
        SendT.Start();
    }

    public static void Stop()
    {
        Run = false;
    }
}