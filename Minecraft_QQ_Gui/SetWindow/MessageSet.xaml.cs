using System.Windows;

namespace Minecraft_QQ_Gui.SetWindow
{
    /// <summary>
    /// MessageSet.xaml 的交互逻辑
    /// </summary>
    public partial class MessageSet : Window
    {
        public Server Server { get; set; }
        public MessageSet(Server Server = null)
        {
            InitializeComponent();
            if (Server == null)
            {
                Server = new Server();
            }
            this.Server = Server;
            DataContext = this;
        }
        public Server Set()
        {
            ShowDialog();
            return Server;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
