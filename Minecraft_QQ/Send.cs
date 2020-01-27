using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Color_yr.Minecraft_QQ
{
    public class Send_Obj
    { 
        public long group { get; set; }
        public string message { get; set; }
    }
    class Send
    {
        public static Thread Send_T;
        public static List<Send_Obj> Send_List { get; set; } = new List<Send_Obj>() { };

        public static void Send_()
        {
            logs.Log_write("线程启动");
            while (true)
            {
                if (Send_List.Count != 0)
                {
                    var group = Send_List.First().group;
                    string b = null;
                    lock (Send_List)
                    {
                        var Send_List_C = new List<Send_Obj>(Send_List);

                        foreach (var a in Send_List_C)
                        {
                            if (group == a.group && string.IsNullOrWhiteSpace(a.message) == false)
                            {
                                b += a.message + '\n';
                            }
                            Send_List.Remove(a);
                        }
                        if (string.IsNullOrWhiteSpace(b) == false)
                        {
                            b = b.Substring(0, b.Length - 1);
                            Minecraft_QQ.Plugin.SendGroupMessage(group, b);
                        }
                    }
                }
                Thread.Sleep(Minecraft_QQ.Mainconfig.设置.发送群消息间隔);
            }
        }
    }
}