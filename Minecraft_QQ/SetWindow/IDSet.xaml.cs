using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// IDSet.xaml 的交互逻辑
    /// </summary>
    public partial class IDSet : Window
    {
        public string ID { get; set; }
        public IDSet()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string Set()
        {
            ShowDialog();
            return ID;
        }
    }
}
