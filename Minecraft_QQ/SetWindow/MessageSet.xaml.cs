using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// MessageSet.xaml 的交互逻辑
    /// </summary>
    public partial class MessageSet : Window
    {
        public Window1.Server Server { get; set; }
        public MessageSet(Window1.Server Server = null)
        {
            InitializeComponent();
            if (Server == null)
            {
                Server = new Window1.Server();
            }
            this.Server = Server;
            DataContext = this;
        }
        public Window1.Server Set()
        {
            ShowDialog();
            return Server;
        }
    }
}
