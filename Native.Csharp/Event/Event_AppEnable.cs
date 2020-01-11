using Native.Csharp.App.Common;
using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_AppEnable : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            Minecraft_QQ.Plugin = AppData.CQApi;
            Task.Factory.StartNew(() => Minecraft_QQ.Start());
        }
    }
}
