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
        public ServerSet(ConfigOBJ Config = null)
        {
            InitializeComponent();
            DataContext = this;
            if (Config == null)
            {
                this.Config = new ConfigOBJ();
            }
            else
                this.Config = Config;
        }
        public void Set()
        {
            ShowDialog();
        }
    }
}
