using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_PrivateMessage : IPrivateMessage
    {
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.PrivateMessage(e));
        }
    }
}
