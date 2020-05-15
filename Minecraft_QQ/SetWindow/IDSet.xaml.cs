using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// IDSet.xaml 的交互逻辑
    /// </summary>
    public partial class IDSet : Window
    {
        public string ID { get; set; }
        public IDSet(string ID = null)
        {
            InitializeComponent();
            DataContext = this;
            if (ID != null)
            {
                this.ID = ID;
            }
        }
        public string Set()
        {
            ShowDialog();
            return ID;
        }
    }
}
