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
public class SendGroup
{
    private Thread SendT;
    private bool Run;
    private List<SendObj> SendList { get; set; } = new();

    private readonly Minecraft_QQ Main;
    public SendGroup(Minecraft_QQ Minecraft_QQ)
    {
        Main = Minecraft_QQ;
    }

    public void AddSend(SendObj obj)
    {
        SendList.Add(obj);
    }

    private void SendToGroup()
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
                            temp += a.Message + Environment.NewLine;
                        }
                    }
                    if (have)
                    {
                        temp = temp[0..^1];
                        Main.robot.Robot.SendGroupMessage(Main.MainConfig.RobotSetting.QQ, group, new() 
                        { temp });
                    }
                    SendList.RemoveAll(a => a.Group == group);
                }
            }
            Thread.Sleep(Main.MainConfig.Setting.SendDelay);
        }
    }
    public void Start()
    {
        Run = true;
        SendT = new(SendToGroup);
        SendT.Start();
    }

    public void Stop()
    {
        Run = false;
    }
}