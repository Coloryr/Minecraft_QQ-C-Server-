using System.Threading.Tasks;

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
        public static void Start()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.Start());
        }
        public static void Stop()
        {
            Minecraft_QQ.Stop();
        }
        public static void SGroupMessage(long group, string message)
        {
            Api.SendGroupMessage(group, message);
        }
        public static void SPrivateMessage(long user, string message)
        {
            Api.SendPrivateMessage(user, message);
        }
        public static void RGroupMessage(long group, long user, string message)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.GroupMessage(group, user, message));
        }
        public static void RPrivateMessage(long user, string message)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.PrivateMessage(user, message));
        }

        public static string CodeAt(long user)
        {
            return CQApi.CQCode_At(user).ToSendString();
        }
    }
}
