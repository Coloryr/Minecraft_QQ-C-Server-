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
    public class PlayerObj
    {
        public string 名字 { get; set; }
        public string 昵称 { get; set; }
        public long QQ号 { get; set; }
        public bool 管理员 { get; set; }
    }
    /// <summary>
    /// 服务器命令储存格式
    /// </summary>
    internal class CommandObj
    {
        public string 命令 { get; set; }
        public bool 玩家使用 { get; set; }
        public bool 玩家发送 { get; set; }
        public bool 附带参数 { get; set; }
        public List<string> 服务器使用 { get; set; } = new List<string>();
    }
    /// <summary>
    /// 发送/接受数据格式
    /// </summary>
    internal class MessageObj
    {
        public string group { get; set; }
        public string player { get; set; }
        public string message { get; set; }
        public string commder { get; set; }
        public bool is_commder { get; set; }
    }
    /// <summary>
    /// 群储存格式
    /// </summary>
    public class GroupObj
    {
        public string 群号 { get; set; }
        public bool 启用命令 { get; set; }
        public bool 开启对话 { get; set; }
        public bool 主群 { get; set; }

        public GroupObj Clone()
        {
            return new GroupObj
            {
                群号 = 群号,
                启用命令 = 启用命令,
                开启对话 = 开启对话,
                主群 = 主群
            };
        }
    }
}
