using System.Windows;
using System.Windows.Input;

namespace Minecraft_QQ_Gui.SetWindow
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
            DataContext = this;
        }
        public Server Set()
        {
            ShowDialog();
            return Obj;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Close();
            }
        }
    }
}
