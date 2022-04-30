using System.Collections.Generic;

namespace Minecraft_QQ_Core.Robot;

abstract record PackBase
{
    public long qq { get; set; }
}
record PackStart
{
    public string Name { get; set; }
    public List<byte> Reg { get; set; }
}
record GroupMessageEventPack : PackBase
{
    public long id { get; set; }
    public long fid { get; set; }
    public string name { get; set; }
    public List<string> message { get; set; }
}
record SendGroupMessagePack : PackBase
{
    public long id { get; set; }
    public List<string> message { get; set; }
}
record SendGroupPrivateMessagePack : PackBase
{
    public long id { get; set; }
    public long fid { get; set; }
    public List<string> message { get; set; }
}
