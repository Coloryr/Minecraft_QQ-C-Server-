using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// ServerName.xaml 的交互逻辑
    /// </summary>
    public partial class ServerName : Window
    {
        public Window1.Server Obj { get; set; }
        public ServerName(Window1.Server Obj = null)
        {
            InitializeComponent();
            if (Obj == null)
                this.Obj = new Window1.Server();
            else
                this.Obj = Obj;
            DataContext = Obj;
        }
        public Window1.Server Set()
        {
            ShowDialog();
            return Obj;
        }
    }
}
