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
    }
}
