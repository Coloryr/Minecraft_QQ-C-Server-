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
        public static Dictionary<string, ReceiveThread> MCServers = new Dictionary<string, ReceiveThread>();

        public static object lock1 = new object();

        private static Socket ServerSocket;
        private static Thread ServerThread;

        public static bool Start;
        private static void SetState(bool st)
        {
            Start = st;
            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
        }

        public static bool IsReady()
        {
            return MCServers.Count != 0;
        }

        public static void StartServer()
        {
            try
            {
                Logs.LogOut("[Socket]正在启动端口");
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(Minecraft_QQ.MainConfig.链接.地址);
                ServerSocket.Bind(new IPEndPoint(ip, Minecraft_QQ.MainConfig.链接.端口));
                ServerSocket.Listen(5);

                ServerThread = new Thread(ListenClientConnect);
                ServerThread.Start();
                SetState(true);
                if (Minecraft_QQ.MainConfig.设置.发送日志到主群)
                    RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + Minecraft_QQ.MainConfig.链接.地址 + ":" + Minecraft_QQ.MainConfig.链接.端口);
                Logs.LogOut("[Socket]端口已启动");
            }
            catch (Exception e)
            {
                RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]启动失败，请看日志" +
                    "\n酷Q/Minecraft_QQ/logs.log");
                Logs.LogError(e);
                SetState(false);
            }
        }
        private static void ListenClientConnect()
        {
            try
            {
                while (true)
                {
                    Socket clientScoket = ServerSocket.Accept();
                    new ReceiveThread().Start(clientScoket);
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
        public static void Close(string name)
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

        public static void Remove(string name)
        {
            lock (lock1)
            {
                if (MCServers.ContainsKey(name))
                {
                    MCServers.Remove(name);
                }
            }
        }

        public static void Send(TranObj info, List<string> servers = null)
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
                    foreach (ReceiveThread socket in new List<ReceiveThread>(MCServers.Values))
                    {
                        SendData(socket, data);
                    }
                }
            }
            else
                RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private static void SendData(ReceiveThread Client, string data)
        {
            try
            {
                if (Client != null && data != null && !data.Equals(""))
                {
                    data = Minecraft_QQ.MainConfig.链接.数据头 + data + Minecraft_QQ.MainConfig.链接.数据尾;
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
                    RobotSocket.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]连接已断开，无法发送\n" + e.Message);
            }
        }
        public static void AddServer(string name, ReceiveThread receive)
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
        public static void ServerStop()
        {
            var temp = new Dictionary<string, ReceiveThread>(MCServers);
            foreach (var item in temp.Values)
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
                ServerThread.Abort();
                ServerThread = null;
            }
            Start = false;
        }
    }
}
