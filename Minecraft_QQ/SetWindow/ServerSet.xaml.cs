using Minecraft_QQ.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
