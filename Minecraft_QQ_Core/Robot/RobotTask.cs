using Newtonsoft.Json;
using System.Text;

namespace Minecraft_QQ_Core.Robot
{
    class RobotTask
    {
        public byte index { get; set; }
        public string data { get; set; }
    }
    class BuildPack
    {
        public static byte[] Build(object obj, byte index)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj) + " ");
            data[data.Length - 1] = index;
            return data;
        }
    }
}
