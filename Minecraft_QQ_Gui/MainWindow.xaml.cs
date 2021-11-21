using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Gui.SetWindow;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Minecraft_QQ_Gui
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isGet;
        private bool isSearch;
        private Server GetServer;

        public void AddLog(string logs)
        {
            Dispatcher.Invoke(() =>
            {
                log.Text += logs + "\n";
                if (log.Text.Length > 100000)
                    log.Text = "";
            });
        }
        public void ServerConfig(string server, string config)
        {
            if (isGet)
                return;
            if (GetServer.Name != server)
                return;
            isGet = true;
            var temp = JsonConvert.DeserializeObject<ConfigOBJ>(config);
            Dispatcher.Invoke(() => temp = new ServerSet(temp).Set());
            var temp1 = new TranObj
            {
                command = DataType.set,
                message = JsonConvert.SerializeObject(temp)
            };
            IMinecraft_QQ.Main.Server.Send(temp1, new List<string>
            {
                server
            });
            isGet = false;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Click(null, null);
                e.Handled = true;
            }
        }
        private void InitQQList()
        {
            var list = IMinecraft_QQ.Main.GroupConfig.群列表.Values;
            QQList.Items.Clear();
            foreach (var item in list)
            {
                QQList.Items.Add(item);
            }
        }

        public void InitServerList()
        {
            ServerList.Items.Clear();
            if (!IMinecraft_QQ.Main.Server.Start)
            {
                State.Content = "未就绪";
                SocketST.Content = "启动端口";
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = true;
                return;
            }
            else if (IMinecraft_QQ.Main.Server.Start && !IMinecraft_QQ.Main.Server.IsReady())
            {
                State.Content = "等待连接";
                SocketST.Content = "关闭端口";
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                return;
            }
            else if (IMinecraft_QQ.Main.Server.IsReady())
            {
                State.Content = "运行中";
                SocketST.Content = "关闭端口";
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
            }
            foreach (var item in IMinecraft_QQ.Main.Server.MCServers)
            {
                if (item.Value.Client.Connected)
                    ServerList.Items.Add(new Server
                    {
                        Name = item.Key,
                        Addr = item.Value.Client.Client.RemoteEndPoint.ToString()
                    });
            }
        }
        private void InitMessageList()
        {
            MessageList.Items.Clear();
            foreach (var item in IMinecraft_QQ.Main.AskConfig.自动应答列表)
            {
                MessageList.Items.Add(new Server
                {
                    Name = item.Key,
                    Addr = item.Value
                });
            }
        }
        public void InitPlayerList()
        {
            var list = IMinecraft_QQ.Main.PlayerConfig.玩家列表.Values;
            if (IMinecraft_QQ.Main.PlayerConfig.玩家列表 == null)
            {
                MessageBox.Show("数据错误，请检查Mysql数据库是否连接，检查后重启");
                return;
            }
            PlayerList.Items.Clear();
            foreach (var item in list)
            {
                PlayerList.Items.Add(item);
            }
            var list1 = IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表;
            NoIDList.Items.Clear();
            foreach (var item in list1)
            {
                NoIDList.Items.Add(item);
            }
            list1 = IMinecraft_QQ.Main.PlayerConfig.禁言列表;
            MuteList.Items.Clear();
            foreach (var item in list1)
            {
                MuteList.Items.Add(item);
            }
        }
        private void InitCommandList()
        {
            var list = IMinecraft_QQ.Main.CommandConfig.命令列表;
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
                    data = data[0..^1];
                CommandList.Items.Add(new CommandOBJ
                {
                    Check = item.Key,
                    Command = item.Value.命令,
                    Use = item.Value.玩家使用,
                    Send = item.Value.玩家发送,
                    Server = data,
                    ServerS = item.Value.服务器使用
                });
            }
        }
        private void InitMysql()
        {
            MysqlPassword.Password = IMinecraft_QQ.Main.MainConfig.数据库.密码;
            if (IMinecraft_QQ.Main.MysqlOK)
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
            var olditem = (GroupObj)QQList.SelectedItem;
            var item = olditem.Copy();
            item = new QQSet(item).Set();
            long group;
            if (string.IsNullOrWhiteSpace(item.群号))
            {
                return;
            }
            else if (!long.TryParse(item.群号, out group))
            {
                MessageBox.Show("请检查你修改后的群号", "修改失败");
                return;
            }
            IMinecraft_QQ.Main.GroupConfig.群列表.Remove(long.Parse(olditem.群号));
            IMinecraft_QQ.Main.GroupConfig.群列表.Add(group, item);
            InitQQList();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItems.Count == 0)
                return;
            foreach (var item in QQList.SelectedItems)
            {
                var temp = (GroupObj)item;
                long.TryParse(temp.群号, out long group);
                IMinecraft_QQ.Main.GroupConfig.群列表.Remove(group);
            }
            InitQQList();
        }

        private void AddQQ(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            if (string.IsNullOrWhiteSpace(item.群号))
            {
                return;
            }
            if (!long.TryParse(item.群号, out long group))
            {
                MessageBox.Show("群号错误", "添加失败");
                return;
            }
            if (IMinecraft_QQ.Main.GroupConfig.群列表.ContainsKey(group))
            {
                IMinecraft_QQ.Main.GroupConfig.群列表.Remove(group);
            }
            IMinecraft_QQ.Main.GroupConfig.群列表.Add(group, item);
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
            ConfigWrite.All();
            MessageBox.Show("配置已经保存", "已保存");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IMinecraft_QQ.Main.Reload();
            MessageBox.Show("配置文件已重载", "已重读");
            DataContext = IMinecraft_QQ.Main.MainConfig;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IP.Text = "127.0.0.1";
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            IP.Text = "0.0.0.0";
        }

        private void SocketST_Click(object sender, RoutedEventArgs e)
        {
            if (IMinecraft_QQ.Main.Server.Start)
            {
                IMinecraft_QQ.Main.Server.ServerStop();
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = true;
                SocketST.Content = "启动端口";
            }
            else
            {
                IMinecraft_QQ.Main.Server.StartServer();
                IP.IsEnabled = Local.IsEnabled = Out.IsEnabled = Port.IsEnabled = false;
                SocketST.Content = "关闭端口";
            }
            InitServerList();
        }
        private void SocketE(object sender, RoutedEventArgs e)
        {
            if (ServerList.SelectedItem == null)
                return;
            GetServer = (Server)ServerList.SelectedItem;
            IMinecraft_QQ.Main.Server.Send(new TranObj
            {
                command = DataType.config
            },
            new List<string>
            {
                GetServer.Name
            });
        }
        private void SocketD(object sender, RoutedEventArgs e)
        {
            foreach (var item in ServerList.SelectedItems)
            {
                var temp = (Server)item;
                IMinecraft_QQ.Main.Server.Close(temp.Name);
            }
            InitServerList();
        }

        private void PlayerD(object sender, RoutedEventArgs e)
        {
            foreach (var item in PlayerList.SelectedItems)
            {
                var temp = (PlayerObj)item;
                IMinecraft_QQ.Main.PlayerConfig.玩家列表.Remove(temp.QQ号);
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
            IMinecraft_QQ.Main.PlayerConfig.玩家列表.Remove(olditem);
            IMinecraft_QQ.Main.PlayerConfig.玩家列表.Add(item.QQ号, item);
            InitPlayerList();
        }
        private void PlayerA(object sender, RoutedEventArgs e)
        {
            var item = new PlayerSet().Set();
            if (item.QQ号 == 0 || item.QQ号 < 0)
            {
                return;
            }
            IMinecraft_QQ.Main.PlayerConfig.玩家列表.Add(item.QQ号, item);
            InitPlayerList();
        }

        private void NoIDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in NoIDList.SelectedItems)
            {
                IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表.Remove((string)item);
            }
            InitPlayerList();
        }
        private void NoIDE(object sender, RoutedEventArgs e)
        {
            if (NoIDList.SelectedItem != null)
            {
                var item = (string)NoIDList.SelectedItem;
                IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表.Remove(item);
                item = new IDSet(item).Set();
                IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表.Add(item);
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
            if (IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表.Contains(item))
            {
                return;
            }
            IMinecraft_QQ.Main.PlayerConfig.禁止绑定列表.Add(item);
            InitPlayerList();
        }

        private void MuteDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MuteList.SelectedItems)
            {
                IMinecraft_QQ.Main.PlayerConfig.禁言列表.Remove((string)item);
            }
            InitPlayerList();
        }
        private void MuteDE(object sender, RoutedEventArgs e)
        {
            if (MuteList.SelectedItem != null)
            {
                var item = (string)MuteList.SelectedItem;
                IMinecraft_QQ.Main.PlayerConfig.禁言列表.Remove(item);
                item = new IDSet(item).Set();
                IMinecraft_QQ.Main.PlayerConfig.禁言列表.Add(item);
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
            if (IMinecraft_QQ.Main.PlayerConfig.禁言列表.Contains(item))
            {
                return;
            }
            IMinecraft_QQ.Main.PlayerConfig.禁言列表.Add(item);
            InitPlayerList();
        }

        private void MessageD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MessageList.SelectedItems)
            {
                var temp = (Server)item;
                IMinecraft_QQ.Main.AskConfig.自动应答列表.Remove(temp.Name);
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
            IMinecraft_QQ.Main.AskConfig.自动应答列表.Remove(olditem);
            IMinecraft_QQ.Main.AskConfig.自动应答列表.Add(item.Name, item.Addr);
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
            IMinecraft_QQ.Main.AskConfig.自动应答列表.Add(item.Name, item.Addr);
            InitMessageList();
        }

        private void CommandD(object sender, RoutedEventArgs e)
        {
            foreach (var item in CommandList.SelectedItems)
            {
                var temp = (CommandOBJ)item;
                IMinecraft_QQ.Main.CommandConfig.命令列表.Remove(temp.Check);
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
            IMinecraft_QQ.Main.CommandConfig.命令列表.Remove(olditem);
            IMinecraft_QQ.Main.CommandConfig.命令列表.Add(item.Check, new CommandObj
            {
                命令 = item.Command,
                玩家使用 = item.Use,
                玩家发送 = item.Send,
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
            IMinecraft_QQ.Main.CommandConfig.命令列表.Add(item.Check, new CommandObj
            {
                命令 = item.Command,
                玩家使用 = item.Use,
                玩家发送 = item.Send,
                服务器使用 = item.ServerS
            });
            InitCommandList();
        }

        private void MysqlPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            IMinecraft_QQ.Main.MainConfig.数据库.密码 = MysqlPassword.Password;
        }

        private void MysqlConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IMinecraft_QQ.Main.MysqlOK)
                IMinecraft_QQ.Main.Mysql.MysqlStop();
            else
                IMinecraft_QQ.Main.Mysql.MysqlStart();
            InitMysql();
        }

        private void Turnto(PlayerObj obj)
        {
            Dispatcher.Invoke(() =>
            {
                PlayerList.SelectedItem = obj;
                PlayerList.ScrollIntoView(obj);
            });
        }

        private async void SearchQQ_(object sender, RoutedEventArgs e)
        {
            if (isSearch)
                return;
            var set = new PlayerSet(null).Set();
            bool ok = !string.IsNullOrWhiteSpace(set.名字)
                || !string.IsNullOrWhiteSpace(set.昵称)
                || set.QQ号 != 0;
            if (!ok)
            {
                MessageBox.Show("请输入要搜索的内容", "选择内容空");
                return;
            }
            bool haveName = !string.IsNullOrWhiteSpace(set.名字);
            bool haveNick = !string.IsNullOrWhiteSpace(set.昵称);
            isSearch = true;
            await Task.Run(() =>
            {
                foreach (var item in IMinecraft_QQ.Main.PlayerConfig.玩家列表)
                {
                    if (item.Key == set.QQ号)
                    {
                        Turnto(item.Value);
                        return;
                    }
                    else if (haveName && !string.IsNullOrWhiteSpace(item.Value.名字))
                    {
                        if (item.Value.名字.Contains(set.名字) || set.名字.Contains(item.Value.名字))
                        {
                            Turnto(item.Value);
                            return;
                        }
                    }
                    else if (haveNick && !string.IsNullOrWhiteSpace(item.Value.昵称))
                    {
                        if (item.Value.昵称.Contains(set.昵称) || set.昵称.Contains(item.Value.昵称))
                        {
                            Turnto(item.Value);
                            return;
                        }
                    }
                }
                MessageBox.Show("你搜索的内容找不到", "找不到内容");
                isSearch = false;
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = MessageBox.Show("你确定要关闭Minecraft_QQ吗?", "关闭窗口", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                App.Stop();
            }
            else
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitQQList();
            InitServerList();
            InitPlayerList();
            InitMessageList();
            InitCommandList();
            InitMysql();
            DataContext = IMinecraft_QQ.Main.MainConfig;
            App.MainWindow_ = this;
            App.CloseWin();
        }
    }
}
