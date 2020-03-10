using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using System.Threading.Tasks;

namespace Color_yr.Minecraft_QQ.Event
{
    public class Event_MenuCall : IMenuCall
    {
        public void MenuCall(object sender, CQMenuCallEventArgs e)
        {
            Task.Factory.StartNew(() => Minecraft_QQ.OpenSettingForm());
        }
    }
}
