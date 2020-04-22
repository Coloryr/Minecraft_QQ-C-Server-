using System.Collections.Generic;

namespace Minecraft_QQ.Utils
{
    internal class CommderList
    {
        public static readonly string SPEAK = "speak";
        public static readonly string ONLINE = "online";
        public static readonly string SERVER = "server";
    }
    /// <summary>
    /// 玩家数据储存格式
    /// </summary>
    internal class PlayerObj
    {
        public string 名字;
        public string 昵称;
        public long QQ号 = 0;
        public bool 管理员 = false;
    }
    /// <summary>
    /// 服务器命令储存格式
    /// </summary>
    internal class CommandObj
    {
        public string 命令;
        public bool 玩家使用 = false;
        public bool 玩家发送 = false;
        public bool 附带参数 = false;
        public List<string> 服务器使用 = new List<string>();
    }
    /// <summary>
    /// 发送/接受数据格式
    /// </summary>
    internal class MessageObj
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
    internal class GroupObj
    {
        public string 群号 { get; set; }
        public bool 启用命令 { get; set; }
        public bool 开启对话 { get; set; }
        public bool 主群 { get; set; }
    }
}
