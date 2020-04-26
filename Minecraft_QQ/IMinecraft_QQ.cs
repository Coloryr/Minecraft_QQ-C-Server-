using Native.Sdk.Cqp;
using System.Threading.Tasks;

namespace Minecraft_QQ
{
    public class IMinecraft_QQ
    {
        public const string Version = "2.8.0.3";
        public static CQApi Api { get; set; }
        public static void Start()
        {
            Task.Factory.StartNew(() => Minecraft_QQ.Start());
        }
        public static void Stop()
        {
            Minecraft_QQ.Stop();
        }
        public static void Menu()
        {
            Minecraft_QQ.OpenSettingForm();
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
