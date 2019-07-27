using Color_yr.Minecraft_QQ;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;

namespace Native.Csharp.App.Event
{
    public class Event_GroupMessage : IReceiveGroupMessage
    {
        public void ReceiveGroupMessage(object sender, CqGroupMessageEventArgs e)
        {
            logs.Log_write("开始处理");
            Minecraft_QQ.GroupMessage(e.FromGroup, e.FromQQ, e.Message);
            logs.Log_write("处理完成");
        }
    }
}
