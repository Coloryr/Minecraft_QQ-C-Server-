using System.Collections.Generic;

namespace Minecraft_QQ_Core.Robot;

public record GroupMessagePack
{
    public record Message
    {
        public record Data
        {
            public string text { get; set; }
            public string file { get; set; }
            public string url { get; set; }
            public string qq { get; set; }
        }
        public string type { get; set; }
        public Data data { get; set; }
    }
    public record Sender
    {
        public long user_id { get; set; }
        public string nickname { get; set; }
        public string card { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string area { get; set; }
        public string level { get; set; }
        public string role { get; set; }
        public string title { get; set; }
    }
    public record Anonymous
    {
        public long id { get; set; }
        public string name { get; set; }
        public string flag { get; set; }
    }
    public long time { get; set; }
    public long self_id { get; set; }
    public string message_type { get; set; }
    public string sub_type { get; set; }
    public int message_id { get; set; }
    public long group_id { get; set; }
    public long user_id { get; set; }
    public Anonymous anonymous { get; set; }
    public List<Message> message { get; set; }
    public string raw_message { get; set; }
    public int font { get; set; }
    public Sender sender { get; set; }
}
