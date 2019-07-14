using System.Collections.Generic;

namespace Color_yr.Minecraft_QQ
{
    class config_file
    {
        /// <summary>
        /// 主要配置文件
        /// </summary>
        public static string config = "config.xml";
        /// <summary>
        /// 玩家管理配置文件
        /// </summary>
        public static string player = "player.xml";
        /// <summary>
        /// 自动应答配置文件
        /// </summary>
        public static string message = "message.xml";
        /// <summary>
        /// 服务器命令配置文件
        /// </summary>
        public static string commder = "commder.xml";
        /// <summary>
        /// 群配置文件
        /// </summary>
        public static string group = "group.xml";
        /// <summary>
        /// 玩家数据储存
        /// </summary>
        public static Dictionary<long, player_save> player_list = new Dictionary<long, player_save> { };
        /// <summary>
        /// 服务器指令数据储存
        /// </summary>
        public static Dictionary<string, commder_save> commder_list = new Dictionary<string, commder_save> { };
        /// <summary>
        /// 设置的群数据储存
        /// </summary>
        public static Dictionary<long, group_save> group_list = new Dictionary<long, group_save> { };
        /// <summary>
        /// 设置的群数据储存
        /// </summary>
        public static Dictionary<string, message_save> message_list = new Dictionary<string, message_save> { };
        /// <summary>
        /// 禁止绑定数据储存
        /// </summary>
        public static List<string> cant_bind = new List<string> { };
    }
    class main_config
    {
        /// <summary>
        /// 自动应答-开关
        /// </summary>
        public static bool message_enable = false;
        /// <summary>
        /// 彩色代码-开关
        /// </summary>
        public static bool color_code = false;
        /// <summary>
        /// 维护模式-开关
        /// </summary>
        public static bool fix_mode = false;
        /// <summary>
        /// 同步对话-开关
        /// </summary>
        public static bool allways_send = false;
        /// <summary>
        /// 服务器昵称-开关
        /// </summary>
        public static bool nick_server = true;
        /// <summary>
        /// 群昵称-开关
        /// </summary>
        public static bool nick_group = true;
        /// <summary>
        /// 允许玩家绑定ID
        /// </summary>
        public static bool set_name = true;
    }
    class message_config
    {
        /// <summary>
        /// 发送至服务器的文本
        /// </summary>
        public static string send_text = "%player%:%message%";
        /// <summary>
        /// 维护时发送的文本
        /// </summary>
        public static string fix_send = "服务器正在维护，请等待维护结束！";
        /// <summary>
        /// 未知的指令
        /// </summary>
        public static string unknow = "未知指令";
    }
    class check_config
    {
        /// <summary>
        /// 检测头
        /// </summary>
        public static string head = "#";
        /// <summary>
        /// 在线人数
        /// </summary>
        public static string online_players = "在线人数";
        /// <summary>
        /// 状态检测
        /// </summary>
        public static string online_servers = "服务器状态";
        /// <summary>
        /// 玩家绑定ID
        /// </summary>
        public static string player_setid = "绑定：";
        /// <summary>
        /// 玩家发送消息
        /// </summary>
        public static string send_message = "服务器：";
    }
    class admin_config
    {
        /// <summary>
        /// 禁言玩家
        /// </summary>
        public static string mute = "禁言：";
        /// <summary>
        /// 取消禁言
        /// </summary>
        public static string unmute = "解禁：";
        /// <summary>
        /// 查询ID
        /// </summary>
        public static string check = "查询：";
        /// <summary>
        /// 重命名玩家
        /// </summary>
        public static string rename = "修改：";
        /// <summary>
        /// 切换服务器维护模式
        /// </summary>
        public static string fix = "服务器维护";
        /// <summary>
        /// 配置文件重读
        /// </summary>
        public static string reload = "重读文件";
        /// <summary>
        /// 打开菜单
        /// </summary>
        public static string menu = "打开菜单";
        /// <summary>
        /// 昵称
        /// </summary>
        public static string nick = "昵称：";
        /// <summary>
        /// 发送绑定信息群号
        /// </summary>
        public static long Admin_Send = 0;
    }
    class socket_config
    {
        /// <summary>
        /// 地址
        /// </summary>
        public static string setip = "127.0.0.1";
        /// <summary>
        /// 端口
        /// </summary>
        public static int Port = 25555;
        /// <summary>
        /// 编码类型
        /// </summary>
        public static string code = "ANSI";
        /// <summary>
        /// 数据包头
        /// </summary>
        public static string data_Head = "[Head]";
        /// <summary>
        /// 数据包尾
        /// </summary>
        public static string data_End = "[End]";
        /// <summary>
        /// 绑定IP-开关
        /// </summary>
        public static bool useip = false;
    }
}
