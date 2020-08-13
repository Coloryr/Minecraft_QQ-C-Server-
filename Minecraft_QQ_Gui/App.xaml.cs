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
        public static MainWindow MainWindow_;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Click += NotifyIcon_Click;
            IMinecraft_QQ.Start();
            IMinecraft_QQ.ShowMessageCall = new IMinecraft_QQ.ShowMessage((string data) =>
            {
                MessageBox.Show(data);
            });
            IMinecraft_QQ.ServerConfigCall = new IMinecraft_QQ.ServerConfig((string key, string value) =>
            {
                MainWindow_?.ServerConfig(key, value);
            });
            IMinecraft_QQ.GuiCall = new IMinecraft_QQ.Gui((GuiFun fun) =>
            {
                Dispatcher.Invoke(() =>
                {
                    switch (fun)
                    {
                        case GuiFun.ServerList:
                            MainWindow_?.InitServerList();
                            break;
                        case GuiFun.PlayerList:
                            MainWindow_?.InitPlayerList();
                            break;
                    }
                });
            });
            IMinecraft_QQ.LogCall = new IMinecraft_QQ.Log((string data) =>
            {
                MainWindow_?.AddLog(data);
            });

        }

        public static void Stop()
        {
            MainWindow_ = null;
            notifyIcon.Dispose();
            IMinecraft_QQ.Stop();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            
        }
    }
}
