using Minecraft_QQ_Core.Robot;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Minecraft_QQ_Core.Utils
{
    internal record SendObj
    {
        public long Group { get; set; }
        public string Message { get; set; }
    }
    internal class SendGroup
    {
        private static Thread SendT;
        private static bool Run;
        public static List<SendObj> SendList { get; set; } = new();

        public static void SendToGroup()
        {
            while (Run)
            {
                if (SendList.Count != 0)
                {
                    var group = SendList.First().Group;
                    string b = null;
                    lock (SendList)
                    {
                        var SendList_C = SendList.Where(a => a.Group == group);
                        var have = false;
                        foreach (var a in SendList_C)
                        {
                            if (string.IsNullOrWhiteSpace(a.Message) == false)
                            {
                                have = true;
                                b += a.Message + '\n';
                            }
                        }
                        if (have)
                        {
                            b = b[0..^1];
                            RobotSocket.SendGroupMessage(group, b);
                        }
                        SendList.RemoveAll(a => a.Group == group);
                    }
                }
                Thread.Sleep(Minecraft_QQ.MainConfig.设置.发送群消息间隔);
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
}