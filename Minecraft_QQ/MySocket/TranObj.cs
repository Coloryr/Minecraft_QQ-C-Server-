namespace Minecraft_QQ.MySocket
{
    class TranObj
    {
        public string group { get; set; }
        public string message { get; set; }
        public string player { get; set; }
        public bool isCommand { get; set; } = false;
        public string command { get; set; }
    }
    class ReadObj
    {
        public string group { get; set; }
        public string message { get; set; }
        public string player { get; set; }
        public string data { get; set; }
    }
}
