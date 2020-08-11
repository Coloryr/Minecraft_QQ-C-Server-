using Minecraft_QQ_Core.Utils;

namespace Minecraft_QQ_Core.Config
{
    internal class ConfigShow
    {
        public static void Show(dynamic obj)
        {
            Logs.LogOut("[Config]配置文件：" + obj.ToString());
        }
    }
}
