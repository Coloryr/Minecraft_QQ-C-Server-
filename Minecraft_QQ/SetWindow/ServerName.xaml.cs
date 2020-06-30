using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// ServerName.xaml 的交互逻辑
    /// </summary>
    public partial class ServerName : Window
    {
        public Server Obj { get; set; }
        public ServerName(Server Obj = null)
        {
            InitializeComponent();
            if (Obj == null)
                this.Obj = new Server();
            else
                this.Obj = Obj;
            DataContext = this.Obj;
        }
        public Server Set()
        {
            ShowDialog();
            return Obj;
        }
    }
}
