using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_AppDisable : IAppDisable
    {
        public void AppDisable(object sender, CQAppDisableEventArgs e)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.stop());
        }
    }
}
