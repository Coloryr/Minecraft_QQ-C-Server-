using System.Threading.Tasks;

namespace Minecraft_QQ_Core
{
    public enum GuiFun
    {
        ServerList, PlayerList
    }
    public class IMinecraft_QQ
    {
        public const string Version = "3.7.1";

        /// <summary>
        /// 已经启动
        /// </summary>
        public static bool IsStart = false;

        public delegate void ShowMessage(string message);
        public delegate void ServerConfig(string name, string config);
        public delegate void ConfigInit();
        public delegate void Gui(GuiFun dofun);
        public delegate void Log(string message);

        public static ShowMessage ShowMessageCall;
        public static ServerConfig ServerConfigCall;
        public static ConfigInit ConfigInitCall;
        public static Gui GuiCall;
        public static Log LogCall;

        public static Minecraft_QQ Main { get; private set; }

        public static Task Start()
        {
            Main = new();
            return Main.Start();
        }
        public static void Stop()
        {
            Main.Stop();
        }
    }
}
