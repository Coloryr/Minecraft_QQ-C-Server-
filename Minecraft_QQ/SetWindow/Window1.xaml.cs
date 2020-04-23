using Minecraft_QQ.Config;
using Minecraft_QQ.MyMysql;
using Minecraft_QQ.MySocket;
using Minecraft_QQ.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public class Server
        {
            public string Name { get; set; }
            public string Addr { get; set; }
        }
        public class CommandOBJ 
        {
            public string Check { get; set; }
            public string Command { get; set; }
            public bool Use { get; set; }
            public bool Send { get; set; }
            public bool Pr { get; set; }
            public string Server { get; set; }

            public List<string> ServerS = new List<string>();
        }
        public Window1()
        {
            Closed += MetroWindow_Closed;
            KeyDown += Window_KeyDown;
            InitializeComponent();
            InitQQList();
            InitServerList();
            InitPlayerList();
            InitMessageList();
            InitCommandList();
            InitMysql();
            DataContext = Minecraft_QQ.MainConfig;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Click(null, null);
            }
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
        private void InitMessageList()
        {
            MessageList.Items.Clear();
            foreach (var item in Minecraft_QQ.AskConfig.自动应答列表)
            {
                MessageList.Items.Add(new Server
                {
                    Name = item.Key,
                    Addr = item.Value
                });
            }
        }
        private void InitPlayerList()
        {
            var list = Minecraft_QQ.PlayerConfig.玩家列表.Values;
            PlayerList.Items.Clear();
            foreach (var item in list)
            {
                PlayerList.Items.Add(item);
            }
            var list1 = Minecraft_QQ.PlayerConfig.禁止绑定列表;
            NoIDList.Items.Clear();
            foreach (var item in list1)
            {
                NoIDList.Items.Add(item);
            }
            list1 = Minecraft_QQ.PlayerConfig.禁言列表;
            MuteList.Items.Clear();
            foreach (var item in list1)
            {
                MuteList.Items.Add(item);
            }
        }
        private void InitCommandList()
        {
            var list = Minecraft_QQ.CommandConfig.命令列表;
            CommandList.Items.Clear();
            foreach (var item in list)
            {
                string data = "";
                foreach (var temp in item.Value.服务器使用)
                {
                    data += temp;
                    data += ',';
                }
                if (!string.IsNullOrWhiteSpace(data))
                    data = data.Substring(0, data.Length - 1);
                CommandList.Items.Add(new CommandOBJ
                {
                    Check = item.Key,
                    Command = item.Value.命令,
                    Use = item.Value.玩家使用,
                    Send = item.Value.玩家发送,
                    Pr = item.Value.附带参数,
                    Server = data,
                    ServerS = item.Value.服务器使用
                });
            }
        }
        private void InitMysql()
        {
            MysqlPassword.Password = Minecraft_QQ.MainConfig.数据库.密码;
            if (Minecraft_QQ.MysqlOK)
            {
                MysqlState.Content = "已连接";
                MysqlConnect.Content = "断开";
                MysqlIP.IsEnabled = MysqlPort.IsEnabled = MysqlUser.IsEnabled =
                    MysqlPassword.IsEnabled = MysqlDataBase.IsEnabled = false;
            }
            else
            {
                MysqlState.Content = "未连接";
                MysqlConnect.Content = "连接";
                MysqlIP.IsEnabled = MysqlPort.IsEnabled = MysqlUser.IsEnabled =
                    MysqlPassword.IsEnabled = MysqlDataBase.IsEnabled = true;
            }
        }

        private void ChangeQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItems.Count == 0)
                return;
            var item = ((GroupObj)QQList.SelectedItem).Clone();
            long.TryParse(item.群号, out long oldgroup);
            item = new QQSet(item).Set();
            long group;
            if (!string.IsNullOrWhiteSpace(item.群号))
            {
                return;
            }
            else if(!long.TryParse(item.群号, out group))
            { 
                MessageBox.Show("请检查你修改后的群号", "修改失败");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表.Remove(oldgroup);
            Minecraft_QQ.GroupConfig.群列表.Add(group, item);
            InitQQList();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItems.Count == 0)
                return;
            foreach(var item in QQList.SelectedItems)
            {
                var temp = (GroupObj)item;
                long.TryParse(temp.群号, out long group);
                Minecraft_QQ.GroupConfig.群列表.Remove(group);
            }
            InitQQList();
        }

        private void AddQQ(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            long group = 0;
            if (string.IsNullOrWhiteSpace(item.群号))
            {
                return;
            }
            if (!long.TryParse(item.群号, out group))
            {
                MessageBox.Show("群号错误", "添加失败");
                return;
            }
            if (Minecraft_QQ.GroupConfig.群列表.ContainsKey(group))
            {
                Minecraft_QQ.GroupConfig.群列表.Remove(group);
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
            new ConfigWrite().All();
            MessageBox.Show("配置已经保存", "已保存");
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
        private void SocketD(object sender, RoutedEventArgs e)
        {
            foreach (var item in ServerList.SelectedItems)
            {
                var temp = (Server)item;
                MySocketServer.Close(temp.Name);
            }
            InitServerList();
        }

        private void PlayerD(object sender, RoutedEventArgs e)
        {
            foreach (var item in PlayerList.SelectedItems)
            {
                var temp = (PlayerObj)item;
                Minecraft_QQ.PlayerConfig.玩家列表.Remove(temp.QQ号);
            }
            InitPlayerList();
        }
        private void PlayerC(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;
            var item = (PlayerObj)PlayerList.SelectedItem;
            long olditem = item.QQ号;
            item = new PlayerSet(item).Set();
            if (item.QQ号 == 0 || item.QQ号 < 0)
            {
                MessageBox.Show("请检查你写的QQ号", "修改失败");
                return;
            }
            Minecraft_QQ.PlayerConfig.玩家列表.Remove(olditem);
            Minecraft_QQ.PlayerConfig.玩家列表.Add(item.QQ号, item);
            InitPlayerList();
        }
        private void PlayerA(object sender, RoutedEventArgs e)
        {
            var item = new PlayerSet().Set();
            if (item.QQ号 == 0 || item.QQ号 < 0)
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.玩家列表.Add(item.QQ号, item);
            InitPlayerList();
        }

        private void NoIDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in NoIDList.SelectedItems)
            {
                Minecraft_QQ.PlayerConfig.禁止绑定列表.Remove((string)item);
            }
            InitPlayerList();
        }
        private void NoIDA(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }
            if (Minecraft_QQ.PlayerConfig.禁止绑定列表.Contains(item))
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.禁止绑定列表.Add(item);
            InitPlayerList();
        }

        private void MuteDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MuteList.SelectedItems)
            {
                Minecraft_QQ.PlayerConfig.禁言列表.Remove((string)item);
            }
            InitPlayerList();
        }
        private void MuteDA(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }
            if (Minecraft_QQ.PlayerConfig.禁言列表.Contains(item))
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.禁言列表.Add(item);
            InitPlayerList();
        }

        private void MessageD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MessageList.SelectedItems)
            {
                var temp = (Server)item;
                Minecraft_QQ.AskConfig.自动应答列表.Remove(temp.Name);
            }
            InitMessageList();
        }
        private void MessageC(object sender, RoutedEventArgs e)
        {
            if (MessageList.SelectedItem == null)
                return;
            var item = (Server)MessageList.SelectedItem;
            string olditem = item.Name;
            item = new MessageSet(item).Set();
            if (string.IsNullOrWhiteSpace(item.Name) ||
                string.IsNullOrWhiteSpace(item.Addr))
            {
                MessageBox.Show("请检查你写内容", "修改失败");
                return;
            }
            Minecraft_QQ.AskConfig.自动应答列表.Remove(olditem);
            Minecraft_QQ.AskConfig.自动应答列表.Add(item.Name, item.Addr);
            InitMessageList();
        }
        private void MessageA(object sender, RoutedEventArgs e)
        {
            var item = new MessageSet().Set();
            if (string.IsNullOrWhiteSpace(item.Name) ||
               string.IsNullOrWhiteSpace(item.Addr))
            {
                return;
            }
            Minecraft_QQ.AskConfig.自动应答列表.Add(item.Name, item.Addr);
            InitMessageList();
        }

        private void CommandD(object sender, RoutedEventArgs e)
        {
            foreach (var item in CommandList.SelectedItems)
            {
                var temp = (CommandOBJ)item;
                Minecraft_QQ.CommandConfig.命令列表.Remove(temp.Check);
            }
            InitCommandList();
        }
        private void CommandC(object sender, RoutedEventArgs e)
        {
            if (CommandList.SelectedItem == null)
                return;
            var item = (CommandOBJ)CommandList.SelectedItem;
            string olditem = item.Check;
            item = new CommandSet(item).Set();
            if (string.IsNullOrWhiteSpace(item.Check) ||
                string.IsNullOrWhiteSpace(item.Command))
            {
                MessageBox.Show("请检查你写内容", "修改失败");
                return;
            }
            Minecraft_QQ.CommandConfig.命令列表.Remove(olditem);
            Minecraft_QQ.CommandConfig.命令列表.Add(item.Check, new CommandObj
            {
                命令 = item.Command,
                玩家使用 = item.Use,
                玩家发送 = item.Send,
                附带参数 = item.Pr,
                服务器使用 = item.ServerS
            });
            InitCommandList();
        }
        private void CommandA(object sender, RoutedEventArgs e)
        {
            var item = new CommandSet().Set();
            if (string.IsNullOrWhiteSpace(item.Check) ||
                string.IsNullOrWhiteSpace(item.Command))
            {
                return;
            }
            Minecraft_QQ.CommandConfig.命令列表.Add(item.Check, new CommandObj
            {
                命令 = item.Command,
                玩家使用 = item.Use,
                玩家发送 = item.Send,
                附带参数 = item.Pr,
                服务器使用 = item.ServerS
            });
            InitCommandList();
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            if (ANSIC.IsChecked == true)
            {
                UTF8C.IsChecked = false;
                Minecraft_QQ.MainConfig.链接.编码 = Code.ANSI;
            }
            else if(UTF8C.IsChecked == true)
            {
                ANSIC.IsChecked = false;
                Minecraft_QQ.MainConfig.链接.编码 = Code.UTF8;
            }
        }

        private void MysqlPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Minecraft_QQ.MainConfig.数据库.密码 = MysqlPassword.Password;
        }

        private void MysqlConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Minecraft_QQ.MysqlOK)
                Mysql.MysqlStop();
            else
                Mysql.MysqlStart();
            InitMysql();
        }
    }
}
