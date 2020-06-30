using Minecraft_QQ.Config;
using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// ServerSet.xaml 的交互逻辑
    /// </summary>
    public partial class ServerSet : Window
    {
        public ConfigOBJ Config { get; set; }
        private bool isLoad;
        public ServerSet(ConfigOBJ Config = null)
        {
            isLoad = true;
            InitializeComponent();
            DataContext = Config;
            if (Config == null)
            {
                this.Config = new ConfigOBJ();
            }
            else
                this.Config = Config;
            isLoad = false;
            switch (Config.ServerSet.Mode)
            {
                case 0:
                    Mode1.IsChecked = true;
                    break;
                case 1:
                    Mode2.IsChecked = true;
                    break;
                case 2:
                    Mode3.IsChecked = true;
                    break;
            }
            Re();
        }
        public void Re()
        {
            ServerNameList.Items.Clear();
            foreach (var item in Config.Servers)
            {
                ServerNameList.Items.Add(new Server
                {
                    Name = item.Key,
                    Addr = item.Value
                });
            }
            MuteList.Items.Clear();
            foreach (var item in Config.Mute)
            {
                MuteList.Items.Add(item);
            }
        }
        public ConfigOBJ Set()
        {
            ShowDialog();
            return Config;
        }

        private void Click_(object sender, RoutedEventArgs e)
        {
            if (isLoad)
                return;
            if (Mode1.IsChecked == true)
            {
                Mode2.IsChecked = false;
                Mode3.IsChecked = false;
                Config.ServerSet.Mode = 0;
            }
            else if (Mode2.IsChecked == true)
            {
                Mode1.IsChecked = false;
                Mode3.IsChecked = false;
                Config.ServerSet.Mode = 1;
            }
            else if (Mode3.IsChecked == true)
            {
                Mode2.IsChecked = false;
                Mode1.IsChecked = false;
                Config.ServerSet.Mode = 2;
            }
        }

        private void AddS(object sender, RoutedEventArgs e)
        {
            var item = new ServerName().Set();
            if (string.IsNullOrWhiteSpace(item.Name) || string.IsNullOrWhiteSpace(item.Addr))
                return;
            Config.Servers.Add(item.Name, item.Addr);
            Re();
        }
        private void EditS(object sender, RoutedEventArgs e)
        {
            if (ServerNameList.SelectedItem == null)
                return;
            var item = (Server)ServerNameList.SelectedItem;
            Config.Servers.Remove(item.Name);
            item = new ServerName(item).Set();
            Config.Servers.Add(item.Name, item.Addr);
            Re();
        }
        private void DeleteS(object sender, RoutedEventArgs e)
        {
            foreach (var item in ServerNameList.SelectedItems)
            {
                var temp = (Server)item;
                Config.Servers.Remove(temp.Name);
            }
            Re();
        }

        private void AddM(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
                return;
            Config.Mute.Add(item);
            Re();
        }
        private void EditM(object sender, RoutedEventArgs e)
        {
            if (MuteList.SelectedItem == null)
                return;
            var item = (string)MuteList.SelectedItem;
            Config.Mute.Remove(item);
            item = new IDSet(item).Set();
            Config.Mute.Add(item);
            Re();
        }
        private void DeleteM(object sender, RoutedEventArgs e)
        {
            foreach (var item in MuteList.SelectedItems)
            {
                Config.Mute.Remove((string)item);
            }
            Re();
        }
    }
}
