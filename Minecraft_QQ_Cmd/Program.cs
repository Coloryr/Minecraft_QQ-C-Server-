using Minecraft_QQ_Core;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ_Cmd
{
    class Program
    {
        public const string Version = IMinecraft_QQ.Version;
        static async Task Main()
        {
            Console.WriteLine("正在启动Minecraft_QQ");
            IMinecraft_QQ.ShowMessageCall = Message;
            IMinecraft_QQ.LogCall = Message;
            IMinecraft_QQ.ConfigInitCall = ConfigInit;
            await Minecraft_QQ.Start();

            if (Environment.UserInteractive)
            {
                while (true)
                {
                    string a = Console.ReadLine();
                    var arg = a.Split(' ');
                    switch (arg[0])
                    {
                        case "help":
                            Console.WriteLine("帮助手册");
                            Console.WriteLine("reload 重读配置文件");
                            Console.WriteLine("stop 关闭");
                            break;
                        case "reload":
                            Console.WriteLine("正在读取配置文件");
                            var res = Minecraft_QQ.Reload();
                            if (res)
                            {
                                Console.WriteLine("已成功读取配置文件");
                            }
                            else
                            {
                                Console.WriteLine("配置文件读取失败");
                            }
                            break;
                        case "stop":
                            Console.WriteLine("正在关闭");
                            Minecraft_QQ.Stop();
                            return;
                    }
                }
            }
        }
        private static void Message(string message)
        {
            Console.WriteLine(message);
        }

        private static void ConfigInit() 
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("进行初始配置");
                while (true)
                {
                    Console.Write("请输入主群号：");
                    string a = Console.ReadLine();
                    if (long.TryParse(a, out var group))
                    {
                        group = Math.Abs(group);
                        Minecraft_QQ.GroupConfig.Groups.Add(group, new()
                        {
                            Group = group.ToString(),
                            EnableCommand = true,
                            EnableSay = true,
                            IsMain = true
                        });
                        break;
                    }
                    Console.WriteLine("非法输入");
                }
            }
        }
    }
}
