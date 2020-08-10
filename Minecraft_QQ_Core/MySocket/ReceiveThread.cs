using Minecraft_QQ.Robot;
using Minecraft_QQ.Utils;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ.MySocket
{
    class ReceiveThread
    {
        public string Name { get; private set; }
        public Socket Socket { get; private set; }
        private Thread RThread;
        private bool IsRun;
        private bool IsCheck = false;
        private int count;
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
                    if (count >= 100)
                    {
                        count = 0;
                        if (Socket.Poll(1000, SelectMode.SelectRead))
                        {
                            RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "异常断开");
                            Stop();
                            MySocketServer.Remove(Name);
                            return null;
                        }
                    }
                }
                bytes = new byte[Socket.Available];
                int receiveNumber = Socket.Receive(bytes);

                return Encoding.UTF8.GetString(bytes, 0, receiveNumber);
            }
            catch
            {
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
                        return;
                    if (!IsCheck)
                    {
                        Name = Message.StartCheck(str);
                        if (Name != null)
                        {
                            if (Minecraft_QQ.MainConfig.设置.发送日志到主群)
                            {
                                RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "已连接");
                            }
                            Logs.LogOut("[Socket]服务器" + Name + "已连接");
                            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                        }
                        else if (Minecraft_QQ.MainConfig.设置.发送日志到主群)
                        {
                            RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器已连接");
                        }
                        IsCheck = true;
                        IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                    }
                    else
                        Message.MessageDo(Name, str);
                }
                catch (Exception e)
                {
                    if (Minecraft_QQ.MainConfig.设置.发送日志到主群)
                        RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器" + Name + "异常断开");
                    Logs.LogError(e);
                    Stop();
                    MySocketServer.Remove(Name);
                    IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                    return;
                }
            }
        }
    }
}
