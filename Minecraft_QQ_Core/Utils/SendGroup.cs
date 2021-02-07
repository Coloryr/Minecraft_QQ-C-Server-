using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Minecraft_QQ_Core.Utils
{
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
                            Main.Robot.SendGroupMessage(group, b);
                        }
                        SendList.RemoveAll(a => a.Group == group);
                    }
                }
                Thread.Sleep(Main.MainConfig.设置.发送群消息间隔);
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
}