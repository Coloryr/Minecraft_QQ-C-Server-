namespace Color_yr.Minecraft_QQ.Utils
{
    class Commder_list
    {
        public static readonly string SPEAK = "speak";
        public static readonly string ONLINE = "online";
        public static readonly string SERVER = "server";
    }
    /// <summary>
    /// 玩家数据储存格式
    /// </summary>
    public class PlayerObj
    {
        public string 名字;
        public string 昵称;
        public long QQ号 = 0;
        public bool 管理员 = false;
    }
    /// <summary>
    /// 服务器命令储存格式
    /// </summary>
    public class CommandObj
    {
        public string 命令;
        public bool 玩家使用 = false;
        public bool 玩家发送 = false;
        public bool 附带参数 = false;
    }
    /// <summary>
    /// 发送/接受数据格式
    /// </summary>
    public class MessageObj
    {
        public string group;
        public string player;
        public string message;
        public string commder;
        public bool is_commder = false;
    }
    /// <summary>
    /// 群储存格式
    /// </summary>
    public class GroupObj
    {
        public string 群号;
        public bool 启用命令 = false;
        public bool 开启对话 = false;
        public bool 主群 = false;
    }
}
