using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ_Core.MySocket
{
    public class MySocketServer
    {
        public Dictionary<string, MCServerSocket> MCServers = new();

        public readonly object lock1 = new();

        private Socket ServerSocket;
        private Thread ServerThread;

        public bool Start { get; private set; }

        private readonly Minecraft_QQ Main;

        public MySocketServer(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
        }

        private void SetState(bool st)
        {
            Start = st;
            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
        }

        public bool IsReady()
        {
            return MCServers.Count != 0;
        }

        public void StartServer()
        {
            try
            {
                Logs.LogOut("[Socket]正在启动端口");
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(Main.MainConfig.链接.地址);
                ServerSocket.Bind(new IPEndPoint(ip, Main.MainConfig.链接.端口));
                ServerSocket.Listen(5);

                ServerThread = new Thread(ListenClientConnect);
                ServerThread.Start();
                SetState(true);
                if (Main.MainConfig.设置.发送日志到主群)
                {
                    Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + Main.MainConfig.链接.地址 + ":" + Main.MainConfig.链接.端口);
                }

                Logs.LogOut("[Socket]端口已启动");
            }
            catch (Exception e)
            {
                Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]启动失败，请看日志" +
                    "\n/Minecraft_QQ/logs.log");
                Logs.LogError(e);
                SetState(false);
            }
        }
        private void ListenClientConnect()
        {
            try
            {
                while (true)
                {
                    Socket clientScoket = ServerSocket.Accept();
                    new MCServerSocket(Main).Start(clientScoket);
                    GC.Collect();
                    Thread.Sleep(1000);
                    if (!Start)
                    {
                        if (ServerSocket != null)
                            ServerSocket.Close();
                        return;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void Close(string name)
        {
            lock (lock1)
            {
                if (MCServers.ContainsKey(name))
                {
                    MCServers[name].Stop();
                    MCServers.Remove(name);
                }
            }
        }

        public void Remove(string name)
        {
            lock (lock1)
            {
                if (MCServers.ContainsKey(name))
                {
                    MCServers.Remove(name);
                }
            }
        }

        public void Send(TranObj info, List<string> servers = null)
        {
            if (MCServers.Count != 0)
            {
                var data = JsonConvert.SerializeObject(info);
                if (servers != null)
                {
                    foreach (var temp in servers)
                    {
                        SendData(MCServers[temp], data);
                    }
                }
                else
                {
                    foreach (MCServerSocket socket in new List<MCServerSocket>(MCServers.Values))
                    {
                        SendData(socket, data);
                    }
                }
            }
            else
                Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private void SendData(MCServerSocket Client, string data)
        {
            try
            {
                if (Client != null && data != null && !data.Equals(""))
                {
                    data = Main.MainConfig.链接.数据头 + data + Main.MainConfig.链接.数据尾;
                    Client.Socket.Send(Encoding.UTF8.GetBytes(data));
                }
            }
            catch (Exception e)
            {
                Logs.LogError(e);
                Close(Client.Name);
                IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                GC.Collect();
                if (MCServers.Count == 0)
                {
                    Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]连接已断开，无法发送\n" + e.Message);
                }
            }
        }
        public void AddServer(string name, MCServerSocket receive)
        {
            if (MCServers.ContainsKey(name))
            {
                MCServers[name].Stop();
                MCServers[name] = receive;
            }
            else
            {
                MCServers.Add(name, receive);
            }
        }
        public void ServerStop()
        {
            Dictionary<string, MCServerSocket> temp = new(MCServers);
            foreach (MCServerSocket item in temp.Values)
            {
                item.Stop();
            }
            if (ServerSocket != null)
            {
                ServerSocket.Close();
                ServerSocket = null;
            }
            if (ServerThread != null)
            {
                ServerThread = null;
            }
            Start = false;
        }
    }
}
