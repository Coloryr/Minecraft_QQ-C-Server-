using Native.Csharp.App.EventArgs;
using Color_yr.Minecraft_QQ;
using Native.Csharp.App.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Event
{
    class Event_FriendMessage : IReceiveFriendMessage
    {
        public void ReceiveFriendMessage(object sender, CqPrivateMessageEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Minecraft_QQ.PrivateMessage(e.FromQQ, e.Message);
            });
        }
    }
}
