using Color_yr.Minecraft_QQ.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Color_yr.Minecraft_QQ.MySocket
{
    public class MySocketServer
    {
        public static Dictionary<Socket, Thread> clients = new Dictionary<Socket, Thread>();

        private static Socket ServerSocket;

        public static bool start;
        public static bool ready = false;

        private static Thread serverThread;

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
                ready = false;
                if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                    Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]端口已启动\n" +
                        "已绑定在：" + Minecraft_QQ.MainConfig.链接.地址 + ":" + Minecraft_QQ.MainConfig.链接.端口);
                logs.LogWrite("[INFO][Socket]端口已启动");
            }
            catch (Exception exception)
            {
                Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]启动失败，请看日志" +
                    "\n酷Q/Minecraft_QQ/logs.log");
                logs.LogWrite("[ERROR][Socket]端口启动失败\n" + exception.Message);
                start = false;
                ready = false;
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
                    if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                        Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器已连接");
                    logs.LogWrite("[INFO][Socket]服务器已连接");
                    ready = true;

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
                while (true)
                {
                    try
                    {
                        string str = Receive(socket);
                        if (!str.Equals(""))
                            Message.MessageDo(str);
                    }
                    catch (Exception e)
                    {
                        if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                            Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]连接已断开-连接丢失");
                        logs.LogWrite("[INFO][Socket]连接已断开-连接丢失:" + e.ToString());
                        ready = false;
                        Close(socket);
                        GC.Collect();
                        return;
                    }

                    if (!start)
                    {
                        Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "线程已关闭");
                        Close(socket);
                        return;
                    }
                    Thread.Sleep(10);      // 延时0.01秒后再接收客户端发送的消息
                }
            }
            catch (ThreadAbortException e)
            {
                if (Minecraft_QQ.MainConfig.设置.发送日志到群)
                    Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]连接已断开-主动断开");
                logs.LogWrite("[INFO][Socket]连接已断开-主动断开:" + e.ToString());
                return;
            }
        }
        private static void Close(Socket socket)
        {
            //socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            clients[socket].Abort();
            clients.Remove(socket);
        }

        public static void Send(MessageObj info)
        {
            if (clients.Count != 0)
            {
                foreach (Socket socket in clients.Keys)
                {
                    try
                    {
                        JObject jsonData = new JObject(
                            new JProperty("group", info.group),
                            new JProperty("message", info.message),
                            new JProperty("player", info.player),
                            new JProperty("commder", info.commder),
                            new JProperty("is_commder", info.is_commder));
                        SendData(socket, jsonData.ToString());
                    }
                    catch
                    {
                        Close(socket);
                        GC.Collect();
                        Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]连接已断开，无法发送");
                        ready = false;
                    }
                }
            }
            else
                Minecraft_QQ.Plugin.SendGroupMessage(Minecraft_QQ.GroupSetMain, "[Minecraft_QQ]服务器未连接，无法发送");
        }
        private static void SendData(Socket socket, string data)
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
