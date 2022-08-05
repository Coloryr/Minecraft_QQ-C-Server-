namespace Minecraft_QQ_Core.MySocket;

public record TranObj
{
    public string group { get; set; } = "";
    public string message { get; set; } = "";
    public string player { get; set; } = "";
    public string command { get; set; } = "";
    public bool isCommand { get; set; } = false;
}
public record ReadObj
{
    public string group { get; set; } = "";
    public string message { get; set; } = "";
    public string player { get; set; } = "";
    public string data { get; set; } = "";
}
