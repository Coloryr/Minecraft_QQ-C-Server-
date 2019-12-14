using Native.Csharp.App;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Color_yr.Minecraft_QQ
{
    class Send
    {
        public static Thread Send_T;
        public static Dictionary<long, string> Send_List { get; set; } = new Dictionary<long, string>() { };

        public static void Send_()
        {
            logs.Log_write("线程启动");
            while (true)
            {
                if (Send_List.Count != 0)
                {
                    Dictionary<long, string> Send_List_C;

                    long group = Send_List.Keys.First();
                    string b = null;
                    lock (Send_List)
                    {
                        Send_List_C = new Dictionary<long, string>(Send_List);
                        Send_List.Clear();
                    }
                    foreach (KeyValuePair<long, string> a in Send_List_C.ToArray())
                    {
                        if (group == a.Key && string.IsNullOrWhiteSpace(a.Value) == false)
                        {
                            b += a.Value + '\n';
                        }
                        Send_List.Remove(a.Key);
                    }
                    if (string.IsNullOrWhiteSpace(b) == false)
                    {
                        b = b.Substring(0, b.Length - 1);
                        Common.CqApi.SendGroupMessage(group, b);
                    }
                }
                Thread.Sleep(admin_config.Send_Tick);
            }
        }
    }
}