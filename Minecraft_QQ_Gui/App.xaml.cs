using Minecraft_QQ_Core;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Minecraft_QQ_Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string Version = IMinecraft_QQ.Version;
        public static System.Windows.Forms.NotifyIcon notifyIcon;
        public static MainWindow MainWindow_;

        private static InitWindow InitWindow_;

        public static void SetIcon(Icon icon)
        {
            notifyIcon.Icon = icon;
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            notifyIcon = new();
            notifyIcon.Visible = true;
            notifyIcon.Click += NotifyIcon_Click;

            InitWindow_ = new InitWindow();
            InitWindow_.Show();

            IMinecraft_QQ.ShowMessageCall = new IMinecraft_QQ.ShowMessage((string data) =>
            {
                MessageBox.Show(data);
            });
            IMinecraft_QQ.ServerConfigCall = new IMinecraft_QQ.ServerConfig((string key, string value) =>
            {
                MainWindow_?.ServerConfig(key, value);
            });
            IMinecraft_QQ.LogCall = new IMinecraft_QQ.Log((string data) =>
            {
                MainWindow_?.AddLog(data);
            });
            await IMinecraft_QQ.Start();

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
        }

        public static void CloseWin()
        {
            InitWindow_?.Close();
        }

        public static void Stop()
        {
            MainWindow_ = null;
            notifyIcon.Dispose();
            IMinecraft_QQ.Stop();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            MainWindow_?.Activate();
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                new MessageWindow("捕获未处理异常:" + e.Exception.ToString());
            }
            catch (Exception ex)
            {
                new MessageWindow("发生错误" + ex.ToString());
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("发生错误，将关闭\n");
            }
            sbEx.Append("捕获未处理异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).ToString());
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            new MessageWindow(sbEx.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                new MessageWindow("捕获线程内未处理异常：" + Environment.NewLine + e.Exception.InnerException.ToString() 
                    + Environment.NewLine + e.Exception.InnerException.StackTrace.ToString());
                e.SetObserved();
            });
        }
    }
}
