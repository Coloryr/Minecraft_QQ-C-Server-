using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            QQList.ItemsSource = Minecraft_QQ.GroupConfig.群列表;
        }
        private void init()
        {
            var list = Minecraft_QQ.GroupConfig.群列表;
            foreach (var item in list)
            { 
                
            }
        }

        private void ChangeQQ(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
        }
    }
}
