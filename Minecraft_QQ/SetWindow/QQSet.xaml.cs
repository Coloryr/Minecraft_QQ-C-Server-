using Minecraft_QQ.Utils;
using System.Windows;
namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// QQSet.xaml 的交互逻辑
    /// </summary>
    public partial class QQSet : Window
    {
        internal GroupObj Obj { get; set; }
        internal QQSet(GroupObj Obj = null)
        {
            if (Obj == null)
                Obj = new GroupObj();
            this.Obj = Obj;
            DataContext = this;
            Loaded += QQSet_Loaded;
            InitializeComponent();
        }

        private void QQSet_Loaded(object sender, RoutedEventArgs e)
        {
            new SetTran(this);
        }

        internal GroupObj Set()
        {
            ShowDialog();
            return Obj;
        }
    }
}
