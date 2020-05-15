using Minecraft_QQ.Config;
using System.Windows;

namespace Minecraft_QQ.SetWindow
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
    }
}
