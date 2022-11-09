namespace Minecraft_QQ_Core;

public enum GuiFun
{
    ServerList, PlayerList
}
public class IMinecraft_QQ
{
    public const string Version = "4.2.0";

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
}
