using Minecraft_QQ_Core;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Minecraft_QQ_Cmd
{
    class Program
    {
        public const string Version = IMinecraft_QQ.Version;
        static void Main()
        {
            Console.WriteLine("正在启动Minecraft_QQ");
            IMinecraft_QQ.ShowMessageCall = new IMinecraft_QQ.ShowMessage(Message);
            IMinecraft_QQ.LogCall = new IMinecraft_QQ.Log(Message);
            IMinecraft_QQ.Start();

            while (!IMinecraft_QQ.IsStart)
            {
                Thread.Sleep(100);
                if (IMinecraft_QQ.IsStop)
                {
                    IMinecraft_QQ.IsStop = false;
                    Console.WriteLine("请手动设置后重启");
                    Console.ReadLine();
                    return;
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                while (true)
                {
                    string a = Console.ReadLine();
                    if (a == "stop")
                    {
                        IMinecraft_QQ.Stop();
                    }
                }
            }
        }
        private static void Message(string message)
        {
            Console.WriteLine(message);
        }
    }
}
