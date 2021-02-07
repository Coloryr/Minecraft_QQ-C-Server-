using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ_Core.MySocket
{
    public class MCServerSocket
    {
        public string Name { get; private set; }
        public Socket Socket { get; private set; }
        private Thread RThread;
        private bool IsRun;
        private bool IsCheck = false;
        private int count;

        private readonly Minecraft_QQ Main;
        private readonly Message Message;
        public MCServerSocket(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
            Message = new(Main);
        }
        public void Start(Socket Socket)
        {
            this.Socket = Socket;
            IsRun = true;
            RThread = new Thread(ReceiveData);
            RThread.Start();
        }
        public void Stop()
        {
            IsRun = false;
            Socket.Close();
            Socket.Dispose();
        }
        private string Receive()
        {
            try
            {
                byte[] bytes;
                while (Socket.Available <= 0)
                {
                    Thread.Sleep(10);
                    count++;
                    if (Main.MainConfig.链接.检测断开 && count >= 1000)
                    {
                        count = 0;
                        if (Socket.Poll(10000, SelectMode.SelectRead))
                        {
                            Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "异常断开");
                            Stop();
                            Main.Server.Remove(Name);
                            return null;
                        }
                    }
                }
                bytes = new byte[Socket.Available];
                int receiveNumber = Socket.Receive(bytes);

                return Encoding.UTF8.GetString(bytes, 0, receiveNumber);
            }
            catch(Exception e)
            {
                Logs.LogError(e);
                return null;
            }
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
                        continue;
                    }
                    if (!IsCheck)
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
                        Message.MessageDo(Name, str);
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
