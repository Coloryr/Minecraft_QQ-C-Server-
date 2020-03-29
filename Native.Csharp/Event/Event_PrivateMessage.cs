using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_PrivateMessage : IPrivateMessage
    {
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            IMinecraft_QQ.RPrivateMessage(e.FromQQ.Id, e.Message.Text);
        }
    }
}
