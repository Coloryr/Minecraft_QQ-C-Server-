using System.Threading.Tasks;

namespace Minecraft_QQ_Core
{
    public enum GuiFun
    {
        ServerList, PlayerList
    }
    public class IMinecraft_QQ
    {
        public const string Version = "3.2.0.1";

        public static bool Run;
        public static bool IsStop;
        /// <summary>
        /// 已经启动
        /// </summary>
        public static bool IsStart = false;

        public delegate void ShowMessage(string message);
        public static ShowMessage ShowMessageCall;

        public delegate void ServerConfig(string name, string config);
        public static ServerConfig ServerConfigCall;

        public delegate void Gui(GuiFun dofun);
        public static Gui GuiCall;

        public delegate void Log(string message);
        public static Log LogCall;
        public static async Task Start()
        {
            await Task.Run(() =>
            {
                Minecraft_QQ.Start();
            });
        }
        public static void Stop()
        {
            Minecraft_QQ.Stop();
        }
    }
}
