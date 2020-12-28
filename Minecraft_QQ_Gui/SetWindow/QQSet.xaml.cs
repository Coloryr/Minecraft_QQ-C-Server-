using Minecraft_QQ_Core.Config;
using System.Windows;
using System.Windows.Input;

namespace Minecraft_QQ_Gui.SetWindow
{
    /// <summary>
    /// QQSet.xaml 的交互逻辑
    /// </summary>
    public partial class QQSet : Window
    {
        public GroupObj Obj { get; set; }
        internal QQSet(GroupObj Obj = null)
        {
            if (Obj == null)
                Obj = new GroupObj();
            this.Obj = Obj;
            DataContext = this;
            InitializeComponent();
        }

        internal GroupObj Set()
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
