using Native.Csharp.App;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Color_yr.Minecraft_QQ
{
    public class socket
    {
        public static Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        public Print print;                     // 运行时的信息输出方法
        public delegate void Print(string info);
        private static Socket serverSocket;
        public static string MCserver = null;

        public static bool start;
        public static bool ready = false;

        private static Thread server_thread;
        private static Thread read_thread;

        public static void Start_socket()
        {
            try
            {
                logs.Log_write("[INFO][Socket]正在启动端口");
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (useip == true)
                {
                    IPAddress ip = IPAddress.Parse(setip);
                    serverSocket.Bind(new IPEndPoint(ip, Port));
                }
                else
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                serverSocket.Listen(5);

                server_thread = new Thread(listenClientConnect);
                server_thread.Start(serverSocket);
                start = true;
                ready = false;
                if (useip == true)
                    Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + setip + ":" + Port);
                else
                    Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在端口：" + Port);
                logs.Log_write("[INFO][Socket]端口已启动");
            }
            catch (Exception exception)
            {
                Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]启动失败，请看日志");
                logs.Log_write("[ERROR][Socket]端口启动失败\n" + exception.Message);
                start = false;
                ready = false;
            }
        }
        private static void listenClientConnect(object obj)
        {
            Socket socket = (Socket)obj;
            try
            {
                while (true)
                {
                    Socket clientScoket = socket.Accept();

                    if (read_thread != null)
                    {
                        read_thread.Abort();
                        read_thread.DisableComObjectEagerCleanup();
                        read_thread = null;
                    }
                    read_thread = new Thread(receiveData);
                    read_thread.Start(clientScoket);                   // 在新的线程中接收客户端信息

                    GC.Collect();
                    Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]服务器已连接");
                    logs.Log_write("[INFO][Socket]服务器已连接");
                    if (config_read.debug_mode == true)
                        logs.Log_write(clientScoket.ToString());
                    ready = true;

                    Thread.Sleep(1000);                            // 延时1秒后，接收连接请求
                    if (!start) return;
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

                if (config_read.ANSI == "UTF-8")
                    data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
                if (config_read.ANSI == "ANSI（GBK）")
                    data = Encoding.Default.GetString(bytes, 0, receiveNumber);
            }
            return data;
        }

        private static void receiveData(object obj)
        {
            try
            {
                Socket socket = (Socket)obj;

                string clientIp = socket.RemoteEndPoint.ToString();                 // 获取客户端标识 ip和端口
                if (!clients.ContainsKey(clientIp)) clients.Add(clientIp, socket);  // 将连接的客户端socket添加到clients中保存
                else clients[clientIp] = socket;
                MCserver = clientIp;

                while (true)
                {
                    try
                    {
                        string str = Receive(socket);
                        if (!str.Equals(""))
                        {
                            message.Message(str);
                            if (config_read.debug_mode == true)
                                logs.Log_write("收到数据：" + str);
                        }
                    }
                    catch (Exception e)
                    {
                        Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]连接已断开-连接丢失");
                        logs.Log_write("[INFO][Socket]连接已断开-连接丢失:" + e.ToString());
                        ready = false;

                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        socket = null;

                        clients.Clear();
                        MCserver = null;

                        GC.Collect();
                        return;
                    }

                    if (!start)
                    {
                        Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "线程已关闭");
                        return;
                    }
                    Thread.Sleep(100);      // 延时0.1秒后再接收客户端发送的消息
                }
            }
            catch (ThreadAbortException e)
            {
                Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]连接已断开-主动断开");
                logs.Log_write("[INFO][Socket]连接已断开-主动断开:" + e.ToString());
                return;
            }
        }
        public static void Send(messagelist info)
        {
            if (clients.ContainsKey(MCserver))
            {
                Socket socket = clients[MCserver];
                try
                {
                    JObject jsonData = new JObject(
                        new JProperty("group", info.group),
                        new JProperty("message", info.message),
                        new JProperty("player", info.player),
                        new JProperty("is_commder", info.is_commder));
                    Send_data(socket, jsonData.ToString());
                }
                catch (Exception)
                {
                    clients.Clear();
                    MCserver = null;

                    GC.Collect();
                    Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]连接已断开，无法发送");
                    ready = false;
                }
            }
            else
                Common.CqApi.SendGroupMessage(config_read.GroupSet_Main, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private static void Send_data(Socket socket, string data)
        {
            if (socket != null && data != null && !data.Equals(""))
            {
                data = config_read.data_Head + data + config_read.data_End;
                byte[] bytes = null;
                if (config_read.ANSI == "UTF-8")
                    bytes = Encoding.UTF8.GetBytes(data);
                if (config_read.ANSI == "ANSI（GBK）")
                    bytes = Encoding.Default.GetBytes(data);
                socket.Send(bytes);
                if (config_read.debug_mode == true)
                    logs.Log_write("发送数据：" + data);
            }
        }
        public static void socket_stop()
        {
            if (clients.ContainsKey(MCserver))
            {
                Socket socket = clients[MCserver];
                if (socket != null)
                    socket.Close();
            }
            if (read_thread != null)
            {
                read_thread.Abort();
                read_thread.DisableComObjectEagerCleanup();
                read_thread = null;
            }
            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
            if (server_thread != null)
            {
                server_thread.Abort();
                server_thread = null;
            }
            clients.Clear();
        }
    }
}
