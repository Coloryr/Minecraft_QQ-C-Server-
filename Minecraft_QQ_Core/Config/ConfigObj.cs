using System.Collections.Generic;
using System.IO;

namespace Minecraft_QQ_Core.Config
{
    public class ConfigFile
    {
        /// <summary>
        /// 主要配置文件
        /// </summary>
        public static FileInfo 主要配置文件 { get; set; }
        /// <summary>
        /// 玩家管理配置文件
        /// </summary>
        public static FileInfo 玩家储存 { get; set; }
        /// <summary>
        /// 自动应答配置文件
        /// </summary>
        public static FileInfo 自动应答 { get; set; }
        /// <summary>
        /// 服务器命令配置文件
        /// </summary>
        public static FileInfo 自定义指令 { get; set; }
        /// <summary>
        /// 群配置文件
        /// </summary>
        public static FileInfo 群设置 { get; set; }
    }
    public record PlayerConfig
    {
        /// <summary>
        /// 禁止绑定数据储存
        /// </summary>
        public List<string> 禁止绑定列表 { get; set; } = new();
        /// <summary>
        /// 禁言数据存储
        /// </summary>
        public List<string> 禁言列表 { get; set; } = new();
        /// <summary>
        /// 玩家数据储存
        /// </summary>
        public Dictionary<long, PlayerObj> 玩家列表 { get; set; } = new();
    }
    public record CommandConfig
    {
        /// <summary>
        /// 服务器指令数据储存
        /// </summary>
        public Dictionary<string, CommandObj> 命令列表 { get; set; } = new();
    }
    public record GroupConfig
    {
        /// <summary>
        /// 设置的群数据储存
        /// </summary>
        public Dictionary<long, GroupObj> 群列表 { get; set; } = new();
    }
    public record AskConfig
    {
        /// <summary>
        /// 自动应答存储
        /// </summary>
        public Dictionary<string, string> 自动应答列表 { get; set; } = new();
    }
    public class MainConfig
    {
        /// <summary>
        /// 设置
        /// </summary>
        public SettingConfig 设置 { get; set; } = new();
        /// <summary>
        /// 消息
        /// </summary>
        public MessageConfig 消息 { get; set; } = new();
        /// <summary>
        /// 检测消息
        /// </summary>
        public CheckConfig 检测 { get; set; } = new();
        /// <summary>
        /// 管理员指令
        /// </summary>
        public AdminConfig 管理员 { get; set; } = new();
        /// <summary>
        /// Socket配置
        /// </summary>
        public SocketConfig 链接 { get; set; } = new();
        /// <summary>
        /// Mysql配置文件
        /// </summary>
        public MysqlConfig 数据库 { get; set; } = new();
        /// <summary>
        /// 机器人设定
        /// </summary>
        public SettingRobot 机器人设置 { get; set; } = new();

        public override string ToString()
        {
            return "设置:" + 设置.ToString() + "\n"
                + "消息:" + 消息.ToString() + "\n"
                + "检测:" + 检测.ToString() + "\n"
                + "管理员:" + 管理员.ToString() + "\n"
                + "链接:" + 链接.ToString() + "\n"
                + "数据库:" + 数据库.ToString() + "\n"
                + "机器人设置:" + 机器人设置.ToString();
        }
    }

    public class SettingRobot
    {
        public long QQ机器人账户 { get; set; } = 123456789;
        public string 地址 { get; set; } = "127.0.0.1";
        public int 端口 { get; set; } = 23333;
        public int 自动重连延迟 { get; set; } = 10000;
        public bool 检查是否断开 { get; set; } = true;
        public override string ToString()
        {
            return "地址:" + 地址.ToString() + "\n"
                + "端口:" + 端口.ToString() + "\n"
                + "自动重连延迟:" + 自动重连延迟.ToString() + "\n"
                + "检查是否断开:" + 检查是否断开.ToString() + "\n"
                + "QQ机器人账户:" + QQ机器人账户.ToString();
        }
    }

    public class SettingConfig
    {
        /// <summary>
        /// 自动应答-开关
        /// </summary>
        public bool 自动应答开关 { get; set; } = false;
        /// <summary>
        /// 彩色代码-开关
        /// </summary>
        public bool 颜色代码开关 { get; set; } = false;
        /// <summary>
        /// 维护模式-开关
        /// </summary>
        public bool 维护模式 { get; set; } = false;
        /// <summary>
        /// 同步对话-开关
        /// </summary>
        public bool 始终发送消息 { get; set; } = false;
        /// <summary>
        /// 服务器昵称-开关
        /// </summary>
        public bool 使用昵称发送至服务器 { get; set; } = true;
        /// <summary>
        /// 群昵称-开关
        /// </summary>
        public bool 使用昵称发送至群 { get; set; } = true;
        /// <summary>
        /// 允许玩家绑定ID
        /// </summary>
        public bool 可以绑定名字 { get; set; } = true;
        /// <summary>
        /// 发送日志消息到群
        /// </summary>
        public bool 发送日志到主群 { get; set; } = true;
        /// <summary>
        /// 不发送指令到服务器
        /// </summary>
        public bool 不发送指令到服务器 { get; set; } = false;
        /// <summary>
        /// 发送群消息间隔
        /// </summary>
        public int 发送群消息间隔 { get; set; } = 100;

        public override string ToString()
        {
            return "自动应答开关:" + 自动应答开关 + "\n"
                + "颜色代码开关:" + 颜色代码开关 + "\n"
                + "维护模式:" + 维护模式 + "\n"
                + "始终发送消息:" + 始终发送消息 + "\n"
                + "使用昵称发送至服务器:" + 使用昵称发送至服务器 + "\n"
                + "使用昵称发送至群:" + 使用昵称发送至群 + "\n"
                + "可以绑定名字:" + 可以绑定名字 + "\n"
                + "发送日志到群:" + 发送日志到主群 + "\n"
                + "发送群消息间隔:" + 发送群消息间隔;
        }
    }
    public class MessageConfig
    {
        /// <summary>
        /// 维护时发送的文本
        /// </summary>
        public string 维护提示文本 { get; set; } = "服务器正在维护，请等待维护结束！";
        /// <summary>
        /// 未知的指令
        /// </summary>
        public string 未知指令文本 { get; set; } = "未知指令";
        /// <summary>
        /// 禁止绑定ID
        /// </summary>
        public string 不能绑定文本 { get; set; } = "绑定ID已关闭";
        /// <summary>
        /// 没有绑定ID
        /// </summary>
        public string 没有绑定ID { get; set; } = $"你没有绑定服务器ID，发送：#绑定：[ID]来绑定，如：\n绑定：Color_yr";
        /// <summary>
        /// 重复绑定ID
        /// </summary>
        public string 重复绑定ID { get; set; } = "你已经绑定ID了，请找腐竹更改";

        public override string ToString()
        {
            return "维护提示文本:" + 维护提示文本 + "\n"
                + "未知指令文本:" + 未知指令文本 + "\n"
                + "不能绑定文本:" + 不能绑定文本 + "\n"
                + "没有绑定ID文本：" + 没有绑定ID + "\n"
                + "重复绑定ID文本：" + 重复绑定ID;
        }
    }
    public class CheckConfig
    {
        /// <summary>
        /// 检测头
        /// </summary>
        public string 检测头 { get; set; } = "#";
        /// <summary>
        /// 在线人数
        /// </summary>
        public string 在线玩家获取 { get; set; } = "在线人数";
        /// <summary>
        /// 状态检测
        /// </summary>
        public string 服务器在线检测 { get; set; } = "服务器状态";
        /// <summary>
        /// 玩家绑定ID
        /// </summary>
        public string 玩家设置名字 { get; set; } = "绑定：";
        /// <summary>
        /// 玩家发送消息
        /// </summary>
        public string 发送消息至服务器 { get; set; } = "服务器：";

        public override string ToString()
        {
            return "检测头:" + 检测头 + "\n"
               + "在线玩家获取:" + 在线玩家获取 + "\n"
               + "服务器在线检测:" + 服务器在线检测 + "\n"
               + "玩家设置名字:" + 玩家设置名字 + "\n"
               + "发送消息至服务器:" + 发送消息至服务器;
        }
    }
    public class AdminConfig
    {
        /// <summary>
        /// 禁言玩家
        /// </summary>
        public string 禁言 { get; set; } = "禁言：";
        /// <summary>
        /// 取消禁言
        /// </summary>
        public string 取消禁言 { get; set; } = "解禁：";
        /// <summary>
        /// 查询ID
        /// </summary>
        public string 查询绑定名字 { get; set; } = "查询：";
        /// <summary>
        /// 重命名玩家
        /// </summary>
        public string 重命名 { get; set; } = "修改：";
        /// <summary>
        /// 切换服务器维护模式
        /// </summary>
        public string 维护模式切换 { get; set; } = "服务器维护";
        /// <summary>
        /// 配置文件重读
        /// </summary>
        public string 重读配置 { get; set; } = "重读文件";
        /// <summary>
        /// 昵称
        /// </summary>
        public string 设置昵称 { get; set; } = "昵称：";
        /// <summary>
        /// 禁止绑定列表
        /// </summary>
        public string 获取禁止绑定列表 { get; set; } = "禁止绑定列表";
        /// <summary>
        /// 禁言列表
        /// </summary>
        public string 获取禁言列表 { get; set; } = "禁言列表";
        /// <summary>
        /// 发送绑定信息QQ号
        /// </summary>
        public long 发送绑定信息QQ号 { get; set; } = 0;

        public override string ToString()
        {
            return "禁言:" + 禁言 + "\n"
                + "取消禁言:" + 取消禁言 + "\n"
                + "查询绑定名字:" + 查询绑定名字 + "\n"
                + "重命名:" + 重命名 + "\n"
                + "维护模式切换:" + 维护模式切换 + "\n"
                + "重读配置:" + 重读配置 + "\n"
                + "设置昵称:" + 设置昵称 + "\n"
                + "获取禁止绑定列表:" + 获取禁止绑定列表 + "\n"
                + "获取禁言列表:" + 获取禁言列表 + "\n"
                + "发送绑定信息QQ号:" + 发送绑定信息QQ号;
        }
    }
    public class SocketConfig
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string 地址 { get; set; } = "127.0.0.1";
        /// <summary>
        /// 端口
        /// </summary>
        public int 端口 { get; set; } = 25555;
        /// <summary>
        /// 数据包头
        /// </summary>
        public string 数据头 { get; set; } = "[Head]";
        /// <summary>
        /// 数据包尾
        /// </summary>
        public string 数据尾 { get; set; } = "[End]";

        public bool 检测断开 { get; set; } = false;

        public override string ToString()
        {
            return "地址:" + 地址 + "\n"
                + "端口:" + 端口 + "\n"
                + "数据头:" + 数据头 + "\n"
                + "数据尾:" + 数据尾 + "\n"
                + "检测断开:" + 检测断开;
        }
    }

    public class MysqlConfig
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string 地址 { get; set; } = "127.0.0.1";
        /// <summary>
        /// 端口
        /// </summary>
        public int 端口 { get; set; } = 3306;
        /// <summary>
        /// 账户
        /// </summary>
        public string 用户名 { get; set; } = "root";
        /// <summary>
        /// 密码
        /// </summary>
        public string 密码 { get; set; } = "root";
        /// <summary>
        /// Mysql启用
        /// </summary>
        public bool 是否启用 { get; set; } = false;
        /// <summary>
        /// 数据库名
        /// </summary>
        public string 数据库 { get; set; } = "minecraft_qq";

        public override string ToString()
        {
            return "地址:" + 地址 + "\n"
                + "端口:" + 端口 + "\n"
                + "用户名:" + 用户名 + "\n"
                + "密码:" + 密码 + "\n"
                + "是否启用:" + 是否启用 + "\n"
                + "数据库:" + 数据库;
        }
    }
    /// <summary>
    /// 玩家数据储存格式
    /// </summary>
    public record PlayerObj
    {
        public string 名字 { get; set; }
        public string 昵称 { get; set; }
        public long QQ号 { get; set; }
        public bool 管理员 { get; set; }
    }
    /// <summary>
    /// 服务器命令储存格式
    /// </summary>
    public record CommandObj
    {
        public string 命令 { get; set; }
        public bool 玩家使用 { get; set; }
        public bool 玩家发送 { get; set; }
        public List<string> 服务器使用 { get; set; } = new();
    }
    /// <summary>
    /// 群储存格式
    /// </summary>
    public record GroupObj
    {
        public string 群号 { get; set; }
        public bool 启用命令 { get; set; }
        public bool 开启对话 { get; set; }
        public bool 主群 { get; set; }

        public GroupObj Copy()
        {
            return new()
            {
                群号 = 群号,
                启用命令 = 启用命令,
                开启对话 = 开启对话,
                主群 = 主群
            };
        }
    }
}
