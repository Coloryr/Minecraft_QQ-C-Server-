using Color_yr.Minecraft_QQ;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Event
{
    class Event_Openform : ICallMenu
    {
        public void CallMenu(object sender, CqCallMenuEventArgs e)
        {
            Minecraft_QQ.OpenSettingForm();
        }
    }
}
