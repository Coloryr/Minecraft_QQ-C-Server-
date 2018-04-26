using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace yan_color.Minecraft_QQ
{
    public class socket
    {
        public static Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        public static string MCserver = null;
        public Print print;                     // 运行时的信息输出方法
        public delegate void Print(string info);
        static Socket serverSocket;
        private static byte[] read = new byte[4096];
        public static Boolean start;
        public static Boolean ready = false;
        static Thread thread1 = new Thread(listenClientConnect);
        static Thread thread2;
        public static void Start_socket()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(Minecraft_QQ.ipaddress);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ip, Minecraft_QQ.Port));
                serverSocket.Listen(10);
                start = true;
                ready = false;
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]已启动");
                thread1.Start(serverSocket);
                
            }
            catch (Exception exception)
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]启动失败，请看后台错误");
                start = false;
                ready = false;
                MessageBox.Show(exception.Message);
            }
        }
        private static void listenClientConnect(object obj)
        {
            Socket socket = (Socket)obj;
            while (true)
            {
                Socket clientScoket = socket.Accept();
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]服务器已连接");
                ready = true;
                if (thread2 == null)
                {
                    thread2 = new Thread(receiveData);
                    thread2.Start(clientScoket);
                }
                else
                {
                    thread2.Abort();
                    thread2 = null;
                    thread2 = new Thread(receiveData);
                    thread2.Start(clientScoket);                     // 在新的线程中接收客户端信息
                }
                Thread.Sleep(1000);                            // 延时1秒后，接收连接请求
                if (!start) return;
            }
        }
        private static string Receive(Socket socket)
        {
            string data = "";

            byte[] bytes = null;
            int len = socket.Available;
            if (len > 0)
            {
                bytes = new byte[len];
                int receiveNumber = socket.Receive(bytes);

                if (LinqXML.read(Minecraft_QQ.config, "编码") == "UTF-8")
                {
                    data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);
                }
                if (LinqXML.read(Minecraft_QQ.config, "编码") == "ANSI（GBK）")
                {
                    data = Encoding.Default.GetString(bytes, 0, receiveNumber);
                }

            }
            return data;
        }

        private static void receiveData(object obj)
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
                        message(str);
                    }
                }
                catch (Exception exception)
                {
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]连接已断开");
                    ready = false;

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    if (clients.ContainsKey(clientIp)) clients.Remove(clientIp);
                    MCserver = null;
                    return;
                }
             
                if (!start) return;
                Thread.Sleep(200);      // 延时0.2秒后再接收客户端发送的消息
            }
        }
        public static void Send(string info, string id)
        {
            if (clients.ContainsKey(id))
            {
                Socket socket = clients[id];

                try
                {
                    Send(socket, info);
                }
                catch (Exception ex)
                {
                    clients.Remove(id);
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, "[Minecraft_QQ]连接已断开，无法发送");
                    ready = false;
                }
            }
        }
        private static void Send(Socket socket, string data)
        {
            if (socket != null && data != null && !data.Equals(""))
            {
                byte[] bytes = null;
                if (LinqXML.read(Minecraft_QQ.config, "编码") == "UTF-8")
                {
                    bytes = Encoding.UTF8.GetBytes(data);
                }
                if (LinqXML.read(Minecraft_QQ.config, "编码") == "ANSI（GBK）")
                {
                    bytes = Encoding.Default.GetBytes(data);
                }
                socket.Send(bytes); 
            }
        }
        private static void message(string read)
        {
            if (read.IndexOf("[群消息]") == 0)
            {
                var sb = new StringBuilder(read);
                sb.Replace("[群消息]", string.Empty);
                string x = sb.ToString();
                string z = Minecraft_QQ.get_string(x, "(", ")");
                if (LinqXML.read(Minecraft_QQ.mute, z) != "true")
                {
                    x = x.Replace("(" + z + ")", "");
                    CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, x);
                    if (Minecraft_QQ.GroupSet2 != 0)
                    {
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet2, x);
                    }
                    if (Minecraft_QQ.GroupSet3 != 0)
                    {
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet3, x);
                    }
                }
                Minecraft_QQ.g = 0;
                return;
            }
            else if (Minecraft_QQ.g == 1)
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, read);
                Minecraft_QQ.g = 0;
                return;
            }
            else if (Minecraft_QQ.g == 2)
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet2, read);
                Minecraft_QQ.g = 0;
                return;
            }
            else if (Minecraft_QQ.g == 3)
            {
                CQ.SendGroupMessage(Minecraft_QQ.GroupSet3, read);
                Minecraft_QQ.g = 0;
                return;
            }
        }
    }
}
