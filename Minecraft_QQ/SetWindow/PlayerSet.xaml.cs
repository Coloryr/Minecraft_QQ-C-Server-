using Minecraft_QQ.Utils;
using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// PlayerSet.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerSet : Window
    {
        public PlayerObj PlayerObj { get; set; }
        public PlayerSet(PlayerObj PlayerObj = null)
        {
            InitializeComponent();
            if (PlayerObj == null)
                PlayerObj = new PlayerObj();
            this.PlayerObj = PlayerObj;
            DataContext = this;
        }

        public PlayerObj Set()
        {
            ShowDialog();
            return PlayerObj;
        }
    }
}
