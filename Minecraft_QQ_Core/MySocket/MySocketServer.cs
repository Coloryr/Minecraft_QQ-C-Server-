using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ_Core.MySocket
{
    class TaskObj
    {
        public MCServerSocket Client;
        public string data;
    }
    public class MySocketServer
    {
        public Dictionary<string, MCServerSocket> MCServers = new();
        public static byte[] Checkpack = Encoding.UTF8.GetBytes("test");

        public readonly object lock1 = new();

        private TcpListener ServerSocket;
        private Thread SendThread;

        private ConcurrentQueue<TaskObj> sendtask = new();

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
                IPAddress ip = IPAddress.Parse(Main.MainConfig.链接.地址);
                ServerSocket = new(ip, Main.MainConfig.链接.端口);

                ServerSocket.Start();

                SetState(true);
                if (Main.MainConfig.设置.发送日志到主群)
                {
                    Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + Main.MainConfig.链接.地址 + ":" + Main.MainConfig.链接.端口);
                }

                SendThread = new Thread(SendTask);
                SendThread.Start();

                ServerSocket.BeginAcceptTcpClient(ListenClientConnect, null);

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

        private void SendTask()
        {
            while (Start)
            {
                if (sendtask.TryDequeue(out var task))
                {
                    try
                    {
                        if (task.Client != null && !string.IsNullOrEmpty(task.data))
                        {
                            task.Client.Send(Encoding.UTF8.GetBytes(task.data));
                        }
                    }
                    catch (Exception e)
                    {
                        Logs.LogError(e);
                        Close(task.Client.Name);
                        IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
                        GC.Collect();
                        if (MCServers.Count == 0)
                        {
                            Main.Robot.SendGroupMessage(Main.GroupSetMain, "[Minecraft_QQ]连接已断开，无法发送\n" + e.Message);
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void ListenClientConnect(IAsyncResult ar)
        {
            if (!Start)
                return;
            var client = ServerSocket.EndAcceptTcpClient(ar);
            ServerSocket.BeginAcceptTcpClient(ListenClientConnect, null);
            MCServerSocket clientScoket = new MCServerSocket(Main).Start(client);
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
            sendtask.Enqueue(new TaskObj
            {
                Client = Client,
                data = data
            });
        }
        public void AddServer(string name, MCServerSocket receive)
        {
            if (MCServers.ContainsKey(name))
            {
                Main.Robot.SendGroupMessage(Main.GroupSetMain, $"[Minecraft_QQ]同名服务器{name}连接，旧连接已断开");
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
                ServerSocket.Stop();
            }
            Start = false;
        }
    }
}
