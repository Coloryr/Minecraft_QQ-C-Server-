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
                ServerList.Items.Clear();
                if (!MySocketServer.Start)
                {
                    State.Content = "未就绪";
                    SocketST.Content = "启动端口";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = true;
                    return;
                }
                else if (MySocketServer.Start && !MySocketServer.IsReady())
                {
                    State.Content = "等待连接";
                    SocketST.Content = "关闭端口";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                    return;
                }
                else if (MySocketServer.IsReady())
                {
                    State.Content = "运行中";
                    SocketST.Content = "关闭端口";
                    IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                }
                foreach (var item in MySocketServer.MCServers)
                {
                    if (item.Value.Connected)
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
            if (QQList.SelectedItem == null)
                return;
            var item = ((GroupObj)QQList.SelectedItem).Clone();
            item = new QQSet(item).Set();
            bool ok = long.TryParse(item.群号, out long group);
            if (!ok)
            {
                MessageBox.Show("请检查你修改后的群号", "修改失败");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表[group] = item;
            InitQQList();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItem == null)
                return;
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
                MessageBox.Show("请检查你写的群号", "添加失败");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表.Add(group, item);
            InitQQList();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Port.Text, out int port))
            {
                MessageBox.Show("端口设置不为数字", "保存失败");
                return;
            }
            else if (port < 0 || port > 65535)
            {
                MessageBox.Show("端口设置范围超出", "保存失败");
                return;
            }
            Minecraft_QQ.MainConfig.链接.地址 = IP.Text;
            Minecraft_QQ.MainConfig.链接.端口 = port;
            new ConfigWrite().Group();
            new ConfigWrite().Config();
            MessageBox.Show("群设置已经保存", "已保存");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Minecraft_QQ.Reload();
            MessageBox.Show("配置文件已重载", "已重读");
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
            InitServerList();
        }

        private void MetroWindow_Closed(object sender, System.EventArgs e)
        {
            Minecraft_QQ.CloseSetWindow();
        }

        private void SocketST_Click(object sender, RoutedEventArgs e)
        {
            if (MySocketServer.Start)
            {
                MySocketServer.ServerStop();
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = true;
                SocketST.Content = "启动端口";
            }
            else
            {
                MySocketServer.StartServer();
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                SocketST.Content = "关闭端口";
            }
            InitServerList();
        }
        private void ScoketD(object sender, RoutedEventArgs e)
        {
            if (ServerList.SelectedItem == null)
                return;
            var item = (Server)ServerList.SelectedItem;
            MySocketServer.Close(item.Name);
            InitServerList();
        }
    }
}
