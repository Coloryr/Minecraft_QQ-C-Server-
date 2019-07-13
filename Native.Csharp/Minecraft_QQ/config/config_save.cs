using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ
{
    /// <summary>
    /// 玩家数据储存格式
    /// <param name="player">玩家名字</param>
    /// <param name="nick">昵称</param>
    /// <param name="qq">QQ号</param>
    /// <param name="mute">是否被禁言</param>
    /// <param name="admin">是否为管理员</param>
    /// </summary>
    class player_save
    {
        public string player;
        public string nick;
        public long qq;
        public bool mute;
        public bool admin;
    }
    /// <summary>
    /// 服务器命令储存格式
    /// <param name="check">触发条件</param>
    /// <param name="commder">命令</param>
    /// <param name="player_use">是否玩家可用</param>
    /// <param name="player_send">是否为玩家发送</param>
    /// <param name="parameter">是否附带参数</param>
    /// </summary>
    class commder_save
    {
        public string check;
        public string commder;
        public bool player_use;   
        public bool player_send;
        public bool parameter;
    }
    /// <summary>
    /// 发送/接受数据格式
    /// <param name="group">群</param>
    /// <param name="player">玩家</param>
    /// <param name="msg">消息</param>
    /// <param name="is_commder">是否为指令</param>
    /// </summary>
    public class message_send
    {
        public string group;
        public string player;
        public string message;
        public bool is_commder;
    }
    /// <summary>
    /// 群储存格式
    /// <param name="group_s">群String</param>
    /// <param name="commder">是否启用群命令</param>
    /// <param name="say">是否启用聊天</param>
    /// <param name="main">是否为主群</param>
    /// <param name="group_l">群long</param>
    /// </summary>
    public class group_save
    {
        public string group_s;
        public bool commder;
        public bool say;
        public bool main;
        public long group_l;
    }
    /// <summary>
    /// 自动回复消息储存格式
    /// <param name="check">检测</param>
    /// <param name="message">回复消息</param>
    /// </summary>
    public class message_save
    {
        public string check;
        public string message;
    }
}
