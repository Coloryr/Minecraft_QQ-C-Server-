using Minecraft_QQ.Config;
using Minecraft_QQ.MySocket;
using Minecraft_QQ.Utils;
using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        class Server 
        {
            public string Name { get; set; }
            public string Addr { get; set; }
        }
        public Window1()
        {
            Closed += MetroWindow_Closed;
            InitializeComponent();
            InitQQList();
            InitServerList();
        }
        private void InitQQList()
        {
            var list = Minecraft_QQ.GroupConfig.群列表.Values;
            QQList.Items.Clear();
            foreach (var item in list)
            {
                QQList.Items.Add(item);
            }
        }

        public void InitServerList()
        {
            Dispatcher.Invoke(() =>
            {
                IP.Text = Minecraft_QQ.MainConfig.链接.地址;
                Port.Text = Minecraft_QQ.MainConfig.链接.端口.ToString();
                if (!MySocketServer.Start)
                {
                    State.Content = "未就绪";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = true;
                }
                else if (MySocketServer.Start && !MySocketServer.IsReady())
                {
                    State.Content = "等待连接";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                }
                else if (MySocketServer.IsReady())
                {
                    State.Content = "运行中";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                }
                ServerList.Items.Clear();
                foreach (var item in MySocketServer.MCServers)
                {
                    ServerList.Items.Add(new Server
                    {
                        Name = item.Key,
                        Addr = item.Value.RemoteEndPoint.ToString()
                    });
                }
            });
        }

        private void ChangeQQ(object sender, RoutedEventArgs e)
        {
            var item = ((GroupObj) QQList.SelectedItem).Clone();
            item = new QQSet(item).Set();
            bool ok = long.TryParse(item.群号, out long group);
            if (!ok)
            {
                MessageBox.Show("修改失败", "请检查你修改后的群号");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表[group] = item;
            InitQQList();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            var item = ((GroupObj)QQList.SelectedItem).Clone();
            long.TryParse(item.群号, out long group);
            Minecraft_QQ.GroupConfig.群列表.Remove(group);
            InitQQList();
        }

        private void AddQQ(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            bool ok = long.TryParse(item.群号, out long group);
            if (!ok)
            {
                MessageBox.Show("添加失败","请检查你写的群号");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表.Add(group, item);
            InitQQList();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Port.Text, out int port))
            {
                MessageBox.Show("保存失败", "端口设置不为数字");
                return;
            }
            else if (port < 0 || port > 65535)
            {
                MessageBox.Show("保存失败", "端口设置范围超出");
                return;
            }
            Minecraft_QQ.MainConfig.链接.地址 = IP.Text;
            Minecraft_QQ.MainConfig.链接.端口 = port;
            new ConfigWrite().Group();
            new ConfigWrite().Config();
            MessageBox.Show("已保存", "群设置已经保存");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Minecraft_QQ.Reload();
            MessageBox.Show("已重读", "配置文件已重载");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IP.Text = "127.0.0.1";
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            IP.Text = "0.0.0.0";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MySocketServer.ServerStop();
            MySocketServer.StartServer();
        }

        private void MetroWindow_Closed(object sender, System.EventArgs e)
        {
            Minecraft_QQ.CloseSetWindow();
        }

        private void SocketRE_Copy_Click(object sender, RoutedEventArgs e)
        {
            MySocketServer.ServerStop();
            IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
        }
    }
}
