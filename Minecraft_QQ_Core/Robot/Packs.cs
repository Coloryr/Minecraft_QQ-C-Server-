using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.Robot
{
    class PackStart
    {
        public string Name { get; set; }
        public List<byte> Reg { get; set; }
    }
    class GroupMessageEventPack
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }

    class SendGroupMessagePack
    {
        public long id { get; set; }
        public string message { get; set; }
    }
    class SendFriendMessagePack
    {
        public long id { get; set; }
        public string message { get; set; }
    }
    class SendGroupPrivateMessagePack
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string message { get; set; }
    }
    class TempMessageEventPack
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }
    class FriendMessageEventPack
    {
        public long id { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }
    class SendGroupImagePack
    {
        public long id { get; set; }
        public string img { get; set; }
    }
    class SendGroupPrivateImagePack
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string img { get; set; }
    }
    class SendFriendImagePack
    {
        public long id { get; set; }
        public string img { get; set; }
    }
    class GroupMuteAll
    {
        public long id { get; set; }
    }
    class GroupUnmuteAll
    {
        public long id { get; set; }
    }
    class SetGroupMemberCard
    {
        public long id { get; set; }
        public long fid { get; set; }
        public string card { get; set; }
    }
    class SetGroupName
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}
