using Flexlive.CQP.Framework;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace yan_color.Minecraft_QQ
{
    public class socket
    {
        static Socket serverSocket;
        private static byte[] read = new byte[4096];
        static Thread myThread = new Thread(Read);
        public static void Start_socket()
        {   
            IPAddress ip = IPAddress.Parse(Minecraft_QQ.ipaddress);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, Minecraft_QQ.Port));
            serverSocket.Listen(10);
            myThread.Start();
        }
        private static void Read()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Socket myClientSocket = (Socket)clientSocket;
                int a;
                a = myClientSocket.Receive(read);
                if (LinqXML.read(Minecraft_QQ.config, "编码") == "UTF-8")
                {
                    Minecraft_QQ.read_text = Encoding.UTF8.GetString(read, 0, a);
                }
                if (LinqXML.read(Minecraft_QQ.config, "编码") == "ANSI（GBK）")
                {
                    Minecraft_QQ.read_text = Encoding.Default.GetString(read, 0, a);
                }
                if (Minecraft_QQ.read_text != "")
                {
                    if (Minecraft_QQ.g == 1)
                    {
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, Minecraft_QQ.read_text);
                        Minecraft_QQ.g = 0;
                    }
                    else if (Minecraft_QQ.g == 2)
                    {
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet2, Minecraft_QQ.read_text);
                        Minecraft_QQ.g = 0;
                    }
                    else if (Minecraft_QQ.g == 3)
                    {
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet3, Minecraft_QQ.read_text);
                        Minecraft_QQ.g = 0;
                    }
                    if (Minecraft_QQ.read_text.IndexOf("[群消息]") == 0)
                    {
                        var sb = new StringBuilder(Minecraft_QQ.read_text);
                        sb.Replace("[群消息]", string.Empty);
                        CQ.SendGroupMessage(Minecraft_QQ.GroupSet1, sb.ToString());
                        if (Minecraft_QQ.GroupSet2 != 0)
                        {
                            CQ.SendGroupMessage(Minecraft_QQ.GroupSet2, sb.ToString());
                        }
                        if (Minecraft_QQ.GroupSet3 != 0)
                        {
                            CQ.SendGroupMessage(Minecraft_QQ.GroupSet3, sb.ToString());
                        }
                    }
                }
                Minecraft_QQ.read_text = "";
                if (Minecraft_QQ.text != "")
                {
                    if (LinqXML.read(Minecraft_QQ.config, "编码") == "UTF-8")
                    {
                        clientSocket.Send(Encoding.UTF8.GetBytes(Minecraft_QQ.text));
                    }
                    if (LinqXML.read(Minecraft_QQ.config, "编码") == "ANSI（GBK）")
                    {
                        clientSocket.Send(Encoding.Default.GetBytes(Minecraft_QQ.text));
                    }
                }
                Minecraft_QQ.text = "";
                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
            }
        }
        public static void stop_socket()
        {
            myThread.Abort();
            myThread.Join();
            //MessageBox.Show("程序已关闭");
        }
    }
}
