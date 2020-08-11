using Minecraft_QQ_Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Minecraft_QQ_Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static System.Windows.Forms.NotifyIcon notifyIcon;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Click += NotifyIcon_Click;
            IMinecraft_QQ.Start();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            
        }
    }
}
