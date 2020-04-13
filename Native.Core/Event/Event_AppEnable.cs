using Native.Csharp.App.Common;
using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_AppEnable : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            IMinecraft_QQ.api = AppData.CQApi;
            IMinecraft_QQ.Start();
        }
    }
}
