using Native.Csharp.Sdk.Cqp;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ
{
    public class IMinecraft_QQ
    {
        public const string Version = "2.7.3.6";
        public static CQApi api { get; set; }
        public static void Start()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.Start());
        }
        public static void Stop()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.stop());
        }
        public static void Menu()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.OpenSettingForm());
        }
        public static void SendGroupMessage(long group, string message)
        {
            api.SendGroupMessage(group, message);
        }
        public static void SendPrivateMessage(long user, string message)
        {
            api.SendPrivateMessage(user, message);
        }
        public static void RGroupMessage(long group, long user, string message)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.GroupMessage(group, user, message));
        }
        public static void RPrivateMessage(long user, string message)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.PrivateMessage(user, message));
        }

        public static string Code_At(long user)
        {
            return CQApi.CQCode_At(user).ToSendString();
        }
    }
}
