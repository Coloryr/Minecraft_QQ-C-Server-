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
                string old = (string)ServerList.SelectedItem;
                string new_ = new IDSet(old).Set();
                if (string.IsNullOrWhiteSpace(new_))
                {
                    MessageBox.Show("无效的服务器名");
                    return;
                }
                Command.ServerS.Remove(old);
                Command.ServerS.Add(new_);
                Re();
            }
        }

        public CommandOBJ Set()
        {
            ShowDialog();
            return Command;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
