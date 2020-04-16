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
            init();
        }
        private void init()
        {
            var list = Minecraft_QQ.GroupConfig.群列表.Values;
            foreach (var item in list)
            {
                QQList.Items.Add(item);
            }
        }

        private void ChangeQQ(object sender, RoutedEventArgs e)
        {
            dynamic item = QQList.SelectedItem;
            item = new QQSet(item).Set();
            long.TryParse(item.群号, out long group);
            Minecraft_QQ.GroupConfig.群列表[group] = item;
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            dynamic item = QQList.SelectedItem;
            long.TryParse(item.群号, out long group);
            Minecraft_QQ.GroupConfig.群列表.Remove(group);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            long.TryParse(item.群号, out long group);
            Minecraft_QQ.GroupConfig.群列表.Add(group, item);
        }
    }
}
