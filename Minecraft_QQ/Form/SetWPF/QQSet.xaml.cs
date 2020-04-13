
using System.Windows;

namespace Minecraft_QQ.Form.SetWPF
{
    /// <summary>
    /// QQSet.xaml 的交互逻辑
    /// </summary>
    public partial class QQSet : Window
    {
        public QQSet()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new SetTran(this);
        }
    }
}
