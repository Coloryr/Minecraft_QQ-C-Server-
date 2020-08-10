using Minecraft_QQ.Utils;

namespace Minecraft_QQ.Config
{
    internal class ConfigShow
    {
        public static void Show(dynamic obj)
        {
            Logs.LogOut("[Config]配置文件：" + obj.ToString());
        }
    }
}
