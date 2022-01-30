using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minecraft_QQ_Gui
{
    /// <summary>
    /// SelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectWindow : Window
    {
        private bool Res;
        public SelectWindow(string text, string title = "")
        {
            InitializeComponent();
            Title = title;
            Text.Text = text;
        }

        public bool Set() 
        {
            ShowDialog();
            return Res;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Res = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Res = false;
            Close();
        }
    }
}
