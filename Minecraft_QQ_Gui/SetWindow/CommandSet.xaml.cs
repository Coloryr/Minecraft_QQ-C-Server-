using System.Windows;

namespace Minecraft_QQ_Gui.SetWindow
{
    /// <summary>
    /// CommandSet.xaml 的交互逻辑
    /// </summary>
    public partial class CommandSet : Window
    {
        public CommandOBJ Command { get; set; }
        public CommandSet(CommandOBJ Command = null)
        {
            InitializeComponent();
            if (Command == null)
            {
                Command = new CommandOBJ();
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

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (ServerList.SelectedItem != null)
            {
                ServerList.SelectedItem = new IDSet((string)ServerList.SelectedItem).Set();
            }
        }

        public CommandOBJ Set()
        {
            ShowDialog();
            return Command;
        }
    }
}
