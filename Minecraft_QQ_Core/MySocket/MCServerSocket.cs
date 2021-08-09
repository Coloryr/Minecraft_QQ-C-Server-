using Minecraft_QQ_Core.Utils;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.MySocket
{
    public class MCServerSocket
    {
        public string Name { get; private set; }
        public TcpClient Client { get; private set; }
        private Thread RThread;
        private bool IsRun;
        private bool IsCheck;
        private int count;

        private readonly Minecraft_QQ Main;
        private readonly Message Message;
        public MCServerSocket(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
            Message = new(Main);
        }
        public MCServerSocket Start(TcpClient client)
        {
            Client = client;
            IsRun = true;
            RThread = new Thread(ReceiveData);
            RThread.Start();
            return this;
        }
        public void Stop()
        {
            IsRun = false;
            Client.Close();
            Client.Dispose();
        }
        private string Receive()
        {
            try
            {
                byte[] bytes;
                while (Client.Available <= 0)
                {
                    Thread.Sleep(10);
                    count++;
                    if (Main.MainConfig.链接.检测断开 && count >= 1000)
                    {
                        count = 0;
                        Client.GetStream().Write(MySocketServer.Checkpack);
                    }
                }
                bytes = new byte[Client.Available];
                int receiveNumber = Client.GetStream().Read(bytes);

                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception e)
            {
                Logs.LogError(e);
                return null;
            }
        }
        public void Send(byte[] data)
        {
            Client.GetStream().Write(data);
        }
        private void ReceiveData()
        {
            while (IsRun)
            {
                try
                {
                    string str = Receive();
                    if (str == null)
                    {
                        throw new Exception();
                    }
                    else if (str == "test")
                    {
                        continue;
                    }
                    else if (!IsCheck)
                    {
                        Name = Message.StartCheck(str);
                        if (Name != null)
                        {
                            if (Main.MainConfig.设置.发送日志到主群)
                            {
                                Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "已连接");
                            }
                            Logs.LogOut("[Socket]服务器" + Name + "已连接");
                            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                        }
                        else if (Main.MainConfig.设置.发送日志到主群)
                        {
                            Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]服务器已连接");
                        }
                        IsCheck = true;
                        Main.Server.AddServer(Name, this);
                        IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                    }
                    else
                        Task.Run(() => Message.MessageDo(Name, str));
                }
                catch (Exception e)
                {
                    if (Main.MainConfig.设置.发送日志到主群)
                        Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "异常断开");
                    Logs.LogError(e);
                    Stop();
                    Main.Server.Remove(Name);
                    IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                    return;
                }
            }
        }
    }
}
