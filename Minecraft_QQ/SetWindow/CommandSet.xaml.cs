using System.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// CommandSet.xaml 的交互逻辑
    /// </summary>
    public partial class CommandSet : Window
    {
        public Window1.CommandOBJ Command { get; set; }
        public CommandSet(Window1.CommandOBJ Command = null)
        {
            InitializeComponent();
            if (Command == null)
            {
                Command = new Window1.CommandOBJ();
            }
            this.Command = Command;
            DataContext = this;
            Re();
        }
        private void Re()
        {
            ServerList.Items.Clear();
            foreach (var item in Command.ServerS)
            {
                ServerList.Items.Add(item);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
                return;
            Command.ServerS.Add(item);
            Re();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            foreach (var item in ServerList.SelectedItems)
            {
                Command.ServerS.Remove((string)item);
            }
            Re();
        }

        public Window1.CommandOBJ Set()
        {
            ShowDialog();
            return Command;
        }
    }
}
