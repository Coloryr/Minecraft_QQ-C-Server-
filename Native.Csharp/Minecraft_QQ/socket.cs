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
        
        public Print print;                     // 运行时的信息输出方法
        public delegate void Print(string info);

        private static Socket serverSocket;
        public static Socket client;
     
        public static string setip = null;

        public static bool start;
        public static bool ready = false;
        public static bool useip = false;

        private static Thread server_thread;
        private static Thread read_thread;

        public static int Port;

        public void Start_socket()
        {
            try
            {
                logs logs = new logs();
                logs.Log_write("[INFO][Socket]正在启动端口");
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (useip == true)
                {
                    IPAddress ip = IPAddress.Parse("127.0.0.1");
                    serverSocket.Bind(new IPEndPoint(ip, Port));
                }
                else
                {
                    setip = null;
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                }
                serverSocket.Listen(5);

                server_thread = new Thread(listenClientConnect);
                server_thread.Start(serverSocket);
                start = true;
                ready = false;
                if (useip == true)
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + setip + ":" + Port);
                else
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在端口：" + Port);
                logs.Log_write("[INFO][Socket]端口已启动");
            }
            catch (Exception exception)
            {
                logs logs = new logs();
                Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]启动失败，请看日志");
                logs.Log_write("[ERROR][Socket]端口启动失败\n" + exception.Message);
                start = false;
                ready = false;
            }
        }
        private static void listenClientConnect(object obj)
        {
            Socket socket = (Socket)obj;
            logs logs = new logs();
            try
            {
                while (true)
                {                 
                    client = socket.Accept();

                    if (read_thread != null)
                    {
                        read_thread.Abort();
                        read_thread.DisableComObjectEagerCleanup();
                        read_thread = null;
                    }
                    read_thread = new Thread(receiveData);
                    read_thread.Start();                   // 在新的线程中接收客户端信息

                    GC.Collect();
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]服务器已连接");
                    logs.Log_write("[INFO][Socket]服务器已连接");
                    if (config_read.debug_mode == true)
                        logs.Log_write(client.ToString());
                    ready = true;

                    Thread.Sleep(1000);                            // 延时1秒后，接收连接请求
                    if (!start) return;
                }
            }
            catch (ThreadAbortException e)
            {
                Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]Socket发生错误，请重启");
                logs.Log_write("[ERROR][socket]接收线程发生错误：" + e.Message);
                return;
            }
        }
        private static string Receive(Socket socket)
        {
            string data = "";
            int len = socket.Available;
            if (len > 0)
            {
                byte[] bytes = new byte[len];
                int receiveNumber = socket.Receive(bytes);

                if (config_read.ANSI == "UTF-8")
                    data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
                if (config_read.ANSI == "ANSI（GBK）")
                    data = Encoding.Default.GetString(bytes, 0, receiveNumber);
            }
            return data;
        }

        private static void receiveData()
        {
            try
            {
                string clientIp = client.RemoteEndPoint.ToString();                 // 获取客户端标识 ip和端口

                while (true)
                {
                    try
                    {
                        string str = Receive(client);
                        if (!str.Equals(""))
                        {
                            message.Message(str);
                            if (config_read.debug_mode == true)
                            {
                                logs logs = new logs();
                                logs.Log_write("收到数据：" + str);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logs logs = new logs();
                        Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]连接已断开-连接丢失");
                        logs.Log_write("[INFO][Socket]连接已断开-连接丢失：" + e.Message);
                        ready = false;

                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        client = null;

                        GC.Collect();
                        return;
                    }

                    if (!start)
                    {
                        Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "线程已关闭");
                        return;
                    }
                    Thread.Sleep(100);      // 延时0.1秒后再接收客户端发送的消息
                }
            }
            catch (ThreadAbortException e)
            {
                logs logs = new logs();
                Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]连接已断开-主动断开");
                logs.Log_write("[INFO][Socket]连接已断开-主动断开：" + e.Message);
                return;
            }
        }
        public static void Send(messagelist info)
        {
            if (client!=null)
            {
                try
                {
                    JObject jsonData = new JObject(
                        new JProperty("group", info.group),
                        new JProperty("message", info.message),
                        new JProperty("player", info.player),
                        new JProperty("is_commder", info.is_commder));
                    Send_data(jsonData.ToString());
                }
                catch (Exception)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    client = null;

                    GC.Collect();
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]连接已断开，无法发送");
                    ready = false;
                }
            }
            else
                Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private static void Send_data(string data)
        {
            if (client != null && data != null && !data.Equals(""))
            {
                data = message.Head + data + message.End;
                byte[] bytes = null;
                if (config_read.ANSI == "UTF-8")
                    bytes = Encoding.UTF8.GetBytes(data);
                else if (config_read.ANSI == "ANSI（GBK）")
                    bytes = Encoding.Default.GetBytes(data);
                client.Send(bytes);
                if (config_read.debug_mode == true)
                {
                    logs logs = new logs();
                    logs.Log_write("发送数据：" + data);
                }
            }
        }

        public static void socket_stop()
        {
            if (client != null)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                client = null;
            }
            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
            if(server_thread!=null)
            { 
                server_thread.Abort();
                server_thread = null;
            }
        }
    }
}
