using System.Collections.Generic;

namespace Minecraft_QQ_Core.Robot
{
    abstract class PackBase
    {
        public long qq { get; set; }
    }
    class PackStart
    {
        public string Name { get; set; }
        public List<byte> Reg { get; set; }
    }
    class GroupMessageEventPack : PackBase
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string name { get; set; }
        public List<string> message { get; set; }
    }

    class SendGroupMessagePack : PackBase
    {
        public long id { get; set; }
        public List<string> message { get; set; }
    }
    class SendGroupPrivateMessagePack : PackBase
    {
        public long id { get; set; }
        public long fid { get; set; }
        public List<string> message { get; set; }
    }
    class TempMessageEventPack : PackBase
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string name { get; set; }
        public List<string> message { get; set; }
        public int time { get; set; }
    }
    class FriendMessageEventPack : PackBase
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<string> message { get; set; }
        public int time { get; set; }
    }
}
