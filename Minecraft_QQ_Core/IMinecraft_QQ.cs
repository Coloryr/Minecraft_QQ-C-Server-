﻿using System.Threading.Tasks;

namespace Minecraft_QQ
{
    public enum GuiFun
    {
        ServerList
    }
    public class IMinecraft_QQ
    {
        public const string Version = "3.0.0.0";

        public static bool Run;

        public delegate void ShowMessage(string message);
        public static ShowMessage ShowMessageCall;

        public delegate void ServerConfig(string name, string config);
        public static ServerConfig ServerConfigCall;

        public delegate void Gui(GuiFun dofun);
        public static Gui GuiCall;

        public delegate void Log(string message);
        public static Log LogCall;
        public static void Start()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.Start());
        }
        public static void Stop()
        {
            Minecraft_QQ.Stop();
        }
    }
}
