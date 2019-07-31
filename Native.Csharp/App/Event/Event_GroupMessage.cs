using Color_yr.Minecraft_QQ;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using System.Threading.Tasks;

namespace Native.Csharp.App.Event
{
    public class Event_GroupMessage : IReceiveGroupMessage
    {
        public void ReceiveGroupMessage(object sender, CqGroupMessageEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Minecraft_QQ.GroupMessage(e.FromGroup, e.FromQQ, e.Message);
            });
        }
    }
}
