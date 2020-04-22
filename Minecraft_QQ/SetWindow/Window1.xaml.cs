using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Minecraft_QQ.Config;
using Minecraft_QQ.Utils;
using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : MetroWindow
    {
        public Window1()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            var list = Minecraft_QQ.GroupConfig.群列表.Values;
            QQList.Items.Clear();
            foreach (var item in list)
            {
                QQList.Items.Add(item);
            }
        }

        private async void ChangeQQ(object sender, RoutedEventArgs e)
        {
            var item = ((GroupObj) QQList.SelectedItem).Clone();
            item = new QQSet(item).Set();
            bool ok = long.TryParse(item.群号, out long group);
            if (!ok)
            {
                await this.ShowMessageAsync("修改失败", "请检查你修改后的群号");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表[group] = item;
            Init();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            var item = ((GroupObj)QQList.SelectedItem).Clone();
            long.TryParse(item.群号, out long group);
            Minecraft_QQ.GroupConfig.群列表.Remove(group);
            Init();
        }

        private async void AddQQ(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            bool ok = long.TryParse(item.群号, out long group);
            if (!ok)
            {
                await this.ShowMessageAsync("添加失败","请检查你写的群号");
                return;
            }
            Minecraft_QQ.GroupConfig.群列表.Add(group, item);
            Init();
        }

        private async void launchButton_Click(object sender, RoutedEventArgs e)
        {
            new ConfigWrite().Group();
            await this.ShowMessageAsync("已保存", "群设置已经保存");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ConfigRead().ReadGroup();
        }
    }
}
