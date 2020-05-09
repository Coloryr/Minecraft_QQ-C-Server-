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
        public PlayerSet(PlayerObj PlayerObj = null, bool admin = true)
        {
            InitializeComponent();
            if (PlayerObj == null)
                PlayerObj = new PlayerObj();
            this.PlayerObj = PlayerObj;
            DataContext = this;
            if (admin == false)
            {
                Admin_C.Visibility = Visibility.Collapsed;
            }
        }

        public PlayerObj Set()
        {
            ShowDialog();
            return PlayerObj;
        }
    }
}
