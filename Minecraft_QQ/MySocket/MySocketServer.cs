using Minecraft_QQ.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ.MySocket
{
    internal class MySocketServer
    {
        public static Dictionary<Socket, Thread> clients = new Dictionary<Socket, Thread>();
        public static Dictionary<string, Socket> MCServers = new Dictionary<string, Socket>();

        private static Socket ServerSocket;

        public static bool start;

        private static Thread serverThread;

        public static bool isready()
        {
            return clients.Count != 0;
        }

        public static void StartSocket()
        {
            try
            {
                logs.LogWrite("[INFO][Socket]正在启动端口");
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(Minecraft_QQ.MainConfig.链接.地址);
                ServerSocket.Bind(new IPEndPoint(ip, Minecraft_QQ.MainConfig.链接.端口));
                ServerSocket.Listen(5);

                serverThread = new Thread(listenClientConnect);
                serverThread.Start();
                start = true;
                if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                    IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + Minecraft_QQ.MainConfig.链接.地址 + ":" + Minecraft_QQ.MainConfig.链接.端口);
                logs.LogWrite("[INFO][Socket]端口已启动");
            }
            catch (Exception exception)
            {
                IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]启动失败，请看日志" +
                    "\n酷Q/Minecraft_QQ/logs.log");
                logs.LogWrite("[ERROR][Socket]端口启动失败\n" + exception.Message);
                start = false;
            }
        }
        private static void listenClientConnect()
        {
            try
            {
                while (true)
                {
                    Socket clientScoket = ServerSocket.Accept();

                    var readThread = new Thread(receiveData);
                    readThread.Start(clientScoket);                   // 在新的线程中接收客户端信息

                    clients.Add(clientScoket, readThread);

                    GC.Collect();
                    logs.LogWrite("[INFO][Socket]服务器已连接");

                    Thread.Sleep(1000);                            // 延时1秒后，接收连接请求
                    if (!start)
                    {
                        if (ServerSocket != null)
                            ServerSocket.Close();
                        return;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
        }
        private static string Receive(Socket socket)
        {
            string data = "";

            byte[] bytes;
            int len = socket.Available;
            if (len > 0)
            {
                bytes = new byte[len];
                int receiveNumber = socket.Receive(bytes);

                if (Minecraft_QQ.MainConfig.链接.编码 == "UTF-8")
                    data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
                if (Minecraft_QQ.MainConfig.链接.编码 == "ANSI")
                    data = Encoding.Default.GetString(bytes, 0, receiveNumber);
            }
            return data;
        }

        private static void receiveData(dynamic socket)
        {
            try
            {
                bool isCheck = false;
                while (true)
                {
                    try
                    {
                        string str = Receive(socket);
                        if (!str.Equals(""))
                        {
                            Task.Factory.StartNew(() =>
                            {
                                if (!isCheck)
                                {
                                    var temp = Message.MessageCheck(str);
                                    if (temp != null)
                                    {
                                        if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                                        {
                                            IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器" + temp + "已连接");
                                        }
                                        logs.LogWrite("[INFO][Socket]服务器" + temp + "已连接");
                                        if (MCServers.ContainsKey(temp))
                                        {
                                            Close(MCServers[temp]);
                                            MCServers.Remove(temp);
                                        }
                                        MCServers.Add(temp, socket);
                                    }
                                    else if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                                    {
                                        IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器已连接");
                                    }
                                    isCheck = true;
                                }
                                else
                                    Message.MessageDo(str);
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        string keys = MCServers.Where(q => q.Value == socket).Select(q => q.Key).FirstOrDefault();
                        if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                            IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器" + keys + "连接已断开");
                        logs.LogWrite("[INFO][Socket]服务器" + keys + "连接已断开");
                        Close(socket);
                        GC.Collect();
                        return;
                    }

                    if (!start)
                    {
                        IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "线程已关闭");
                        Close(socket);
                        return;
                    }
                    Thread.Sleep(10);      // 延时0.01秒后再接收客户端发送的消息
                }
            }
            catch { }
        }
        private static void Close(Socket socket)
        {
            if (socket != null)
                socket.Close();
            if (clients.ContainsKey(socket))
            {
                clients[socket].Abort();
            }
            clients.Remove(socket);
        }

        public static void Send(MessageObj info, List<string> servers = null)
        {
            if (clients.Count != 0)
            {

                JObject jsonData = new JObject(
                                new JProperty("group", info.group),
                                new JProperty("message", info.message),
                                new JProperty("player", info.player),
                                new JProperty("commder", info.commder),
                                new JProperty("is_commder", info.is_commder));
                if (servers != null)
                {
                    foreach (var temp in servers)
                    {
                        SendData(MCServers[temp], jsonData.ToString());
                    }
                }
                else
                {
                    foreach (Socket socket in new List<Socket>(clients.Keys))
                    {
                        SendData(socket, jsonData.ToString());
                    }
                }
            }
            else
                IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private static void SendData(Socket socket, string data)
        {
            try
            {
                if (socket != null && data != null && !data.Equals(""))
                {
                    data = Minecraft_QQ.MainConfig.链接.数据头 + data + Minecraft_QQ.MainConfig.链接.数据尾;
                    byte[] bytes = null;
                    if (Minecraft_QQ.MainConfig.链接.编码 == "UTF-8")
                        bytes = Encoding.UTF8.GetBytes(data);
                    if (Minecraft_QQ.MainConfig.链接.编码 == "ANSI")
                        bytes = Encoding.Default.GetBytes(data);
                    socket.Send(bytes);
                }
            }
            catch (Exception e)
            {
                Close(socket);
                GC.Collect();
                if (clients.Count == 0)
                    IMinecraft_QQ.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]连接已断开，无法发送\n" + e.Message);
            }
        }
        public static void ServerStop()
        {
            foreach (var item in clients)
            {
                Close(item.Key);
                if (item.Value != null)
                {
                    item.Value.Abort();
                }
            }
            if (ServerSocket != null)
            {
                ServerSocket.Close();
                ServerSocket = null;
            }
            if (serverThread != null)
            {
                serverThread.Abort();
                serverThread = null;
            }
        }
    }
}
