using Newtonsoft.Json;
using System.Text;

namespace Minecraft_QQ_Core.Robot;

record RobotTask
{
    public byte Index { get; set; }
    public string Data { get; set; }
}
class BuildPack
{
    public static byte[] Build(object obj, byte index)
    {
        byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj) + " ");
        data[^1] = index;
        return data;
    }
}
